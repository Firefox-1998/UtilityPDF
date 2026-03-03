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
