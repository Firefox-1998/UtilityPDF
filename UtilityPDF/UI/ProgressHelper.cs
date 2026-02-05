using System;
using UtilityPDF.Controls;

namespace UtilityPDF.UI
{
    /// <summary>
    /// Helper per gestire l'aggiornamento thread-safe della progress bar
    /// </summary>
    internal static class ProgressHelper
    {
        /// <summary>
        /// Aggiorna la progress bar in modo thread-safe
        /// </summary>
        public static void UpdateProgress(ModernProgressBar progressBar, int percentage)
        {
            if (progressBar == null) return;

            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<ModernProgressBar, int>(UpdateProgress), progressBar, percentage);
            }
            else
            {
                progressBar.Value = Math.Max(0, Math.Min(100, percentage));
            }
        }

        /// <summary>
        /// Reset della progress bar
        /// </summary>
        public static void ResetProgress(ModernProgressBar progressBar)
        {
            UpdateProgress(progressBar, 0);
        }
    }
}