using Freeware;
using Spire.Doc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Resources;
using UtilityPDF.UI;

namespace UtilityPDF
{
    /// <summary>
    /// Handles PDF to DOCX/RTF conversion operations
    /// </summary>
    internal static class ConvertDOCX
    {
        /// <summary>
        /// Executes the PDF to DOCX conversion operation
        /// </summary>
        public static async Task Execute(string pdfPath, string outputPath, Label lblProgress, LoadingSpinner spinner, int formatOutput)
        {
            OutputFormat format = (OutputFormat)formatOutput;

            using (ColorFader colorFader = new ColorFader())
            {
                UIHelper.StartSpinner(spinner, colorFader);
                UIHelper.SetLabelVisibility(lblProgress, true);

                await Task.Run(() => PerformConversion(pdfPath, outputPath, colorFader, format, lblProgress));
            }
        }

        private static void PerformConversion(string pdfPath, string outputPath, ColorFader colorFader, OutputFormat format, Label lblProgress)
        {
            try
            {
                ConvertPdfToDocx(pdfPath, outputPath);
                ProcessOutputFormat(outputPath, format);

                UIHelper.StopSpinner(colorFader);
                UIHelper.ShowMessageBox(lblProgress, Strings.ConvertCompleted, Strings.MsgBoxInformationTitle);
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
}
