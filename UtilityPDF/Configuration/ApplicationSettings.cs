using Newtonsoft.Json;

namespace UtilityPDF.Configuration
{
    /// <summary>
    /// Application configuration root model
    /// </summary>
    internal class ApplicationSettings
    {
        [JsonProperty("Application")]
        public ApplicationConfig Application { get; set; }

        [JsonProperty("Logging")]
        public LoggingConfig Logging { get; set; }

        public ApplicationSettings()
        {
            Application = new ApplicationConfig();
            Logging = new LoggingConfig();
        }
    }

    /// <summary>
    /// Application-specific settings
    /// </summary>
    internal class ApplicationConfig
    {
        [JsonProperty("Version")]
        public string Version { get; set; } = string.Empty;

        [JsonProperty("Environment")]
        public string Environment { get; set; } = "Production";

        [JsonProperty("LanguageUI")]
        public string LanguageUI { get; set; } = "en-US";

        [JsonProperty("AutoUpdateCheck")]
        public bool AutoUpdateCheck { get; set; } = true;

        [JsonProperty("UpdateCheckIntervalHours")]
        public int UpdateCheckIntervalHours { get; set; } = 24;

        [JsonProperty("UpdateURL")]
        public string UpdateURL { get; set; } = string.Empty;

        [JsonProperty("ApplicationName")]
        public string ApplicationName { get; set; } = "Utility PDF";

        [JsonProperty("Developer")]
        public string Developer { get; set; } = "Firefox_1998";

        [JsonProperty("SupportEmail")]
        public string SupportEmail { get; set; } = "support@gldevelop.com";
    }

    /// <summary>
    /// Logging configuration settings
    /// </summary>
    internal class LoggingConfig
    {
        [JsonProperty("LogDirectory")]
        public string LogDirectory { get; set; } = string.Empty;

        [JsonProperty("LogRetentionDays")]
        public int LogRetentionDays { get; set; } = 15;

        [JsonProperty("MaxLogFileSizeMB")]
        public int MaxLogFileSizeMB { get; set; } = 10;

        [JsonProperty("EnableOperationLogs")]
        public bool EnableOperationLogs { get; set; } = true;

        [JsonProperty("OperationLogRetentionDays")]
        public int OperationLogRetentionDays { get; set; } = 7;
    }
}
