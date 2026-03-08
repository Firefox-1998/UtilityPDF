#include "BackupService.h"
#include "Logger.h"
#include "ArchiveManager.h"
#include <chrono>
#include <iomanip>
#include <sstream>
#include <regex>
#include <algorithm>

// ✅ Backup selettivo solo dei file da aggiornare
BackupService::BackupResult BackupService::CreateSelectiveBackup(
    const std::wstring& appDir,
    const std::wstring& backupDir,
    const std::wstring& updateDir)
{
    BackupResult result = { false, L"", L"", 0 };

    try
    {
        Logger::LogInfo(L"SELECTIVE_BACKUP_START", 
            L"Inizio backup selettivo - Solo file da aggiornare");
        Logger::LogInfo(L"BACKUP_SOURCE_APP", L"App dir: " + appDir);
        Logger::LogInfo(L"BACKUP_SOURCE_UPDATE", L"Update dir: " + updateDir);

        // 1. Ottieni lista file da aggiornare
        auto filesToUpdate = GetFilesToUpdate(updateDir);
        
        if (filesToUpdate.empty())
        {
            result.success = false;
            result.message = L"Nessun file da aggiornare trovato";
            Logger::LogWarning(L"BACKUP_NO_FILES", result.message);
            return result;
        }

        Logger::LogInfo(L"BACKUP_FILES_COUNT", 
            L"File da backuppare: " + std::to_wstring(filesToUpdate.size()));

        // 2. Crea directory backup con timestamp
        if (!fs::exists(backupDir))
        {
            fs::create_directories(backupDir);
        }

        auto now = std::chrono::system_clock::now();
        auto time_t_now = std::chrono::system_clock::to_time_t(now);
        std::tm tm_now;
        localtime_s(&tm_now, &time_t_now);

        std::wostringstream oss;
        oss << L"backup_" << std::put_time(&tm_now, L"%Y-%m-%d_%H-%M-%S");
        std::wstring backupName = oss.str();
        fs::path backupPath = fs::path(backupDir) / backupName;

        fs::create_directories(backupPath);
        Logger::LogInfo(L"BACKUP_DIR_CREATED", L"Directory backup: " + backupPath.wstring());

        // 3. Backup selettivo solo dei file che verranno sovrascritti
        uintmax_t totalSize = 0;
        int backedUpCount = 0;
        int skippedCount = 0;
        int notFoundCount = 0;

        for (const auto& relativeUpdatePath : filesToUpdate)
        {
            try
            {
                fs::path sourceFile = fs::path(appDir) / relativeUpdatePath;
                
                if (!fs::exists(sourceFile))
                {
                    notFoundCount++;
                    Logger::LogInfo(L"BACKUP_FILE_NOT_EXISTS", 
                        L"File nuovo (non presente): " + relativeUpdatePath.wstring());
                    continue;
                }

                fs::path destFile = backupPath / relativeUpdatePath;
                fs::path destDir = destFile.parent_path();

                if (!fs::exists(destDir))
                {
                    fs::create_directories(destDir);
                }

                fs::copy_file(sourceFile, destFile, fs::copy_options::overwrite_existing);
                
                uintmax_t fileSize = fs::file_size(destFile);
                totalSize += fileSize;
                backedUpCount++;

                Logger::LogInfo(L"BACKUP_FILE_SUCCESS", 
                    L"Backuppato: " + relativeUpdatePath.wstring() + 
                    L" (" + std::to_wstring(fileSize / 1024) + L" KB)");
            }
            catch (const std::exception& ex)
            {
                std::string errorMsg = ex.what();
                Logger::LogWarning(L"BACKUP_FILE_ERROR", 
                    L"Errore backup: " + relativeUpdatePath.wstring() + 
                    L" -> " + std::wstring(errorMsg.begin(), errorMsg.end()));
                skippedCount++;
            }
        }

        // 4. Verifica risultato
        if (backedUpCount == 0 && notFoundCount == 0)
        {
            result.success = false;
            result.message = L"Nessun file backuppato (tutti saltati)";
            Logger::LogError(L"BACKUP_FAILED", result.message);
            return result;
        }

        result.success = true;
        result.message = L"Backup selettivo completato";
        result.backupPath = backupPath.wstring();
        result.backupSize = totalSize;

        Logger::LogInfo(L"SELECTIVE_BACKUP_SUCCESS", 
            L"Backup completato: " + std::to_wstring(backedUpCount) + L" file backuppati, " +
            std::to_wstring(notFoundCount) + L" nuovi file, " +
            std::to_wstring(skippedCount) + L" errori, " +
            std::to_wstring(totalSize / 1024) + L" KB");

        // 5. Pulizia backup vecchi
        CleanOldBackups(backupDir);

        return result;
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        result.success = false;
        result.message = L"Errore backup selettivo: " + 
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"SELECTIVE_BACKUP_ERROR", result.message);
        return result;
    }
}

