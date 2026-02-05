using System;
using System.Windows.Forms;
using UtilityPDF.Controls;

namespace UtilityPDF
{
    /// <summary>
    /// Gestisce lo spinner di caricamento animato
    /// </summary>
    internal class ColorFader : IDisposable
    {
        private LoadingSpinner spinner;
        private bool disposed = false;

        /// <summary>
        /// Avvia lo spinner di caricamento con un controllo LoadingSpinner
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
        /// Ferma lo spinner di caricamento
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        ~ColorFader()
        {
            Dispose(false);
        }
    }
}
