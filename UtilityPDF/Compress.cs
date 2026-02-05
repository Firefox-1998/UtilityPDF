using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Resources;
using UtilityPDF.UI;

namespace UtilityPDF
{
    /// <summary>
    /// Handles PDF compression operations
    /// </summary>
    internal class Compress
    {
        /// <summary>
        /// Executes the PDF compression operation
        /// </summary>
        public static async Task Execute(string pdfPath, string levelCompress, string outputPath, Label lblProgress, LoadingSpinner spinner)
        {
            using (ColorFader colorFader = new ColorFader())
            {
                UIHelper.StartSpinner(spinner, colorFader);
                UIHelper.SetLabelVisibility(lblProgress, true);

                await Task.Run(() => StartExec(pdfPath, levelCompress, outputPath, colorFader, lblProgress));
            }
        }

        /// <summary>
        /// Performs the actual compression process
        /// </summary>
        private static void StartExec(string pdfPath, string levelCompress, string outputPath, ColorFader colorFader, Label lblProgress)
        {
            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor(SettingsString.Gvi))
                {
                    List<string> switches = new List<string>
                    {
                        "gs",
                        "-sDEVICE=pdfwrite",
                        $"-dPDFSETTINGS={levelCompress}",
                        "-dNOPAUSE",
                        "-dQUIET",
                        "-dCompressFonts=true",
                        "-dCompressStreams=true",
                        "-dDetectDuplicateImages=true",
                        $"-sOutputFile={outputPath}",
                        "-dColorImageDownsampleType=/Bicubic",
                        "-dColorImageResolution=150",
                        "-dGrayImageDownsampleType=/Bicubic",
                        "-dGrayImageResolution=150",
                        "-dMonoImageDownsampleType=/Bicubic",
                        "-dMonoImageResolution=150",
                        pdfPath
                    };
                    processor.StartProcessing(switches.ToArray(), null);
                }

                UIHelper.StopSpinner(colorFader);
                UIHelper.ShowMessageBox(lblProgress, Strings.CompressCompleted, Strings.MsgBoxInformationTitle);
            }
            catch (IOException ex)
            {
                UIHelper.StopSpinner(colorFader);
                DisplayError.ErrorIO(ex);
            }
            catch (Exception ex)
            {
                UIHelper.StopSpinner(colorFader);
                DisplayError.ErrorGeneric(ex);
            }
        }
    }
}