// ✅ NUOVO: Ottiene lista file da updateDirectory (escludendo file di servizio)
std::vector<fs::path> BackupService::GetFilesToUpdate(const std::wstring& updateDir)
{
    std::vector<fs::path> files;

    try
    {
        if (!fs::exists(updateDir))
        {
            Logger::LogError(L"UPDATE_DIR_NOT_EXISTS", L"Directory update non esiste: " + updateDir);
            return files;
        }

        for (const auto& entry : fs::recursive_directory_iterator(updateDir))
        {
            if (!entry.is_regular_file()) continue;

            fs::path relativePath = fs::relative(entry.path(), updateDir);
            std::wstring relativePathStr = relativePath.wstring();

            // Escludi file di servizio
            std::wstring lowerPath = relativePathStr;
            std::transform(lowerPath.begin(), lowerPath.end(), lowerPath.begin(), ::tolower);

            if (lowerPath == L"updateappsettings.json" ||
                lowerPath == L"update_in_progress.json" ||
                lowerPath == L"update_manifest.json" ||
                lowerPath.ends_with(L".log") ||
                lowerPath.ends_with(L".tmp") ||
                lowerPath.ends_with(L".bak"))
            {
                Logger::LogInfo(L"UPDATE_FILE_SKIP_SERVICE", 
                    L"File di servizio escluso da lista update: " + relativePathStr);
                continue;
            }

            files.push_back(relativePath);
        }

        Logger::LogInfo(L"UPDATE_FILES_FOUND", 
            L"File da aggiornare: " + std::to_wstring(files.size()));
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogError(L"GET_UPDATE_FILES_ERROR", 
            L"Errore lettura file update: " + std::wstring(errorMsg.begin(), errorMsg.end()));
    }

    return files;
}

bool BackupService::RestoreBackup(
    const std::wstring& backupPath,
    const std::wstring& appDir)
{
    try
    {
        Logger::LogInfo(L"RESTORE_START", L"Starting backup restore: " + backupPath);

        if (fs::is_directory(backupPath))
        {
            // Directory-based backup (legacy or fallback)
            int filesRestored = 0;
            for (const auto& entry : fs::recursive_directory_iterator(backupPath))
            {
                if (!entry.is_regular_file()) { continue; }

                fs::path relativePath = fs::relative(entry.path(), backupPath);
                fs::path destPath = fs::path(appDir) / relativePath;
                fs::path destDir = destPath.parent_path();

                if (!fs::exists(destDir))
                {
                    fs::create_directories(destDir);
                }

                fs::copy_file(entry.path(), destPath, fs::copy_options::overwrite_existing);
                filesRestored++;
            }

            Logger::LogInfo(L"RESTORE_COMPLETE",
                L"Directory backup restored: " + std::to_wstring(filesRestored) + L" files");
            return filesRestored > 0;
        }
        else if (fs::is_regular_file(backupPath))
        {
            // Zip archive backup
            ArchiveManager::ArchiveResult extractResult =
                ArchiveManager::ExtractZip(backupPath, appDir);

            if (extractResult.success)
            {
                Logger::LogInfo(L"RESTORE_ZIP_COMPLETE",
                    L"Zip backup restored: " + std::to_wstring(extractResult.filesProcessed) + L" files");
            }
            else
            {
                Logger::LogError(L"RESTORE_ZIP_FAILED", extractResult.message);
            }
            return extractResult.success;
        }

        Logger::LogError(L"RESTORE_NOT_FOUND", L"Backup path not found: " + backupPath);
        return false;
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogError(L"RESTORE_EXCEPTION",
            L"Restore error: " + std::wstring(errorMsg.begin(), errorMsg.end()));
        return false;
    }
}

