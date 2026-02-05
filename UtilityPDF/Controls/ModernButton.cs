using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Bottone moderno con effetti hover e animazioni fluide
    /// </summary>
    public class ModernButton : Button
    {
        private Color normalColor = Color.FromArgb(52, 152, 219);
        private Color hoverColor = Color.FromArgb(41, 128, 185);
        private Color pressedColor = Color.FromArgb(30, 100, 150);
        private Color disabledColor = Color.FromArgb(189, 195, 199);
        private int borderRadius = 8;
        private bool isHovered = false;
        private bool isPressed = false;
        private Timer animationTimer;
        private float animationProgress = 0f;

        public ModernButton()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            BackColor = Color.Transparent;
            ForeColor = Color.White;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            Font = new Font("Segoe UI Emoji", 8.5F, FontStyle.Regular);
            Cursor = Cursors.Hand;
            
            this.AutoSize = false;
            this.UseCompatibleTextRendering = false;

            animationTimer = new Timer { Interval = 20 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public Color NormalColor
        {
            get { return normalColor; }
            set { normalColor = value; Invalidate(); }
        }

        public Color HoverColor
        {
            get { return hoverColor; }
            set { hoverColor = value; Invalidate(); }
        }

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            animationTimer.Start();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            animationTimer.Start();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isPressed = false;
            Invalidate();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isHovered && animationProgress < 1f)
            {
                animationProgress += 0.1f;
            }
            else if (!isHovered && animationProgress > 0f)
            {
                animationProgress -= 0.1f;
            }
            else
            {
                animationTimer.Stop();
            }

            animationProgress = Math.Max(0f, Math.Min(1f, animationProgress));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.Clear(Parent.BackColor); // Pulisce completamente il background
            
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Rettangolo del bottone con padding per evitare clip sui bordi
            Rectangle rect = new Rectangle(2, 2, Width - 4, Height - 4);
            
            // Determina il colore basandosi sullo stato
            Color currentColor;
            if (!Enabled)
            {
                currentColor = disabledColor;
            }
            else if (isPressed)
            {
                currentColor = pressedColor;
            }
            else
            {
                currentColor = InterpolateColor(normalColor, hoverColor, animationProgress);
            }

            // Disegna ombra leggera
            if (Enabled)
            {
                Rectangle shadowRect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height);
                using (GraphicsPath shadowPath = GetRoundedRectPath(shadowRect, borderRadius))
                {
                    using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                    {
                        shadowBrush.CenterColor = Color.FromArgb(30, 0, 0, 0);
                        shadowBrush.SurroundColors = new[] { Color.Transparent };
                        g.FillPath(shadowBrush, shadowPath);
                    }
                }
            }

            // Disegna il bottone con gradient
            using (GraphicsPath path = GetRoundedRectPath(rect, borderRadius))
            {
                // Background principale
                Color gradientEnd = Color.FromArgb(
                    Math.Max(0, currentColor.R - 20),
                    Math.Max(0, currentColor.G - 20),
                    Math.Max(0, currentColor.B - 20));

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect, currentColor, gradientEnd, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, path);
                }

                // Effetto brillantezza nella parte superiore
                if (Enabled && !isPressed)
                {
                    Rectangle glossRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height / 2);
                    using (GraphicsPath glossPath = GetRoundedRectPath(glossRect, borderRadius))
                    {
                        using (LinearGradientBrush glossBrush = new LinearGradientBrush(
                            glossRect,
                            Color.FromArgb(30, 255, 255, 255),
                            Color.FromArgb(0, 255, 255, 255),
                            LinearGradientMode.Vertical))
                        {
                            g.FillPath(glossBrush, glossPath);
                        }
                    }
                }

                // Bordo sottile per definizione
                Color borderColor = Color.FromArgb(
                    Math.Max(0, currentColor.R - 40),
                    Math.Max(0, currentColor.G - 40),
                    Math.Max(0, currentColor.B - 40));

                using (Pen borderPen = new Pen(borderColor, 1f))
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    g.DrawPath(borderPen, path);
                }
            }

            // Disegna il testo con word wrap
            Rectangle textRect = new Rectangle(rect.X + 5, rect.Y + 3, rect.Width - 10, rect.Height - 6);
            
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | 
                                   TextFormatFlags.VerticalCenter | 
                                   TextFormatFlags.WordBreak |
                                   TextFormatFlags.EndEllipsis;
            
            // Ombra del testo per leggibilità
            if (Enabled)
            {
                Rectangle shadowTextRect = new Rectangle(textRect.X, textRect.Y + 1, textRect.Width, textRect.Height);
                TextRenderer.DrawText(g, Text, Font, shadowTextRect, 
                    Color.FromArgb(80, 0, 0, 0), flags);
            }

            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, flags);
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Assicura che il raggio non sia troppo grande
            diameter = Math.Min(diameter, Math.Min(rect.Width, rect.Height));
            
            if (diameter <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        private Color InterpolateColor(Color color1, Color color2, float progress)
        {
            int r = (int)(color1.R + (color2.R - color1.R) * progress);
            int g = (int)(color1.G + (color2.G - color1.G) * progress);
            int b = (int)(color1.B + (color2.B - color1.B) * progress);
            return Color.FromArgb(r, g, b);
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