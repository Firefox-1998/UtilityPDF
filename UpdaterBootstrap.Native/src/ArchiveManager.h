#pragma once
#include <string>
#include <vector>
#include <filesystem>
#include <functional>
#include <cstdint>

namespace fs = std::filesystem;

/// <summary>
/// Archive management for update operations.
/// - Extraction of .7z archives (GitHub releases): 7-Zip LZMA SDK (pure C)
/// - Compression/extraction of .zip archives (backups): minizip + zlib (pure C)
/// Zero external DLL dependencies — all compiled statically.
/// </summary>
class ArchiveManager
{
public:
    struct ArchiveResult
    {
        bool success = false;
        std::wstring message;
        int filesProcessed = 0;
        uintmax_t totalSize = 0;
    };

    using ProgressCallback = std::function<void(int percent)>;

    // ===== 7z (for GitHub release extraction) =====

    /// <summary>
    /// Extracts a standard .7z archive to destDir.
    /// Uses 7z SDK SzArEx API.
    /// </summary>
    static ArchiveResult Extract7z(
        const std::wstring& archivePath,
        const std::wstring& destDir,
        ProgressCallback progressCallback = nullptr);

    // ===== ZIP (for backup compression/extraction) =====

    /// <summary>
    /// Creates a .zip archive from a directory.
    /// Uses minizip (zlib). Preserves relative paths.
    /// </summary>
    static ArchiveResult CompressZip(
        const std::wstring& sourceDir,
        const std::wstring& zipPath,
        ProgressCallback progressCallback = nullptr);

    /// <summary>
    /// Extracts a .zip archive to destDir.
    /// Uses minizip (zlib).
    /// </summary>
    static ArchiveResult ExtractZip(
        const std::wstring& zipPath,
        const std::wstring& destDir,
        ProgressCallback progressCallback = nullptr);

    /// <summary>
    /// Checks if a file is a supported archive (.7z or .zip)
    /// </summary>
    static bool IsSupportedArchive(const std::wstring& filePath);

private:
    static bool EnsureDirectoryExists(const std::wstring& filePath);
    static void InitCrc();
};
