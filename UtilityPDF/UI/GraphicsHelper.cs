using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UtilityPDF.UI
{
    /// <summary>
    /// Helper class for common graphics operations
    /// </summary>
    internal static class GraphicsHelper
    {
        /// <summary>
        /// Creates a rounded rectangle path
        /// </summary>
        public static GraphicsPath CreateRoundedRectPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            if (rect.Width < diameter || rect.Height < diameter || rect.Width <= 0 || rect.Height <= 0)
            {
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

        /// <summary>
        /// Darkens a color by the specified amount
        /// </summary>
        public static Color DarkenColor(Color color, int amount)
        {
            return Color.FromArgb(
                color.A,
                Math.Max(0, color.R - amount),
                Math.Max(0, color.G - amount),
                Math.Max(0, color.B - amount));
        }

        /// <summary>
        /// Lightens a color by the specified amount
        /// </summary>
        public static Color LightenColor(Color color, int amount)
        {
            return Color.FromArgb(
                color.A,
                Math.Min(255, color.R + amount),
                Math.Min(255, color.G + amount),
                Math.Min(255, color.B + amount));
        }

        /// <summary>
        /// Interpolates between two colors based on progress (0.0 to 1.0)
        /// </summary>
        public static Color InterpolateColor(Color color1, Color color2, float progress)
        {
            progress = Math.Max(0f, Math.Min(1f, progress));

            int r = (int)(color1.R + (color2.R - color1.R) * progress);
            int g = (int)(color1.G + (color2.G - color1.G) * progress);
            int b = (int)(color1.B + (color2.B - color1.B) * progress);

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Configures graphics for high-quality rendering
        /// </summary>
        public static void ConfigureHighQuality(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
        }
    }
}
