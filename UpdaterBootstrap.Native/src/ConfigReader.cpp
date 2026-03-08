#include "ConfigReader.h"
#include "Logger.h"
#include <windows.h>
#include <fstream>
#include <sstream>
#include <filesystem>
#include <algorithm>
#include <regex>
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

std::wstring ConfigReader::Utf8ToWide(const std::string& utf8)
{
    if (utf8.empty())
    {
        return L"";
    }

    int size = MultiByteToWideChar(CP_UTF8, 0, utf8.c_str(), -1, nullptr, 0);
    std::wstring wide(size, 0);
    MultiByteToWideChar(CP_UTF8, 0, utf8.c_str(), -1, &wide[0], size);
    wide.pop_back(); // Remove null terminator
    return wide;
}

std::wstring ConfigReader::ExtractHostFromUrl(const std::wstring& url)
{
    // Extract host from URL: https://host/path
    // Find "://" then the next "/"
    size_t schemeEnd = url.find(L"://");
    if (schemeEnd == std::wstring::npos)
    {
        return L"";
    }

    size_t hostStart = schemeEnd + 3;
    size_t hostEnd = url.find(L'/', hostStart);
    if (hostEnd == std::wstring::npos)
    {
        hostEnd = url.length();
    }

    std::wstring host = url.substr(hostStart, hostEnd - hostStart);

    // Remove port if present
    size_t portPos = host.find(L':');
    if (portPos != std::wstring::npos)
    {
        host = host.substr(0, portPos);
    }

    // Convert to lowercase
    std::transform(host.begin(), host.end(), host.begin(), ::towlower);

    return host;
}

bool ConfigReader::IsValidGitHubUrl(const std::wstring& url)
{
    if (url.empty())
    {
        return false;
    }

    // Must start with https://
    std::wstring lowerUrl = url;
    std::transform(lowerUrl.begin(), lowerUrl.end(), lowerUrl.begin(), ::towlower);

    if (lowerUrl.find(L"https://") != 0)
    {
        Logger::LogError(L"URL_NOT_HTTPS", L"URL does not use HTTPS: " + url);
        return false;
    }

    // Host must be github.com
    std::wstring host = ExtractHostFromUrl(lowerUrl);
    if (host != L"github.com")
    {
        Logger::LogError(L"URL_NOT_GITHUB", L"URL host is not github.com: " + host);
        return false;
    }

    // Must match pattern: https://github.com/{owner}/{repo}
    std::wregex pattern(L"^https://github\\.com/[A-Za-z0-9_.\\-]+/[A-Za-z0-9_.\\-]+/?$",
        std::regex_constants::icase);

    if (!std::regex_match(url, pattern))
    {
        Logger::LogError(L"URL_INVALID_FORMAT", L"URL does not match GitHub repo format: " + url);
        return false;
    }

    return true;
}

bool ConfigReader::IsValidGitHubDownloadUrl(const std::wstring& url)
{
    if (url.empty())
    {
        return false;
    }

    // Must start with https://
    std::wstring lowerUrl = url;
    std::transform(lowerUrl.begin(), lowerUrl.end(), lowerUrl.begin(), ::towlower);

    if (lowerUrl.find(L"https://") != 0)
    {
        Logger::LogError(L"DOWNLOAD_URL_NOT_HTTPS", L"Download URL does not use HTTPS: " + url);
        return false;
    }

    // Host must be github.com or objects.githubusercontent.com
    std::wstring host = ExtractHostFromUrl(lowerUrl);

    if (host != L"github.com" && host != L"objects.githubusercontent.com")
    {
        Logger::LogError(L"DOWNLOAD_URL_UNTRUSTED_HOST",
            L"Download URL host is not trusted: " + host +
            L" (allowed: github.com, objects.githubusercontent.com)");
        return false;
    }

    return true;
}

