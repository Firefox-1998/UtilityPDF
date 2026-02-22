using Freeware;
using Spire.Doc;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Operations;
using UtilityPDF.Resources;
using UtilityPDF.UI;

namespace UtilityPDF.Processing
{
    /// <summary>
    /// Handles PDF to DOCX/RTF conversion operations
    /// </summary>
    internal sealed class ConvertOperation : AsyncOperationBase
    {
        /// <summary>
        /// Executes the PDF to DOCX conversion operation
        /// </summary>
        public async Task ExecuteAsync(string pdfPath, string outputPath, Label lblProgress, LoadingSpinner spinner, OutputFormat format)
        {
            await ExecuteWithSpinnerAsync(lblProgress, spinner, () =>
            {
                PerformConversion(pdfPath, outputPath, format);
                ShowCompletionMessage(Strings.ConvertCompleted);
            });
        }

        private static void PerformConversion(string pdfPath, string outputPath, OutputFormat format)
        {
            ConvertPdfToDocx(pdfPath, outputPath);
            ProcessOutputFormat(outputPath, format);
        }

        private static void ConvertPdfToDocx(string pdfPath, string outputPath)
        {
            using (FileStream pdfStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read))
            {
                byte[] docxBytes = Pdf2Docx.Convert(pdfStream);

                using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    outputStream.Write(docxBytes, 0, docxBytes.Length);
                    outputStream.Flush();
                }
            }
        }

        private static void ProcessOutputFormat(string outputPath, OutputFormat format)
        {
            if (format == OutputFormat.Docx)
            {
                return;
            }

            ConvertToRtf(outputPath);

            if (format == OutputFormat.RtfOnly)
            {
                File.Delete(outputPath);
            }
        }

        private static void ConvertToRtf(string filePath)
        {
            using (Document document = new Document())
            {
                document.LoadFromFile(filePath);
                document.SaveToFile(filePath, FileFormat.Rtf);
            }
        }
    }

    /// <summary>
    /// Static entry point for backward compatibility
    /// </summary>
    internal static class ConvertDOCX
    {
        public static async Task Execute(string pdfPath, string outputPath, Label lblProgress, LoadingSpinner spinner, int formatOutput)
        {
            ConvertOperation operation = new ConvertOperation();
            OutputFormat format = (OutputFormat)formatOutput;
            await operation.ExecuteAsync(pdfPath, outputPath, lblProgress, spinner, format);
        }
    }
}
