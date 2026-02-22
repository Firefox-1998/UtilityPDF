using System;
using System.Collections.Generic;

namespace UtilityPDF.Logging
{
    /// <summary>
    /// Represents a single log entry
    /// </summary>
    internal sealed class LogEntry
    {
        public string Timestamp { get; set; }
        public string Level { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }

        public LogEntry()
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            Level = "ERROR";
            ExceptionType = string.Empty;
            Message = string.Empty;
            StackTrace = string.Empty;
            AdditionalData = new Dictionary<string, string>();
        }

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
