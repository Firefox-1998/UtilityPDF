#include "ConfigMerger.h"
#include <fstream>
#include <algorithm>
#include <cctype>
#include <stdexcept>

bool ConfigMerger::MergeAppSettings(
    const fs::path& appSettingsPath,
    const fs::path& updateSettingsPath
)
{
    try
    {
        Logger::LogInfo(L"CONFIG_MERGE_START", L"Inizio merge configurazione");
        Logger::LogInfo(L"MERGE_TARGET", L"File destinazione: " + appSettingsPath.wstring());
        Logger::LogInfo(L"MERGE_SOURCE", L"File istruzioni: " + updateSettingsPath.wstring());

        if (!fs::exists(appSettingsPath))
        {
            Logger::LogError(L"MERGE_TARGET_NOT_FOUND", 
                L"File appsettings.json non trovato: " + appSettingsPath.wstring());
            return false;
        }

        if (!fs::exists(updateSettingsPath))
        {
            Logger::LogWarning(L"MERGE_SOURCE_NOT_FOUND", 
                L"File updateappsettings.json non trovato, nessun merge necessario");
            return true;
        }

        // Carica appsettings.json corrente
        json currentConfig = LoadJsonFile(appSettingsPath);
        Logger::LogInfo(L"CONFIG_LOADED", 
            L"Caricato appsettings.json (" + 
            std::to_wstring(currentConfig.size()) + L" sezioni)");

        // Carica updateappsettings.json
        json updateInstructions = LoadJsonFile(updateSettingsPath);
        Logger::LogInfo(L"UPDATE_INSTRUCTIONS_LOADED", 
            L"Caricato updateappsettings.json (" + 
            std::to_wstring(updateInstructions.size()) + L" istruzioni)");

        // Applica modifiche con tracking
        bool hasChanges = false;
        ApplyConfigUpdates(currentConfig, updateInstructions, L"", hasChanges);

        // Salva SOLO se ci sono modifiche
        if (!hasChanges)
        {
            Logger::LogInfo(L"CONFIG_NO_CHANGES", L"Nessuna modifica necessaria, file non sovrascritto");
            return true;
        }

        SaveJsonFile(appSettingsPath, currentConfig);
        Logger::LogInfo(L"CONFIG_MERGE_SUCCESS", L"appsettings.json aggiornato con successo");

        return true;
    }
    catch (const json::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogError(L"MERGE_JSON_ERROR", 
            L"Errore parsing JSON: " + std::wstring(errorMsg.begin(), errorMsg.end()));
        return false;
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogError(L"MERGE_ERROR", 
            L"Errore merge configurazione: " + std::wstring(errorMsg.begin(), errorMsg.end()));
        return false;
    }
}

json ConfigMerger::LoadJsonFile(const fs::path& path)
{
    std::ifstream file(path);
    if (!file.is_open())
    {
        throw std::runtime_error("Impossibile aprire file: " + path.string());
    }

    json data;
    file >> data;
    return data;
}

void ConfigMerger::SaveJsonFile(const fs::path& path, const json& data)
{
    std::ofstream file(path);
    if (!file.is_open())
    {
        throw std::runtime_error("Impossibile scrivere file: " + path.string());
    }

    file << data.dump(4);
}

