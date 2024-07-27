using Freeware;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;
using UtilityPDF.Properties;


namespace UtilityPDF
{
    public partial class FrmUtiPDF_Main : Form
    {
        private bool bConvert = false;
        private string LevelCompress = "/prepress";
        
        private bool abortFlag = false;

        //Get, to resources, string message.
        private static readonly string WarnSelectOutDirTXT = Resources.WarnSelectOutDirTXT;
        private static readonly string WarnAbortedExtraction = Resources.ConvertAborted;
        private static readonly string InfoCompleteExtraction = Resources.ConvertCompleted;
        private static readonly string WarnSelectOutDirMERGE = Resources.WarnSelectOutDirMERGE;     
        private static readonly string WarnSelectOutDirCOMP = Resources.WarnSelectOutDirCOMP;
        private static readonly string WarnConfirmAbort = Resources.WarnConfirmAbort;
        private static readonly string CompressLvl_0 = Resources.CompressLvl_0;
        private static readonly string CompressLvl_1 = Resources.CompressLvl_1;
        private static readonly string CompressLvl_2 = Resources.CompressLvl_2;
        private static readonly string CompressLvl_3 = Resources.CompressLvl_3;
        private static readonly string LblMsgInputPDF_Extr = Resources.LblMsgInputPDF_Extr;
        private static readonly string LblMsgOutputDIR_Extr = Resources.LblMsgOutputDIR_Extr;
        private static readonly string LblMsgOutputDIR_Merge = Resources.LblMsgOutputDIR_Merge;
        private static readonly string LblMsgSelLang = Resources.LblMsgSelLang;
        private static readonly string TxtSelectPDFBtn = Resources.TxtSelectPDFBtn;
        private static readonly string TxtResetBtn = Resources.TxtResetBtn;
        private static readonly string TxtOutputDirBtn = Resources.TxtOutputDirBtn;

        public FrmUtiPDF_Main()
        {
            InitializeComponent();
        }