void BackupService::CleanOldBackups(
    const std::wstring& backupDir,
    int retentionDays)
{
    try
    {
        if (!fs::exists(backupDir)) return;

        auto now = std::chrono::system_clock::now();
        auto cutoffDate = now - std::chrono::hours(24 * retentionDays);

        int deletedCount = 0;
        uintmax_t deletedSize = 0;

        for (const auto& entry : fs::directory_iterator(backupDir))
        {
            if (!entry.is_directory()) continue;

            auto backupDate = ExtractDateFromBackupName(entry.path().filename().wstring());
            
            if (backupDate.has_value() && backupDate.value() < cutoffDate)
            {
                try
                {
                    for (const auto& file : fs::recursive_directory_iterator(entry.path()))
                    {
                        if (file.is_regular_file())
                        {
                            deletedSize += fs::file_size(file.path());
                        }
                    }

                    fs::remove_all(entry.path());
                    deletedCount++;

                    Logger::LogInfo(L"BACKUP_CLEANED", 
                        L"Backup vecchio eliminato: " + entry.path().filename().wstring());
                }
                catch (const std::exception& ex)
                {
                    std::string errorMsg = ex.what();
                    Logger::LogWarning(L"BACKUP_CLEAN_ERROR", 
                        L"Errore eliminazione: " + std::wstring(errorMsg.begin(), errorMsg.end()));
                }
            }
        }

        if (deletedCount > 0)
        {
            Logger::LogInfo(L"BACKUP_CLEANUP_COMPLETE", 
                L"Pulizia backup: " + std::to_wstring(deletedCount) + 
                L" backup eliminati, " + std::to_wstring(deletedSize / 1024) + L" KB liberati");
        }
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogWarning(L"BACKUP_CLEANUP_ERROR", 
            L"Errore pulizia: " + std::wstring(errorMsg.begin(), errorMsg.end()));
    }
}

bool BackupService::ShouldExcludeFromBackup(const std::wstring& path)
{
    std::vector<std::wstring> excludePatterns = {
        L"logs\\",
        L"cache\\",
        L"temp\\",
        L"backups\\",
        L".sqlite",
        L".db",
        L".log",
        L".tmp",
        L".bak",
        L"update_in_progress.json",
        L"update_failed.json"
    };

    std::wstring lowerPath = path;
    std::transform(lowerPath.begin(), lowerPath.end(), lowerPath.begin(), ::tolower);

    for (const auto& pattern : excludePatterns)
    {
        std::wstring lowerPattern = pattern;
        std::transform(lowerPattern.begin(), lowerPattern.end(), lowerPattern.begin(), ::tolower);

        if (lowerPath.find(lowerPattern) != std::wstring::npos)
        {
            return true;
        }
    }

    return false;
}

std::optional<std::chrono::system_clock::time_point> 
BackupService::ExtractDateFromBackupName(const std::wstring& fileName)
{
    try
    {
        std::wregex dateRegex(L"backup_(\\d{4})-(\\d{2})-(\\d{2})_(\\d{2})-(\\d{2})-(\\d{2})");
        std::wsmatch match;

        if (std::regex_search(fileName, match, dateRegex))
        {
            std::tm tm = {};
            tm.tm_year = std::stoi(match[1].str()) - 1900;
            tm.tm_mon = std::stoi(match[2].str()) - 1;
            tm.tm_mday = std::stoi(match[3].str());
            tm.tm_hour = std::stoi(match[4].str());
            tm.tm_min = std::stoi(match[5].str());
            tm.tm_sec = std::stoi(match[6].str());

            std::time_t time = std::mktime(&tm);
            return std::chrono::system_clock::from_time_t(time);
        }

        return std::nullopt;
    }
    catch (...)
    {
        return std::nullopt;
    }
}

// ============================================================================
// ADD the following methods at the END of the existing BackupService.cpp file
// (after CleanOldBackups)
// ============================================================================

