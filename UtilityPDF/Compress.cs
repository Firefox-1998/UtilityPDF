using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class Compress
    {


        public static async Task Execute(string pdfPath, string LevelCompress, string outputPath, Label lblProgress)
        {
            using (var colorFader = new ColorFader())
            {
                colorFader.StartFader(lblProgress);
                await Task.Run(() => StartExec(pdfPath, LevelCompress, outputPath, colorFader));
            };
        }

        private static void StartExec(string pdfPath, string LevelCompress, string outputPath, ColorFader colorFader)
        {
            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor(SettingsString.gvi))
                {
                    List<string> switches = new List<string>
                    {
                        $"gs",
                        $"-sDEVICE=pdfwrite",
                        $"-dPDFSETTINGS={LevelCompress}",
                        $"-dNOPAUSE",
                        $"-dQUIET",
                        $"-dCompressFonts=true",
                        $"-dCompressStreams=true",
                        $"-dDetectDuplicateImages=true",
                        $"-sOutputFile={outputPath}",
                        $"-dColorImageDownsampleType=/Bicubic",
                        $"-dColorImageResolution=150",
                        $"-dGrayImageDownsampleType=/Bicubic",
                        $"-dGrayImageResolution=150", 
                        $"-dMonoImageDownsampleType=/Bicubic",
                        $"-dMonoImageResolution=150",
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
