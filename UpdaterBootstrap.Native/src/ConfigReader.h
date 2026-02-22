#pragma once
#include <string>
#include <optional>

class ConfigReader {
public:
    struct AppConfig {
        std::wstring updateDirectory;
        std::wstring logDirectory;
        std::wstring applicationName;
        std::wstring version;
    };

    explicit ConfigReader(const std::wstring& appDirectory);
    
    std::optional<AppConfig> LoadConfiguration();
    
private:
    std::wstring _appDirectory;
    std::wstring _configPath;
    
    std::wstring GetConfigPath() const;
    std::optional<std::wstring> ReadJsonFile(const std::wstring& path);
    std::optional<AppConfig> ParseConfiguration(const std::string& jsonContent);
};
