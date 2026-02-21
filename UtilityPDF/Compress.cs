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
                PerformGhostscriptCompression(pdfPath, levelCompress, outputPath);
                ShowCompletionMessage(Strings.CompressCompleted);

            });
        }

        private static void PerformGhostscriptCompression(string pdfPath, string levelCompress, string outputPath)
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
                        "-dBATCH",
                        "-dCompatibilityLevel=1.5",
                        "-dAutoRotatePages=/None",
                        "-dCompressFonts=true",
                        "-dCompressStreams=true",
                        "-dDetectDuplicateImages=true",
                        "-dDownsampleColorImages=true",
                        "-dDownsampleGrayImages=true",
                        "-dDownsampleMonoImages=true",
                        "-dColorImageDownsampleType=/Bicubic",
                        "-dColorImageResolution=72",
                        "-dGrayImageDownsampleType=/Bicubic",
                        "-dGrayImageResolution=72",
                        "-dMonoImageDownsampleType=/Subsample",
                        "-dMonoImageResolution=72",
                        "-dPassThroughJPEGImages=false",
                        "-dAutoFilterColorImages=false",
                        "-dColorImageFilter=/DCTEncode",
                        "-dAutoFilterGrayImages=false",
                        "-dGrayImageFilter=/DCTEncode",
                        "-dSubsetFonts=true",
                        "-dEmbedAllFonts=false",
                        $"-sOutputFile={outputPath}",
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
