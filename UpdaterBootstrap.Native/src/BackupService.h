#pragma once
#include <string>
#include <filesystem>
#include <vector>
#include <optional>
#include <chrono>

namespace fs = std::filesystem;

/// <summary>
/// Gestione backup selettivo per aggiornamenti
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
    /// Backup selettivo solo dei file che verranno aggiornati
    /// </summary>
    static BackupResult CreateSelectiveBackup(
        const std::wstring& appDir,
        const std::wstring& backupDir,
        const std::wstring& updateDir
    );

    /// <summary>
    /// Ripristina backup
    /// </summary>
    static bool RestoreBackup(
        const std::wstring& backupPath,
        const std::wstring& appDir
    );

    /// <summary>
    /// Pulizia backup vecchi
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
};
