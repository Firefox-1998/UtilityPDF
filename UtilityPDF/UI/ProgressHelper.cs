using System;
using UtilityPDF.Controls;

namespace UtilityPDF.UI
{
    /// <summary>
    /// Helper for thread-safe progress bar updates
    /// </summary>
    internal static class ProgressHelper
    {
        /// <summary>
        /// Updates the progress bar in a thread-safe manner
        /// </summary>
        public static void UpdateProgress(ModernProgressBar progressBar, int percentage)
        {
            if (progressBar == null)
            {
                return;
            }

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<ModernProgressBar, int>(UpdateProgress), progressBar, percentage);
            }
            else
            {
                int clampedValue = Math.Max(0, Math.Min(100, percentage));
                progressBar.Value = clampedValue;
            }
        }

        /// <summary>
        /// Resets the progress bar to zero
        /// </summary>
        public static void ResetProgress(ModernProgressBar progressBar)
        {
            UpdateProgress(progressBar, 0);
        }

        /// <summary>
        /// Sets the progress bar to complete (100%)
        /// </summary>
        public static void CompleteProgress(ModernProgressBar progressBar)
        {
            UpdateProgress(progressBar, 100);
        }
    }
}