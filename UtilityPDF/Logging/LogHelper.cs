using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UtilityPDF.Configuration;

namespace UtilityPDF.Logging
{
    /// <summary>
    /// Log type enumeration
    /// </summary>
    internal enum LogType
    {
        Error,
        Operation,
        Warning
    }

    /// <summary>
    /// Centralized logging helper for file-based JSON logging
    /// </summary>
    internal static class LogHelper
    {
        private static readonly object lockObject = new object();

        /// <summary>
        /// Initializes the logging system (call at application startup)
        /// </summary>
        public static void Initialize()
        {
            try
            {
                string logDirectory = ConfigurationManager.GetLogDirectory();
                if (string.IsNullOrEmpty(logDirectory))
                {
                    Debug.WriteLine("Logging disabled: No valid log directory available.");
                    return;
                }

                // Create subdirectories for each log type
                EnsureLogDirectoryExists(GetLogTypeDirectory(logDirectory, LogType.Error));
                EnsureLogDirectoryExists(GetLogTypeDirectory(logDirectory, LogType.Operation));
                EnsureLogDirectoryExists(GetLogTypeDirectory(logDirectory, LogType.Warning));

                // Cleanup old logs
                CleanupOldLogs(GetLogTypeDirectory(logDirectory, LogType.Error),
                    ConfigurationManager.Settings.Logging.LogRetentionDays, "error_*.json");

                CleanupOldLogs(GetLogTypeDirectory(logDirectory, LogType.Warning),
                    ConfigurationManager.Settings.Logging.LogRetentionDays, "warning_*.json");

                if (ConfigurationManager.Settings.Logging.EnableOperationLogs)
                {
                    CleanupOldLogs(GetLogTypeDirectory(logDirectory, LogType.Operation),
                        ConfigurationManager.Settings.Logging.OperationLogRetentionDays, "operation_*.json");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing logging system: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the directory path for a specific log type
        /// </summary>
        private static string GetLogTypeDirectory(string baseDirectory, LogType logType)
        {
            string subDirectory = logType.ToString().ToLowerInvariant() + "s";
            return Path.Combine(baseDirectory, subDirectory);
        }

        /// <summary>
        /// Logs an exception to the JSON log file (always logged)
        /// </summary>
        public static void LogException(Exception ex, Dictionary<string, string> additionalData = null)
        {
            if (ex == null)
            {
                return;
            }

            try
            {
                LogEntry entry = new LogEntry(ex)
                {
                    AdditionalData = additionalData ?? new Dictionary<string, string>()
                };

                WriteLogEntry(entry, LogType.Error);
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"Critical: Failed to write error log - {logEx.Message}");
            }
        }

        /// <summary>
        /// Logs an operation to the JSON log file (only if EnableOperationLogs is true)
        /// </summary>
        public static void LogOperation(string operation, string status, Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(operation))
            {
                return;
            }

            try
            {
                if (!ConfigurationManager.Settings.Logging.EnableOperationLogs)
                {
                    return;
                }

                Dictionary<string, string> data = additionalData ?? new Dictionary<string, string>();
                data["Operation"] = operation;
                data["Status"] = status ?? "Completed";

                LogEntry entry = new LogEntry
                {
                    Level = "OPERATION",
                    Message = operation,
                    ExceptionType = "N/A",
                    AdditionalData = data
                };

                WriteLogEntry(entry, LogType.Operation);
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"Critical: Failed to write operation log - {logEx.Message}");
            }
        }

        /// <summary>
        /// Logs a warning message to the JSON log file (always logged)
        /// </summary>
        public static void LogWarning(string message, Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            try
            {
                LogEntry entry = new LogEntry
                {
                    Level = "WARNING",
                    Message = message,
                    ExceptionType = "N/A",
                    AdditionalData = additionalData ?? new Dictionary<string, string>()
                };

                WriteLogEntry(entry, LogType.Warning);
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"Critical: Failed to write warning log - {logEx.Message}");
            }
        }

