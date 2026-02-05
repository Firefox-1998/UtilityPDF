using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    internal class ControlTextImgAssigner
    {
        public static void AssignControlTextxImg(FrmUtiPDF_Main frmMain)
        {
            System.Reflection.FieldInfo[] controls = frmMain.GetType()
                .GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(Label) || 
                           f.FieldType == typeof(Button) || 
                           f.FieldType == typeof(RadioButton) || 
                           f.FieldType == typeof(PictureBox) ||
                           f.FieldType.IsSubclassOf(typeof(Button)))
                .ToArray();

            foreach (System.Reflection.FieldInfo control in controls)
            {
                if (control.FieldType == typeof(Label))
                {
                    Label label = (Label)control.GetValue(frmMain);
                    AssignLabelText(label);
                }
                else if (control.FieldType == typeof(Button) || control.FieldType.IsSubclassOf(typeof(Button)))
                {
                    Button button = (Button)control.GetValue(frmMain);
                    AssignButtonText(button);
                }
                else if (control.FieldType == typeof(RadioButton))
                {
                    RadioButton radiobutton = (RadioButton)control.GetValue(frmMain);
                    AssignRadioButtonText(radiobutton);
                }
            }
        }

        private static void AssignLabelText(Label label)
        {
            // Imposta font che supporta emoji per le label
            label.Font = new Font("Segoe UI Emoji", label.Font.Size, label.Font.Style);
            
            switch (label.Name)
            {
                case "lblLang":
                    label.Text = "🌍 " + Strings.LblMsgSelLang;
                    break;
                case "lbl_LvlCompr":
                    label.Text = "⚙ " + Strings.LblCompressionLvl;
                    break;
                case "lbl_CompressInProgress":
                    label.Text = "⏳ " + Strings.LblCompressInProgress;
                    break;
                case "lbl_MergeInProgress":
                    label.Text = "⏳ " + Strings.LblMergeInProgress;
                    break;
                case "lbl_ConvertInProgress":
                    label.Text = "⏳ " + Strings.LblConvertInProgress;
                    break;
                case "lbl_PDF":
                    label.Text = Strings.LblMsgInputPDF_Extr;
                    break;
                case "lbl_TXT":
                    label.Text = Strings.LblMsgOutputDIR_Extr;
                    break;
                case "lbl_DIROutputMergePDF":
                    label.Text = Strings.LblMsgOutputDIR_Merge;
                    break;
                case "lbl_PDFToConvert":
                    label.Text = Strings.LblMsgInputPDF_Conv;
                    break;
                case "lbl_DIROutputConvertPDF":
                    label.Text = Strings.LblMsgOutputDIR_Conv;
                    break;
                case "lbl_Language":
                    label.Text = "🌐 " + Strings.LblLanguage;
                    break;
                case "lbl_PDFToCompress":
                    label.Text = Strings.PDFFileToCOMPRESS;
                    break;
                case "lbl_DIROutputCompressPDF":
                    label.Text = Strings.DirectoryOutputCompressedPDF;
                    break;
            }
        }

        private static void AssignButtonText(Button button)
        {
            // Imposta font più piccolo che supporta emoji per i bottoni
            button.Font = new Font("Segoe UI Emoji", 8.5F, FontStyle.Regular);
            
            switch (button.Name)
            {
                case "Btn_SelectPDF":
                    button.Text = "📄\r\n" + Strings.TxtSelectPDFBtn;
                    break;
                case "Btn_SelectPDFToCompress":
                    button.Text = "📄 " + Strings.TxtSelectPDFBtn;
                    break;
                case "Btn_SelectPDFToMerge":
                    button.Text = "📄\r\n" + Strings.TxtSelectPDFBtn;
                    break;
                case "Btn_SelectPDFToConvert":
                    button.Text = "📄 " + Strings.TxtSelectPDFBtn;
                    break;
                case "Btn_Reset":
                case "Btn_ResetCompres":
                case "Btn_ResetMerge":
                case "Btn_ResetConvert":
                    button.Text = "🔄 " + Strings.TxtResetBtn;
                    break;
                case "Btn_SelectDIROutputTXT":
                    button.Text = "📁\r\n" + Strings.TxtOutputDirBtn + " TXT";
                    break;
                case "Btn_SelectDIROutputMergedPDF":
                case "Btn_SelectDIROutputCompressPDF":
                    button.Text = "📁\r\n" + Strings.TxtOutputDirBtn + " PDF";
                    break;
                case "Btn_SelectDIROutputConvertPDF":
                    button.Text = "📁 " + Strings.TxtOutputDirBtn;
                    break;
                case "Btn_Abort":
                    button.Text = "⏹ " + Strings.TxtAbortBtn;
                    break;
                case "Btn_Compress":
                    button.Text = "🗜 " + Strings.TxtCompressBtn;
                    break;
                case "Btn_Convert":
                    button.Text = "🔄 " + Strings.TxtConvertBtn;
                    break;
                case "Btn_Start":
                    button.Text = "▶ " + Strings.TxtExtractBtn;
                    break;
                case "Btn_Merge":
                    button.Text = "🔗 " + Strings.TxtMergetBtn;
                    break;
                case "Btn_Exit":
                    button.Text = "❌ " + Strings.TxtExitBtn;
                    break;
            }
        }

        private static void AssignRadioButtonText(RadioButton radiobutton)
        {
            // Imposta font che supporta emoji
            radiobutton.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular);
            
            switch (radiobutton.Name)
            {
                case "rBOutputFormat_0":
                    radiobutton.Text = "📝 " + Strings.RdBtnOutFormat_0;
                    break;

                case "rBOutputFormat_1":
                    radiobutton.Text = "📝 " + Strings.RdBtnOutFormat_1;
                    break;

                case "rBOutputFormat_2":
                    radiobutton.Text = "📝 " + Strings.RdBtnOutFormat_2;
                    break;
            }
        }
    }
}
