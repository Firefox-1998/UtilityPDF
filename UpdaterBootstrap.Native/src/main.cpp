#include <windows.h>
#include <string>
#include <filesystem>
#include <fstream>
#include <vector>
#include <algorithm>
#include <urlmon.h>
#include "ConfigReader.h"
#include "Logger.h"
#include "ProcessManager.h"
#include "FileOperations.h"
#include "BackupService.h"
#include "UpdateValidator.h"
#include "ArchiveManager.h"
#include "ProgressDialog.h"

#pragma comment(lib, "urlmon.lib")

namespace fs = std::filesystem;

std::wstring GetApplicationDirectory() {
    wchar_t buffer[MAX_PATH];
    GetModuleFileNameW(nullptr, buffer, MAX_PATH);
    fs::path exePath(buffer);
    return exePath.parent_path().wstring();
}

void ShowErrorDialog(const std::wstring& message) {
    MessageBoxW(nullptr, message.c_str(),
               L"UpdaterBootstrap - Error",
               MB_OK | MB_ICONERROR | MB_TOPMOST | MB_SETFOREGROUND);
}

void ShowInfoDialog(const std::wstring& message) {
    MessageBoxW(nullptr, message.c_str(),
               L"UpdaterBootstrap",
               MB_OK | MB_ICONINFORMATION | MB_TOPMOST | MB_SETFOREGROUND);
}

int ShowTopMostMessageBox(const wchar_t* message, const wchar_t* title, UINT type)
{
    HWND tempOwner = CreateWindowExW(
        WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
        L"STATIC", L"", WS_POPUP,
        0, 0, 0, 0,
        nullptr, nullptr, GetModuleHandle(nullptr), nullptr
    );

    int result = MessageBoxW(tempOwner, message, title, type | MB_TOPMOST | MB_SETFOREGROUND);

    if (tempOwner)
    {
        DestroyWindow(tempOwner);
    }

    return result;
}

void CleanupUpdateMarkerFiles(const std::wstring& appDir)
{
    try
    {
        fs::path configDir = fs::path(appDir) / L"config";

        std::vector<std::wstring> markerFiles = {
            (configDir / L"update_in_progress.json").wstring(),
            (configDir / L"update_failed.json").wstring()
        };

        for (const auto& markerFile : markerFiles)
        {
            if (fs::exists(markerFile))
            {
                try
                {
                    fs::remove(markerFile);
                    Logger::LogInfo(L"MARKER_FILE_DELETED",
                        L"Update marker file deleted: " + fs::path(markerFile).filename().wstring());
                }
                catch (const std::exception& ex)
                {
                    std::string errorMsg = ex.what();
                    Logger::LogWarning(L"MARKER_FILE_DELETE_ERROR",
                        L"Cannot delete marker file: " + fs::path(markerFile).filename().wstring() +
                        L" - " + std::wstring(errorMsg.begin(), errorMsg.end()));
                }
            }
        }
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogWarning(L"CLEANUP_MARKERS_ERROR",
            L"Error cleaning marker files: " + std::wstring(errorMsg.begin(), errorMsg.end()));
    }
}

/// <summary>
/// Downloads a file from a URL to a local path using URLDownloadToFile
/// </summary>
bool DownloadFile(const std::wstring& url, const std::wstring& destPath)
{
    Logger::LogInfo(L"DOWNLOAD_START", L"Downloading: " + url);

    HRESULT hr = URLDownloadToFileW(nullptr, url.c_str(), destPath.c_str(), 0, nullptr);

    if (SUCCEEDED(hr))
    {
        Logger::LogInfo(L"DOWNLOAD_OK", L"Downloaded to: " + destPath);
        return true;
    }

    Logger::LogError(L"DOWNLOAD_FAILED",
        L"Download failed (HRESULT: " + std::to_wstring(hr) + L"): " + url);
    return false;
}

