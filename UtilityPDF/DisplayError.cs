using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using UtilityPDF.Resources;

namespace UtilityPDF
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
            LogError(ex);
            MessageBox.Show(
                Strings.SpecificMessageErrorIO + ex.Message,
                Strings.MsgBoxErrorTitle,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Logs error details to debug output
        /// </summary>
        private static void LogError(Exception ex)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Debug.WriteLine($"[{timestamp}] ERROR: {ex.GetType().Name}");
            Debug.WriteLine($"Message: {ex.Message}");
            Debug.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }
}