BackupService::BackupResult BackupService::CreateVersionedBackup(
    const std::wstring& appDir,
    const std::wstring& backupDir,
    const std::wstring& updateDir,
    const std::wstring& currentVersion)
{
    BackupResult result = { false, L"", L"", 0 };

    try
    {
        Logger::LogInfo(L"VERSIONED_BACKUP_START",
            L"Creating versioned backup for v" + currentVersion);

        // 1. Get list of files that will be overwritten
        auto filesToUpdate = GetFilesToUpdate(updateDir);

        if (filesToUpdate.empty())
        {
            result.success = false;
            result.message = L"No files to update found";
            Logger::LogWarning(L"BACKUP_NO_FILES", result.message);
            return result;
        }

        Logger::LogInfo(L"BACKUP_FILES_COUNT",
            L"Files to back up: " + std::to_wstring(filesToUpdate.size()));

        // 2. Create backup directory
        if (!fs::exists(backupDir))
        {
            fs::create_directories(backupDir);
        }

        // 3. Create staging directory: pre-update_v{version}
        std::wstring sanitizedVersion = currentVersion;
        for (wchar_t& ch : sanitizedVersion)
        {
            if (ch == L' ' || ch == L'\\' || ch == L'/' || ch == L':')
            {
                ch = L'_';
            }
        }

        std::wstring backupName = L"pre-update_v" + sanitizedVersion;
        fs::path stagingPath = fs::path(backupDir) / backupName;

        // If a backup with same version exists, add timestamp
        if (fs::exists(stagingPath) || fs::exists(fs::path(backupDir) / (backupName + L".zip")))
        {
            auto now = std::chrono::system_clock::now();
            auto time_t_now = std::chrono::system_clock::to_time_t(now);
            std::tm tm_now;
            localtime_s(&tm_now, &time_t_now);

            std::wostringstream oss;
            oss << backupName << L"_" << std::put_time(&tm_now, L"%Y%m%d_%H%M%S");
            backupName = oss.str();
            stagingPath = fs::path(backupDir) / backupName;
        }

        fs::create_directories(stagingPath);
        Logger::LogInfo(L"BACKUP_DIR_CREATED", L"Staging directory: " + stagingPath.wstring());

        // 4. Copy files that will be overwritten to staging directory
        uintmax_t totalSize = 0;
        int backedUpCount = 0;
        int skippedCount = 0;
        int notFoundCount = 0;

        for (const auto& relativeUpdatePath : filesToUpdate)
        {
            try
            {
                fs::path sourceFile = fs::path(appDir) / relativeUpdatePath;

                if (!fs::exists(sourceFile))
                {
                    notFoundCount++;
                    Logger::LogInfo(L"BACKUP_FILE_NEW",
                        L"New file (not present in current version): " + relativeUpdatePath.wstring());
                    continue;
                }

                fs::path destFile = stagingPath / relativeUpdatePath;
                fs::path destDir = destFile.parent_path();

                if (!fs::exists(destDir))
                {
                    fs::create_directories(destDir);
                }

                fs::copy_file(sourceFile, destFile, fs::copy_options::overwrite_existing);

                uintmax_t fileSize = fs::file_size(destFile);
                totalSize += fileSize;
                backedUpCount++;

                Logger::LogInfo(L"BACKUP_FILE_OK",
                    L"Backed up: " + relativeUpdatePath.wstring() +
                    L" (" + std::to_wstring(fileSize / 1024) + L" KB)");
            }
            catch (const std::exception& ex)
            {
                std::string errorMsg = ex.what();
                Logger::LogWarning(L"BACKUP_FILE_ERROR",
                    L"Backup error: " + relativeUpdatePath.wstring() +
                    L" -> " + std::wstring(errorMsg.begin(), errorMsg.end()));
                skippedCount++;
            }
        }

        if (backedUpCount == 0 && notFoundCount == 0)
        {
            result.success = false;
            result.message = L"No files were backed up (all skipped)";
            Logger::LogError(L"BACKUP_FAILED", result.message);
            return result;
        }

        // 5. Compress staging directory to zip
        std::wstring zipPath = (fs::path(backupDir) / (backupName + L".zip")).wstring();

        ArchiveManager::ArchiveResult zipResult =
            ArchiveManager::CompressZip(stagingPath.wstring(), zipPath);

        if (zipResult.success)
        {
            fs::remove_all(stagingPath);
            result.backupPath = zipPath;
            Logger::LogInfo(L"BACKUP_ZIPPED", L"Backup compressed: " + zipPath);
        }
        else
        {
            result.backupPath = stagingPath.wstring();
            Logger::LogWarning(L"BACKUP_ZIP_FAILED",
                L"Zip failed, keeping directory: " + stagingPath.wstring());
        }

        result.success = true;
        result.message = L"Versioned backup completed";
        result.backupSize = totalSize;

        Logger::LogInfo(L"VERSIONED_BACKUP_SUCCESS",
            L"Backup: " + std::to_wstring(backedUpCount) + L" files, " +
            std::to_wstring(notFoundCount) + L" new, " +
            std::to_wstring(totalSize / 1024) + L" KB");

        // 6. Clean old backups
        CleanOldBackups(backupDir);

        return result;
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        result.success = false;
        result.message = L"Versioned backup error: " +
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"VERSIONED_BACKUP_ERROR", result.message);
        return result;
    }
}
