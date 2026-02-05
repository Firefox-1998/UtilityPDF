using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Resources;
using UtilityPDF.UI;

namespace UtilityPDF.Operations
{
    /// <summary>
    /// Classe base per operazioni asincrone con gestione UI comune
    /// </summary>
    internal abstract class AsyncOperationBase
    {
        protected Label LblProgress { get; private set; }
        protected LoadingSpinner Spinner { get; private set; }
        protected ColorFader ColorFader { get; private set; }

        protected async Task ExecuteWithSpinnerAsync(Label lblProgress, LoadingSpinner spinner, Func<Task> operation)
        {
            LblProgress = lblProgress;
            Spinner = spinner;

            using (ColorFader = new ColorFader())
            {
                StartSpinner();
                ShowProgressLabel();

                try
                {
                    await operation();
                }
                finally
                {
                    StopSpinner();
                }
            }
        }

        private void StartSpinner()
        {
            if (Spinner != null)
            {
                ColorFader.StartFader(Spinner);
            }
        }

        private void StopSpinner()
        {
            ColorFader?.StopFader();
        }

        private void ShowProgressLabel()
        {
            UIHelper.SetLabelVisibility(LblProgress, true);
        }

        protected void ShowCompletionMessage(string message)
        {
            UIHelper.ShowMessageBox(LblProgress, message, Strings.MsgBoxInformationTitle);
        }
    }
}