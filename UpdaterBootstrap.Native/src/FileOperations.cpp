#include "FileOperations.h"
#include "ConfigMerger.h"
#include "Logger.h"
#include <windows.h>        // ✅ AGGIUNGI: Per MultiByteToWideChar, CP_UTF8
#include <regex>            // ✅ AGGIUNGI: Per std::wregex, std::wsmatch, std::regex_search
#include <fstream>
#include <algorithm>

using json = nlohmann::json;

std::optional<std::wstring> FileOperations::FindUpdateConfigPath(const std::wstring& updateDir)
{
    try
    {
        // 1️⃣ Prova percorso diretto
        fs::path directPath = fs::path(updateDir) / L"updateappsettings.json";
        if (fs::exists(directPath))
        {
            Logger::LogInfo(L"UPDATE_CONFIG_FOUND", L"File configurazione trovato: " + directPath.wstring());
            return directPath.wstring();
        }

        // 2️⃣ Ricerca ricorsiva
        for (const auto& entry : fs::recursive_directory_iterator(updateDir))
        {
            if (entry.is_regular_file() &&
                entry.path().filename() == L"updateappsettings.json")
            {
                Logger::LogInfo(L"UPDATE_CONFIG_FOUND_RECURSIVE", 
                    L"File configurazione trovato (ricerca ricorsiva): " + entry.path().wstring());
                return entry.path().wstring();
            }
        }

        Logger::LogError(L"UPDATE_CONFIG_NOT_FOUND", L"File updateappsettings.json non trovato in: " + updateDir);
        return std::nullopt;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"FIND_CONFIG_ERROR", 
            L"Errore ricerca file configurazione: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return std::nullopt;
    }
}

