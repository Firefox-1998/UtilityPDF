using System;
using System.Windows.Forms;
using UtilityPDF.Controls;

namespace UtilityPDF.UI
{
    /// <summary>
    /// Centralized helper for thread-safe UI operations
    /// </summary>
    internal static class UIHelper
    {
        /// <summary>
        /// Executes an action on the UI thread in a thread-safe manner
        /// </summary>
        public static void InvokeIfRequired(Control control, Action action)
        {
            if (control == null || control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action); // Use BeginInvoke for async (non-blocking)
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Executes an action synchronously on the UI thread
        /// </summary>
        public static void InvokeSync(Control control, Action action)
        {
            if (control == null || control.IsDisposed)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Shows or hides a label in a thread-safe manner
        /// </summary>
        public static void SetLabelVisibility(Label label, bool visible)
        {
            if (label == null)
            {
                return;
            }

            InvokeIfRequired(label, () => label.Visible = visible);
        }

        /// <summary>
        /// Shows a MessageBox from the correct thread
        /// </summary>
        public static void ShowMessageBox(Control control, string message, string title,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            Action showMessage = () =>
            {
                Form parentForm = control?.FindForm();
                MessageBox.Show(parentForm, message, title, buttons, icon);
            };

            if (control != null && !control.IsDisposed)
            {
                InvokeSync(control, showMessage);
            }
            else
            {
                // No control context, show directly
                MessageBox.Show(message, title, buttons, icon);
            }
        }

        /// <summary>
        /// Starts the loading spinner with color fader
        /// </summary>
        public static void StartSpinner(LoadingSpinner spinner, ColorFader colorFader)
        {
            if (spinner != null && colorFader != null)
            {
                colorFader.StartFader(spinner);
            }
        }

        /// <summary>
        /// Stops the loading spinner
        /// </summary>
        public static void StopSpinner(ColorFader colorFader)
        {
            colorFader?.StopFader();
        }
    }
}
