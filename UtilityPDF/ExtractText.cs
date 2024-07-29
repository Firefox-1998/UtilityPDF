using Freeware;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;

namespace UtilityPDF
{
    internal class ExtractText
    {
        private Action<int> updateProgressBar;
        private Func<bool> shouldAbort;

        public static void Execute(string pdfPath, string txtPath, string selectedLanguage,
                                   Action<int> updateProgressBar, Func<bool> shouldAbort)
        {
            ExtractText MyStartExec = new ExtractText();
            MyStartExec.StartExec(pdfPath,
                                  txtPath,
                                  selectedLanguage,
                                  updateProgressBar,
                                  shouldAbort);
        }

        private void StartExec(string pdfPath, string txtPath, string selectedLanguage, Action<int> updateProgressBar,
                               Func<bool> shouldAbort)
        {
            this.updateProgressBar = updateProgressBar;
            this.shouldAbort = shouldAbort;

            // Clear the content of the output file if it exists
            if (File.Exists(txtPath))
            {
                File.WriteAllText(txtPath, String.Empty);
            }

            try
            {
                int numPages = GetPageCount(pdfPath);
                using (var engine = new TesseractEngine(@"./tessdata", selectedLanguage, EngineMode.LstmOnly))
                {
                    using (Stream pdfStream = File.OpenRead(pdfPath))
                    {
                        ProcessPages(pdfStream, numPages, engine, txtPath);
                    }
                }
                if (shouldAbort())
                {
                    MessageBox.Show(ResourceString.WarnAbortedExtraction, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(ResourceString.InfoCompleteExtraction, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private int GetPageCount(string pdfPath)
        {
            using (PdfDocument inputDocument = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import))
            {
                return inputDocument.PageCount;
            }
        }

        private void ProcessPages(Stream pdfStream, int numPages, TesseractEngine engine, string txtPath)
        {
            for (int i = 0; i < numPages; i++)
            {
                ProcessPage(pdfStream, i, engine, txtPath);
                updateProgressBar((i + 1) * 100 / numPages);

                // Verify abortFlag 
                if (shouldAbort())
                {
                    // If abortFlag is "true" cancel extraction.
                    break;
                }
            }
        }
        private void ProcessPage(Stream pdfStream, int i, TesseractEngine engine, string txtPath)
        {
            byte[] page = Pdf2Png.Convert(pdfStream, i + 1, 300);
            Application.DoEvents();
            using (var ms = new MemoryStream(page))
            {
                Application.DoEvents();
                Image img = Image.FromStream(ms);

                Application.DoEvents();
                using (var imgPix = PixConverter.ToPix((Bitmap)img))
                {
                    var grayImage = imgPix.ConvertRGBToGray();

                    Application.DoEvents();
                    using (var imgPage = engine.Process(grayImage))
                    {
                        Application.DoEvents();
                        string text = imgPage.GetText();
                        Application.DoEvents();
                        File.AppendAllText(txtPath, text);
                        Application.DoEvents();
                    }
                }
            }
        }
    }
}
