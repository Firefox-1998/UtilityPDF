using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilityPDF.Resources;
using UtilityPDF.Controls;
using UtilityPDF.UI;

namespace UtilityPDF
{
    /// <summary>
    /// Main application form for PDF utility operations
    /// </summary>
    internal partial class FrmUtiPDF_Main : Form
    {
        #region Fields and Constants

        private bool isOperationInProgress;
        private string levelCompress = CompressionLevels.Default;
        private bool abortFlag;
        private readonly Dictionary<string, string> languageMapping = new Dictionary<string, string>();

        private const int MinFilesForMerge = 2;
        private const string DefaultOcrLanguage = "eng";
        private const int SpinnerVerticalOffset = 80;

        #endregion

        #region Compression Levels

        /// <summary>
        /// Contains compression level constants
        /// </summary>
        private static class CompressionLevels
        {
            public const string Prepress = "/prepress";
            public const string Printer = "/printer";
            public const string Ebook = "/ebook";
            public const string Screen = "/screen";
            public const string Default = Printer;
        }

        #endregion

        #region Constructor

        public FrmUtiPDF_Main()
        {
            InitializeComponent();
            PopulateOcrLanguages();
        }

        #endregion

        #region Extract Text Operations

        private void Btn_SelectPDF_Click(object sender, EventArgs e)
        {
            if (ShowOpenFileDialog())
            {
                lbl_PDF.Text = oFD_PDF.FileName;
                SetExtractControlsAfterFileSelection();
            }
        }

        private async void Btn_Start_Click(object sender, EventArgs e)
        {
            ToggleControlsExtract(false);

            string pdfPath = lbl_PDF.Text;
            string txtPath = lbl_TXT.Text;
            Btn_Reset.Enabled = false;
            string selectedLanguage = GetSelectedOcrLanguage();

            await Task.Run(() => ExtractText.Execute(pdfPath, txtPath, selectedLanguage, UpdateProgress, () => abortFlag));

            ToggleControlsExtract(true);
        }

        private string GetSelectedOcrLanguage()
        {
            if (cmbLangConv.SelectedItem == null)
            {
                return DefaultOcrLanguage;
            }

            string selectedText = cmbLangConv.SelectedItem.ToString();
            return languageMapping.TryGetValue(selectedText, out string languageCode)
                ? languageCode
                : DefaultOcrLanguage;
        }

        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            ResetExtractPanel();
        }

        private void ResetExtractPanel()
        {
            lbl_PDF.Text = Strings.LblMsgInputPDF_Extr;
            lbl_TXT.Text = Strings.LblMsgOutputDIR_Extr;
            SetControlStates(
                (Btn_SelectDIROutputTXT, false),
                (Btn_SelectPDF, true),
                (Btn_Start, false),
                (Btn_Reset, false),
                (Btn_Abort, false));

            SelectFirstComboBoxItem(cmbLangConv);
            UpdateProgress(0);
        }

        private void SetExtractControlsAfterFileSelection()
        {
            SetControlStates(
                (Btn_SelectDIROutputTXT, true),
                (Btn_Reset, true),
                (Btn_SelectPDF, false));
        }

        private void Btn_SelectDIROutputTXT_Click(object sender, EventArgs e)
        {
            if (!ConfirmAndSelectOutputDirectory(Strings.WarnSelectOutDirTXT, lbl_PDF.Text, ".txt", lbl_TXT))
            {
                ResetExtractPanel();
                return;
            }

            SetControlStates(
                (Btn_Start, true),
                (Btn_SelectDIROutputTXT, false));
        }

        private void Btn_Abort_Click(object sender, EventArgs e)
        {
            if (ConfirmAction(Strings.WarnConfirmAbort))
            {
                abortFlag = true;
            }
        }

        #endregion

        #region Merge Operations