std::optional<std::wstring> ConfigReader::ReadJsonFile(const std::wstring& path) {
    try {
        std::ifstream file(path, std::ios::binary);
        if (!file.is_open()) {
            Logger::LogError(L"CONFIG_FILE_NOT_FOUND",
                L"Configuration file not found: " + path);
            return std::nullopt;
        }

        std::ostringstream ss;
        ss << file.rdbuf();
        std::string content = ss.str();

        return Utf8ToWide(content);
    }
    catch (const std::exception& ex) {
        Logger::LogError(L"CONFIG_READ_ERROR",
            L"Error reading file: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return std::nullopt;
    }
}

std::optional<ConfigReader::AppConfig> ConfigReader::LoadConfiguration() {
    auto jsonContent = ReadJsonFile(_configPath);
    if (!jsonContent.has_value()) {
        return std::nullopt;
    }

    int size = WideCharToMultiByte(CP_UTF8, 0, jsonContent->c_str(), -1, nullptr, 0, nullptr, nullptr);
    std::string utf8Content(size, 0);
    WideCharToMultiByte(CP_UTF8, 0, jsonContent->c_str(), -1, &utf8Content[0], size, nullptr, nullptr);

    return ParseConfiguration(utf8Content);
}

std::optional<ConfigReader::AppConfig> ConfigReader::ParseConfiguration(const std::string& jsonContent) {
    try {
        auto jsonObj = json::parse(jsonContent, nullptr, true, true);

        AppConfig config;

        if (jsonObj.contains("application") || jsonObj.contains("Application")) {
            auto appSection = jsonObj.contains("application") ? jsonObj["application"] : jsonObj["Application"];

            // updateDirectory
            if (appSection.contains("updateDirectory")) {
                config.updateDirectory = Utf8ToWide(appSection["updateDirectory"].get<std::string>());
            } else if (appSection.contains("UpdateDirectory")) {
                config.updateDirectory = Utf8ToWide(appSection["UpdateDirectory"].get<std::string>());
            }

            // updateUrl (GitHub repository URL)
            if (appSection.contains("updateURL")) {
                config.updateUrl = Utf8ToWide(appSection["updateURL"].get<std::string>());
            } else if (appSection.contains("UpdateURL")) {
                config.updateUrl = Utf8ToWide(appSection["UpdateURL"].get<std::string>());
            }

            // applicationName
            if (appSection.contains("applicationName") || appSection.contains("ApplicationName")) {
                std::string key = appSection.contains("applicationName") ? "applicationName" : "ApplicationName";
                config.applicationName = Utf8ToWide(appSection[key].get<std::string>());
            }

            // version
            if (appSection.contains("version") || appSection.contains("Version")) {
                std::string key = appSection.contains("version") ? "version" : "Version";
                config.version = Utf8ToWide(appSection[key].get<std::string>());
            }
        }

        // Logging section
        if (jsonObj.contains("logging") || jsonObj.contains("Logging")) {
            auto logSection = jsonObj.contains("logging") ? jsonObj["logging"] : jsonObj["Logging"];

            if (logSection.contains("logDirectory") || logSection.contains("LogDirectory")) {
                std::string key = logSection.contains("logDirectory") ? "logDirectory" : "LogDirectory";
                config.logDirectory = Utf8ToWide(logSection[key].get<std::string>());
            }
        }

        if (config.logDirectory.empty()) {
            config.logDirectory = _appDirectory + L"\\logs";
        }

        // Validate UpdateURL security
        if (!config.updateUrl.empty() && !IsValidGitHubUrl(config.updateUrl))
        {
            Logger::LogError(L"CONFIG_INVALID_UPDATE_URL",
                L"UpdateURL failed security validation — cleared: " + config.updateUrl);
            config.updateUrl.clear();
        }

        Logger::LogInfo(L"CONFIG_LOADED_SUCCESSFULLY",
            L"Configuration loaded: UpdateDir=" + config.updateDirectory +
            L", UpdateURL=" + (config.updateUrl.empty() ? L"(not set)" : config.updateUrl));

        return config;
    }
    catch (const json::parse_error& ex) {
        Logger::LogError(L"JSON_PARSE_ERROR",
            L"JSON parse error: " + std::wstring(ex.what(), ex.what() + strlen(ex.what())));
        return std::nullopt;
    }
}

std::optional<ConfigReader::UpdateMarkerInfo> ConfigReader::LoadUpdateMarker()
{
    try
    {
        std::wstring markerPath = _appDirectory + L"\\config\\update_in_progress.json";

        if (!fs::exists(markerPath))
        {
            Logger::LogWarning(L"MARKER_NOT_FOUND", L"Update marker file not found: " + markerPath);
            return std::nullopt;
        }

        std::ifstream file(markerPath, std::ios::binary);
        if (!file.is_open())
        {
            Logger::LogError(L"MARKER_OPEN_FAILED", L"Cannot open marker file: " + markerPath);
            return std::nullopt;
        }

        std::ostringstream ss;
        ss << file.rdbuf();
        std::string content = ss.str();

        auto markerJson = json::parse(content, nullptr, true, true);

        UpdateMarkerInfo info;
        info.currentVersion = Utf8ToWide(markerJson.value("currentVersion", ""));
        info.newVersion = Utf8ToWide(markerJson.value("newVersion", ""));
        info.archiveUrl = Utf8ToWide(markerJson.value("archiveUrl", ""));
        info.hashUrl = Utf8ToWide(markerJson.value("hashUrl", ""));
        info.signatureUrl = Utf8ToWide(markerJson.value("signatureUrl", ""));
        info.sourceUpdateUrl = Utf8ToWide(markerJson.value("sourceUpdateUrl", ""));

        // ================================================================
        // SECURITY: Validate ALL URLs from the marker file
        // Even though UtilityPDF wrote them, the file could be tampered.
        // ================================================================

        // Validate sourceUpdateUrl matches appsettings.json
        auto configOpt = LoadConfiguration();
        if (configOpt.has_value() && !configOpt->updateUrl.empty())
        {
            if (info.sourceUpdateUrl != configOpt->updateUrl)
            {
                Logger::LogError(L"MARKER_URL_MISMATCH",
                    L"Marker sourceUpdateUrl does not match appsettings.json UpdateURL! "
                    L"Marker: " + info.sourceUpdateUrl + L" vs Config: " + configOpt->updateUrl);
                return std::nullopt;
            }
        }

        // Validate all download URLs point to trusted GitHub domains
        if (!IsValidGitHubDownloadUrl(info.archiveUrl))
        {
            Logger::LogError(L"MARKER_INVALID_ARCHIVE_URL",
                L"Archive URL failed security validation: " + info.archiveUrl);
            return std::nullopt;
        }

        if (!IsValidGitHubDownloadUrl(info.hashUrl))
        {
            Logger::LogError(L"MARKER_INVALID_HASH_URL",
                L"Hash URL failed security validation: " + info.hashUrl);
            return std::nullopt;
        }

        if (!IsValidGitHubDownloadUrl(info.signatureUrl))
        {
            Logger::LogError(L"MARKER_INVALID_SIG_URL",
                L"Signature URL failed security validation: " + info.signatureUrl);
            return std::nullopt;
        }

        // Validate required fields are present
        if (info.newVersion.empty() || info.archiveUrl.empty() ||
            info.hashUrl.empty() || info.signatureUrl.empty())
        {
            Logger::LogError(L"MARKER_INCOMPLETE",
                L"Update marker is missing required fields");
            return std::nullopt;
        }

        Logger::LogInfo(L"MARKER_LOADED",
            L"Update marker validated: v" + info.currentVersion + L" -> v" + info.newVersion);

        return info;
    }
    catch (const std::exception& ex)
    {
        std::string errorMsg = ex.what();
        Logger::LogError(L"MARKER_LOAD_ERROR",
            L"Error loading update marker: " + std::wstring(errorMsg.begin(), errorMsg.end()));
        return std::nullopt;
    }
}