/// <summary>
/// Reads all bytes from a file into a vector
/// </summary>
std::vector<uint8_t> ReadFileBytes(const std::wstring& filePath)
{
    std::ifstream file(filePath, std::ios::binary | std::ios::ate);
    if (!file.is_open())
    {
        return {};
    }

    std::streamsize size = file.tellg();
    file.seekg(0, std::ios::beg);

    std::vector<uint8_t> buffer(static_cast<size_t>(size));
    file.read(reinterpret_cast<char*>(buffer.data()), size);

    return buffer;
}

/// <summary>
/// Reads the first line of a text file and extracts the hex hash
/// </summary>
std::wstring ReadHashFile(const std::wstring& filePath)
{
    std::wifstream file(filePath);
    if (!file.is_open())
    {
        return L"";
    }

    std::wstring line;
    std::getline(file, line);

    size_t spacePos = line.find(L' ');
    if (spacePos != std::wstring::npos)
    {
        line = line.substr(0, spacePos);
    }

    return line;
}

/// <summary>
/// Copies extracted files from extractedDir to appDir, preserving relative paths.
/// Skips service files (updateappsettings.json, markers, logs, signatures).
/// Returns the number of files updated, skipped, and errors.
/// </summary>
struct ApplyResult
{
    bool success = false;
    std::wstring message;
    int filesUpdated = 0;
    int filesSkipped = 0;
    int filesError = 0;
};

ApplyResult ApplyExtractedFiles(
    const std::wstring& appDir,
    const std::wstring& extractedDir)
{
    ApplyResult result;

    try
    {
        Logger::LogInfo(L"APPLY_START", L"Applying extracted files to: " + appDir);

        for (const auto& entry : fs::recursive_directory_iterator(extractedDir))
        {
            if (!entry.is_regular_file())
            {
                continue;
            }

            fs::path relativePath = fs::relative(entry.path(), extractedDir);
            fs::path destPath = fs::path(appDir) / relativePath;
            std::wstring fileName = entry.path().filename().wstring();

            // Skip service files
            std::wstring lowerName = fileName;
            std::transform(lowerName.begin(), lowerName.end(), lowerName.begin(), ::towlower);

            if (lowerName == L"updateappsettings.json" ||
                lowerName == L"update_in_progress.json" ||
                lowerName == L"update_manifest.json" ||
                lowerName.ends_with(L".log") ||
                lowerName.ends_with(L".tmp") ||
                lowerName.ends_with(L".bak") ||
                lowerName.ends_with(L".sha256") ||
                lowerName.ends_with(L".sig"))
            {
                result.filesSkipped++;
                Logger::LogInfo(L"FILE_SKIPPED", L"Skipped service file: " + fileName);
                continue;
            }

            try
            {
                fs::path destDir = destPath.parent_path();
                if (!fs::exists(destDir))
                {
                    fs::create_directories(destDir);
                }

                fs::copy_file(entry.path(), destPath, fs::copy_options::overwrite_existing);
                result.filesUpdated++;

                Logger::LogInfo(L"FILE_UPDATED", L"Updated: " + relativePath.wstring());
            }
            catch (const std::exception& ex)
            {
                result.filesError++;
                std::string errorMsg = ex.what();
                Logger::LogError(L"FILE_UPDATE_ERROR",
                    L"Error updating file: " + relativePath.wstring() +
                    L" - " + std::wstring(errorMsg.begin(), errorMsg.end()));
            }
        }

        result.success = (result.filesError == 0);
        result.message = result.success
            ? L"Update applied successfully"
            : L"Update completed with errors";

        Logger::LogInfo(L"APPLY_COMPLETE",
            L"Updated: " + std::to_wstring(result.filesUpdated) +
            L", Skipped: " + std::to_wstring(result.filesSkipped) +
            L", Errors: " + std::to_wstring(result.filesError));
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        result.success = false;
        result.message = L"Critical error applying update: " +
            std::wstring(errorMsg.begin(), errorMsg.end());
        Logger::LogError(L"APPLY_EXCEPTION", result.message);
    }

    return result;
}

