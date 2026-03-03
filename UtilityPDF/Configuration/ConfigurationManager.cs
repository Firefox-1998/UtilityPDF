using System;
using System.IO;
using Newtonsoft.Json;
using UtilityPDF.UI;
using UtilityPDF.Resources;

namespace UtilityPDF.Configuration
{
    /// <summary>
    /// Manages application configuration from appsettings.json
    /// </summary>
    internal static class ConfigurationManager
    {
        private static ApplicationSettings currentSettings;
        private static string configFilePath;
        private static readonly object lockObject = new object();
        private static bool userNotifiedOfLogDirectoryFallback = false;

        /// <summary>
        /// Gets the current application settings
        /// </summary>
        public static ApplicationSettings Settings
        {
            get
            {
                if (currentSettings == null)
                {
                    throw new InvalidOperationException(Strings.ConfigNotInit);
                }
                return currentSettings;
            }
        }

        /// <summary>
        /// Initializes the configuration manager and loads appsettings.json
        /// </summary>
        public static void Initialize()
        {
            lock (lockObject)
            {
                string configDirectory = Path.Combine(SettingsString.BinPath, "config");
                configFilePath = Path.Combine(configDirectory, "appsettings.json");

                if (!Directory.Exists(configDirectory))
                {
                    Directory.CreateDirectory(configDirectory);
                }

                if (File.Exists(configFilePath))
                {
                    LoadSettings();
                }
                else
                {
                    CreateDefaultSettings();
                }

                // CRITICAL: Resolve and validate log directory immediately after loading configuration
                ResolveAndSetLogDirectory();

                // Save the final configuration with resolved log directory
                SaveSettings();
            }
        }

        /// <summary>
        /// Loads settings from appsettings.json file
        /// </summary>
        private static void LoadSettings()
        {
            string jsonContent = File.ReadAllText(configFilePath);
            currentSettings = JsonConvert.DeserializeObject<ApplicationSettings>(jsonContent);

            currentSettings ??= new ApplicationSettings();

            // Set runtime values
            if (currentSettings.Application != null)
            {
                currentSettings.Application.Version = GetApplicationVersion();
            }
        }

        /// <summary>
        /// Creates default settings and saves to appsettings.json
        /// </summary>
        private static void CreateDefaultSettings()
        {
            currentSettings = new ApplicationSettings();

            // Set runtime values that cannot be in defaults
            currentSettings.Application.Version = GetApplicationVersion();
        }

        /// <summary>
        /// Resolves the log directory with fallback strategy and sets it in configuration
        /// </summary>
        private static void ResolveAndSetLogDirectory()
        {
            string applicationName = currentSettings.Application.ApplicationName;
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                applicationName = "UtilityPDF";
            }

            // If already configured and valid, keep it
            if (!string.IsNullOrWhiteSpace(currentSettings.Logging.LogDirectory) &&
                TryCreateAndTestDirectory(currentSettings.Logging.LogDirectory))
            {
                return;
            }

            // Primary: Application directory/logs
            string primaryLogPath = Path.Combine(SettingsString.BinPath, "logs");

            if (TryCreateAndTestDirectory(primaryLogPath))
            {
                currentSettings.Logging.LogDirectory = primaryLogPath;
                return;
            }

            // Fallback: User Documents/ApplicationName/logs
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fallbackLogPath = Path.Combine(documentsPath, applicationName, "logs");

            if (TryCreateAndTestDirectory(fallbackLogPath))
            {
                currentSettings.Logging.LogDirectory = fallbackLogPath;

                // Notify user about fallback (only once)
                if (!userNotifiedOfLogDirectoryFallback)
                {
                    userNotifiedOfLogDirectoryFallback = true;
                    DisplayError.ShowInfo(string.Format(Strings.FallbackLogDir,fallbackLogPath), Strings.FallbackLogDirTitle);
                }

                return;
            }

            // Last resort: notify user of complete failure
            if (!userNotifiedOfLogDirectoryFallback)
            {
                userNotifiedOfLogDirectoryFallback = true;
                DisplayError.ShowError(string.Format(Strings.ErrorCreateLogDir, primaryLogPath, fallbackLogPath), Strings.ErrorCreateLogDirTitle);
            }

            currentSettings.Logging.LogDirectory = string.Empty;
        }

        /// <summary>
        /// Tries to create and test write access to a directory
        /// </summary>
        private static bool TryCreateAndTestDirectory(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Test write access
                string testFile = Path.Combine(directoryPath, $"test_{Guid.NewGuid()}.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Saves current settings to appsettings.json
        /// </summary>
        public static void SaveSettings()
        {
            lock (lockObject)
            {
                if (currentSettings == null)
                {
                    throw new InvalidOperationException(Strings.ErrorSettingsSave);
                }

                JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };

                string jsonContent = JsonConvert.SerializeObject(currentSettings, jsonSettings);
                File.WriteAllText(configFilePath, jsonContent);
            }
        }

        /// <summary>
        /// Gets the application version from assembly
        /// </summary>
        private static string GetApplicationVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            return version.ToString();
        }

        /// <summary>
        /// Resets configuration to defaults
        /// </summary>
        public static void ResetToDefaults()
        {
            lock (lockObject)
            {
                CreateDefaultSettings();
                ResolveAndSetLogDirectory();
                SaveSettings();
            }
        }

        /// <summary>
        /// Gets the full path to the log directory (convenience method)
        /// </summary>
        public static string GetLogDirectory()
        {
            return currentSettings?.Logging?.LogDirectory ?? string.Empty;
        }
    }
}
