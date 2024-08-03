using Ghostscript.NET;
using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class Compress
    {
        private static readonly string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
        private static readonly GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);

        private static Timer progressTimer;
        private static Label progressLabel;
        private static int colorStep = 0;
        private static bool fadingOut = true;
        private static Color startColor;
        private static Color endColor;

        public static async Task Execute(string pdfPath, string LevelCompress, string outputPath, Label lblProgress)
        {
            progressLabel = lblProgress;
            InitializeTimer();
            InitializeColors();
            progressTimer.Start();

            await Task.Run(() => StartExec(pdfPath, LevelCompress, outputPath));

            progressTimer.Stop();
        }

        private static void StartExec(string pdfPath, string LevelCompress, string outputPath)
        {
            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor(gvi))
                {
                    List<string> switches = new List<string>
                    {
                        $"gs",
                        $"-sDEVICE=pdfwrite",
                        $"-dPDFSETTINGS={LevelCompress}",
                        $"-dNOPAUSE",
                        $"-dQUIET",
                        $"-sOutputFile={outputPath}",
                        $"{pdfPath}"
                    };
                    processor.StartProcessing(switches.ToArray(), null);
                }

                progressTimer.Stop();
                MessageBox.Show(SettingsString.CompressCompleted, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException ex)
            {
                // Display a more specific error message for IO exceptions
                DisplayError.ErrorIO(ex);
            }
            catch (Exception ex)
            {
                // Display the exception message
                DisplayError.ErrorGeneric(ex);
            }
        }
        private static void ProgressTimer_Tick(object sender, EventArgs e)
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
        private static void InitializeTimer()
        {
            progressTimer = new Timer
            {
                Interval = 100 // Cambia l'opacità ogni 100 ms                
            };
            progressTimer.Tick += ProgressTimer_Tick;
        }
        private static void InitializeColors()
        {
            startColor = Color.Black;
            endColor = progressLabel.BackColor;
        }
    }
}
