using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UtilityPDF.Logging;
using UtilityPDF.Resources;

namespace UtilityPDF.UI
{
    /// <summary>
    /// Handles error display and logging
    /// </summary>
    internal static class DisplayError
    {
        /// <summary>
        /// Displays a generic error message
        /// </summary>
        public static void ErrorGeneric(Exception ex)
        {
            LogError(ex);
            MessageBox.Show(
                Strings.GenericMessageError + ex.Message,
                Strings.MsgBoxErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Displays an IO-specific error message
        /// </summary>
        public static void ErrorIO(IOException ex)
        {
            LogError(ex, new Dictionary<string, string>
            {
                { "ErrorType", "IOException" },
                { "FilePath", ex.Source ?? "Unknown" }
            });

            MessageBox.Show(
                Strings.SpecificMessageErrorIO + ex.Message,
                Strings.MsgBoxErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Displays an information message
        /// </summary>
        public static void ShowInfo(string message, string title = "Information")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Displays a warning message
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Displays an error message
        /// </summary>
        public static void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(
                message,
                title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a confirmation dialog and returns the result
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="title">Dialog title</param>
        /// <param name="buttons">Buttons to show (default: OKCancel)</param>
        /// <param name="icon">Icon to display (default: Question)</param>
        /// <returns>True if user clicked OK/Yes, false otherwise</returns>
        public static bool Confirm(string message, string title = "Confirm",
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel,
            MessageBoxIcon icon = MessageBoxIcon.Question)
        {
            DialogResult result = MessageBox.Show(message, title, buttons, icon);

            return result == DialogResult.OK || result == DialogResult.Yes;
        }

        /// <summary>
        /// Logs error details to JSON log file
        /// </summary>
        private static void LogError(Exception ex, Dictionary<string, string> additionalData = null)
        {
            if (ex == null)
            {
                return;
            }

            LogHelper.LogException(ex, additionalData);
        }

        /// <summary>
        /// Logs a custom error message
        /// </summary>
        public static void LogCustomError(string message, Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            LogHelper.LogMessage(message, Strings.MsgBoxErrorTitle.ToUpper(), additionalData);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        public static void LogWarning(string message, Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            LogHelper.LogWarning(message, additionalData);
        }

        /// <summary>
        /// Logs an operation (e.g., PDF merge, conversion)
        /// </summary>
        public static void LogOperation(string operation, string status = "Completed", Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(operation))
            {
                return;
            }

            LogHelper.LogOperation(operation, status, additionalData);
        }

        /// <summary>
        /// Logs an informational message
        /// </summary>
        public static void LogInfo(string message, Dictionary<string, string> additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            LogHelper.LogMessage(message, Strings.MsgBoxInformationTitle.ToUpper(), additionalData);
        }
    }
}
