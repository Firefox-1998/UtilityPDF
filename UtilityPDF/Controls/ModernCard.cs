using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Modern card control with shadow and rounded corners for elegant UI
    /// </summary>
    public class ModernCard : Panel
    {
        private int borderRadius = 12;
        private Color shadowColor = Color.FromArgb(50, 0, 0, 0);
        private int shadowSize = 8;
        private Color headerColor = Color.FromArgb(41, 128, 185);
        private string headerText = string.Empty;
        private Font headerFont = new Font("Segoe UI", 11F, FontStyle.Bold);

        private const int HeaderHeight = 40;
        private const int HeaderPadding = 15;
        private const int ColorLightenAmount = 20;
        private const string EmojiFontFamily = "Segoe UI Emoji";

        private static readonly Color BorderColor = Color.FromArgb(230, 230, 230);

        public ModernCard()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            BackColor = Color.White;
            Padding = new Padding(15, 50, 15, 15);
        }

        #region Properties

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

        /// <summary>
        /// Gets or sets the header background color
        /// </summary>
        public Color HeaderColor
        {
            get { return headerColor; }
            set
            {
                headerColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the header text
        /// </summary>
        public string HeaderText
        {
            get { return headerText; }
            set
            {
                headerText = value ?? string.Empty;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the header font (size and style are used, family is always Segoe UI Emoji for emoji support)
        /// </summary>
        public Font HeaderFont
        {
            get { return headerFont; }
            set
            {
                headerFont = value;
                Invalidate();
            }
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ConfigureGraphics(g);

            DrawShadow(g);
            DrawCardBody(g);
            DrawHeader(g);
        }

        private static void ConfigureGraphics(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        private void DrawShadow(Graphics g)
        {
            Rectangle shadowRect = new Rectangle(shadowSize, shadowSize, Width - shadowSize, Height - shadowSize);

            using GraphicsPath shadowPath = CreateRoundedRectPath(shadowRect, borderRadius);
            using PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath);
            shadowBrush.CenterColor = shadowColor;
            shadowBrush.SurroundColors = new[] { Color.Transparent };
            shadowBrush.FocusScales = new PointF(0.9f, 0.9f);
            g.FillPath(shadowBrush, shadowPath);
        }

        private void DrawCardBody(Graphics g)
        {
            Rectangle cardRect = new Rectangle(0, 0, Width - shadowSize - 2, Height - shadowSize - 2);

            using GraphicsPath cardPath = CreateRoundedRectPath(cardRect, borderRadius);
            using (SolidBrush backBrush = new SolidBrush(BackColor))
            {
                g.FillPath(backBrush, cardPath);
            }

            using Pen borderPen = new Pen(BorderColor, 1);
            g.DrawPath(borderPen, cardPath);
        }

        private void DrawHeader(Graphics g)
        {
            if (string.IsNullOrEmpty(headerText))
            {
                return;
            }

            Rectangle headerRect = new Rectangle(0, 0, Width - shadowSize - 2, HeaderHeight);

            using GraphicsPath headerPath = CreateRoundedRectPath(headerRect, borderRadius, topOnly: true);
            DrawHeaderBackground(g, headerPath, headerRect);
            DrawHeaderText(g, headerRect);
        }

        private void DrawHeaderBackground(Graphics g, GraphicsPath path, Rectangle rect)
        {
            Color lighterColor = LightenColor(headerColor, ColorLightenAmount);

            using LinearGradientBrush headerBrush = new LinearGradientBrush(
                rect, headerColor, lighterColor, LinearGradientMode.Horizontal);
            g.FillPath(headerBrush, path);
        }

        private void DrawHeaderText(Graphics g, Rectangle headerRect)
        {
            Rectangle textRect = new Rectangle(HeaderPadding, 0, headerRect.Width - (HeaderPadding * 2), HeaderHeight);

            // Always use Segoe UI Emoji to support emoji characters in header text
            // Use size and style from headerFont property
            float fontSize = headerFont?.Size ?? 11F;
            FontStyle fontStyle = headerFont?.Style ?? FontStyle.Bold;

            using Font emojiFont = new Font(EmojiFontFamily, fontSize, fontStyle);
            using StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            using SolidBrush textBrush = new SolidBrush(Color.White);
            g.DrawString(headerText, emojiFont, textBrush, textRect, sf);
        }

        #endregion

        #region Helper Methods

        private static Color LightenColor(Color color, int amount)
        {
            return Color.FromArgb(
                Math.Min(255, color.R + amount),
                Math.Min(255, color.G + amount),
                Math.Min(255, color.B + amount));
        }

        private static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius, bool topOnly = false)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Top-left arc
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Top-right arc
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);

            if (topOnly)
            {
                // Right side, bottom, left side (straight lines)
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius);
            }
            else
            {
                // Bottom-right arc
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                // Bottom-left arc
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            }

            path.CloseFigure();
            return path;
        }

        #endregion

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}
