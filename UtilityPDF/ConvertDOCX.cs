﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilityPDF
{
    internal class ConvertDOCX
    {
        public static async Task Execute(string pdfPath, string outputPath, Label lblProgress)
        {
            using (var colorFader = new ColorFader())
            {
                colorFader.StartFader(lblProgress);
                await Task.Run(() => StartExec(pdfPath, outputPath, colorFader));
            }
        }

        private static void StartExec(string pdfPath, string outputPath, ColorFader colorFader)
        {
            try
            {
                using (PdfReader pdfReader = new PdfReader(pdfPath))
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                    {
                        ProcessPage(pdfDocument, page, body, mainPart);
                    }
                }
                colorFader.StopFader();
                MessageBox.Show(SettingsString.ConvertCompleted, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private static void ProcessPage(PdfDocument pdfDocument, int page, Body body, MainDocumentPart mainPart)
        {
            PdfPage pdfPage = pdfDocument.GetPage(page);
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            string text = PdfTextExtractor.GetTextFromPage(pdfPage, strategy);

            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text(text));

            IEventListener listener = new ImageRenderListener(body, mainPart);
            PdfCanvasProcessor processor = new PdfCanvasProcessor(listener);
            processor.ProcessPageContent(pdfPage);
        }
    }
}