int WINAPI wWinMain(
    _In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR lpCmdLine,
    _In_ int nShowCmd)
{
    // 1. Get application directory
    std::wstring appDir = GetApplicationDirectory();

    // 2. Load configuration
    ConfigReader configReader(appDir);
    auto configOpt = configReader.LoadConfiguration();

    if (!configOpt.has_value()) {
        ShowErrorDialog(
            L"Configuration not valid.\n\n"
            L"Check config\\appsettings.json"
        );
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    auto config = configOpt.value();

    // 3. Initialize logger
    Logger::Initialize(config.logDirectory);
    Logger::LogInfo(L"BOOTSTRAP_START", L"UpdaterBootstrap.Native started");
    Logger::LogInfo(L"APP_DIRECTORY", appDir);

    // 4. Validate application directory
    if (!UpdateValidator::ValidateApplicationDirectory(appDir)) {
        ShowErrorDialog(L"Invalid application directory.");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    // 5. Security check (Marker + Parent process)
    fs::path markerFile = fs::path(appDir) / L"config" / L"update_in_progress.json";
    bool markerExists = fs::exists(markerFile);
    bool validParent = ProcessManager::IsLaunchedByMainApp();

    if (!markerExists || !validParent)
    {
        std::wstring errorMsg =
            L"UpdaterBootstrap can only be started by UtilityPDF.exe\n\n"
            L"Security check:\n";

        errorMsg += L"  Marker file: ";
        errorMsg += markerExists ? L"OK" : L"MISSING";
        errorMsg += L"\n";
        errorMsg += L"  Parent process: ";
        errorMsg += validParent ? L"OK" : L"UNAUTHORIZED";

        ShowErrorDialog(errorMsg);

        Logger::LogError(L"SECURITY_CHECK_FAILED",
            L"Security check failed - Marker: " +
            std::wstring(markerExists ? L"OK" : L"FAIL") +
            L", Parent: " +
            std::wstring(validParent ? L"OK" : L"FAIL"));

        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    Logger::LogInfo(L"SECURITY_CHECK_PASSED", L"Security check completed successfully");

    // 6. Load and validate update marker (download URLs)
    auto markerInfoOpt = configReader.LoadUpdateMarker();
    if (!markerInfoOpt.has_value())
    {
        ShowErrorDialog(
            L"Update marker file is invalid or contains untrusted URLs.\n"
            L"The update cannot proceed.");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    auto markerInfo = markerInfoOpt.value();
    Logger::LogInfo(L"MARKER_VALIDATED",
        L"Update: v" + markerInfo.currentVersion + L" -> v" + markerInfo.newVersion);

    // 7. Wait for main app to close
    std::wstring mainAppExe = L"UtilityPDF.exe";
    if (ProcessManager::IsProcessRunning(mainAppExe)) {
        Logger::LogInfo(L"WAIT_APP_CLOSE", L"Waiting for " + mainAppExe + L" to close");
        ProcessManager::WaitForProcessTermination(mainAppExe);
    }

    // 8. User confirmation
    std::wstring confirmMessage =
        L"Update available\n\n"
        L"Current version: " + markerInfo.currentVersion + L"\n"
        L"New version: " + markerInfo.newVersion + L"\n\n"
        L"Proceed with the update?";

    int userChoice = ShowTopMostMessageBox(confirmMessage.c_str(),
                                           L"Confirm Update",
                                           MB_YESNO | MB_ICONQUESTION);

    if (userChoice != IDYES) {
        Logger::LogInfo(L"UPDATE_CANCELLED", L"Update cancelled by user");
        CleanupUpdateMarkerFiles(appDir);
        return 0;
    }

    // 9. Create progress dialog
    ProgressDialog progressDlg;
    if (!progressDlg.Create(hInstance))
    {
        ShowErrorDialog(L"Cannot create progress window");
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    progressDlg.SetVersionInfo(markerInfo.currentVersion, markerInfo.newVersion);

    // ========================================================================
    // 10. DOWNLOAD archive + hash + signature from GitHub Release
    // ========================================================================
    progressDlg.SetStatus(L"Downloading update archive...");
    progressDlg.SetProgress(5);

    fs::path tempDir = fs::path(appDir) / L"temp_update";
    if (fs::exists(tempDir))
    {
        fs::remove_all(tempDir);
    }
    fs::create_directories(tempDir);

    std::wstring archivePath = (tempDir / L"update_archive.7z").wstring();
    std::wstring hashPath = (tempDir / L"update_archive.sha256").wstring();
    std::wstring sigPath = (tempDir / L"update_archive.sig").wstring();

    bool downloadOk = true;

    progressDlg.SetStatus(L"Downloading archive...");
    if (!DownloadFile(markerInfo.archiveUrl, archivePath))
    {
        downloadOk = false;
    }

    progressDlg.SetProgress(10);
    progressDlg.SetStatus(L"Downloading hash file...");
    if (!DownloadFile(markerInfo.hashUrl, hashPath))
    {
        downloadOk = false;
    }

    progressDlg.SetProgress(15);
    progressDlg.SetStatus(L"Downloading signature file...");
    if (!DownloadFile(markerInfo.signatureUrl, sigPath))
    {
        downloadOk = false;
    }

    if (!downloadOk)
    {
        Logger::LogError(L"DOWNLOAD_FAILED", L"One or more downloads failed");
        progressDlg.ShowError(L"Download failed");
        ShowTopMostMessageBox(
            L"Failed to download update files from GitHub.\n"
            L"Please check your internet connection and try again.",
            L"Download Error", MB_OK | MB_ICONERROR);
        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    progressDlg.SetProgress(20);

    // ========================================================================
    // 11. VERIFY archive integrity (SHA-256 + RSA signature)
    // ========================================================================
    progressDlg.SetStatus(L"Verifying archive signature...");
    progressDlg.SetProgress(25);

    std::wstring expectedHash = ReadHashFile(hashPath);
    std::vector<uint8_t> signatureBytes = ReadFileBytes(sigPath);

    if (expectedHash.empty() || signatureBytes.empty())
    {
        Logger::LogError(L"VERIFY_FILES_INVALID", L"Hash or signature file is empty/invalid");
        progressDlg.ShowError(L"Invalid hash or signature file");
        ShowTopMostMessageBox(
            L"Hash file or signature file is empty or invalid.\n"
            L"The update cannot be verified and will not proceed.",
            L"Security Error", MB_OK | MB_ICONERROR);
        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    auto verifyResult = UpdateValidator::VerifyArchiveFull(archivePath, expectedHash, signatureBytes);

    if (!verifyResult.IsValid())
    {
        Logger::LogError(L"ARCHIVE_VERIFICATION_FAILED", verifyResult.errorMessage);
        progressDlg.ShowError(L"Archive verification failed!");

        std::wstring securityMsg =
            L"SECURITY: The downloaded archive failed verification.\n\n" +
            verifyResult.errorMessage +
            L"\n\nThe update will NOT proceed.\n"
            L"Please download the update manually from GitHub.";

        ShowTopMostMessageBox(securityMsg.c_str(), L"Security Error", MB_OK | MB_ICONERROR);

        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    Logger::LogInfo(L"ARCHIVE_VERIFIED", L"Archive passed SHA-256 + RSA verification");
    progressDlg.SetProgress(30);

    // ========================================================================
    // 12. EXTRACT .7z archive to temp directory
    // ========================================================================
    progressDlg.SetStatus(L"Extracting update archive...");
    progressDlg.SetProgress(35);

    fs::path extractDir = tempDir / L"extracted";

    ArchiveManager::ArchiveResult extractResult = ArchiveManager::Extract7z(
        archivePath, extractDir.wstring(),
        [&progressDlg](int percent) {
            // Map 7z extraction progress (0-100) to our range (35-50)
            int mapped = 35 + (percent * 15 / 100);
            progressDlg.SetProgress(mapped);
        });

    if (!extractResult.success)
    {
        Logger::LogError(L"EXTRACT_FAILED", extractResult.message);
        progressDlg.ShowError(L"Archive extraction failed");
        ShowTopMostMessageBox(
            (L"Failed to extract the update archive:\n" + extractResult.message).c_str(),
            L"Extraction Error", MB_OK | MB_ICONERROR);
        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    Logger::LogInfo(L"EXTRACT_COMPLETE",
        L"Extracted " + std::to_wstring(extractResult.filesProcessed) + L" files (" +
        std::to_wstring(extractResult.totalSize / 1024) + L" KB)");
    progressDlg.SetProgress(50);

    // ========================================================================
    // 13. SELECTIVE BACKUP: only files that exist in the .7z
    //     (i.e., only files that will actually be overwritten)
    // ========================================================================
    progressDlg.SetStatus(L"Creating backup: pre-update_v" + markerInfo.currentVersion + L"...");
    progressDlg.SetProgress(55);

    std::wstring backupDir = appDir + L"\\backups";

    // Use extractDir as updateDir — this way GetFilesToUpdate() enumerates
    // exactly the files from the .7z, and the backup only saves the
    // corresponding current files that will be overwritten.
    auto backupResult = BackupService::CreateVersionedBackup(
        appDir, backupDir, extractDir.wstring(), markerInfo.currentVersion);

    if (!backupResult.success) {
        progressDlg.ShowError(L"Backup creation error");
        ShowTopMostMessageBox((L"Backup creation error:\n" + backupResult.message).c_str(),
                              L"Error",
                              MB_OK | MB_ICONERROR);
        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);
        return 1;
    }

    Logger::LogInfo(L"BACKUP_CREATED", L"Backup saved: " + backupResult.backupPath);
    progressDlg.SetProgress(65);

    // ========================================================================
    // 14. APPLY extracted files to application directory
    // ========================================================================
    progressDlg.SetStatus(L"Applying update files...");
    progressDlg.SetProgress(70);

    ApplyResult applyResult = ApplyExtractedFiles(appDir, extractDir.wstring());

    progressDlg.SetProgress(85);

    if (!applyResult.success) {
        Logger::LogError(L"UPDATE_FAILED",
            L"Update failed (" + std::to_wstring(applyResult.filesError) + L" errors) — restoring backup");
        progressDlg.ShowError(L"Update failed. Restoring backup...");

        BackupService::RestoreBackup(backupResult.backupPath, appDir);
        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);

        Sleep(3000);
        return 1;
    }

    // ========================================================================
    // 15. POST-UPDATE integrity check
    // ========================================================================
    progressDlg.SetStatus(L"Verifying update integrity...");
    progressDlg.SetProgress(90);

    if (!UpdateValidator::VerifyUpdateIntegrity(appDir)) {
        Logger::LogError(L"INTEGRITY_CHECK_FAILED", L"Post-update integrity check failed");
        progressDlg.ShowError(L"Integrity check failed. Restoring backup...");

        BackupService::RestoreBackup(backupResult.backupPath, appDir);
        fs::remove_all(tempDir);
        CleanupUpdateMarkerFiles(appDir);
        Sleep(3000);
        return 1;
    }

    // ========================================================================
    // 16. SUCCESS — cleanup and restart
    // ========================================================================
    progressDlg.SetProgress(100);
    progressDlg.ShowSuccess(L"Update completed successfully!");

    Logger::LogInfo(L"UPDATE_SUCCESS", L"Update completed: " +
        markerInfo.currentVersion + L" -> " + markerInfo.newVersion +
        L" (" + std::to_wstring(applyResult.filesUpdated) + L" files updated)");

    // Cleanup temp files
    try
    {
        fs::remove_all(tempDir);
    }
    catch (...) {}

    CleanupUpdateMarkerFiles(appDir);

    Sleep(2000);

    std::wstring completionMessage =
        L"Update completed!\n\n"
        L"Version: " + markerInfo.newVersion + L"\n\n"
        L"The application will now restart.";

    ShowTopMostMessageBox(
        completionMessage.c_str(),
        L"Update Completed",
        MB_OK | MB_ICONINFORMATION);

    // 17. Restart application
    std::wstring mainAppPath = appDir + L"\\" + mainAppExe;

    if (fs::exists(mainAppPath)) {
        ProcessManager::LaunchProcess(mainAppPath);
    }

    progressDlg.Close();
    return 0;
}