        private void Btn_SelectPDFToMerge_Click(object sender, EventArgs e)
        {
            if (ShowOpenFileDialog())
            {
                Btn_ResetMerge.Enabled = true;
                Lstb_FileMerge.Items.Add(oFD_PDF.FileName);

                if (Lstb_FileMerge.Items.Count >= MinFilesForMerge)
                {
                    Btn_SelectDIROutputMergedPDF.Enabled = true;
                }
            }
        }

        private void Btn_ResetMerge_Click(object sender, EventArgs e)
        {
            ResetMergePanel();
        }

        private void ResetMergePanel()
        {
            Lstb_FileMerge.Items.Clear();
            lbl_DIROutputMergePDF.Text = Strings.LblMsgOutputDIR_Merge;
            SetControlStates(
                (Btn_SelectDIROutputMergedPDF, false),
                (Btn_Merge, false),
                (Btn_ResetMerge, false),
                (Btn_SelectPDFToMerge, true));
        }

        private async void Btn_Merge_Click(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(
                ToggleControlMerge,
                spinnerMerge,
                lbl_MergeInProgress,
                () => Merge.Execute(lbl_DIROutputMergePDF.Text, Lstb_FileMerge.Items, lbl_MergeInProgress, spinnerMerge),
                ResetMergePanel);
        }

        private void Btn_SelectDIROutputMergedPDF_Click(object sender, EventArgs e)
        {
            if (!ConfirmOutputDirectorySelection(Strings.WarnSelectOutDirMERGE))
            {
                return;
            }

            if (ShowFolderBrowserDialog())
            {
                string firstFileName = Lstb_FileMerge.Items[0].ToString();
                lbl_DIROutputMergePDF.Text = BuildOutputPath(fBD_TXT.SelectedPath, firstFileName, "_Merged.pdf");
                SetControlStates(
                    (Btn_SelectDIROutputMergedPDF, false),
                    (Btn_Merge, true),
                    (Btn_SelectPDFToMerge, false));
            }
        }

        #endregion

        #region Compress Operations

        private async void Btn_Compress_Click(object sender, EventArgs e)
        {
            await ExecuteOperationAsync(
                ToggleControlCompress,
                spinnerCompress,
                lbl_CompressInProgress,
                () => Compress.Execute(lbl_PDFToCompress.Text, levelCompress, lbl_DIROutputCompressPDF.Text, lbl_CompressInProgress, spinnerCompress),
                ResetCompressPanel);
        }

        private void Btn_ResetCompres_Click(object sender, EventArgs e)
        {
            ResetCompressPanel();
        }

        private void ResetCompressPanel()
        {
            lbl_PDFToCompress.Text = Strings.PDFFileToCOMPRESS;
            lbl_DIROutputCompressPDF.Text = Strings.DirectoryOutputCompressedPDF;
            lbl_ViewLvlCompres.Text = Strings.CompressLvl_1;
            Tb_Compress.Value = 1;
            SetControlStates(
                (Tb_Compress, false),
                (Btn_Compress, false),
                (Btn_ResetCompres, false),
                (Btn_SelectPDFToCompress, true),
                (Btn_SelectDIROutputCompressPDF, false));
        }

        private void Btn_SelectDIROutputCompressPDF_Click(object sender, EventArgs e)
        {
            if (!ConfirmAndSelectOutputDirectory(Strings.WarnSelectOutDirCOMP, lbl_PDFToCompress.Text, "_Compressed.pdf", lbl_DIROutputCompressPDF))
            {
                return;
            }

            SetControlStates(
                (Btn_SelectDIROutputCompressPDF, false),
                (Tb_Compress, true),
                (Btn_Compress, true),
                (Btn_SelectPDFToCompress, false));
        }

        private void Btn_SelectPDFToCompress_Click(object sender, EventArgs e)
        {
            if (ShowOpenFileDialog())
            {
                lbl_PDFToCompress.Text = oFD_PDF.FileName;
                SetControlStates(
                    (Btn_SelectDIROutputCompressPDF, true),
                    (Btn_ResetCompres, true),
                    (Btn_SelectPDFToCompress, false));
            }
        }

