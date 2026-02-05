using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Barra di progresso moderna con animazioni e percentuale integrata
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
                    // Se il controllo non è ancora dimensionato, aggiorna direttamente
                    currentWidth = 0;
                }
            }
        }

        public int Maximum
        {
            get { return maximum; }
            set { maximum = Math.Max(1, value); Invalidate(); }
        }

        public Color ProgressColor
        {
            get { return progressColor; }
            set { progressColor = value; Invalidate(); }
        }

        public Color ProgressBackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; Invalidate(); }
        }

        public bool ShowPercentage
        {
            get { return showPercentage; }
            set { showPercentage = value; Invalidate(); }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            float difference = targetWidth - currentWidth;
            
            if (Math.Abs(difference) < 0.5f)
            {
                currentWidth = targetWidth;
                animationTimer.Stop();
            }
            else
            {
                currentWidth += difference * 0.2f; // Smooth easing
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Disegna background
            using (GraphicsPath bgPath = GetRoundedRectPath(ClientRectangle, borderRadius))
            {
                using (SolidBrush bgBrush = new SolidBrush(backgroundColor))
                {
                    g.FillPath(bgBrush, bgPath);
                }
            }

            // Disegna progress solo se la larghezza è maggiore di 2 pixel
            if (currentWidth > 2)
            {
                // Assicura che il rettangolo abbia dimensioni valide
                int progressWidth = Math.Max(1, (int)currentWidth);
                Rectangle progressRect = new Rectangle(0, 0, progressWidth, Height);
                
                using (GraphicsPath progressPath = GetRoundedRectPath(progressRect, borderRadius))
                {
                    // Verifica che il rettangolo sia valido per il LinearGradientBrush
                    if (progressRect.Width > 0 && progressRect.Height > 0)
                    {
                        using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                            progressRect,
                            progressColor,
                            Color.FromArgb(
                                Math.Max(0, progressColor.R - 30),
                                Math.Max(0, progressColor.G - 30),
                                Math.Max(0, progressColor.B - 30)),
                            LinearGradientMode.Horizontal))
                        {
                            g.FillPath(progressBrush, progressPath);
                        }

                        // Effetto brillantezza
                        Rectangle glossRect = new Rectangle(0, 0, progressWidth, Height / 2);
                        if (glossRect.Width > 0 && glossRect.Height > 0)
                        {
                            using (LinearGradientBrush glossBrush = new LinearGradientBrush(
                                glossRect,
                                Color.FromArgb(80, 255, 255, 255),
                                Color.FromArgb(0, 255, 255, 255),
                                LinearGradientMode.Vertical))
                            {
                                g.FillPath(glossBrush, progressPath);
                            }
                        }
                    }
                    else
                    {
                        // Fallback: usa un colore solido se il rettangolo è troppo piccolo
                        using (SolidBrush solidBrush = new SolidBrush(progressColor))
                        {
                            g.FillPath(solidBrush, progressPath);
                        }
                    }
                }
            }

            // Disegna percentuale
            if (showPercentage)
            {
                string percentText = $"{(value * 100 / maximum)}%";
                using (Font font = new Font("Segoe UI", 10F, FontStyle.Bold))
                {
                    SizeF textSize = g.MeasureString(percentText, font);
                    PointF textLocation = new PointF(
                        (Width - textSize.Width) / 2,
                        (Height - textSize.Height) / 2);

                    // Ombra testo per leggibilità
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
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (rect.Width < diameter || rect.Height < diameter || rect.Width <= 0 || rect.Height <= 0)
            {
                // Se il rettangolo è troppo piccolo, restituisci un rettangolo normale
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
            
            // Ricalcola la larghezza target quando il controllo viene ridimensionato
            if (Width > 0 && maximum > 0)
            {
                targetWidth = (float)Width * value / maximum;
                currentWidth = targetWidth; // Aggiorna immediatamente senza animazione
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