        /// <summary>
        /// Logs a custom message to the JSON log file (logged as error, always written)
        /// </summary>
        public static void LogMessage(string message, string level = "INFO", Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            try
            {
                LogEntry entry = new LogEntry
                {
                    Level = level,
                    Message = message,
                    ExceptionType = "N/A",
                    AdditionalData = additionalData ?? new Dictionary<string, string>()
                };

                WriteLogEntry(entry, LogType.Error);
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"Critical: Failed to write log message - {logEx.Message}");
            }
        }

        /// <summary>
        /// Writes a log entry to the appropriate log file
        /// </summary>
        private static void WriteLogEntry(LogEntry entry, LogType logType)
        {
            lock (lockObject)
            {
                string logDirectory = ConfigurationManager.GetLogDirectory();
                if (string.IsNullOrEmpty(logDirectory))
                {
                    return;
                }

                string logFilePath = GetCurrentLogFilePath(logDirectory, logType);
                string logTypeDirectory = GetLogTypeDirectory(logDirectory, logType);
                EnsureLogDirectoryExists(logTypeDirectory);

                // Check if rotation is needed
                if (File.Exists(logFilePath))
                {
                    FileInfo fileInfo = new FileInfo(logFilePath);
                    long maxSizeBytes = ConfigurationManager.Settings.Logging.MaxLogFileSizeMB * 1024L * 1024L;

                    if (fileInfo.Length >= maxSizeBytes)
                    {
                        RotateLogFile(logFilePath);
                        logFilePath = GetCurrentLogFilePath(logDirectory, logType);
                    }
                }

                // Read existing entries or create new list
                List<LogEntry> entries = new List<LogEntry>();
                if (File.Exists(logFilePath))
                {
                    try
                    {
                        string existingJson = File.ReadAllText(logFilePath, Encoding.UTF8);
                        entries = JsonConvert.DeserializeObject<List<LogEntry>>(existingJson) ?? new List<LogEntry>();
                    }
                    catch
                    {
                        entries = new List<LogEntry>();
                    }
                }

                entries.Add(entry);

                string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
                File.WriteAllText(logFilePath, json, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Gets the current log file path based on date and log type
        /// </summary>
        private static string GetCurrentLogFilePath(string baseDirectory, LogType logType)
        {
            string logDirectory = GetLogTypeDirectory(baseDirectory, logType);
            string prefix = logType.ToString().ToLowerInvariant();
            string fileName = $"{prefix}_{DateTime.Now:yyyy-MM-dd}.json";
            return Path.Combine(logDirectory, fileName);
        }

        /// <summary>
        /// Rotates the log file when it exceeds max size
        /// </summary>
        private static void RotateLogFile(string logFilePath)
        {
            try
            {
                string directory = Path.GetDirectoryName(logFilePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(logFilePath);
                string extension = Path.GetExtension(logFilePath);
                string timestamp = DateTime.Now.ToString("HHmmss");
                string rotatedFileName = $"{fileNameWithoutExtension}_{timestamp}{extension}";
                string rotatedFilePath = Path.Combine(directory, rotatedFileName);

                File.Move(logFilePath, rotatedFilePath);
            }
            catch (Exception)
            {
                // Silently fail - log rotation is not critical
            }
        }

        /// <summary>
        /// Ensures the log directory exists
        /// </summary>
        private static void EnsureLogDirectoryExists(string logDirectory)
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Cleans up log files older than the retention period
        /// </summary>
        private static void CleanupOldLogs(string logDirectory, int retentionDays, string searchPattern)
        {
            try
            {
                if (!Directory.Exists(logDirectory))
                {
                    return;
                }

                DateTime cutoffDate = DateTime.Now.AddDays(-retentionDays);
                DirectoryInfo dirInfo = new DirectoryInfo(logDirectory);
                FileInfo[] logFiles = dirInfo.GetFiles(searchPattern);

                foreach (FileInfo file in logFiles)
                {
                    if (file.CreationTime < cutoffDate)
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception)
                        {
                            // Silently skip files that cannot be deleted
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail - cleanup is not critical
            }
        }

        /// <summary>
        /// Gets all log entries from a specific date and type
        /// </summary>
        public static List<LogEntry> GetLogEntriesForDate(DateTime date, LogType logType)
        {
            try
            {
                string logDirectory = ConfigurationManager.GetLogDirectory();
                if (string.IsNullOrEmpty(logDirectory))
                {
                    return new List<LogEntry>();
                }

                string logFilePath = GetLogFilePathForDate(logDirectory, logType, date);

                if (!File.Exists(logFilePath))
                {
                    return new List<LogEntry>();
                }

                string json = File.ReadAllText(logFilePath, Encoding.UTF8);
                List<LogEntry> entries = JsonConvert.DeserializeObject<List<LogEntry>>(json);
                return entries ?? new List<LogEntry>();
            }
            catch (Exception)
            {
                return new List<LogEntry>();
            }
        }

        /// <summary>
        /// Gets the log file path for a specific date
        /// </summary>
        private static string GetLogFilePathForDate(string baseDirectory, LogType logType, DateTime date)
        {
            string logDirectory = GetLogTypeDirectory(baseDirectory, logType);
            string prefix = logType.ToString().ToLowerInvariant();
            string fileName = $"{prefix}_{date:yyyy-MM-dd}.json";
            return Path.Combine(logDirectory, fileName);
        }
    }
}
