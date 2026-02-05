using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Spinner di caricamento animato
    /// </summary>
    public class LoadingSpinner : Control
    {
        private Timer animationTimer;
        private int currentAngle = 0;
        private Color spinnerColor = Color.FromArgb(52, 152, 219);

        public LoadingSpinner()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            BackColor = Color.Transparent;
            Size = new Size(40, 40);

            animationTimer = new Timer { Interval = 50 }; // 20 FPS
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public Color SpinnerColor
        {
            get { return spinnerColor; }
            set { spinnerColor = value; Invalidate(); }
        }

        public void Start()
        {
            animationTimer.Start();
        }

        public void Stop()
        {
            animationTimer.Stop();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            currentAngle = (currentAngle + 30) % 360;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = Math.Min(Width, Height) / 2 - 4;

            // Disegna 8 linee rotanti
            for (int i = 0; i < 8; i++)
            {
                float angle = (currentAngle + i * 45) * (float)Math.PI / 180f;
                float opacity = 1.0f - (i / 8.0f);
                
                int alpha = (int)(255 * opacity);
                Color lineColor = Color.FromArgb(alpha, spinnerColor);

                float startX = centerX + (float)Math.Cos(angle) * (radius * 0.5f);
                float startY = centerY + (float)Math.Sin(angle) * (radius * 0.5f);
                float endX = centerX + (float)Math.Cos(angle) * radius;
                float endY = centerY + (float)Math.Sin(angle) * radius;

                using (Pen pen = new Pen(lineColor, 3))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    g.DrawLine(pen, startX, startY, endX, endY);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (animationTimer != null)
                {
                    animationTimer.Stop();
                    animationTimer.Dispose();
                    animationTimer = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}