#include "ConfigReader.h"
#include "Logger.h"
#include <windows.h>
#include <fstream>
#include <sstream>
#include <filesystem>
#include <nlohmann/json.hpp>

using json = nlohmann::json;
namespace fs = std::filesystem;

ConfigReader::ConfigReader(const std::wstring& appDirectory)
    : _appDirectory(appDirectory) {
    _configPath = GetConfigPath();
}

std::wstring ConfigReader::GetConfigPath() const {
    return _appDirectory + L"\\config\\appsettings.json";
}

std::optional<std::wstring> ConfigReader::ReadJsonFile(const std::wstring& path) {
    try {
        std::ifstream file(path, std::ios::binary);
        if (!file.is_open()) {
            Logger::LogError(L"CONFIG_FILE_NOT_FOUND", 
                L"File configurazione non trovato: " + path);
            return std::nullopt;
        }

        std::ostringstream ss;
        ss << file.rdbuf();
        std::string content = ss.str();
        
        // Converti UTF-8 a wide string
        int size = MultiByteToWideChar(CP_UTF8, 0, content.c_str(), -1, nullptr, 0);
        std::wstring wideContent(size, 0);
        MultiByteToWideChar(CP_UTF8, 0, content.c_str(), -1, &wideContent[0], size);
        
        return wideContent;
    }
    catch (const std::exception& ex) {
        Logger::LogError(L"CONFIG_READ_ERROR", 
            L"Errore lettura file: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return std::nullopt;
    }
}

std::optional<ConfigReader::AppConfig> ConfigReader::LoadConfiguration() {
    auto jsonContent = ReadJsonFile(_configPath);
    if (!jsonContent.has_value()) {
        return std::nullopt;
    }

    // Converti wide string a UTF-8 per nlohmann::json
    int size = WideCharToMultiByte(CP_UTF8, 0, jsonContent->c_str(), -1, nullptr, 0, nullptr, nullptr);
    std::string utf8Content(size, 0);
    WideCharToMultiByte(CP_UTF8, 0, jsonContent->c_str(), -1, &utf8Content[0], size, nullptr, nullptr);

    return ParseConfiguration(utf8Content);
}

std::optional<ConfigReader::AppConfig> ConfigReader::ParseConfiguration(const std::string& jsonContent) {
    try {
        auto jsonObj = json::parse(jsonContent, nullptr, true, true); // allow_exceptions, ignore_comments
        
        AppConfig config;
        
        // Parsing case-insensitive per "application"
        if (jsonObj.contains("application") || jsonObj.contains("Application")) {
            auto appSection = jsonObj.contains("application") ? jsonObj["application"] : jsonObj["Application"];
            
            // updateDirectory (OBBLIGATORIO)
            if (appSection.contains("updateDirectory")) {
                std::string updateDir = appSection["updateDirectory"].get<std::string>();
                int wsize = MultiByteToWideChar(CP_UTF8, 0, updateDir.c_str(), -1, nullptr, 0);
                config.updateDirectory.resize(wsize);
                MultiByteToWideChar(CP_UTF8, 0, updateDir.c_str(), -1, &config.updateDirectory[0], wsize);
                config.updateDirectory.pop_back(); // Rimuovi null terminator
            } else if (appSection.contains("UpdateDirectory")) {
                std::string updateDir = appSection["UpdateDirectory"].get<std::string>();
                int wsize = MultiByteToWideChar(CP_UTF8, 0, updateDir.c_str(), -1, nullptr, 0);
                config.updateDirectory.resize(wsize);
                MultiByteToWideChar(CP_UTF8, 0, updateDir.c_str(), -1, &config.updateDirectory[0], wsize);
                config.updateDirectory.pop_back();
            }
            
            // applicationName (opzionale)
            if (appSection.contains("applicationName")) {
                std::string appName = appSection["applicationName"].get<std::string>();
                int wsize = MultiByteToWideChar(CP_UTF8, 0, appName.c_str(), -1, nullptr, 0);
                config.applicationName.resize(wsize);
                MultiByteToWideChar(CP_UTF8, 0, appName.c_str(), -1, &config.applicationName[0], wsize);
                config.applicationName.pop_back();
            }
            
            // version (opzionale)
            if (appSection.contains("version")) {
                std::string ver = appSection["version"].get<std::string>();
                int wsize = MultiByteToWideChar(CP_UTF8, 0, ver.c_str(), -1, nullptr, 0);
                config.version.resize(wsize);
                MultiByteToWideChar(CP_UTF8, 0, ver.c_str(), -1, &config.version[0], wsize);
                config.version.pop_back();
            }
        }
        
        // Parsing "logging" per logDirectory
        if (jsonObj.contains("logging") || jsonObj.contains("Logging")) {
            auto logSection = jsonObj.contains("logging") ? jsonObj["logging"] : jsonObj["Logging"];
            
            if (logSection.contains("logDirectory") || logSection.contains("LogDirectory")) {
                std::string logDir = logSection.contains("logDirectory") 
                    ? logSection["logDirectory"].get<std::string>()
                    : logSection["LogDirectory"].get<std::string>();
                    
                int wsize = MultiByteToWideChar(CP_UTF8, 0, logDir.c_str(), -1, nullptr, 0);
                config.logDirectory.resize(wsize);
                MultiByteToWideChar(CP_UTF8, 0, logDir.c_str(), -1, &config.logDirectory[0], wsize);
                config.logDirectory.pop_back();
            }
        }
        
        // Fallback logDirectory se vuoto
        if (config.logDirectory.empty()) {
            config.logDirectory = _appDirectory + L"\\logs";
        }
        
        // Validazione: updateDirectory DEVE esistere
        if (config.updateDirectory.empty()) {
            Logger::LogError(L"UPDATE_BLOCKED_NO_UPDATE_DIR", 
                L"⛔ AGGIORNAMENTO BLOCCATO: UpdateDirectory non configurato");
            return std::nullopt;
        }
        
        if (!fs::exists(config.updateDirectory)) {
            Logger::LogError(L"UPDATE_DIR_NOT_EXISTS", 
                L"⛔ Directory aggiornamento non esiste: " + config.updateDirectory);
            return std::nullopt;
        }
        
        Logger::LogInfo(L"CONFIG_LOADED_SUCCESSFULLY", 
            L"✅ Configurazione caricata: UpdateDir=" + config.updateDirectory);
        
        return config;
    }
    catch (const json::parse_error& ex) {
        Logger::LogError(L"JSON_PARSE_ERROR", 
            L"Errore parsing JSON: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return std::nullopt;
    }
}
