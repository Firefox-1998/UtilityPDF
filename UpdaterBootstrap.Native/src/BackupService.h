#pragma once
#include <string>
#include <filesystem>
#include <vector>
#include <optional>
#include <chrono>

namespace fs = std::filesystem;

/// <summary>
/// Selective backup management for updates.
/// Creates versioned zip backups of files that will be overwritten.
/// </summary>
class BackupService
{
public:
    struct BackupResult
    {
        bool success;
        std::wstring message;
        std::wstring backupPath;
        uintmax_t backupSize;
    };

    /// <summary>
    /// Creates a versioned backup of files that will be overwritten.
    /// Backup is saved as: backups/pre-update_v{currentVersion}.zip
    /// </summary>
    static BackupResult CreateVersionedBackup(
        const std::wstring& appDir,
        const std::wstring& backupDir,
        const std::wstring& updateDir,
        const std::wstring& currentVersion
    );

    /// <summary>
    /// Selective backup of only the files that will be updated (legacy)
    /// </summary>
    static BackupResult CreateSelectiveBackup(
        const std::wstring& appDir,
        const std::wstring& backupDir,
        const std::wstring& updateDir
    );

    /// <summary>
    /// Restores a backup (supports both zip and directory backups)
    /// </summary>
    static bool RestoreBackup(
        const std::wstring& backupPath,
        const std::wstring& appDir
    );

    /// <summary>
    /// Cleans up old backups beyond retention period
    /// </summary>
    static void CleanOldBackups(
        const std::wstring& backupDir,
        int retentionDays = 30
    );

private:
    static std::vector<fs::path> GetFilesToUpdate(const std::wstring& updateDir);
    static bool ShouldExcludeFromBackup(const std::wstring& path);
    static std::optional<std::chrono::system_clock::time_point>
        ExtractDateFromBackupName(const std::wstring& fileName);

    /// <summary>
    /// Creates a zip archive from a directory using PowerShell Compress-Archive
    /// </summary>
    static bool CreateZipFromDirectory(const std::wstring& sourceDir, const std::wstring& zipPath);
};
