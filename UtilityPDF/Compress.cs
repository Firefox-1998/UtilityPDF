using Ghostscript.NET.Processor;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Operations;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    /// <summary>
    /// Handles PDF compression operations
    /// </summary>
    internal sealed class CompressOperation : AsyncOperationBase
    {
        /// <summary>
        /// Executes the PDF compression operation
        /// </summary>
        public async Task ExecuteAsync(string pdfPath, string levelCompress, string outputPath, Label lblProgress, LoadingSpinner spinner)
        {
            await ExecuteWithSpinnerAsync(lblProgress, spinner, () =>
            {
                PerformCompression(pdfPath, levelCompress, outputPath);
                ShowCompletionMessage(Strings.CompressCompleted);
            });
        }

        private static void PerformCompression(string pdfPath, string levelCompress, string outputPath)
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
        }
    }

    /// <summary>
    /// Static entry point for backward compatibility
    /// </summary>
    internal static class Compress
    {
        public static async Task Execute(string pdfPath, string levelCompress, string outputPath, Label lblProgress, LoadingSpinner spinner)
        {
            CompressOperation operation = new CompressOperation();
            await operation.ExecuteAsync(pdfPath, levelCompress, outputPath, lblProgress, spinner);
        }
    }
}