void ConfigMerger::ApplyConfigUpdates(
    json& target,
    const json& updates,
    const std::wstring& currentPath,
    bool& hasChanges
)
{
    if (!updates.is_object())
    {
        return;
    }

    for (auto& [key, value] : updates.items())
    {
        // FIX 1: Parser RIMUOVI corretto
        const std::string REMOVE_PREFIX = "RIMUOVI --> ";
        
        if (key.find(REMOVE_PREFIX) == 0)
        {
            // Estrai nome reale del parametro/sezione da rimuovere
            std::string targetKey = key.substr(REMOVE_PREFIX.length());
            
            // Trim spazi bianchi
            targetKey.erase(0, targetKey.find_first_not_of(" \t"));
            targetKey.erase(targetKey.find_last_not_of(" \t") + 1);
            
            std::wstring fullPath = currentPath.empty() 
                ? std::wstring(targetKey.begin(), targetKey.end())
                : currentPath + L"." + std::wstring(targetKey.begin(), targetKey.end());
            
            std::string actualKey;
            if (HasPropertyCaseInsensitive(target, targetKey, actualKey))
            {
                if (target[actualKey].is_object())
                {
                    Logger::LogInfo(L"SECTION_REMOVED", L"Rimossa sezione: " + fullPath);
                }
                else
                {
                    Logger::LogInfo(L"PARAM_REMOVED", L"Rimosso parametro: " + fullPath);
                }

                target.erase(actualKey);
                hasChanges = true;
            }
            else
            {
                Logger::LogWarning(L"REMOVE_TARGET_NOT_FOUND", 
                    L"Elemento da rimuovere non trovato: " + fullPath);
            }
            continue;
        }

        // Path completo per logging
        std::wstring fullPath = currentPath.empty() 
            ? std::wstring(key.begin(), key.end())
            : currentPath + L"." + std::wstring(key.begin(), key.end());

        // Cerca chiave esistente (case-insensitive)
        std::string actualKey;
        bool keyExists = HasPropertyCaseInsensitive(target, key, actualKey);

        // Protezione parametri Directory/Path
        if (keyExists && IsProtectedParameter(key, target[actualKey]))
        {
            std::string currentValue = target[actualKey].get<std::string>();
            Logger::LogInfo(L"PARAM_PROTECTED", 
                L"Parametro protetto ignorato: " + fullPath + 
                L" = \"" + std::wstring(currentValue.begin(), currentValue.end()) + L"\"");
            continue;
        }

        // Merge ricorsivo per oggetti
        if (value.is_object())
        {
            if (!keyExists)
            {
                target[key] = json::object();
                Logger::LogInfo(L"SECTION_ADDED", L"Aggiunta nuova sezione: " + fullPath);
                actualKey = key;
                hasChanges = true;
            }

            if (target[actualKey].is_object())
            {
                ApplyConfigUpdates(target[actualKey], value, fullPath, hasChanges);
            }
            else
            {
                target[actualKey] = value;
                Logger::LogWarning(L"PARAM_TYPE_CHANGED", L"Tipo parametro cambiato: " + fullPath);
                hasChanges = true;
            }
        }
        else
        {
            // FIX 2: Update selettivo con confronto valori
            if (!keyExists)
            {
                // Parametro nuovo
                target[key] = value;
                actualKey = key;
                hasChanges = true;
                
                std::string valueStr = value.is_string() 
                    ? "\"" + value.get<std::string>() + "\""
                    : value.dump();
                Logger::LogInfo(L"PARAM_ADDED", 
                    L"Aggiunto: " + fullPath + L" = " + std::wstring(valueStr.begin(), valueStr.end()));
            }
            else
            {
                // Parametro esistente - confronta valori
                if (target[actualKey] != value)
                {
                    std::string oldValueStr = target[actualKey].is_string() 
                        ? "\"" + target[actualKey].get<std::string>() + "\""
                        : target[actualKey].dump();
                    std::string newValueStr = value.is_string() 
                        ? "\"" + value.get<std::string>() + "\""
                        : value.dump();
                    
                    target[actualKey] = value;
                    hasChanges = true;
                    
                    Logger::LogInfo(L"PARAM_UPDATED", 
                        L"Aggiornato: " + fullPath + 
                        L" (da " + std::wstring(oldValueStr.begin(), oldValueStr.end()) +
                        L" a " + std::wstring(newValueStr.begin(), newValueStr.end()) + L")");
                }
                else
                {
                    // Valore uguale - non serve aggiornamento
                    std::string valueStr = value.is_string() 
                        ? "\"" + value.get<std::string>() + "\""
                        : value.dump();
                    Logger::LogInfo(L"PARAM_UNCHANGED", 
                        L"Non modificato (valore uguale): " + fullPath + 
                        L" = " + std::wstring(valueStr.begin(), valueStr.end()));
                }
            }
        }
    }
}

bool ConfigMerger::IsProtectedParameter(const std::string& paramName, const json& currentValue)
{
    if (!currentValue.is_string())
    {
        return false;
    }

    std::string valueStr = currentValue.get<std::string>();
    if (valueStr.empty())
    {
        return false;
    }

    std::string lowerName = ToLower(paramName);
    return lowerName.ends_with("directory") || lowerName.ends_with("path");
}

std::string ConfigMerger::ToLower(const std::string& str)
{
    std::string result = str;
    std::transform(result.begin(), result.end(), result.begin(),
        [](unsigned char c) { return static_cast<char>(std::tolower(c)); });
    return result;
}

bool ConfigMerger::HasPropertyCaseInsensitive(const json& obj, const std::string& key, std::string& actualKey)
{
    if (!obj.is_object())
    {
        return false;
    }

    if (obj.contains(key))
    {
        actualKey = key;
        return true;
    }

    std::string lowerKey = ToLower(key);
    for (auto& [objKey, objValue] : obj.items())
    {
        if (ToLower(objKey) == lowerKey)
        {
            actualKey = objKey;
            return true;
        }
    }

    return false;
}
