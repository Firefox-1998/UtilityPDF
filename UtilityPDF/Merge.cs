using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class Merge
    {
        private static Timer progressTimer;
        private static Label progressLabel;
        private static int colorStep = 0;
        private static bool fadingOut = true;
        private static Color startColor;
        private static Color endColor;

        public static async Task Execute(string pdfPath, ListBox.ObjectCollection items, Label lblProgress)
        {
            progressLabel = lblProgress;
            InitializeTimer();
            InitializeColors();
            progressTimer.Start();

            await Task.Run(() => StartExec(pdfPath, items));

            progressTimer.Stop();
        }

        private static void StartExec(string pdfPath, ListBox.ObjectCollection Lstb_FileMerge)
        {
            try
            {
                using (PdfDocument outputDocument = new PdfDocument())
                {
                    outputDocument.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                    outputDocument.Options.NoCompression = false;
                    outputDocument.Options.CompressContentStreams = true;
                    outputDocument.Options.EnableCcittCompressionForBilevelImages = true;

                    foreach (string path in Lstb_FileMerge)
                    {
                        PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                        for (int i = 0; i < inputDocument.PageCount; i++)
                        {
                            outputDocument.AddPage(inputDocument.Pages[i]);
                        }
                    }

                    outputDocument.Save(pdfPath);
                }
                progressTimer.Stop();

                MessageBox.Show(SettingsString.MergeCompleted, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
