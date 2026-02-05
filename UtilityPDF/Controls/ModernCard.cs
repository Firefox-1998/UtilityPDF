using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace UtilityPDF.Controls
{
    /// <summary>
    /// Controllo card moderno con ombra e bordi arrotondati per una UI più elegante
    /// </summary>
    public class ModernCard : Panel
    {
        private int borderRadius = 12;
        private Color shadowColor = Color.FromArgb(50, 0, 0, 0);
        private int shadowSize = 8;
        private Color headerColor = Color.FromArgb(41, 128, 185);
        private string headerText = string.Empty;
        private Font headerFont = new Font("Segoe UI", 11F, FontStyle.Bold);

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

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; Invalidate(); }
        }

        public Color HeaderColor
        {
            get { return headerColor; }
            set { headerColor = value; Invalidate(); }
        }

        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; Invalidate(); }
        }

        public Font HeaderFont
        {
            get { return headerFont; }
            set { headerFont = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Disegna ombra
            using (GraphicsPath shadowPath = GetRoundedRectPath(new Rectangle(shadowSize, shadowSize, 
                Width - shadowSize, Height - shadowSize), borderRadius))
            {
                using (PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath))
                {
                    shadowBrush.CenterColor = shadowColor;
                    shadowBrush.SurroundColors = new[] { Color.Transparent };
                    shadowBrush.FocusScales = new PointF(0.9f, 0.9f);
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            // Disegna card principale
            Rectangle cardRect = new Rectangle(0, 0, Width - shadowSize - 2, Height - shadowSize - 2);
            using (GraphicsPath cardPath = GetRoundedRectPath(cardRect, borderRadius))
            {
                g.FillPath(new SolidBrush(BackColor), cardPath);
                
                // Bordo sottile
                using (Pen borderPen = new Pen(Color.FromArgb(230, 230, 230), 1))
                {
                    g.DrawPath(borderPen, cardPath);
                }
            }

            // Disegna header colorato
            if (!string.IsNullOrEmpty(headerText))
            {
                Rectangle headerRect = new Rectangle(0, 0, Width - shadowSize - 2, 40);
                using (GraphicsPath headerPath = GetRoundedRectPath(headerRect, borderRadius, true))
                {
                    using (LinearGradientBrush headerBrush = new LinearGradientBrush(
                        headerRect, headerColor, Color.FromArgb(
                            Math.Min(255, headerColor.R + 20),
                            Math.Min(255, headerColor.G + 20),
                            Math.Min(255, headerColor.B + 20)), 
                        LinearGradientMode.Horizontal))
                    {
                        g.FillPath(headerBrush, headerPath);
                    }
                }

                // Testo header con font che supporta emoji
                using (Font emojiFont = new Font("Segoe UI Emoji", 11F, FontStyle.Bold))
                {
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Center
                        };
                        
                        g.DrawString(headerText, emojiFont, textBrush, 
                            new Rectangle(15, 0, Width - 30, 40), sf);
                    }
                }
            }
        }

        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius, bool topOnly = false)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (topOnly)
            {
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
                path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y + radius);
            }
            else
            {
                path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
                path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
                path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            }
            
            path.CloseFigure();
            return path;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            Invalidate();
        }
    }
}