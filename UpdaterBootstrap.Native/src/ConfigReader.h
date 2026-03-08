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
        std::wstring updateUrl;  // GitHub repository URL (e.g., https://github.com/owner/repo)
    };

    /// <summary>
    /// Download URLs parsed from update_in_progress.json marker file
    /// </summary>
    struct UpdateMarkerInfo {
        std::wstring currentVersion;
        std::wstring newVersion;
        std::wstring archiveUrl;
        std::wstring hashUrl;
        std::wstring signatureUrl;
        std::wstring sourceUpdateUrl;
    };

    explicit ConfigReader(const std::wstring& appDirectory);

    std::optional<AppConfig> LoadConfiguration();

    /// <summary>
    /// Reads and validates the update_in_progress.json marker file
    /// written by UtilityPDF with download URLs.
    /// </summary>
    std::optional<UpdateMarkerInfo> LoadUpdateMarker();

    /// <summary>
    /// Validates that a URL is a trusted GitHub HTTPS URL.
    /// Prevents SSRF by checking scheme + domain.
    /// </summary>
    static bool IsValidGitHubUrl(const std::wstring& url);

    /// <summary>
    /// Validates that a download URL points to GitHub or GitHub's CDN.
    /// Allowed domains: github.com, objects.githubusercontent.com
    /// </summary>
    static bool IsValidGitHubDownloadUrl(const std::wstring& url);

private:
    std::wstring _appDirectory;
    std::wstring _configPath;

    std::wstring GetConfigPath() const;
    std::optional<std::wstring> ReadJsonFile(const std::wstring& path);
    std::optional<AppConfig> ParseConfiguration(const std::string& jsonContent);
    static std::wstring Utf8ToWide(const std::string& utf8);
    static std::wstring ExtractHostFromUrl(const std::wstring& url);
};
