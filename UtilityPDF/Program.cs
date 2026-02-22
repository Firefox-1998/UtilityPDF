using System;
using System.Windows.Forms;
using UtilityPDF.Configuration;
using UtilityPDF.Logging;
using UtilityPDF.UI;

namespace UtilityPDF
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
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
                DisplayError.ShowError(
                    $"Errore durante l'inizializzazione della configurazione:\n{ex.Message}", 
                    "Errore di Configurazione");
                
                // Log the initialization error
                DisplayError.LogCustomError("Application initialization failed", new System.Collections.Generic.Dictionary<string, string>
                {
                    { "ErrorMessage", ex.Message },
                    { "StackTrace", ex.StackTrace }
                });
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmUtiPDF_Main());
        }
    }
}
