using Ghostscript.NET;
using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class Compress
    {
        private static readonly string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
        private static readonly GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);

        public static void Execute(string pdfPath, string LevelCompress, string outputPath)
        {
            StartExec(pdfPath, LevelCompress, outputPath);
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
                MessageBox.Show(ResourceString.CompressCompleted, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
