using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UtilityPDF.Logging
{
    /// <summary>
    /// Represents a single log entry
    /// </summary>
    internal sealed class LogEntry
    {
        public string Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExceptionType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StackTrace { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> AdditionalData { get; set; }

        /// <summary>
        /// Default constructor for non-error entries (INFO, OPERATION)
        /// </summary>
        public LogEntry()
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Level = "INFO";
            Message = string.Empty;
        }

        /// <summary>
        /// Constructor for error/warning entries with exception details
        /// </summary>
        public LogEntry(Exception ex, string level = "ERROR")
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Level = level;
            ExceptionType = ex?.GetType().Name ?? "Unknown";
            Message = ex?.Message ?? string.Empty;
            StackTrace = ex?.StackTrace ?? string.Empty;
            AdditionalData = new Dictionary<string, string>();
        }
    }
}
