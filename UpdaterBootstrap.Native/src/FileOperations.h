#pragma once
#include <string>
#include <vector>
#include <optional>
#include <filesystem>

namespace fs = std::filesystem;

/// <summary>
/// 📁 Gestione operazioni su file per aggiornamenti
/// </summary>
class FileOperations
{
public:
    struct UpdateInfo
    {
        std::wstring currentVersion;
        std::wstring newVersion;
        std::wstring updateDirectory;
        std::wstring updateConfigPath;
    };

    struct UpdateResult
    {
        bool success;
        std::wstring message;
        int filesUpdated;
        int filesSkipped;
        int filesError;
    };

    /// <summary>
    /// 🔍 Cerca file updateappsettings.json
    /// </summary>
    static std::optional<std::wstring> FindUpdateConfigPath(const std::wstring& updateDir);

    /// <summary>
    /// 🔍 Verifica disponibilità aggiornamento
    /// </summary>
    static std::optional<UpdateInfo> CheckForUpdate(
        const std::wstring& appDir,
        const std::wstring& updateDir,
        const std::wstring& updateConfigPath);

    /// <summary>
    /// 🔄 Applica aggiornamento completo
    /// </summary>
    static UpdateResult ApplyCompleteUpdate(
        const std::wstring& appDir,
        const std::wstring& updateDir,
        const UpdateInfo& updateInfo);

    /// <summary>
    /// 📋 Merge configurazione JSON
    /// </summary>
    static bool MergeConfigurationFile(
        const std::wstring& updateConfigPath,
        const std::wstring& mainConfigPath);

private:
    /// <summary>
    /// 🔍 Determina se file deve essere saltato
    /// </summary>
    static bool ShouldSkipFile(const std::wstring& fileName);

    /// <summary>
    /// 📋 Estrae numero versione da stringa
    /// </summary>
    static std::wstring ExtractVersionNumber(const std::wstring& fullVersionString);

    /// <summary>
    /// 📊 Confronta due versioni
    /// </summary>
    static bool IsVersionGreater(const std::wstring& version1, const std::wstring& version2);
};
