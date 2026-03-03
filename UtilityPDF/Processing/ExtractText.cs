using Freeware;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Tesseract;
using UtilityPDF.Configuration;
using UtilityPDF.Resources;
using UtilityPDF.UI;

namespace UtilityPDF.Processing
{
    /// <summary>
    /// Handles PDF text extraction using OCR
    /// </summary>
    internal class ExtractText
    {
        private readonly Action<int> updateProgressBar;
        private readonly Func<bool> shouldAbort;
        private readonly Action onOperationCompleted;
        private readonly Control uiControl;
        private readonly SynchronizationContext syncContext;

        private const int DefaultDpi = 300;
        private const int PagesPerFlush = 5; // Flush to disk every N pages

        private ExtractText(Action<int> updateProgressBar, Func<bool> shouldAbort,
                            Action onOperationCompleted, Control uiControl)
        {
            this.updateProgressBar = updateProgressBar;
            this.shouldAbort = shouldAbort;
            this.onOperationCompleted = onOperationCompleted;
            this.uiControl = uiControl;
            this.syncContext = SynchronizationContext.Current;
        }

        /// <summary>
        /// Executes the text extraction operation
        /// </summary>
        public static void Execute(string pdfPath, string txtPath, string selectedLanguage,
                                   Action<int> updateProgressBar, Func<bool> shouldAbort,
                                   Action onOperationCompleted, Control uiControl)
        {
            ExtractText extractor = new ExtractText(updateProgressBar, shouldAbort, onOperationCompleted, uiControl);
            extractor.StartExtraction(pdfPath, txtPath, selectedLanguage);
        }

        private void StartExtraction(string pdfPath, string txtPath, string selectedLanguage)
        {
            ClearOutputFile(txtPath);

            DisplayError.LogOperation(Strings.Log_TextExtraction, Strings.Log_Started, new Dictionary<string, string>
            {
                { Strings.Log_PdfPath, pdfPath },
                { Strings.Log_OutputPath, txtPath },
                { Strings.Log_OcrLanguage, selectedLanguage }
            });

            try
            {
                int pageCount = GetPageCount(pdfPath);

                DisplayError.LogInfo(string.Format(Strings.Log_PageProcess, pageCount));

                using (TesseractEngine engine = new TesseractEngine(
                    $@"./{SettingsString.TrainerDataFolder}", selectedLanguage, EngineMode.LstmOnly))
                {
                    using FileStream pdfStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    ProcessAllPages(pdfStream, pageCount, engine, txtPath);
                }

                NotifyCompleted();
                DisplayError.LogOperation(Strings.Log_TextExtraction, Strings.Log_Completed, new Dictionary<string, string>
                {
                    { Strings.Log_PdfPath, pdfPath },
                    { Strings.Log_TotalPages, pageCount.ToString() }
                });
                ShowCompletionMessage();
            }
            catch (IOException ex)
            {
                NotifyCompleted();
                DisplayError.LogCustomError(string.Format(Strings.Log_IOErrorTextExtr, ex.Message), new Dictionary<string, string>
                {
                    { Strings.Log_PdfPath, pdfPath },
                    { Strings.Log_ErrorType, "IOException" }
                });
                DisplayError.ErrorIO(ex);
            }
            catch (Exception ex)
            {
                NotifyCompleted();
                DisplayError.LogCustomError(string.Format(Strings.Log_UnexErrTextExt, ex.Message), new Dictionary<string, string>
                {
                    { Strings.Log_PdfPath, pdfPath },
                    { Strings.Log_ErrorType, ex.GetType().Name }
                });
                DisplayError.ErrorGeneric(ex);
            }
        }

        private void NotifyCompleted()
        {
            if (syncContext != null)
            {
                syncContext.Post(_ => onOperationCompleted?.Invoke(), null);
            }
            else
            {
                onOperationCompleted?.Invoke();
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
            StringBuilder textBuffer = new StringBuilder();

            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                if (shouldAbort())
                {
                    // Flush remaining text before aborting
                    FlushTextToFile(txtPath, textBuffer);
                    break;
                }

                string pageText = ProcessSinglePage(pdfStream, pageIndex, engine);
                textBuffer.Append(pageText);

                // Flush to disk periodically to avoid memory buildup
                if ((pageIndex + 1) % PagesPerFlush == 0)
                {
                    FlushTextToFile(txtPath, textBuffer);
                }

                ReportProgress(pageIndex + 1, pageCount);
            }

            // Flush any remaining text
            FlushTextToFile(txtPath, textBuffer);
        }

        private static void FlushTextToFile(string txtPath, StringBuilder buffer)
        {
            if (buffer.Length > 0)
            {
                File.AppendAllText(txtPath, buffer.ToString());
                buffer.Clear();
            }
        }

        private string ProcessSinglePage(Stream pdfStream, int pageIndex, TesseractEngine engine)
        {
            // Convert PDF page to PNG (1-based index for Pdf2Png)
            byte[] pageImage = Pdf2Png.Convert(pdfStream, pageIndex + 1, DefaultDpi);

            using MemoryStream imageStream = new MemoryStream(pageImage);
            using Image image = Image.FromStream(imageStream);
            return ExtractTextFromImage((Bitmap)image, engine);
        }

        private static string ExtractTextFromImage(Bitmap bitmap, TesseractEngine engine)
        {
            using Pix pixImage = PixConverter.ToPix(bitmap);
            using Pix grayImage = pixImage.ConvertRGBToGray();
            using Page ocrPage = engine.Process(grayImage);
            return ocrPage.GetText();
        }

        private void ReportProgress(int currentPage, int totalPages)
        {
            int percentage = currentPage * 100 / totalPages;

            // Use SynchronizationContext if available, otherwise invoke directly
            if (syncContext != null)
            {
                syncContext.Post(_ => updateProgressBar(percentage), null);
            }
            else
            {
                updateProgressBar(percentage);
            }
        }

        private void ShowCompletionMessage()
        {
            string message = shouldAbort() ? Strings.WarnAbortedExtraction : Strings.InfoCompleteExtraction;
            string title = shouldAbort() ? Strings.MsgBoxWarningTitle : Strings.MsgBoxInformationTitle;
            MessageBoxIcon icon = shouldAbort()
                ? MessageBoxIcon.Warning
                : MessageBoxIcon.Information;

            UIHelper.ShowMessageBox(uiControl, message, title, MessageBoxButtons.OK, icon);
        }
    }
}
