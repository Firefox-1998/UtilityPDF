using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using UtilityPDF.Configuration;
using UtilityPDF.Logging;
using UtilityPDF.UI;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    internal static class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                ConfigurationManager.Initialize();

                // Initialize logging system (cleanup old logs and prepare directory)
                LogHelper.Initialize();
            }
            catch (Exception ex)
            {
                DisplayError.ShowError(string.Format(Strings.Log_ConfigError, ex.Message), Strings.Log_ConfigErrorTitle);
            }

            // Block launch if UpdaterBootstrap is currently running
            if (UpdateChecker.IsUpdaterRunning())
            {
                DisplayError.ShowWarning(
                    "UpdaterBootstrap is currently running. Please wait for the update to complete.",
                    "Update In Progress");
                return;
            }

            // Check for updates before starting the application
            if (ConfigurationManager.Settings.Application.AutoUpdateCheck)
            {
                try
                {
                    UpdateCheckResult updateResult = System.Threading.Tasks.Task.Run(
                        () => UpdateChecker.CheckForUpdateAsync()).GetAwaiter().GetResult();

                    if (updateResult.IsSuccess && updateResult.IsUpdateAvailable)
                    {
                        // Verify all download URLs are available for secure update
                        if (!updateResult.HasCompleteDownloadInfo)
                        {
                            DisplayError.LogWarning("Update available but download URLs are incomplete. " +
                                "Ensure the GitHub release has .7z/.zip, .sha256, and .sig assets.");
                        }
                        else
                        {
                            DialogResult userChoice = MessageBox.Show(
                                $"A new version is available: {updateResult.RemoteVersion}\n" +
                                $"Current version: {updateResult.LocalVersion}\n\n" +
                                "Do you want to update now?",
                                "Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (userChoice == DialogResult.Yes)
                            {
                                if (UpdateChecker.LaunchUpdaterAndExit(updateResult))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Update check failure should never block the application from starting
                    DisplayError.LogWarning($"Update check failed: {ex.Message}");
                }
            }

            string appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            DisplayError.LogOperation(Strings.Log_ApplicationStartup, Strings.Log_Started, new Dictionary<string, string>
            {
                { Strings.Log_Version, appVersion },
                { Strings.Log_Runtime, Environment.Version.ToString() },
                { "OS", Environment.OSVersion.ToString() }
            });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmUtiPDF_Main());

            DisplayError.LogOperation(Strings.Log_ApplicationShutdown, Strings.Log_Completed, new Dictionary<string, string>
            {
                { Strings.Log_Version, appVersion }
            });
        }
    }
}
