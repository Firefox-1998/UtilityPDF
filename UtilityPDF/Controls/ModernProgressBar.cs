using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Modern progress bar with animations and integrated percentage display
    /// </summary>
    public class ModernProgressBar : Control
    {
        private int value = 0;
        private int maximum = 100;
        private Color progressColor = Color.FromArgb(46, 204, 113);
        private Color backgroundColor = Color.FromArgb(236, 240, 241);
        private bool showPercentage = true;
        private int borderRadius = 10;
        private Timer animationTimer;
        private float currentWidth = 0f;
        private float targetWidth = 0f;

        private const float AnimationSpeed = 0.2f;
        private const float AnimationThreshold = 0.5f;
        private const int ColorDarkenAmount = 30;

        public ModernProgressBar()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            Height = 30;
            BackColor = Color.Transparent;

            animationTimer = new Timer { Interval = 16 }; // ~60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
        }

        /// <summary>
        /// Gets or sets the current progress value
        /// </summary>
        public int Value
        {
            get { return value; }
            set
            {
                this.value = Math.Max(0, Math.Min(maximum, value));
                targetWidth = (float)Width * this.value / maximum;

                if (!animationTimer.Enabled && Width > 0)
                {
                    animationTimer.Start();
                }
                else if (Width <= 0)
                {
                    // If control is not yet sized, update directly
                    currentWidth = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum value
        /// </summary>
        public int Maximum
        {
            get { return maximum; }
            set
            {
                maximum = Math.Max(1, value);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the progress color
        /// </summary>
        public Color ProgressColor
        {
            get { return progressColor; }
            set
            {
                progressColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the progress bar
        /// </summary>
        public Color ProgressBackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                backgroundColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to show the percentage text
        /// </summary>
        public bool ShowPercentage
        {
            get { return showPercentage; }
            set
            {
                showPercentage = value;
                Invalidate();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            float difference = targetWidth - currentWidth;

            if (Math.Abs(difference) < AnimationThreshold)
            {
                currentWidth = targetWidth;
                animationTimer.Stop();
            }
            else
            {
                currentWidth += difference * AnimationSpeed; // Smooth easing
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            DrawBackground(g);
            DrawProgress(g);
            DrawPercentageText(g);
        }

        private void DrawBackground(Graphics g)
        {
            using (GraphicsPath bgPath = GetRoundedRectPath(ClientRectangle, borderRadius))
            {
                using (SolidBrush bgBrush = new SolidBrush(backgroundColor))
                {
                    g.FillPath(bgBrush, bgPath);
                }
            }
        }

        private void DrawProgress(Graphics g)
        {
            // Draw progress only if width is greater than 2 pixels
            if (currentWidth <= 2)
            {
                return;
            }

            int progressWidth = Math.Max(1, (int)currentWidth);
            Rectangle progressRect = new Rectangle(0, 0, progressWidth, Height);

            using (GraphicsPath progressPath = GetRoundedRectPath(progressRect, borderRadius))
            {
                if (progressRect.Width > 0 && progressRect.Height > 0)
                {
                    DrawProgressGradient(g, progressPath, progressRect);
                    DrawGlossEffect(g, progressPath, progressWidth);
                }
                else
                {
                    // Fallback: use solid color if rectangle is too small
                    using (SolidBrush solidBrush = new SolidBrush(progressColor))
                    {
                        g.FillPath(solidBrush, progressPath);
                    }
                }
            }
        }

        private void DrawProgressGradient(Graphics g, GraphicsPath path, Rectangle rect)
        {
            Color gradientEnd = DarkenColor(progressColor, ColorDarkenAmount);

            using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                rect, progressColor, gradientEnd, LinearGradientMode.Horizontal))
            {
                g.FillPath(progressBrush, path);
            }
        }

        private void DrawGlossEffect(Graphics g, GraphicsPath path, int progressWidth)
        {
            Rectangle glossRect = new Rectangle(0, 0, progressWidth, Height / 2);

            if (glossRect.Width > 0 && glossRect.Height > 0)
            {
                using (LinearGradientBrush glossBrush = new LinearGradientBrush(
                    glossRect,
                    Color.FromArgb(80, 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    g.FillPath(glossBrush, path);
                }
            }
        }

        private void DrawPercentageText(Graphics g)
        {
            if (!showPercentage)
            {
                return;
            }

            string percentText = $"{(value * 100 / maximum)}%";

            using (Font font = new Font("Segoe UI", 10F, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(percentText, font);
                PointF textLocation = new PointF(
                    (Width - textSize.Width) / 2,
                    (Height - textSize.Height) / 2);

                // Text shadow for readability
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
                {
                    g.DrawString(percentText, font, shadowBrush,
                        textLocation.X + 1, textLocation.Y + 1);
                }

                using (SolidBrush textBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(percentText, font, textBrush, textLocation);
                }
            }
        }

        private static Color DarkenColor(Color color, int amount)
        {
            return Color.FromArgb(
                Math.Max(0, color.R - amount),
                Math.Max(0, color.G - amount),
                Math.Max(0, color.B - amount));
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (rect.Width < diameter || rect.Height < diameter || rect.Width <= 0 || rect.Height <= 0)
            {
                // If rectangle is too small, return a normal rectangle
                if (rect.Width > 0 && rect.Height > 0)
                {
                    path.AddRectangle(rect);
                }
                return path;
            }

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Recalculate target width when control is resized
            if (Width > 0 && maximum > 0)
            {
                targetWidth = (float)Width * value / maximum;
                currentWidth = targetWidth; // Update immediately without animation
            }

            Invalidate();
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
