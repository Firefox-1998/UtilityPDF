using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    /// <summary>
    /// Assigns localized text and emoji icons to form controls
    /// </summary>
    internal static class ControlTextImgAssigner
    {
        private static readonly Font LabelFont = new Font("Segoe UI Emoji", 9F, FontStyle.Regular);
        private static readonly Font ButtonFont = new Font("Segoe UI Emoji", 8.5F, FontStyle.Regular);
        private static readonly Font RadioButtonFont = new Font("Segoe UI Emoji", 9F, FontStyle.Regular);

        /// <summary>
        /// Assigns text and images to all supported controls in the form
        /// </summary>
        public static void AssignControlTextxImg(Form form)
        {
            if (form == null)
            {
                return;
            }

            AssignControlsRecursively(form.Controls);
        }

        private static void AssignControlsRecursively(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                AssignControlText(control);

                if (control.HasChildren)
                {
                    AssignControlsRecursively(control.Controls);
                }
            }
        }

        private static void AssignControlText(Control control)
        {
            switch (control)
            {
                case Label label:
                    AssignLabelText(label);
                    break;
                case RadioButton radioButton:
                    AssignRadioButtonText(radioButton);
                    break;
                case Button button:
                    AssignButtonText(button);
                    break;
            }
        }

        #region Label Assignments

        private static readonly Dictionary<string, Func<string>> LabelTextMap = new Dictionary<string, Func<string>>
        {
            { "lblLang", () => "🌍 " + Strings.LblMsgSelLang },
            { "lbl_LvlCompr", () => "⚙ " + Strings.LblCompressionLvl },
            { "lbl_CompressInProgress", () => "⏳ " + Strings.LblCompressInProgress },
            { "lbl_MergeInProgress", () => "⏳ " + Strings.LblMergeInProgress },
            { "lbl_ConvertInProgress", () => "⏳ " + Strings.LblConvertInProgress },
            { "lbl_PDF", () => Strings.LblMsgInputPDF_Extr },
            { "lbl_TXT", () => Strings.LblMsgOutputDIR_Extr },
            { "lbl_DIROutputMergePDF", () => Strings.LblMsgOutputDIR_Merge },
            { "lbl_PDFToConvert", () => Strings.LblMsgInputPDF_Conv },
            { "lbl_DIROutputConvertPDF", () => Strings.LblMsgOutputDIR_Conv },
            { "lbl_Language", () => "🌐 " + Strings.LblLanguage },
            { "lbl_PDFToCompress", () => Strings.PDFFileToCOMPRESS },
            { "lbl_DIROutputCompressPDF", () => Strings.DirectoryOutputCompressedPDF }
        };

        private static void AssignLabelText(Label label)
        {
            if (label == null)
            {
                return;
            }

            label.Font = new Font(LabelFont.FontFamily, label.Font.Size, label.Font.Style);

            if (LabelTextMap.TryGetValue(label.Name, out Func<string> textFunc))
            {
                label.Text = textFunc();
            }
        }

        #endregion

        #region Button Assignments

        private static readonly Dictionary<string, Func<string>> ButtonTextMap = new Dictionary<string, Func<string>>
        {
            { "Btn_SelectPDF", () => "📄\r\n" + Strings.TxtSelectPDFBtn },
            { "Btn_SelectPDFToCompress", () => "📄 " + Strings.TxtSelectPDFBtn },
            { "Btn_SelectPDFToMerge", () => "📄\r\n" + Strings.TxtSelectPDFBtn },
            { "Btn_SelectPDFToConvert", () => "📄 " + Strings.TxtSelectPDFBtn },
            { "Btn_Reset", () => "🔄 " + Strings.TxtResetBtn },
            { "Btn_ResetCompres", () => "🔄 " + Strings.TxtResetBtn },
            { "Btn_ResetMerge", () => "🔄 " + Strings.TxtResetBtn },
            { "Btn_ResetConvert", () => "🔄 " + Strings.TxtResetBtn },
            { "Btn_SelectDIROutputTXT", () => "📁\r\n" + Strings.TxtOutputDirBtn + " TXT" },
            { "Btn_SelectDIROutputMergedPDF", () => "📁\r\n" + Strings.TxtOutputDirBtn + " PDF" },
            { "Btn_SelectDIROutputCompressPDF", () => "📁\r\n" + Strings.TxtOutputDirBtn + " PDF" },
            { "Btn_SelectDIROutputConvertPDF", () => "📁 " + Strings.TxtOutputDirBtn },
            { "Btn_Abort", () => "⏹ " + Strings.TxtAbortBtn },
            { "Btn_Compress", () => "🗜 " + Strings.TxtCompressBtn },
            { "Btn_Convert", () => "🔄 " + Strings.TxtConvertBtn },
            { "Btn_Start", () => "▶ " + Strings.TxtExtractBtn },
            { "Btn_Merge", () => "🔗 " + Strings.TxtMergetBtn },
            { "Btn_Exit", () => "❌ " + Strings.TxtExitBtn }
        };

        private static void AssignButtonText(Button button)
        {
            if (button == null)
            {
                return;
            }

            button.Font = ButtonFont;

            if (ButtonTextMap.TryGetValue(button.Name, out Func<string> textFunc))
            {
                button.Text = textFunc();
            }
        }

        #endregion

        #region RadioButton Assignments

        private static readonly Dictionary<string, Func<string>> RadioButtonTextMap = new Dictionary<string, Func<string>>
        {
            { "rBOutputFormat_0", () => "📝 " + Strings.RdBtnOutFormat_0 },
            { "rBOutputFormat_1", () => "📝 " + Strings.RdBtnOutFormat_1 },
            { "rBOutputFormat_2", () => "📝 " + Strings.RdBtnOutFormat_2 }
        };

        private static void AssignRadioButtonText(RadioButton radioButton)
        {
            if (radioButton == null)
            {
                return;
            }

            radioButton.Font = RadioButtonFont;

            if (RadioButtonTextMap.TryGetValue(radioButton.Name, out Func<string> textFunc))
            {
                radioButton.Text = textFunc();
            }
        }

        #endregion
    }
}
