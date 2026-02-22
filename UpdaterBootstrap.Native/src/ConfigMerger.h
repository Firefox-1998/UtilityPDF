#pragma once

#include <filesystem>
#include <string>
#include <nlohmann/json.hpp>
#include "Logger.h"

namespace fs = std::filesystem;
using json = nlohmann::json;

/// <summary>
/// Gestisce il merge delle configurazioni tra appsettings.json e updateappsettings.json
/// </summary>
class ConfigMerger
{
public:
    /// <summary>
    /// Applica le modifiche da updateappsettings.json a appsettings.json
    /// </summary>
    static bool MergeAppSettings(
        const fs::path& appSettingsPath,
        const fs::path& updateSettingsPath
    );

private:
    /// <summary>
    /// Carica file JSON da disco
    /// </summary>
    static json LoadJsonFile(const fs::path& path);

    /// <summary>
    /// Salva file JSON su disco con formattazione
    /// </summary>
    static void SaveJsonFile(const fs::path& path, const json& data);

    /// <summary>
    /// Applica ricorsivamente le modifiche con tracking delle modifiche
    /// </summary>
    /// <param name="target">JSON di destinazione da modificare</param>
    /// <param name="updates">JSON con le istruzioni di aggiornamento</param>
    /// <param name="currentPath">Path corrente per logging (uso ricorsivo)</param>
    /// <param name="hasChanges">Flag di output che indica se sono state apportate modifiche</param>
    static void ApplyConfigUpdates(
        json& target,
        const json& updates,
        const std::wstring& currentPath,
        bool& hasChanges
    );

    /// <summary>
    /// Verifica se un parametro è protetto (termina con Directory o Path e ha valore non vuoto)
    /// </summary>
    static bool IsProtectedParameter(const std::string& paramName, const json& currentValue);

    /// <summary>
    /// Converte stringa in minuscolo
    /// </summary>
    static std::string ToLower(const std::string& str);

    /// <summary>
    /// Cerca property in JSON con ricerca case-insensitive
    /// </summary>
    /// <param name="obj">Oggetto JSON in cui cercare</param>
    /// <param name="key">Chiave da cercare (case-insensitive)</param>
    /// <param name="actualKey">Output: chiave reale trovata nell'oggetto</param>
    /// <returns>true se la proprietà è stata trovata, false altrimenti</returns>
    static bool HasPropertyCaseInsensitive(const json& obj, const std::string& key, std::string& actualKey);
};
