using Ghostscript.NET;
using Ghostscript.NET.Processor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using UtilityPDF.Properties;


namespace UtilityPDF
{
    internal partial class FrmUtiPDF_Main : Form
    {
        private bool bConvert = false;
        private string LevelCompress = "/printer";
        private bool abortFlag = false;

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
                Btn_Reset.Enabled = true;
                Btn_SelectPDF.Enabled = false;
            }
        }
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            ToggleControlsExtract(false);

            string pdfPath = lbl_PDF.Text;
            string txtPath = lbl_TXT.Text;
            Btn_Reset.Enabled = false;

            string selectedLanguage = "eng";
            string selectedItem = cmbLangConv.SelectedItem.ToString();
            int index = selectedItem.IndexOf(" -");
            if (index > 0)
            {
                selectedLanguage = selectedItem.Substring(0, index).Trim();                
            }

            ExtractText.Execute(pdfPath, txtPath, selectedLanguage, DrawPercentage, () => abortFlag);
            ToggleControlsExtract(true);
        }
        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            lbl_PDF.Text = SettingsString.LblMsgInputPDF_Extr;
            lbl_TXT.Text = SettingsString.LblMsgOutputDIR_Extr;
            Btn_SelectDIROutputTXT.Enabled = false;
            Btn_SelectPDF.Enabled = true;
            Btn_Start.Enabled = false;
            Btn_Reset.Enabled = false;
            Btn_Abort.Enabled = false;
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
            PopulateComboLang();
            AssignControlTextxImg();
            AssignTextPosLblProgress();
        }
        private void Btn_SelectPDFToMerge_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                if (!Btn_ResetMerge.Enabled)
                    Btn_ResetMerge.Enabled = true;

                Lstb_FileMerge.Items.Add(oFD_PDF.FileName);

                if (Lstb_FileMerge.Items.Count == 2)
                    Btn_SelectDIROutputMergedPDF.Enabled = true;
            }
        }
        private void Btn_ResetMerge_Click(object sender, EventArgs e)
        {
            Lstb_FileMerge.Items.Clear();
            lbl_DIROutputMergePDF.Text = SettingsString.LblMsgOutputDIR_Merge;
            Btn_SelectDIROutputMergedPDF.Enabled = false;
            Btn_Merge.Enabled = false;
            Btn_ResetMerge.Enabled = false;
            Btn_SelectPDFToMerge.Enabled = true;
        }
        private async void Btn_Merge_Click(object sender, EventArgs e)
        {
            ToggleControlMerge(false);
            lbl_MergeInProgress.BringToFront();
            Application.DoEvents();
            string pdfPath = lbl_DIROutputMergePDF.Text;

            await Merge.Execute(pdfPath, Lstb_FileMerge.Items, lbl_MergeInProgress);

            ToggleControlMerge(true);
            lbl_MergeInProgress.SendToBack();
            Application.DoEvents();
            Btn_ResetMerge_Click(sender, e);
        }
        private void Btn_SelectDIROutputMergedPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(SettingsString.WarnSelectOutDirMERGE,
                                                        "Warning",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_DIROutputMergePDF.Text = fBD_TXT.SelectedPath
                                                 + @"\"
                                                 + Path.GetFileNameWithoutExtension(Lstb_FileMerge.Items[0].ToString())
                                                 + "_Merged.pdf";
                    Btn_SelectDIROutputMergedPDF.Enabled = false;
                    Btn_Merge.Enabled = true;
                    Btn_SelectPDFToMerge.Enabled = false;
                }
            }
        }
        private async void Btn_Compress_Click(object sender, EventArgs e)
        {
            ToggleControlCompress(false);
            lbl_CompressInProgress.BringToFront();
            Application.DoEvents();
            string pdfPath = lbl_PDFToCompress.Text;
            string outputPath = lbl_DIROutputCompressPDF.Text;

            await Compress.Execute(pdfPath, LevelCompress, outputPath, lbl_CompressInProgress);

            ToggleControlCompress(true);
            lbl_CompressInProgress.SendToBack();
            Application.DoEvents();
            Btn_ResetCompres_Click(sender, e);
        }
        private void Btn_ResetCompres_Click(object sender, EventArgs e)
        {
            lbl_PDFToCompress.Text = "PDF file to COMPRESS.";
            lbl_DIROutputCompressPDF.Text = "Directory Output Compressed PDF";
            lbl_ViewLvlCompres.Text = SettingsString.CompressLvl_1;
            Tb_Compress.Value = 1;
            Tb_Compress.Enabled = false;
            Btn_Compress.Enabled = false;
            Btn_ResetCompres.Enabled = false;
            Btn_SelectPDFToCompress.Enabled = true;
            Btn_SelectDIROutputCompressPDF.Enabled = false;
        }
        private void Btn_SelectDIROutputCompressPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(SettingsString.WarnSelectOutDirCOMP,
                                                        "Warning",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_DIROutputCompressPDF.Text = fBD_TXT.SelectedPath
                                                    + @"\"
                                                    + Path.GetFileNameWithoutExtension(lbl_PDFToCompress.Text)
                                                    + "_Compressed.pdf";
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
                Btn_ResetCompres.Enabled = true;
                Btn_SelectPDFToCompress.Enabled = false;
            }
        }
        private void Tb_Compress_ValueChanged(object sender, EventArgs e)
        {
            switch (Tb_Compress.Value)
            {
                case 0:
                    LevelCompress = "/prepress";
                    lbl_ViewLvlCompres.Text = SettingsString.CompressLvl_0;
                    break;

                case 1:
                    LevelCompress = "/printer";
                    lbl_ViewLvlCompres.Text = SettingsString.CompressLvl_1;
                    break;

                case 2:
                    LevelCompress = "/ebook";
                    lbl_ViewLvlCompres.Text = SettingsString.CompressLvl_2;
                    break;

                case 3:
                    LevelCompress = "/screen";
                    lbl_ViewLvlCompres.Text = SettingsString.CompressLvl_3;
                    break;
            }
        }
        private void DrawPercentage(int percentage)
        {
            if (pBProgressExtract.InvokeRequired)
            {
                pBProgressExtract.Invoke(new Action<int>(DrawPercentage), percentage);
            }
            else
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
        }
        private void FrmUtiPDF_Main_Shown(object sender, EventArgs e)
        {
            Btn_ResetMerge_Click(null, EventArgs.Empty);
            Btn_ResetCompres_Click(null, EventArgs.Empty);
            Btn_ResetConvert_Click(null, EventArgs.Empty);
            Btn_Reset_Click(null, EventArgs.Empty);            
            DrawPercentage(0);
        }
        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Btn_Abort_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(SettingsString.WarnConfirmAbort,
                                                        "Warning",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
                abortFlag = true;
        }
        private void ToggleControlsExtract(bool isEnabled)
        {
            if (isEnabled)
                Btn_Reset_Click(null, EventArgs.Empty);

            abortFlag = false;
            Btn_Abort.Enabled = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            EnabledDisabledPanel(isEnabled);
            Btn_Start.Enabled = false;
            cmbLangConv.Enabled = isEnabled;
            bConvert = !isEnabled;
        }

        private void EnabledDisabledPanel(bool isEnabled)
        {
            PnlMerge.Enabled = isEnabled;
            PnlCompress.Enabled = isEnabled;
            PnlConvert.Enabled = isEnabled;
        }

        private void ToggleControlMerge(bool isEnabled)
        {
            bConvert = !isEnabled;
            lbl_MergeInProgress.Visible = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            EnabledDisabledPanel(isEnabled);
        }
        private void ToggleControlCompress(bool isEnabled)
        {
            bConvert = !isEnabled;
            lbl_CompressInProgress.Visible = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            EnabledDisabledPanel(isEnabled);
        }

        private void ToggleControlConvert(bool isEnabled)
        {
            bConvert = !isEnabled;
            lbl_ConvertInProgress.Visible = !isEnabled;
            Btn_Exit.Enabled = isEnabled;
            EnabledDisabledPanel(isEnabled);
        }

        private void Btn_SelectDIROutputTXT_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(SettingsString.WarnSelectOutDirTXT,
                                                        "Warning",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_TXT.Text = fBD_TXT.SelectedPath
                                   + @"\"
                                   + Path.GetFileNameWithoutExtension(lbl_PDF.Text)
                                   + ".txt";
                    Btn_Start.Enabled = true;
                    Btn_SelectDIROutputTXT.Enabled = false;
                }
            }
            else
            {
                lbl_PDF.Text = SettingsString.LblMsgInputPDF_Extr;
                lbl_TXT.Text = SettingsString.LblMsgOutputDIR_Extr;
                Btn_SelectDIROutputTXT.Enabled = false;
                Btn_SelectPDF.Enabled = true;
            }
        }
        private void CalculatingCenter(Label lblName, Panel pnlName)
        {
            // Calcola la posizione centrale del pannello
            int centerX = pnlName.Width / 2;
            int centerY = pnlName.Height / 2;

            // Calcola la posizione della label per centrarla nel pannello
            lblName.Left = pnlName.Location.X + (centerX - (lblName.Width / 2));
            lblName.Top = pnlName.Location.Y + (centerY - (lblName.Height / 2));
        }
        private void AssignTextPosLblProgress()
        {
            // Le label di progressione vengono ridimensionate e poste al centro dei rispettvi pannelli
            lbl_CompressInProgress.Size = new Size(240, 110);
            lbl_CompressInProgress.Text = SettingsString.LblCompressInProgress;
            lbl_MergeInProgress.Size = new Size(240, 110);
            lbl_MergeInProgress.Text = SettingsString.LblMergeInProgress;
            lbl_ConvertInProgress.Size = new Size(240, 110);
            lbl_ConvertInProgress.Text = SettingsString.LblConvertInProgress;
            CalculatingCenter(lbl_CompressInProgress, PnlCompress);
            CalculatingCenter(lbl_MergeInProgress, PnlMerge);
            CalculatingCenter(lbl_ConvertInProgress, PnlConvert);            
        }
        private void AssignControlTextxImg()
        {
            // Assegna i testi/immagini ai vari controlli
            pB_ICO.Image = Resources.PDFUti;
            lblOCR.Text = SettingsString.LblPanelExtract;
            lblCompr.Text = SettingsString.LblPanelCompress;
            lblMerge.Text = SettingsString.LblPanelMerge;
            lbl_ConvDOCX.Text = SettingsString.lbl_ConvDOCX;
            lblLang.Text = SettingsString.LblMsgSelLang;
            lbl_LvlCompr.Text = SettingsString.LblCompressionLvl;
            Btn_SelectPDF.Text = SettingsString.TxtSelectPDFBtn;
            Btn_SelectPDFToCompress.Text = SettingsString.TxtSelectPDFBtn;
            Btn_SelectPDFToMerge.Text = SettingsString.TxtSelectPDFBtn;
            Btn_SelectPDFToConvert.Text = SettingsString.TxtSelectPDFBtn;
            Btn_Reset.Text = SettingsString.TxtResetBtn;
            Btn_ResetCompres.Text = SettingsString.TxtResetBtn;
            Btn_ResetMerge.Text = SettingsString.TxtResetBtn;
            Btn_ResetConvert.Text = SettingsString.TxtResetBtn;
            Btn_SelectDIROutputTXT.Text = SettingsString.TxtOutputDirBtn + "TXT";
            Btn_SelectDIROutputMergedPDF.Text = SettingsString.TxtOutputDirBtn + "PDF";
            Btn_SelectDIROutputCompressPDF.Text = SettingsString.TxtOutputDirBtn + "PDF";
            Btn_SelectDIROutputConvertPDF.Text = SettingsString.TxtOutputDirBtn + "PDF";
            Btn_Abort.Text = SettingsString.TxtAbortBtn;
            Btn_Compress.Text = SettingsString.TxtCompressBtn;
            Btn_Convert.Text = SettingsString.TxtConvertBtn;
            Btn_Start.Text = SettingsString.TxtExtractBtn;
            Btn_Merge.Text = SettingsString.TxtMergetBtn;
            Btn_Exit.Text = SettingsString.TxtExitBtn;
        }
        private void PopulateComboLang()
        {
            // Ottieni il percorso della directory dell'eseguibile
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Costruisci il percorso completo del file CSV
            string csvFilePath = Path.Combine(exeDirectory, SettingsString.trainerDataFolder, SettingsString.csvLangFilename);

            // Crea un'istanza della classe DataLoader e carica i dati
            var dataLoader = new DataLangLoader();
            var readOnlyDataList = dataLoader.LoadData(csvFilePath);

            // Ottieni tutti i file nella cartella tessdata (escludendo il file CSV)
            var files = dataLoader.GetFiles(exeDirectory, SettingsString.trainerDataFolder, SettingsString.csvLangFilename);

            // Cerca ogni file nella collection e popola la ComboBox
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var result = dataLoader.FindByParam3(readOnlyDataList, fileName);

                if (result != null)
                {
                    cmbLangConv.Items.Add($"{result.LangParam1} - {result.LangParam2}");
                }
            }
        }

        private async void Btn_Convert_Click(object sender, EventArgs e)
        {
            ToggleControlConvert(false);
            lbl_ConvertInProgress.BringToFront();
            Application.DoEvents();
            string pdfPath = lbl_PDFToConvert.Text;
            string outputPath = lbl_DIROutputConvertPDF.Text;

            await ConvertDOCX.Execute(pdfPath, outputPath, lbl_ConvertInProgress);

            ToggleControlConvert(true);
            lbl_ConvertInProgress.SendToBack();
            Application.DoEvents();
            Btn_ResetConvert_Click(sender, e);
        }

        private void Btn_SelectPDFToConvert_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                lbl_PDFToConvert.Text = oFD_PDF.FileName;
                Btn_SelectDIROutputConvertPDF.Enabled = true;
                Btn_ResetConvert.Enabled = true;
                Btn_SelectPDFToConvert.Enabled = false;
            }
        }

        private void Btn_SelectDIROutputConvertPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(SettingsString.WarnSelectOutDirDOCX,
                                            "Warning",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_DIROutputConvertPDF.Text = fBD_TXT.SelectedPath
                                                    + @"\"
                                                    + Path.GetFileNameWithoutExtension(lbl_PDFToConvert.Text)
                                                    + "_Convert.docx";
                    Btn_SelectDIROutputConvertPDF.Enabled = false;                    
                    Btn_Convert.Enabled = true;
                    Btn_SelectPDFToConvert.Enabled = false;
                }
            }
        }

        private void Btn_ResetConvert_Click(object sender, EventArgs e)
        {
            lbl_PDFToConvert.Text = "PDF file to DOCX CONVERT.";
            lbl_DIROutputConvertPDF.Text = "Directory Output DOCX";
            Btn_Convert.Enabled = false;
            Btn_ResetConvert.Enabled = false;
            Btn_SelectPDFToConvert.Enabled = true;
            Btn_SelectDIROutputConvertPDF.Enabled = false;
        }
    }
}