using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
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
    /// Handles PDF merge operations
    /// </summary>
    internal class Merge
    {
        /// <summary>
        /// Executes the PDF merge operation
        /// </summary>
        public static async Task Execute(string outputPath, ListBox.ObjectCollection items, Label lblProgress, LoadingSpinner spinner)
        {
            using (ColorFader colorFader = new ColorFader())
            {
                UIHelper.StartSpinner(spinner, colorFader);
                UIHelper.SetLabelVisibility(lblProgress, true);

                await Task.Run(() => MergePdfFiles(outputPath, items, colorFader, lblProgress));
            }
        }

        private static void MergePdfFiles(string outputPath, ListBox.ObjectCollection filePaths, ColorFader colorFader, Label lblProgress)
        {
            try
            {
                using (PdfDocument outputDocument = new PdfDocument())
                {
                    ConfigureOutputDocument(outputDocument);
                    AddPagesFromFiles(outputDocument, filePaths);
                    outputDocument.Save(outputPath);
                }

                UIHelper.StopSpinner(colorFader);
                UIHelper.ShowMessageBox(lblProgress, Strings.MergeCompleted, Strings.MsgBoxInformationTitle);
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
}