        private void Tb_Compress_ValueChanged(object sender, EventArgs e)
        {
            (levelCompress, lbl_ViewLvlCompres.Text) = GetCompressionLevel(Tb_Compress.Value);
        }

        private static (string level, string display) GetCompressionLevel(int value)
        {
            switch (value)
            {
                case 0:
                    return (CompressionLevels.Prepress, Strings.CompressLvl_0);
                case 2:
                    return (CompressionLevels.Ebook, Strings.CompressLvl_2);
                case 3:
                    return (CompressionLevels.Screen, Strings.CompressLvl_3);
                default:
                    return (CompressionLevels.Printer, Strings.CompressLvl_1);
            }
        }

        #endregion

        #region Convert Operations

        private async void Btn_Convert_Click(object sender, EventArgs e)
        {
            int formatOutput = GetSelectedOutputFormat();

            await ExecuteOperationAsync(
                ToggleControlConvert,
                spinnerConvert,
                lbl_ConvertInProgress,
                () => ConvertDOCX.Execute(lbl_PDFToConvert.Text, lbl_DIROutputConvertPDF.Text, lbl_ConvertInProgress, spinnerConvert, formatOutput),
                ResetConvertPanel);
        }

        private int GetSelectedOutputFormat()
        {
            if (rBOutputFormat_2.Checked)
            {
                return 2;
            }

            return rBOutputFormat_1.Checked ? 1 : 0;
        }

        private void Btn_SelectPDFToConvert_Click(object sender, EventArgs e)
        {
            if (ShowOpenFileDialog())
            {
                lbl_PDFToConvert.Text = oFD_PDF.FileName;
                SetControlStates(
                    (Btn_SelectDIROutputConvertPDF, true),
                    (Btn_ResetConvert, true),
                    (rBOutputFormat_0, true),
                    (rBOutputFormat_1, true),
                    (rBOutputFormat_2, true),
                    (Btn_SelectPDFToConvert, false));
            }
        }

        private void Btn_SelectDIROutputConvertPDF_Click(object sender, EventArgs e)
        {
            if (!ConfirmAndSelectOutputDirectory(Strings.WarnSelectOutDirDOCX, lbl_PDFToConvert.Text, "_Convert.docx", lbl_DIROutputConvertPDF))
            {
                return;
            }

            SetControlStates(
                (Btn_SelectDIROutputConvertPDF, false),
                (Btn_Convert, true),
                (Btn_SelectPDFToConvert, false));
        }

        private void Btn_ResetConvert_Click(object sender, EventArgs e)
        {
            ResetConvertPanel();
        }

        private void ResetConvertPanel()
        {
            lbl_PDFToConvert.Text = Strings.LblMsgInputPDF_Conv;
            lbl_DIROutputConvertPDF.Text = Strings.LblMsgOutputDIR_Conv;
            rBOutputFormat_0.Checked = true;
            SetControlStates(
                (rBOutputFormat_0, false),
                (rBOutputFormat_1, false),
                (rBOutputFormat_2, false),
                (Btn_Convert, false),
                (Btn_ResetConvert, false),
                (Btn_SelectPDFToConvert, true),
                (Btn_SelectDIROutputConvertPDF, false));
        }

        #endregion

        #region Form Events

        private void FrmUtiPDF_Main_Load(object sender, EventArgs e)
        {
            InitializeLanguageSelector();
            ControlTextImgAssigner.AssignControlTextxImg(this);
            CenterProgressLabelsOnPanels();
        }

        private void FrmUtiPDF_Main_Shown(object sender, EventArgs e)
        {
            ResetAllPanels();
            UpdateProgress(0);
            CenterAllSpinners();
        }

