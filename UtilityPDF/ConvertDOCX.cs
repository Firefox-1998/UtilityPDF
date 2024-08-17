using Freeware;
using Spire.Doc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UtilityPDF
{
    internal class ConvertDOCX
    {
        public static async Task Execute(string pdfPath, string outputPath, Label lblProgress, int formatOutput)
        {
            using (var colorFader = new ColorFader())
            {
                colorFader.StartFader(lblProgress);
                await Task.Run(() => StartExec(pdfPath, outputPath, colorFader, formatOutput));
            }
        }

        private static void StartExec(string pdfPath, string outputPath, ColorFader colorFader, int formatOutput)
        {
            string docxInput = outputPath;
            string rtfOutput = outputPath.Replace("docx", "rtf");

            try
            {
                // Utilizzo del blocco using per gestire lo stream
                using (Stream pdfStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read))
                {
                    // Conversione del file PDF in DOCX utilizzando lo stream
                    byte[] docx = Pdf2Docx.Convert(pdfStream);

                    // Scrittura del file DOCX
                    using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        outputStream.Write(docx, 0, docx.Length);
                        outputStream.Flush(); // Assicurati che tutti i dati siano scritti su disco
                    }
                }

                if (formatOutput == 1 || formatOutput == 2)
                {
                    using (Document document = new Document())
                    {
                        // Leggi il file DOCX                
                        document.LoadFromFile(docxInput);

                        // Salva il documento nel formato RTF
                        document.SaveToFile(rtfOutput, FileFormat.Rtf);
                    }

                    if (formatOutput == 1)
                    {
                        // Elimina il file DOCX
                        File.Delete(docxInput);
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
     }
}
