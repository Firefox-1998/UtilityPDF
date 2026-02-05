using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Resources;
using UtilityPDF.Controls;

namespace UtilityPDF
{
    internal class Compress
    {
        public static async Task Execute(string pdfPath, string LevelCompress, string outputPath, Label lblProgress, LoadingSpinner spinner)
        {
            using (ColorFader colorFader = new ColorFader())
            {
                // Avvia lo spinner
                if (spinner != null)
                {
                    colorFader.StartFader(spinner);
                }
                
                // Mostra la label (il testo è già impostato nel Designer)
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

                await Task.Run(() => StartExec(pdfPath, LevelCompress, outputPath, colorFader, lblProgress));
            }
        }

        private static void StartExec(string pdfPath, string LevelCompress, string outputPath, ColorFader colorFader, Label lblProgress)
        {
            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor(SettingsString.gvi))
                {
                    List<string> switches = new List<string>
                    {
                        $"gs",
                        $"-sDEVICE=pdfwrite",
                        $"-dPDFSETTINGS={LevelCompress}",
                        $"-dNOPAUSE",
                        $"-dQUIET",
                        $"-dCompressFonts=true",
                        $"-dCompressStreams=true",
                        $"-dDetectDuplicateImages=true",
                        $"-sOutputFile={outputPath}",
                        $"-dColorImageDownsampleType=/Bicubic",
                        $"-dColorImageResolution=150",
                        $"-dGrayImageDownsampleType=/Bicubic",
                        $"-dGrayImageResolution=150", 
                        $"-dMonoImageDownsampleType=/Bicubic",
                        $"-dMonoImageResolution=150",
                        $"{pdfPath}"
                    };
                    processor.StartProcessing(switches.ToArray(), null);
                }
                colorFader.StopFader();

                // Mostra la MessageBox nel thread UI principale
                if (lblProgress != null && lblProgress.InvokeRequired)
                {
                    lblProgress.Invoke(new Action(() =>
                    {
                        MessageBox.Show(
                            lblProgress.FindForm(),
                            Strings.CompressCompleted,
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
                        Strings.CompressCompleted,
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
