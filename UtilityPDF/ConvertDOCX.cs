using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class ConvertDOCX
    {
        public static async Task Execute(string pdfPath, string outputPath, Label lblProgress)
        {
            using (var colorFader = new ColorFader())
            {
                colorFader.StartFader(lblProgress);
                await Task.Run(() => StartExec(pdfPath, outputPath, colorFader));
            };
        }

        private static void StartExec(string pdfPath, string outputPath, ColorFader colorFader)
        {
            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor(SettingsString.gvi))
                {
                    List<string> switches = new List<string>
                    {
                        $"gs",
                        $"-sDEVICE=docxwrite",
                        $"-dNOPAUSE",
                        $"-dQUIET",
                        $"-sOutputFile={outputPath}",
                        $"{pdfPath}"
                    };
                    processor.StartProcessing(switches.ToArray(), null);
                }
                colorFader.StopFader();
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
    }
}
