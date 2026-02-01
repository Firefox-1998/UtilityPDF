using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UtilityPDF.Resources;

namespace UtilityPDF
{
    internal class ControlTextImgAssigner
    {
        public static void AssignControlTextxImg(FrmUtiPDF_Main frmMain)
        {
            var controls = frmMain.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(Label) || f.FieldType == typeof(Button) || f.FieldType == typeof(RadioButton) || f.FieldType == typeof(PictureBox));

            foreach (var control in controls)
            {
                if (control.FieldType == typeof(Label))
                {
                    var label = (Label)control.GetValue(frmMain);
                    switch (label.Name)
                    {
                        case "lblOCR":
                            label.Text = Strings.LblPanelExtract;
                            break;
                        case "lblCompr":
                            label.Text = Strings.LblPanelCompress;
                            break;
                        case "lblMerge":
                            label.Text = Strings.LblPanelMerge;
                            break;
                        case "lbl_ConvDOCX":
                            label.Text = Strings.LblPanelConvDOCX;
                            break;
                        case "lblLang":
                            label.Text = Strings.LblMsgSelLang;
                            break;
                        case "lbl_LvlCompr":
                            label.Text = Strings.LblCompressionLvl;
                            break;
                        case "lbl_CompressInProgress":
                            label.Size = new Size(240, 110);
                            label.Text = Strings.LblCompressInProgress;
                            break;
                        case "lbl_MergeInProgress":
                            label.Size = new Size(240, 110);
                            label.Text = Strings.LblMergeInProgress;
                            break;
                        case "lbl_ConvertInProgress":
                            label.Size = new Size(240, 110);
                            label.Text = Strings.LblConvertInProgress;
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
                            label.Text = Strings.LblLanguage;
                            break;
                        case "lbl_PDFToCompress":
                            label.Text = Strings.PDFFileToCOMPRESS;
                            break;
                        case "lbl_DIROutputCompressPDF":
                            label.Text = Strings.DirectoryOutputCompressedPDF;
                            break;
                    }
                }
                else if (control.FieldType == typeof(Button))
                {
                    var button = (Button)control.GetValue(frmMain);
                    switch (button.Name)
                    {
                        case "Btn_SelectPDF":
                        case "Btn_SelectPDFToCompress":
                        case "Btn_SelectPDFToMerge":
                        case "Btn_SelectPDFToConvert":
                            button.Text = Strings.TxtSelectPDFBtn;
                            break;
                        case "Btn_Reset":
                        case "Btn_ResetCompres":
                        case "Btn_ResetMerge":
                        case "Btn_ResetConvert":
                            button.Text = Strings.TxtResetBtn;
                            break;
                        case "Btn_SelectDIROutputTXT":
                            button.Text = Strings.TxtOutputDirBtn + "TXT";
                            break;
                        case "Btn_SelectDIROutputMergedPDF":
                        case "Btn_SelectDIROutputCompressPDF":
                            button.Text = Strings.TxtOutputDirBtn + "PDF";
                            break;
                        case "Btn_SelectDIROutputConvertPDF":
                            button.Text = Strings.TxtOutputDirBtn + "DOCX/RTF";
                            break;
                        case "Btn_Abort":
                            button.Text = Strings.TxtAbortBtn;
                            break;
                        case "Btn_Compress":
                            button.Text = Strings.TxtCompressBtn;
                            break;
                        case "Btn_Convert":
                            button.Text = Strings.TxtConvertBtn;
                            break;
                        case "Btn_Start":
                            button.Text = Strings.TxtExtractBtn;
                            break;
                        case "Btn_Merge":
                            button.Text = Strings.TxtMergetBtn;
                            break;
                        case "Btn_Exit":
                            button.Text = Strings.TxtExitBtn;
                            break;
                    }
                }
                else if (control.FieldType == typeof(RadioButton))
                {
                    var radiobutton = (RadioButton)control.GetValue(frmMain);
                    switch (radiobutton.Name)
                    {
                        case "rBOutputFormat_0":
                            radiobutton.Text = Strings.RdBtnOutFormat_0;
                            break;

                        case "rBOutputFormat_1":
                            radiobutton.Text = Strings.RdBtnOutFormat_1;
                            break;

                        case "rBOutputFormat_2":
                            radiobutton.Text = Strings.RdBtnOutFormat_2;
                            break;
                    }
                }
            }
        }
    }
}
