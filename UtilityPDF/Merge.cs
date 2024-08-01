using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Windows.Forms;
using UtilityPDF.Properties;


namespace UtilityPDF
{
    internal class Merge
    {
        public static void Execute (string pdfPath, ListBox.ObjectCollection items)
        {
            StartExec(pdfPath, items);
        }

        private static void StartExec(string pdfPath, ListBox.ObjectCollection Lstb_FileMerge)
        {
            try
            {
                using (PdfDocument outputDocument = new PdfDocument())
                {
                    outputDocument.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                    outputDocument.Options.NoCompression = false;
                    outputDocument.Options.CompressContentStreams = true;
                    outputDocument.Options.EnableCcittCompressionForBilevelImages = true;

                    foreach (string path in Lstb_FileMerge)
                    {
                        PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                        for (int i = 0; i < inputDocument.PageCount; i++)
                        {
                            outputDocument.AddPage(inputDocument.Pages[i]);
                        }
                    }

                    outputDocument.Save(pdfPath);
                }
                MessageBox.Show(Settings.Default.MergeCompleted, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
