using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Resources;
using UtilityPDF.UI;

namespace UtilityPDF.Operations
{
    /// <summary>
    /// Base class for asynchronous operations with common UI handling
    /// </summary>
    internal abstract class AsyncOperationBase
    {
        protected Label ProgressLabel { get; private set; }
        protected LoadingSpinner Spinner { get; private set; }
        protected ColorFader ColorFader { get; private set; }

        /// <summary>
        /// Executes the operation with spinner and progress label management
        /// </summary>
        protected async Task ExecuteWithSpinnerAsync(Label progressLabel, LoadingSpinner spinner, Func<Task> operation)
        {
            ProgressLabel = progressLabel;
            Spinner = spinner;

            using (ColorFader = new ColorFader())
            {
                StartOperation();

                try
                {
                    await Task.Run(async () => await operation());
                }
                catch (IOException ex)
                {
                    DisplayError.LogCustomError(string.Format(Strings.Log_IOErrorAsyncOp, ex.Message), new System.Collections.Generic.Dictionary<string, string>
                    {
                        { Strings.Log_ErrorType, "IOException" },
                        { Strings.Log_Source, ex.Source ?? Strings.Log_Unknown }
                    });
                    DisplayError.ErrorIO(ex);
                }
                catch (Exception ex)
                {
                    DisplayError.LogCustomError(string.Format(Strings.Log_UnexErrAsyncOp, ex.Message), new System.Collections.Generic.Dictionary<string, string>
                    {
                        { Strings.Log_ErrorType, ex.GetType().Name }
                    });
                    DisplayError.ErrorGeneric(ex);
                }
                finally
                {
                    StopOperation();
                }
            }
        }

        /// <summary>
        /// Executes a synchronous operation with spinner management
        /// </summary>
        protected async Task ExecuteWithSpinnerAsync(Label progressLabel, LoadingSpinner spinner, Action operation)
        {
            await ExecuteWithSpinnerAsync(progressLabel, spinner, () =>
            {
                operation();
                return Task.CompletedTask;
            });
        }

        private void StartOperation()
        {
            UIHelper.StartSpinner(Spinner, ColorFader);
            UIHelper.SetLabelVisibility(ProgressLabel, true);
        }

        private void StopOperation()
        {
            UIHelper.StopSpinner(ColorFader);
        }

        /// <summary>
        /// Shows a completion message on the UI thread
        /// </summary>
        protected void ShowCompletionMessage(string message)
        {
            UIHelper.ShowMessageBox(ProgressLabel, message, Strings.MsgBoxInformationTitle);
        }
    }
}
