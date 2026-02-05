using Freeware;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    /// <summary>
    /// Handles PDF text extraction using OCR
    /// </summary>
    internal class ExtractText
    {
        private readonly Action<int> updateProgressBar;
        private readonly Func<bool> shouldAbort;

        private const int DefaultDpi = 300;

        private ExtractText(Action<int> updateProgressBar, Func<bool> shouldAbort)
        {
            this.updateProgressBar = updateProgressBar;
            this.shouldAbort = shouldAbort;
        }

        /// <summary>
        /// Executes the text extraction operation
        /// </summary>
        public static void Execute(string pdfPath, string txtPath, string selectedLanguage,
                                   Action<int> updateProgressBar, Func<bool> shouldAbort)
        {
            ExtractText extractor = new ExtractText(updateProgressBar, shouldAbort);
            extractor.StartExtraction(pdfPath, txtPath, selectedLanguage);
        }

        private void StartExtraction(string pdfPath, string txtPath, string selectedLanguage)
        {
            ClearOutputFile(txtPath);

            try
            {
                int pageCount = GetPageCount(pdfPath);

                using (TesseractEngine engine = new TesseractEngine(
                    $@"./{SettingsString.TrainerDataFolder}", selectedLanguage, EngineMode.LstmOnly))
                {
                    using Stream pdfStream = File.OpenRead(pdfPath);
                    ProcessAllPages(pdfStream, pageCount, engine, txtPath);
                }

                ShowCompletionMessage();
            }
            catch (IOException ex)
            {
                DisplayError.ErrorIO(ex);
            }
            catch (Exception ex)
            {
                DisplayError.ErrorGeneric(ex);
            }
        }

        private static void ClearOutputFile(string txtPath)
        {
            if (File.Exists(txtPath))
            {
                File.WriteAllText(txtPath, string.Empty);
            }
        }

        private static int GetPageCount(string pdfPath)
        {
            using PdfDocument document = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Import);
            return document.PageCount;
        }

        private void ProcessAllPages(Stream pdfStream, int pageCount, TesseractEngine engine, string txtPath)
        {
            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                if (shouldAbort())
                {
                    break;
                }

                ProcessSinglePage(pdfStream, pageIndex, engine, txtPath);
                ReportProgress(pageIndex + 1, pageCount);
            }
        }

        private void ProcessSinglePage(Stream pdfStream, int pageIndex, TesseractEngine engine, string txtPath)
        {
            // Convert PDF page to PNG (1-based index for Pdf2Png)
            byte[] pageImage = Pdf2Png.Convert(pdfStream, pageIndex + 1, DefaultDpi);
            Application.DoEvents();

            using MemoryStream imageStream = new MemoryStream(pageImage);
            using Image image = Image.FromStream(imageStream);
            Application.DoEvents();
            string extractedText = ExtractTextFromImage((Bitmap)image, engine);
            File.AppendAllText(txtPath, extractedText);
            Application.DoEvents();
        }

        private static string ExtractTextFromImage(Bitmap bitmap, TesseractEngine engine)
        {
            using Pix pixImage = PixConverter.ToPix(bitmap);
            using Pix grayImage = pixImage.ConvertRGBToGray();
            Application.DoEvents();

            using Page ocrPage = engine.Process(grayImage);
            Application.DoEvents();
            return ocrPage.GetText();
        }

        private void ReportProgress(int currentPage, int totalPages)
        {
            int percentage = currentPage * 100 / totalPages;
            updateProgressBar(percentage);
        }

        private void ShowCompletionMessage()
        {
            if (shouldAbort())
            {
                MessageBox.Show(
                    Strings.WarnAbortedExtraction,
                    Strings.MsgBoxWarningTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(
                    Strings.InfoCompleteExtraction,
                    Strings.MsgBoxInformationTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