        private void FrmUtiPDF_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = isOperationInProgress;
        }

        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Language Selection

        private void InitializeLanguageSelector()
        {
            cmb_Language.Items.Clear();

            string[] supportedLanguages = LocalizationManager.GetSupportedLanguages();
            string currentCulture = LocalizationManager.GetCurrentLanguageCode();
            int selectedIndex = 0;

            for (int i = 0; i < supportedLanguages.Length; i++)
            {
                cmb_Language.Items.Add(new LanguageItem(supportedLanguages[i]));

                if (supportedLanguages[i] == currentCulture)
                {
                    selectedIndex = i;
                }
            }

            if (cmb_Language.Items.Count > 0)
            {
                cmb_Language.SelectedIndex = selectedIndex;
            }
        }

        private void Cmb_Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Language.SelectedItem is LanguageItem selectedLanguage)
            {
                LocalizationManager.SetCulture(selectedLanguage.CultureCode);
                RefreshUILanguage();
            }
        }

        private void RefreshUILanguage()
        {
            ControlTextImgAssigner.AssignControlTextxImg(this);
            (_, lbl_ViewLvlCompres.Text) = GetCompressionLevel(Tb_Compress.Value);
        }

        #endregion

        #region OCR Language Population

        private void PopulateOcrLanguages()
        {
            if (cmbLangConv == null)
            {
                return;
            }

            cmbLangConv.Items.Clear();
            languageMapping.Clear();

            DataLangLoader dataLoader = new DataLangLoader();
            ReadOnlyCollection<DataLang> languageData = dataLoader.LoadData(SettingsString.LangCsvPath);
            string[] trainedDataFiles = dataLoader.GetFiles(
                SettingsString.BinPath,
                SettingsString.TrainerDataFolder,
                SettingsString.CsvLangFilename);

            foreach (string file in trainedDataFiles)
            {
                AddLanguageIfValid(dataLoader, languageData, file);
            }

            SelectFirstComboBoxItem(cmbLangConv);
        }

        private void AddLanguageIfValid(DataLangLoader loader, ReadOnlyCollection<DataLang> languageData, string file)
        {
            string fileName = Path.GetFileName(file);
            DataLang langInfo = loader.FindByParam3(languageData, fileName);

            if (langInfo != null && !languageMapping.ContainsKey(langInfo.LangParam2))
            {
                cmbLangConv.Items.Add(langInfo.LangParam2);
                languageMapping.Add(langInfo.LangParam2, langInfo.LangParam1);
            }
        }

        #endregion

        #region UI Toggle Methods

        private void ToggleControlsExtract(bool isEnabled)
        {
            if (isEnabled)
            {
                ResetExtractPanel();
            }

            abortFlag = false;
            isOperationInProgress = !isEnabled;

            SetControlStates(
                (Btn_Abort, !isEnabled),
                (Btn_Exit, isEnabled),
                (Btn_Start, false),
                (cmbLangConv, isEnabled),
                (cmb_Language, isEnabled));

            EnablePanels(isEnabled);
        }

        private void ToggleControlMerge(bool isEnabled)
        {
            ToggleOperationControls(isEnabled, lbl_MergeInProgress);
        }

        private void ToggleControlCompress(bool isEnabled)
        {
            ToggleOperationControls(isEnabled, lbl_CompressInProgress);
        }

        private void ToggleControlConvert(bool isEnabled)
        {
            ToggleOperationControls(isEnabled, lbl_ConvertInProgress);
        }

        private void ToggleOperationControls(bool isEnabled, Label progressLabel)
        {
            isOperationInProgress = !isEnabled;
            progressLabel.Visible = !isEnabled;

            SetControlStates(
                (cmb_Language, isEnabled),
                (Btn_Exit, isEnabled));

            EnablePanels(isEnabled);
        }

        private void EnablePanels(bool isEnabled)
        {
            PnlMerge.Enabled = isEnabled;
            PnlCompress.Enabled = isEnabled;
            PnlConvert.Enabled = isEnabled;
        }

        #endregion

        #region Generic Operation Executor

        private async Task ExecuteOperationAsync(
            Action<bool> toggleControls,
            LoadingSpinner spinner,
            Label progressLabel,
            Func<Task> operation,
            Action resetPanel)
        {
            toggleControls(false);
            BringSpinnerToFront(spinner, progressLabel);

            await operation();

            SendSpinnerToBack(spinner, progressLabel);
            toggleControls(true);
            resetPanel();
        }

        #endregion

        #region Helper Methods

        private void UpdateProgress(int percentage)
        {
            ProgressHelper.UpdateProgress(modernProgressExtract, percentage);
        }

        private bool ShowOpenFileDialog()
        {
            return oFD_PDF.ShowDialog() == DialogResult.OK;
        }

        private bool ShowFolderBrowserDialog()
        {
            return fBD_TXT.ShowDialog() == DialogResult.OK;
        }

        private static bool ConfirmOutputDirectorySelection(string message)
        {
            return MessageBox.Show(
                message,
                Strings.MsgBoxWarningTitle,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) == DialogResult.OK;
        }

        private bool ConfirmAndSelectOutputDirectory(string warningMessage, string sourceFile, string suffix, Label outputLabel)
        {
            if (!ConfirmOutputDirectorySelection(warningMessage))
            {
                return false;
            }

            if (ShowFolderBrowserDialog())
            {
                outputLabel.Text = BuildOutputPath(fBD_TXT.SelectedPath, sourceFile, suffix);
                return true;
            }

            return false;
        }

        private static bool ConfirmAction(string message)
        {
            return MessageBox.Show(
                message,
                Strings.MsgBoxWarningTitle,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning) == DialogResult.OK;
        }

        private static string BuildOutputPath(string directory, string sourceFile, string suffix)
        {
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(sourceFile);
            return Path.Combine(directory, fileNameWithoutExt + suffix);
        }

        private static void SelectFirstComboBoxItem(ComboBox comboBox)
        {
            if (comboBox?.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private static void SetControlStates(params (Control control, bool enabled)[] controlStates)
        {
            foreach ((Control control, bool enabled) in controlStates)
            {
                if (control != null)
                {
                    control.Enabled = enabled;
                }
            }
        }

        private void ResetAllPanels()
        {
            ResetMergePanel();
            ResetCompressPanel();
            ResetConvertPanel();
            ResetExtractPanel();
        }

        private void CenterAllSpinners()
        {
            CenterSpinnerOnLabel(spinnerCompress, lbl_CompressInProgress);
            CenterSpinnerOnLabel(spinnerMerge, lbl_MergeInProgress);
            CenterSpinnerOnLabel(spinnerConvert, lbl_ConvertInProgress);
        }

        private static void BringSpinnerToFront(LoadingSpinner spinner, Label label)
        {
            label.BringToFront();
            spinner.BringToFront();
            Application.DoEvents();
        }

        private static void SendSpinnerToBack(LoadingSpinner spinner, Label label)
        {
            label.SendToBack();
            spinner.SendToBack();
            Application.DoEvents();
        }

        private void CenterProgressLabelsOnPanels()
        {
            CenterLabelOnPanel(lbl_CompressInProgress, PnlCompress);
            CenterLabelOnPanel(lbl_MergeInProgress, PnlMerge);
            CenterLabelOnPanel(lbl_ConvertInProgress, PnlConvert);
        }

        private static void CenterLabelOnPanel(Label label, Control panel)
        {
            label.Left = panel.Location.X + (panel.Width - label.Width) / 2;
            label.Top = panel.Location.Y + (panel.Height - label.Height) / 2;
        }

        private static void CenterSpinnerOnLabel(LoadingSpinner spinner, Label label)
        {
            if (spinner == null || label == null)
            {
                return;
            }

            spinner.Left = label.Left + (label.Width - spinner.Width) / 2;
            spinner.Top = label.Top + (label.Height - spinner.Height) / 2 - SpinnerVerticalOffset;
        }

        #endregion
    }
}
