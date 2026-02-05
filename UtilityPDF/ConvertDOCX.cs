using Freeware;
using Spire.Doc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Controls;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    internal class ConvertDOCX
    {
        public static async Task Execute(string pdfPath, string outputPath, Label lblProgress, LoadingSpinner spinner, int formatOutput)
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

            await Task.Run(() => StartExec(pdfPath, outputPath, colorFader, formatOutput, lblProgress));
        }

        private static void StartExec(string pdfPath, string outputPath, ColorFader colorFader, int formatOutput, Label lblProgress)
        {
            try
            {
                // Utilizzo del blocco using per gestire lo stream
                using (Stream pdfStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read))
                {
                    // Conversione del file PDF in DOCX utilizzando lo stream
                    byte[] docx = Pdf2Docx.Convert(pdfStream);

                    // Scrittura del file DOCX
                    using FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                    outputStream.Write(docx, 0, docx.Length);
                    outputStream.Flush(); // Assicurati che tutti i dati siano scritti su disco
                }

                if (formatOutput == 1 || formatOutput == 2)
                {
                    using (Document document = new Document())
                    {
                        // Leggi il file DOCX                
                        document.LoadFromFile(outputPath);

                        // Salva il documento nel formato RTF
                        document.SaveToFile(outputPath, FileFormat.Rtf);
                    }

                    if (formatOutput == 1)
                    {
                        // Elimina il file DOCX
                        File.Delete(outputPath);
                    }
                }
                colorFader.StopFader();

                // Mostra la MessageBox nel thread UI principale
                if (lblProgress != null && lblProgress.InvokeRequired)
                {
                    lblProgress.Invoke(new Action(() =>
                    {
                        MessageBox.Show(
                            lblProgress.FindForm(),
                            Strings.ConvertCompleted,
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
                        Strings.ConvertCompleted,
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
