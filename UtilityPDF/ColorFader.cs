using System;
using System.Windows.Forms;
using UtilityPDF.Controls;

namespace UtilityPDF
{
    /// <summary>
    /// Manages the animated loading spinner
    /// </summary>
    internal class ColorFader : IDisposable
    {
        private LoadingSpinner spinner;
        private bool disposed = false;

        /// <summary>
        /// Starts the loading spinner with a LoadingSpinner control
        /// </summary>
        public void StartFader(LoadingSpinner loadingSpinner)
        {
            if (loadingSpinner == null)
            {
                return;
            }

            spinner = loadingSpinner;

            if (spinner.InvokeRequired)
            {
                spinner.Invoke(new Action(() =>
                {
                    spinner.Visible = true;
                    spinner.Start();
                }));
            }
            else
            {
                spinner.Visible = true;
                spinner.Start();
            }
        }

        /// <summary>
        /// Stops the loading spinner
        /// </summary>
        public void StopFader()
        {
            if (spinner != null && !spinner.IsDisposed)
            {
                if (spinner.InvokeRequired)
                {
                    spinner.Invoke(new Action(() =>
                    {
                        spinner.Stop();
                        spinner.Visible = false;
                    }));
                }
                else
                {
                    spinner.Stop();
                    spinner.Visible = false;
                }
            }
        }

        /// <summary>
        /// Releases all resources used by the ColorFader
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources and optionally releases the managed resources
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    spinner = null;
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~ColorFader()
        {
            Dispose(false);
        }
    }
}