std::optional<FileOperations::UpdateInfo> FileOperations::CheckForUpdate(
    const std::wstring& appDir,
    const std::wstring& updateDir,
    const std::wstring& updateConfigPath)
{
    try
    {
        // 1️⃣ Verifica esistenza directory aggiornamenti
        if (!fs::exists(updateDir))
        {
            Logger::LogWarning(L"UPDATE_CHECK_NO_DIR", L"Directory aggiornamenti non esiste: " + updateDir);
            return std::nullopt;
        }

        // 2️⃣ Conta file in directory aggiornamenti
        int fileCount = 0;
        for (const auto& entry : fs::recursive_directory_iterator(updateDir))
        {
            if (entry.is_regular_file()) fileCount++;
        }

        if (fileCount == 0)
        {
            Logger::LogInfo(L"UPDATE_CHECK_EMPTY", L"Directory aggiornamenti vuota");
            return std::nullopt;
        }

        // 3️⃣ Leggi versione da updateappsettings.json
        std::ifstream updateFile(updateConfigPath);
        if (!updateFile.is_open())
        {
            Logger::LogError(L"UPDATE_CONFIG_READ_ERROR", L"Impossibile aprire: " + updateConfigPath);
            return std::nullopt;
        }

        json updateConfig;
        try
        {
            updateFile >> updateConfig;
        }
        catch (const json::parse_error& ex)
        {
            Logger::LogError(L"UPDATE_JSON_PARSE_ERROR", 
                L"Errore parsing JSON: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
            return std::nullopt;
        }

        // 4️⃣ Estrai nuova versione (case-insensitive)
        std::string newVersionUtf8;
        if (updateConfig.contains("application") && updateConfig["application"].contains("version"))
        {
            newVersionUtf8 = updateConfig["application"]["version"].get<std::string>();
        }
        else if (updateConfig.contains("Application") && updateConfig["Application"].contains("Version"))
        {
            newVersionUtf8 = updateConfig["Application"]["Version"].get<std::string>();
        }
        else
        {
            Logger::LogWarning(L"UPDATE_VERSION_MISSING", L"Versione non trovata in updateappsettings.json");
            return std::nullopt;
        }

        // Converti UTF-8 → UTF-16
        int size = MultiByteToWideChar(CP_UTF8, 0, newVersionUtf8.c_str(), -1, nullptr, 0);
        std::wstring newVersion(size, 0);
        MultiByteToWideChar(CP_UTF8, 0, newVersionUtf8.c_str(), -1, &newVersion[0], size);
        newVersion.pop_back(); // Rimuovi null terminator

        // 5️⃣ Leggi versione corrente da appsettings.json
        fs::path currentConfigPath = fs::path(appDir) / L"config" / L"appsettings.json";
        if (!fs::exists(currentConfigPath))
        {
            Logger::LogError(L"CURRENT_CONFIG_MISSING", L"File configurazione corrente mancante: " + currentConfigPath.wstring());
            return std::nullopt;
        }

        std::ifstream currentFile(currentConfigPath);
        json currentConfig;
        try
        {
            currentFile >> currentConfig;
        }
        catch (const json::parse_error& ex)
        {
            Logger::LogError(L"CURRENT_JSON_PARSE_ERROR", 
                L"Errore parsing JSON corrente: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
            return std::nullopt;
        }

        // 6️⃣ Estrai versione corrente (case-insensitive)
        std::string currentVersionUtf8;
        if (currentConfig.contains("application") && currentConfig["application"].contains("version"))
        {
            currentVersionUtf8 = currentConfig["application"]["version"].get<std::string>();
        }
        else if (currentConfig.contains("Application") && currentConfig["Application"].contains("Version"))
        {
            currentVersionUtf8 = currentConfig["Application"]["Version"].get<std::string>();
        }
        else
        {
            currentVersionUtf8 = "0.0.0";
        }

        size = MultiByteToWideChar(CP_UTF8, 0, currentVersionUtf8.c_str(), -1, nullptr, 0);
        std::wstring currentVersion(size, 0);
        MultiByteToWideChar(CP_UTF8, 0, currentVersionUtf8.c_str(), -1, &currentVersion[0], size);
        currentVersion.pop_back();

        // 7️⃣ Confronta versioni
        std::wstring newVersionNum = ExtractVersionNumber(newVersion);
        std::wstring currentVersionNum = ExtractVersionNumber(currentVersion);

        if (IsVersionGreater(newVersionNum, currentVersionNum))
        {
            Logger::LogInfo(L"UPDATE_AVAILABLE", 
                L"Aggiornamento disponibile: " + currentVersion + L" → " + newVersion);

            UpdateInfo info;
            info.currentVersion = currentVersion;
            info.newVersion = newVersion;
            info.updateDirectory = updateDir;
            info.updateConfigPath = updateConfigPath;

            return info;
        }

        Logger::LogInfo(L"UPDATE_UP_TO_DATE", 
            L"Applicazione già aggiornata: " + currentVersion);
        return std::nullopt;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"UPDATE_CHECK_ERROR", 
            L"Errore verifica aggiornamenti: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return std::nullopt;
    }
}

FileOperations::UpdateResult FileOperations::ApplyCompleteUpdate(
    const std::wstring& appDir,
    const std::wstring& updateDir,
    const UpdateInfo& updateInfo)
{
    UpdateResult result{ false, L"", 0, 0, 0 };

    try
    {
        Logger::LogInfo(L"UPDATE_APPLY_START", L"🔄 Inizio applicazione aggiornamento completo");

        int filesUpdated = 0;
        int filesSkipped = 0;
        int filesError = 0;

        // 1️⃣ Copia file dalla updateDir all'appDir
        for (const auto& entry : fs::recursive_directory_iterator(updateDir))
        {
            if (!entry.is_regular_file())
            {
                continue;
            }

            fs::path sourcePath = entry.path();
            fs::path relativePath = fs::relative(sourcePath, updateDir);
            fs::path destPath = fs::path(appDir) / relativePath;

            std::wstring fileName = sourcePath.filename().wstring();

            // ✅ Salta file di servizio
            if (ShouldSkipFile(fileName))
            {
                filesSkipped++;
                Logger::LogInfo(L"FILE_SKIPPED", L"⏭️ Saltato file di servizio: " + fileName);
                continue;
            }

            try
            {
                // Crea directory destinazione se non esiste
                fs::path destDir = destPath.parent_path();
                if (!fs::exists(destDir))
                {
                    fs::create_directories(destDir);
                }

                // Copia file
                fs::copy_file(sourcePath, destPath, fs::copy_options::overwrite_existing);
                filesUpdated++;
                
                Logger::LogInfo(L"FILE_UPDATED", L"✅ Aggiornato: " + relativePath.wstring());
            }
            catch (const std::exception& ex)
            {
                filesError++;
                std::string errorMsg = ex.what();
                Logger::LogError(L"FILE_UPDATE_ERROR", 
                    L"❌ Errore aggiornamento file: " + relativePath.wstring() + 
                    L" - " + std::wstring(errorMsg.begin(), errorMsg.end()));
            }
        }

        // 2️⃣ ✅ NUOVO: Merge intelligente configurazione
        fs::path updateConfigPath = fs::path(updateDir) / L"updateappsettings.json";
        fs::path mainConfigPath = fs::path(appDir) / L"config" / L"appsettings.json";

        if (fs::exists(updateConfigPath))
        {
            Logger::LogInfo(L"CONFIG_MERGE_START", 
                L"📋 Merge configurazione: updateappsettings.json → appsettings.json");

            bool mergeSuccess = ConfigMerger::MergeAppSettings(mainConfigPath, updateConfigPath);

            if (!mergeSuccess)
            {
                result.success = false;
                result.message = L"❌ Merge configurazione fallito";
                result.filesUpdated = filesUpdated;
                result.filesSkipped = filesSkipped;
                result.filesError = filesError + 1;
                return result;
            }

            Logger::LogInfo(L"CONFIG_MERGE_SUCCESS", L"✅ Configurazione aggiornata con successo");
        }
        else
        {
            Logger::LogInfo(L"CONFIG_MERGE_SKIP", 
                L"ℹ️ updateappsettings.json non trovato, merge configurazione saltato");
        }

        // 3️⃣ Risultato finale
        result.success = (filesError == 0);
        result.filesUpdated = filesUpdated;
        result.filesSkipped = filesSkipped;
        result.filesError = filesError;
        result.message = result.success 
            ? L"✅ Aggiornamento completato con successo"
            : L"⚠️ Aggiornamento completato con errori";

        Logger::LogInfo(L"UPDATE_APPLY_COMPLETE", 
            L"📊 Risultato: " + std::to_wstring(filesUpdated) + L" aggiornati, " +
            std::to_wstring(filesSkipped) + L" saltati, " +
            std::to_wstring(filesError) + L" errori");

        return result;
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        result.success = false;
        result.message = L"❌ Errore critico durante aggiornamento: " + 
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"UPDATE_APPLY_EXCEPTION", result.message);
        return result;
    }
}

bool FileOperations::MergeConfigurationFile(
    const std::wstring& updateConfigPath,
    const std::wstring& mainConfigPath)
{
    try
    {
        // 1️⃣ Leggi updateappsettings.json
        std::ifstream updateFile(updateConfigPath);
        json updateConfig;
        updateFile >> updateConfig;

        // 2️⃣ Leggi appsettings.json corrente (o crea vuoto)
        json mainConfig;
        if (fs::exists(mainConfigPath))
        {
            std::ifstream mainFile(mainConfigPath);
            mainFile >> mainConfig;
        }
        else
        {
            mainConfig = json::object();
        }

        // 3️⃣ Merge ricorsivo (aggiorna solo nuovi parametri)
        for (auto& [key, value] : updateConfig.items())
        {
            // ❌ Rimozione esplicita: ignora chiavi con prefisso "RIMUOVI --> "
            if (key.find("RIMUOVI --> ") == 0)
            {
                std::string keyToRemove = key.substr(12);
                if (mainConfig.contains(keyToRemove))
                {
                    mainConfig.erase(keyToRemove);
                    Logger::LogInfo(L"CONFIG_KEY_REMOVED", 
                        L"Rimosso parametro: " + std::wstring(keyToRemove.begin(), keyToRemove.end()));
                }
                continue;
            }

            // ✅ Preserva *directory/*path se vuoti in update
            if ((key.find("directory") != std::string::npos || key.find("path") != std::string::npos) &&
                value.is_string() && value.get<std::string>().empty())
            {
                if (mainConfig.contains(key))
                {
                    Logger::LogInfo(L"CONFIG_PRESERVE_PATH", 
                        L"Preservo path esistente: " + std::wstring(key.begin(), key.end()));
                    continue; // Mantieni valore corrente
                }
            }

            // ✅ Aggiorna/Aggiungi parametro
            mainConfig[key] = value;
        }

        // 4️⃣ Salva configurazione merged
        std::ofstream outFile(mainConfigPath);
        outFile << mainConfig.dump(2); // Indentazione 2 spazi

        Logger::LogInfo(L"CONFIG_MERGE_COMPLETE", L"Configurazione merged salvata");
        return true;
    }
    catch (const std::exception& ex)
    {
        Logger::LogError(L"CONFIG_MERGE_ERROR", 
            L"Errore merge configurazione: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return false;
    }
}

bool FileOperations::ShouldSkipFile(const std::wstring& fileName)
{
    // ✅ Lista file di servizio da ignorare
    std::wstring lowerFileName = fileName;
    std::transform(lowerFileName.begin(), lowerFileName.end(), lowerFileName.begin(), ::tolower);

    return lowerFileName == L"update_in_progress.json" ||
           lowerFileName == L"updateappsettings.json" ||
           lowerFileName == L"update_manifest.json" ||
           lowerFileName.ends_with(L".log") ||
           lowerFileName.ends_with(L".tmp") ||
           lowerFileName.ends_with(L".bak");
}

std::wstring FileOperations::ExtractVersionNumber(const std::wstring& fullVersionString)
{
    // Regex: estrai pattern "X.Y.Z"
    std::wregex versionRegex(L"(\\d+\\.\\d+\\.\\d+)");
    std::wsmatch match;

    if (std::regex_search(fullVersionString, match, versionRegex))
    {
        return match[1].str();
    }

    return L"0.0.0"; // Fallback
}

bool FileOperations::IsVersionGreater(const std::wstring& version1, const std::wstring& version2)
{
    // Confronta versioni in formato "X.Y.Z"
    std::wregex versionRegex(L"(\\d+)\\.(\\d+)\\.(\\d+)");
    std::wsmatch match1, match2;

    if (!std::regex_match(version1, match1, versionRegex) || 
        !std::regex_match(version2, match2, versionRegex))
    {
        return false;
    }

    int major1 = std::stoi(match1[1].str());
    int minor1 = std::stoi(match1[2].str());
    int patch1 = std::stoi(match1[3].str());

    int major2 = std::stoi(match2[1].str());
    int minor2 = std::stoi(match2[2].str());
    int patch2 = std::stoi(match2[3].str());

    if (major1 != major2) return major1 > major2;
    if (minor1 != minor2) return minor1 > minor2;
    return patch1 > patch2;
}
