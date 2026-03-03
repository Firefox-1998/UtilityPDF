using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Animated loading spinner control
    /// </summary>
    public class LoadingSpinner : Control
    {
        private Timer animationTimer;
        private int currentAngle = 0;
        private Color spinnerColor = Color.FromArgb(52, 152, 219);

        private const int SpinnerLines = 8;
        private const int AngleIncrement = 30;
        private const float RadiusMultiplier = 0.5f;

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

        /// <summary>
        /// Gets or sets the spinner color
        /// </summary>
        public Color SpinnerColor
        {
            get { return spinnerColor; }
            set
            {
                spinnerColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Starts the spinner animation
        /// </summary>
        public void Start()
        {
            Visible = true;
            animationTimer.Start();
        }

        /// <summary>
        /// Stops the spinner animation
        /// </summary>
        public void Stop()
        {
            animationTimer.Stop();
            Visible = false;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            currentAngle = (currentAngle + AngleIncrement) % 360;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = Math.Min(Width, Height) / 2 - 4;

            // Draw 8 rotating lines with fading opacity
            for (int i = 0; i < SpinnerLines; i++)
            {
                float angle = (currentAngle + i * 45) * (float)Math.PI / 180f;
                float opacity = 1.0f - (i / (float)SpinnerLines);

                int alpha = (int)(255 * opacity);
                Color lineColor = Color.FromArgb(alpha, spinnerColor);

                float innerRadius = radius * RadiusMultiplier;
                float startX = centerX + (float)Math.Cos(angle) * innerRadius;
                float startY = centerY + (float)Math.Sin(angle) * innerRadius;
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
