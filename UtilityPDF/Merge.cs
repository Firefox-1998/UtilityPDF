using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    internal class Merge
    {
        public static async Task Execute(string pdfPath, ListBox.ObjectCollection items, Label lblProgress, LoadingSpinner spinner)
        {
            using ColorFader colorFader = new ColorFader();
            // Avvia lo spinner
            if (spinner != null)
            {
                colorFader.StartFader(spinner);
            }

            // Mostra la label
            if (lblProgress != null)
            {
                if (lblProgress.InvokeRequired)
                {
                    lblProgress.Invoke(new Action(() => lblProgress.Visible = true));
                }
                else
                {
                    lblProgress.Visible = true;
                }
            }

            await Task.Run(() => StartExec(pdfPath, items, colorFader, lblProgress));
        }

        private static void StartExec(string pdfPath, ListBox.ObjectCollection Lstb_FileMerge, ColorFader colorFader, Label lblProgress)
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
                colorFader.StopFader();

                // Mostra la MessageBox nel thread UI principale
                if (lblProgress != null && lblProgress.InvokeRequired)
                {
                    lblProgress.Invoke(new Action(() =>
                    {
                        MessageBox.Show(
                            lblProgress.FindForm(),
                            Strings.MergeCompleted,
                            Strings.MsgBoxInformationTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }));
                }
                else
                {
                    Form parentForm = lblProgress?.FindForm();
                    MessageBox.Show(
                        parentForm,
                        Strings.MergeCompleted,
                        Strings.MsgBoxInformationTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                colorFader.StopFader();
                DisplayError.ErrorIO(ex);
            }
            catch (Exception ex)
            {
                colorFader.StopFader();
                DisplayError.ErrorGeneric(ex);
            }
        }
    }
}
