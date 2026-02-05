using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Modern button with hover effects and smooth animations
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

        private const float AnimationStep = 0.1f;
        private const int ColorDarkenAmount = 20;
        private const int BorderDarkenAmount = 40;

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

            AutoSize = false;
            UseCompatibleTextRendering = false;

            animationTimer = new Timer { Interval = 20 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        /// <summary>
        /// Gets or sets the normal state color
        /// </summary>
        public Color NormalColor
        {
            get { return normalColor; }
            set
            {
                normalColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the hover state color
        /// </summary>
        public Color HoverColor
        {
            get { return hoverColor; }
            set
            {
                hoverColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border radius
        /// </summary>
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                Invalidate();
            }
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
                animationProgress += AnimationStep;
            }
            else if (!isHovered && animationProgress > 0f)
            {
                animationProgress -= AnimationStep;
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
            g.Clear(Parent.BackColor); // Clear background completely

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Button rectangle with padding to avoid edge clipping
            Rectangle rect = new Rectangle(2, 2, Width - 4, Height - 4);

            // Determine color based on state
            Color currentColor = GetCurrentColor();

            // Draw light shadow
            if (Enabled)
            {
                DrawShadow(g, rect);
            }

            // Draw button with gradient
            DrawButton(g, rect, currentColor);

            // Draw text with word wrap
            DrawText(g, rect);
        }

        private Color GetCurrentColor()
        {
            if (!Enabled)
            {
                return disabledColor;
            }

            if (isPressed)
            {
                return pressedColor;
            }

            return InterpolateColor(normalColor, hoverColor, animationProgress);
        }

        private void DrawShadow(Graphics g, Rectangle rect)
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

        private void DrawButton(Graphics g, Rectangle rect, Color currentColor)
        {
            using (GraphicsPath path = GetRoundedRectPath(rect, borderRadius))
            {
                // Main background
                Color gradientEnd = DarkenColor(currentColor, ColorDarkenAmount);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect, currentColor, gradientEnd, LinearGradientMode.Vertical))
                {
                    g.FillPath(brush, path);
                }

                // Gloss effect on top half
                if (Enabled && !isPressed)
                {
                    DrawGlossEffect(g, rect);
                }

                // Thin border for definition
                Color borderColor = DarkenColor(currentColor, BorderDarkenAmount);

                using (Pen borderPen = new Pen(borderColor, 1f))
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    g.DrawPath(borderPen, path);
                }
            }
        }

        private void DrawGlossEffect(Graphics g, Rectangle rect)
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

        private void DrawText(Graphics g, Rectangle rect)
        {
            Rectangle textRect = new Rectangle(rect.X + 5, rect.Y + 3, rect.Width - 10, rect.Height - 6);

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter |
                                    TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.WordBreak |
                                    TextFormatFlags.EndEllipsis;

            // Text shadow for readability
            if (Enabled)
            {
                Rectangle shadowTextRect = new Rectangle(textRect.X, textRect.Y + 1, textRect.Width, textRect.Height);
                TextRenderer.DrawText(g, Text, Font, shadowTextRect,
                    Color.FromArgb(80, 0, 0, 0), flags);
            }

            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor, flags);
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

            // Ensure radius is not too large
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

        private static Color InterpolateColor(Color color1, Color color2, float progress)
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
