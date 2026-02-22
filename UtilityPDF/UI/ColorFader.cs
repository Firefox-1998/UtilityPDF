using System;
using System.Windows.Forms;
using UtilityPDF.Controls;

namespace UtilityPDF.UI
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
                throw new ArgumentNullException(nameof(loadingSpinner));
            }

            spinner = loadingSpinner;
            spinner.Start();
        }

        /// <summary>
        /// Stops the loading spinner
        /// </summary>
        public void StopFader()
        {
            if (spinner != null)
            {
                spinner.Stop();
            }
        }

        /// <summary>
        /// Disposes the ColorFader and stops the spinner
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    StopFader();
                    spinner = null;
                }
                disposed = true;
            }
        }
    }
}
