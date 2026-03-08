using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UtilityPDF.UI;

namespace UtilityPDF.Configuration
{
    /// <summary>
    /// Checks for application updates via GitHub releases API
    /// </summary>
    internal static class UpdateChecker
    {
        private const string GitHubApiBase = "https://api.github.com/repos/";
        private const string UserAgent = "UtilityPDF-UpdateChecker";
        private const int TimeoutSeconds = 15;
        private const string UpdaterProcessName = "UpdaterBootstrap";

        private static readonly Lazy<HttpClient> lazyHttpClient = new Lazy<HttpClient>(() =>
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            return client;
        });

        private static HttpClient SharedHttpClient => lazyHttpClient.Value;

        /// <summary>
        /// Checks if a newer version is available on GitHub
        /// </summary>
        /// <returns>An UpdateCheckResult with the comparison outcome</returns>
        public static async Task<UpdateCheckResult> CheckForUpdateAsync()
        {
            UpdateCheckResult result = new UpdateCheckResult();

            try
            {
                // Validate configuration
                string updateUrl = ConfigurationManager.Settings.Application.UpdateURL;
                if (string.IsNullOrWhiteSpace(updateUrl))
                {
                    result.ErrorMessage = "UpdateURL is not configured in appsettings.json";
                    DisplayError.LogWarning(result.ErrorMessage);
                    return result;
                }

                // Extract owner/repo from GitHub URL with security validation
                string apiEndpoint = BuildApiEndpoint(updateUrl);
                if (string.IsNullOrEmpty(apiEndpoint))
                {
                    result.ErrorMessage = $"Invalid GitHub URL format: {updateUrl}";
                    DisplayError.LogWarning(result.ErrorMessage);
                    return result;
                }

                // Get local version
                string localVersionString = ConfigurationManager.Settings.Application.Version;
                if (!TryParseVersion(localVersionString, out Version localVersion))
                {
                    result.ErrorMessage = $"Cannot parse local version: {localVersionString}";
                    DisplayError.LogWarning(result.ErrorMessage);
                    return result;
                }

                result.LocalVersion = localVersion;

                // Ensure TLS 1.2 for GitHub API (required on .NET Framework 4.8)
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                // Fetch latest release from GitHub
                HttpResponseMessage response = await SharedHttpClient.GetAsync(apiEndpoint).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    result.ErrorMessage = $"GitHub API returned {(int)response.StatusCode}: {response.ReasonPhrase}";
                    DisplayError.LogWarning(result.ErrorMessage, new Dictionary<string, string>
                    {
                        { "StatusCode", ((int)response.StatusCode).ToString() },
                        { "Endpoint", apiEndpoint }
                    });
                    return result;
                }

                string jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                JObject release = JObject.Parse(jsonResponse);

                // Extract tag name (e.g., "v1.8.0.0" or "1.8.0.0")
                string tagName = release["tag_name"]?.ToString();
                if (string.IsNullOrWhiteSpace(tagName))
                {
                    result.ErrorMessage = "No tag_name found in latest release";
                    DisplayError.LogWarning(result.ErrorMessage);
                    return result;
                }

                if (!TryParseVersion(tagName, out Version remoteVersion))
                {
                    result.ErrorMessage = $"Cannot parse remote version from tag: {tagName}";
                    DisplayError.LogWarning(result.ErrorMessage);
                    return result;
                }

                result.RemoteVersion = remoteVersion;
                result.ReleaseNotes = release["body"]?.ToString() ?? string.Empty;
                result.DownloadUrl = release["html_url"]?.ToString() ?? string.Empty;

                // Extract download URLs for archive, hash, and signature from release assets
                JArray assets = release["assets"] as JArray;
                if (assets != null)
                {
                    foreach (JToken asset in assets)
                    {
                        string assetName = asset["name"]?.ToString() ?? string.Empty;
                        string browserDownloadUrl = asset["browser_download_url"]?.ToString() ?? string.Empty;

                        if (string.IsNullOrEmpty(browserDownloadUrl))
                        {
                            continue;
                        }

                        // Validate asset URL points to github.com
                        if (!IsValidGitHubDownloadUrl(browserDownloadUrl))
                        {
                            DisplayError.LogWarning($"Skipping untrusted asset URL: {browserDownloadUrl}");
                            continue;
                        }

                        if (assetName.EndsWith(".7z", StringComparison.OrdinalIgnoreCase) ||
                            assetName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                        {
                            result.ArchiveDownloadUrl = browserDownloadUrl;
                        }
                        else if (assetName.EndsWith(".sha256", StringComparison.OrdinalIgnoreCase))
                        {
                            result.HashDownloadUrl = browserDownloadUrl;
                        }
                        else if (assetName.EndsWith(".sig", StringComparison.OrdinalIgnoreCase))
                        {
                            result.SignatureDownloadUrl = browserDownloadUrl;
                        }
                    }
                }

                // Compare versions
                result.IsUpdateAvailable = remoteVersion > localVersion;

                DisplayError.LogInfo("Update check completed", new Dictionary<string, string>
                {
                    { "LocalVersion", localVersion.ToString() },
                    { "RemoteVersion", remoteVersion.ToString() },
                    { "UpdateAvailable", result.IsUpdateAvailable.ToString() }
                });
            }
            catch (HttpRequestException ex)
            {
                result.ErrorMessage = $"Network error during update check: {ex.Message}";
                DisplayError.LogWarning(result.ErrorMessage);
            }
            catch (TaskCanceledException)
            {
                result.ErrorMessage = "Update check timed out";
                DisplayError.LogWarning(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"Unexpected error during update check: {ex.Message}";
                DisplayError.LogWarning(result.ErrorMessage);
            }

            return result;
        }

        /// <summary>
        /// Creates the update_in_progress.json marker file with download URLs
        /// for UpdaterBootstrap to consume.
        /// </summary>
        public static bool WriteUpdateMarker(UpdateCheckResult updateResult)
        {
            try
            {
                string configDir = Path.Combine(SettingsString.BinPath, "config");
                string markerPath = Path.Combine(configDir, "update_in_progress.json");

                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                JObject marker = new JObject
                {
                    ["timestamp"] = DateTime.UtcNow.ToString("o"),
                    ["currentVersion"] = updateResult.LocalVersion?.ToString() ?? string.Empty,
                    ["newVersion"] = updateResult.RemoteVersion?.ToString() ?? string.Empty,
                    ["archiveUrl"] = updateResult.ArchiveDownloadUrl ?? string.Empty,
                    ["hashUrl"] = updateResult.HashDownloadUrl ?? string.Empty,
                    ["signatureUrl"] = updateResult.SignatureDownloadUrl ?? string.Empty,
                    ["sourceUpdateUrl"] = ConfigurationManager.Settings.Application.UpdateURL ?? string.Empty
                };

                File.WriteAllText(markerPath, marker.ToString());

                DisplayError.LogInfo("Update marker written", new Dictionary<string, string>
                {
                    { "MarkerPath", markerPath },
                    { "NewVersion", updateResult.RemoteVersion?.ToString() ?? "unknown" }
                });

                return true;
            }
            catch (Exception ex)
            {
                DisplayError.LogWarning($"Failed to write update marker: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Launches UpdaterBootstrap and returns true if successfully started.
        /// Writes the update marker and verifies binary integrity before execution.
        /// </summary>
        public static bool LaunchUpdaterAndExit(UpdateCheckResult updateResult)
        {
            try
            {
                string updaterPath = Path.Combine(SettingsString.BinPath, UpdaterProcessName + ".exe");

                if (!File.Exists(updaterPath))
                {
                    DisplayError.LogWarning($"Updater not found at: {updaterPath}");
                    DisplayError.ShowWarning(
                        $"Updater executable not found:\n{updaterPath}",
                        "Update Error");
                    return false;
                }

                // Verify binary integrity before launching
                string hashFilePath = updaterPath + ".sha256";
                string sigFilePath = updaterPath + ".sig";

                if (File.Exists(hashFilePath) && File.Exists(sigFilePath))
                {
                    string expectedHash = File.ReadAllText(hashFilePath).Trim().Split(' ')[0];
                    string signatureBase64 = File.ReadAllText(sigFilePath).Trim();

                    Security.IntegrityResult integrity = Security.BinaryIntegrityVerifier.VerifyFull(
                        updaterPath, expectedHash, signatureBase64);

                    if (!integrity.IsValid)
                    {
                        DisplayError.LogWarning($"Updater integrity check failed: {integrity.ErrorMessage}");
                        DisplayError.ShowError(
                            $"Security verification failed for the updater:\n{integrity.ErrorMessage}\n\n" +
                            "The update will not proceed. Please download manually from GitHub.",
                            "Security Error");
                        return false;
                    }

                    DisplayError.LogInfo("Updater integrity verified successfully");
                }
                else
                {
                    DisplayError.LogWarning("Integrity files (.sha256 / .sig) not found — skipping verification");
                }

                // Write the marker file with download URLs
                if (!WriteUpdateMarker(updateResult))
                {
                    DisplayError.ShowError(
                        "Failed to create update marker file.\nThe update will not proceed.",
                        "Update Error");
                    return false;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = updaterPath,
                    UseShellExecute = true,
                    WorkingDirectory = SettingsString.BinPath
                };

                Process.Start(startInfo);

                DisplayError.LogOperation("UpdaterBootstrap", "Launched", new Dictionary<string, string>
                {
                    { "Path", updaterPath }
                });

                return true;
            }
            catch (Exception ex)
            {
                DisplayError.LogWarning($"Failed to launch updater: {ex.Message}");
                DisplayError.ShowError(
                    $"Failed to launch updater:\n{ex.Message}",
                    "Update Error");
                return false;
            }
        }

        /// <summary>
        /// Checks if UpdaterBootstrap is currently running
        /// </summary>
        public static bool IsUpdaterRunning()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(UpdaterProcessName);
                bool isRunning = processes.Length > 0;

                foreach (Process process in processes)
                {
                    process.Dispose();
                }

                return isRunning;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Validates that a download URL is a legitimate GitHub URL (HTTPS only).
        /// Prevents SSRF by ensuring the domain is github.com or objects.githubusercontent.com
        /// </summary>
        private static bool IsValidGitHubDownloadUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                return false;
            }

            if (!uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string host = uri.Host.ToLowerInvariant();
            return host == "github.com" || host == "objects.githubusercontent.com";
        }

        /// <summary>
        /// Builds the GitHub API endpoint from a repository URL.
        /// Validates HTTPS scheme and github.com domain to prevent SSRF.
        /// </summary>
        private static string BuildApiEndpoint(string githubUrl)
        {
            try
            {
                if (!Uri.TryCreate(githubUrl, UriKind.Absolute, out Uri uri) ||
                    !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ||
                    !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty;
                }

                Match match = Regex.Match(githubUrl.TrimEnd('/'),
                    @"^https://github\.com/([A-Za-z0-9_.\-]+)/([A-Za-z0-9_.\-]+)/?$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromSeconds(2));

                if (!match.Success)
                {
                    return string.Empty;
                }

                string owner = match.Groups[1].Value;
                string repo = match.Groups[2].Value;

                return $"{GitHubApiBase}{owner}/{repo}/releases/latest";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Tries to parse a version string, stripping any leading "v" prefix
        /// </summary>
        private static bool TryParseVersion(string versionString, out Version version)
        {
            version = null;

            if (string.IsNullOrWhiteSpace(versionString))
            {
                return false;
            }

            string cleaned = versionString.TrimStart('v', 'V').Trim();
            return Version.TryParse(cleaned, out version);
        }
    }
}