        private void Btn_SelectPDF_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                lbl_PDF.Text = oFD_PDF.FileName;
                Btn_SelectDIROutputTXT.Enabled = true;
                Btn_SelectPDF.Enabled = false;
            }
        }

        private void Btn_SelectTXT_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(WarnSelectOutDirTXT, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_TXT.Text = fBD_TXT.SelectedPath + @"\" + Path.GetFileNameWithoutExtension(lbl_PDF.Text) + ".txt";
                    Btn_SelectDIROutputTXT.Enabled = false;
                    Btn_Start.Enabled = true;
                }
            }
            else
            {
                lbl_PDF.Text = LblMsgInputPDF_Extr;
                lbl_TXT.Text = LblMsgOutputDIR_Extr;
                Btn_SelectDIROutputTXT.Enabled = false;
                Btn_SelectPDF.Enabled = true;
            }
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            ToggleControlsExtract(false);

            string pdfPath = lbl_PDF.Text;
            string txtPath = lbl_TXT.Text;

            string selectedLanguage = cmbLangConv.SelectedValue.ToString();

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
                if (abortFlag)
                {
                    MessageBox.Show(WarnAbortedExtraction, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    abortFlag = false;
                }
                else
                {
                    MessageBox.Show(InfoCompleteExtraction, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            ToggleControlsExtract(true);
        }

        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            lbl_PDF.Text = LblMsgInputPDF_Extr;
            lbl_TXT.Text = LblMsgOutputDIR_Extr;
            Btn_SelectDIROutputTXT.Enabled = false;
            Btn_SelectPDF.Enabled = true;
            Btn_Start.Enabled = false;
            cmbLangConv.SelectedIndex = 0;
            DrawPercentage(0);
        }

        private void FrmUtiPDF_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bConvert)
            {
                e.Cancel = true;
            }
        }

        private void FrmUtiPDF_Main_Load(object sender, EventArgs e)
        {
            // Ottieni tutti i file nella directory
            string[] files = Directory.GetFiles(@"./tessdata");

            // Estrai i primi tre caratteri di ogni nome file
            List<string> languages = new List<string>();
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.Length >= 3)
                {
                    languages.Add(fileName.Substring(0, 3));
                }
            }

            // Popola la combobox con i valori estratti
            cmbLangConv.DataSource = languages;
        }

        private void Btn_SelectPDFToMerge_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                Lstb_FileMerge.Items.Add(oFD_PDF.FileName);
                if (Lstb_FileMerge.Items.Count == 2)
                {
                    Btn_SelectDIROutputMergedPDF.Enabled = true;
                }
            }
        }

        private void Btn_ResetMerge_Click(object sender, EventArgs e)
        {
            Lstb_FileMerge.Items.Clear();
            lbl_DIROutputMergePDF.Text = LblMsgOutputDIR_Merge;
            Btn_SelectDIROutputMergedPDF.Enabled = false;
            Btn_Merge.Enabled = false;
            Btn_SelectPDFToMerge.Enabled = true;
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            ToggleControlMerge(false);
            string pdfPath = lbl_DIROutputMergePDF.Text;

            Merge.Execute(pdfPath, Lstb_FileMerge.Items);

            ToggleControlMerge(true);
            Btn_ResetMerge_Click(sender, e);
        }

        private void Btn_SelectDIROutputMergedPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(WarnSelectOutDirMERGE, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_DIROutputMergePDF.Text = fBD_TXT.SelectedPath + @"\" + Path.GetFileNameWithoutExtension(Lstb_FileMerge.Items[0].ToString()) + "_Merged.pdf";
                    Btn_SelectDIROutputMergedPDF.Enabled = false;
                    Btn_Merge.Enabled = true;
                    Btn_SelectPDFToMerge.Enabled = false;
                }
            }
        }

        private void Btn_Compress_Click(object sender, EventArgs e)
        {
            ToggleControlCompress(false);

            string pdfPath = lbl_PDFToCompress.Text;
            string outputPath = lbl_DIROutputCompressPDF.Text;

            Compress.Execute(pdfPath, LevelCompress, outputPath);

            ToggleControlCompress(true);
            Btn_ResetCompres_Click(sender, e);
        }

        private void Btn_ResetCompres_Click(object sender, EventArgs e)
        {
            lbl_PDFToCompress.Text = "PDF file to COMPRESS.";
            lbl_DIROutputCompressPDF.Text = "Directory Output Compressed PDF";
            lbl_ViewLvlCompres.Text = CompressLvl_0;
            Tb_Compress.Value = 0;
            Tb_Compress.Enabled = false;
            Btn_Compress.Enabled = false;
            Btn_SelectPDFToCompress.Enabled = true;
            Btn_SelectDIROutputCompressPDF.Enabled = false;
        }

        private void Btn_SelectDIROutputCompressPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(WarnSelectOutDirCOMP, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_DIROutputCompressPDF.Text = fBD_TXT.SelectedPath + @"\" + Path.GetFileNameWithoutExtension(lbl_PDFToCompress.Text) + "_Compressed.pdf";
                    Btn_SelectDIROutputCompressPDF.Enabled = false;
                    Tb_Compress.Enabled = true;
                    Btn_Compress.Enabled = true;
                    Btn_SelectPDFToCompress.Enabled = false;
                }
            }
        }

        private void Btn_SelectPDFToCompress_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                lbl_PDFToCompress.Text = oFD_PDF.FileName;
                Btn_SelectDIROutputCompressPDF.Enabled = true;
                Btn_SelectPDFToCompress.Enabled = false;
            }
        }

        private void Tb_Compress_ValueChanged(object sender, EventArgs e)
        {
            switch (Tb_Compress.Value)
            {
                case 0:
                    LevelCompress = "/prepress";
                    lbl_ViewLvlCompres.Text = CompressLvl_0;
                    break;

                case 1:
                    LevelCompress = "/printer";
                    lbl_ViewLvlCompres.Text = CompressLvl_1;
                    break;

                case 2:
                    LevelCompress = "/ebook";
                    lbl_ViewLvlCompres.Text = CompressLvl_2;
                    break;

                case 3:
                    LevelCompress = "/screen";
                    lbl_ViewLvlCompres.Text = CompressLvl_3;
                    break;
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
                Application.DoEvents();
                ProcessPage(pdfStream, i, engine, txtPath);
                Application.DoEvents();
                DrawPercentage((i + 1) * 100 / numPages);

                // Verify abortFlag 
                if (abortFlag)
                {
                    // If abortFlag is "true" cancel extraction.
                    break;
                }
            }
        }

        private void DrawPercentage(int percentage)
        {
            // Ottieni un oggetto Graphics per il PictureBox
            Graphics g = pBProgressExtract.CreateGraphics();

            // Pulisci il PictureBox
            g.Clear(pBProgressExtract.BackColor);

            // Calcola la larghezza della barra di avanzamento
            int progressWidth = (int)(percentage / 100.0 * pBProgressExtract.Width);

            // Disegna la barra di avanzamento
            g.FillRectangle(Brushes.LightGreen, 0, 0, progressWidth, pBProgressExtract.Height);

            // Disegna il testo della percentuale
            string text = percentage + "%";
            Font boldFont = new Font(pBProgressExtract.Font.FontFamily, pBProgressExtract.Font.Size, FontStyle.Bold);
            SizeF textSize = g.MeasureString(text, boldFont);
            PointF location = new PointF((pBProgressExtract.Width - textSize.Width) / 2, (pBProgressExtract.Height - textSize.Height) / 2);
            g.DrawString(text, boldFont, Brushes.Black, location);

            // Clean
            g.Dispose();
            Application.DoEvents();
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

        private void FrmUtiPDF_Main_Shown(object sender, EventArgs e)
        {
            Btn_ResetMerge_Click(sender, e);
            Btn_ResetCompres_Click(sender, e);
            Btn_Reset_Click(sender, e);
            lblLang.Text = LblMsgSelLang;
            Btn_SelectPDF.Text = TxtSelectPDFBtn;
            Btn_SelectPDFToCompress.Text = TxtSelectPDFBtn;
            Btn_SelectPDFToMerge.Text = TxtSelectPDFBtn;
            Btn_Reset.Text = TxtResetBtn;
            Btn_ResetCompres.Text = TxtResetBtn;
            Btn_ResetMerge.Text = TxtResetBtn;
            Btn_SelectDIROutputTXT.Text = TxtOutputDirBtn + "TXT";
            Btn_SelectDIROutputMergedPDF.Text = TxtOutputDirBtn + "PDF";
            Btn_SelectDIROutputCompressPDF.Text = TxtOutputDirBtn + "PDF";
        }

        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Btn_Abort_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(WarnConfirmAbort, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
                abortFlag = true;
        }

        private void ToggleControlsExtract(bool isEnabled)
        {
            Btn_Abort.Enabled = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            PnlMerge.Enabled = isEnabled;
            PnlCompress.Enabled = isEnabled;
            Btn_Start.Enabled = isEnabled;
            Btn_Reset.Enabled = isEnabled;
            cmbLangConv.Enabled = isEnabled;
            bConvert = !isEnabled;
        }

        private void ToggleControlMerge(bool isEnabled)
        {
            bConvert = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            PnlMerge.Enabled = isEnabled;
            PnlCompress.Enabled = isEnabled;
            PnlOCR.Enabled = isEnabled;
        }

        private void ToggleControlCompress(bool isEnabled)
        {
            bConvert = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            PnlMerge.Enabled = isEnabled;
            PnlCompress.Enabled = isEnabled;
            PnlOCR.Enabled = isEnabled;
        }



        private void Btn_SelectDIROutputTXT_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(WarnSelectOutDirTXT, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_TXT.Text = fBD_TXT.SelectedPath + @"\" + Path.GetFileNameWithoutExtension(lbl_PDF.Text) + ".txt";
                    Btn_SelectDIROutputTXT.Enabled = false;
                    Btn_Start.Enabled = true;
                }
            }
            else
            {
                lbl_PDF.Text = LblMsgInputPDF_Extr;
                lbl_TXT.Text = LblMsgOutputDIR_Extr;
                Btn_SelectDIROutputTXT.Enabled = false;
                Btn_SelectPDF.Enabled = true;
            }
        }
    }
}