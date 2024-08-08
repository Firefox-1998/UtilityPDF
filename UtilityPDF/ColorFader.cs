using System;
using System.Drawing;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class ColorFader : IDisposable
    {
        private Timer progressTimer;
        private int colorStep;
        private bool fadingOut;
        private Color startColor;
        private Color endColor;
        private Label progressLabel;
        private bool disposed = false;

        public void StartFader(Label lblPr)
        {
            Startfd(lblPr);
        }

        public void StopFader()
        {
            progressTimer.Stop();
            progressLabel.ForeColor = Color.Black;
        }

        private void Startfd(Label lblProgr)
        {
            progressLabel = lblProgr;
            InitializeColors();
            InitializeTimer();
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (fadingOut)
            {
                colorStep -= 10;
                if (colorStep <= 0)
                {
                    colorStep = 0;
                    fadingOut = false;
                }
            }
            else
            {
                colorStep += 10;
                if (colorStep >= 255)
                {
                    colorStep = 255;
                    fadingOut = true;
                }
            }

            int r = startColor.R + (endColor.R - startColor.R) * colorStep / 255;
            int g = startColor.G + (endColor.G - startColor.G) * colorStep / 255;
            int b = startColor.B + (endColor.B - startColor.B) * colorStep / 255;

            progressLabel.ForeColor = Color.FromArgb(r, g, b);
        }

        private void InitializeTimer()
        {
            progressTimer = new Timer
            {
                Interval = 100 // Cambia l'opacità ogni 100 ms                
            };
            progressTimer.Tick += ProgressTimer_Tick;
            progressTimer.Start();
        }

        private void InitializeColors()
        {
            startColor = Color.Black;
            endColor = progressLabel.BackColor;
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
                    // Rilascia risorse gestite qui
                    if (progressTimer != null)
                    {
                        progressTimer.Dispose();
                        progressTimer = null;
                    }

                    if (progressLabel != null)
                    {
                        progressLabel.Dispose();
                        progressLabel = null;
                    }
                }

                // Rilascia risorse non gestite qui

                disposed = true;
            }
        }

        ~ColorFader()
        {
            Dispose(false);
        }
    }
}
