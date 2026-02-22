using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Operations;
using UtilityPDF.Resources;

namespace UtilityPDF.Processing
{
    /// <summary>
    /// Handles PDF merge operations
    /// </summary>
    internal sealed class MergeOperation : AsyncOperationBase
    {
        /// <summary>
        /// Executes the PDF merge operation
        /// </summary>
        public async Task ExecuteAsync(string outputPath, ListBox.ObjectCollection items, Label lblProgress, LoadingSpinner spinner)
        {
            await ExecuteWithSpinnerAsync(lblProgress, spinner, () =>
            {
                PerformMerge(outputPath, items);
                ShowCompletionMessage(Strings.MergeCompleted);
            });
        }

        private static void PerformMerge(string outputPath, ListBox.ObjectCollection filePaths)
        {
            using (PdfDocument outputDocument = new PdfDocument())
            {
                ConfigureOutputDocument(outputDocument);
                AddPagesFromFiles(outputDocument, filePaths);
                outputDocument.Save(outputPath);
            }
        }

        private static void ConfigureOutputDocument(PdfDocument document)
        {
            document.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
            document.Options.NoCompression = false;
            document.Options.CompressContentStreams = true;
            document.Options.EnableCcittCompressionForBilevelImages = true;
        }

        private static void AddPagesFromFiles(PdfDocument outputDocument, ListBox.ObjectCollection filePaths)
        {
            foreach (string filePath in filePaths)
            {
                using (PdfDocument inputDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
                {
                    for (int pageIndex = 0; pageIndex < inputDocument.PageCount; pageIndex++)
                    {
                        outputDocument.AddPage(inputDocument.Pages[pageIndex]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Static entry point for backward compatibility
    /// </summary>
    internal static class Merge
    {
        public static async Task Execute(string outputPath, ListBox.ObjectCollection items, Label lblProgress, LoadingSpinner spinner)
        {
            MergeOperation operation = new MergeOperation();
            await operation.ExecuteAsync(outputPath, items, lblProgress, spinner);
        }
    }
}
