using System.Linq;
using System.Windows.Forms;
using UtilityPDF.Properties;

namespace UtilityPDF
{
    internal class ControlTextImgAssigner
    {
            public static void AssignControlTextxImg(FrmUtiPDF_Main frmMain)
            {
                var controls = frmMain.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Where(f => f.FieldType == typeof(Label) || f.FieldType == typeof(Button) || f.FieldType == typeof(PictureBox));

                foreach (var control in controls)
                {
                    if (control.FieldType == typeof(Label))
                    {
                        var label = (Label)control.GetValue(frmMain);
                        switch (label.Name)
                        {
                            case "lblOCR":
                                label.Text = SettingsString.LblPanelExtract;
                                break;
                            case "lblCompr":
                                label.Text = SettingsString.LblPanelCompress;
                                break;
                            case "lblMerge":
                                label.Text = SettingsString.LblPanelMerge;
                                break;
                            case "lbl_ConvDOCX":
                                label.Text = SettingsString.lbl_ConvDOCX;
                                break;
                            case "lblLang":
                                label.Text = SettingsString.LblMsgSelLang;
                                break;
                            case "lbl_LvlCompr":
                                label.Text = SettingsString.LblCompressionLvl;
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
                                button.Text = SettingsString.TxtSelectPDFBtn;
                                break;
                            case "Btn_Reset":
                            case "Btn_ResetCompres":
                            case "Btn_ResetMerge":
                            case "Btn_ResetConvert":
                                button.Text = SettingsString.TxtResetBtn;
                                break;
                            case "Btn_SelectDIROutputTXT":
                                button.Text = SettingsString.TxtOutputDirBtn + "TXT";
                                break;
                            case "Btn_SelectDIROutputMergedPDF":
                            case "Btn_SelectDIROutputCompressPDF":
                                button.Text = SettingsString.TxtOutputDirBtn + "PDF";
                                break;
                        case "Btn_SelectDIROutputConvertPDF":
                            button.Text = SettingsString.TxtOutputDirBtn + "DOCX";
                            break;
                        case "Btn_Abort":
                                button.Text = SettingsString.TxtAbortBtn;
                                break;
                            case "Btn_Compress":
                                button.Text = SettingsString.TxtCompressBtn;
                                break;
                            case "Btn_Convert":
                                button.Text = SettingsString.TxtConvertBtn;
                                break;
                            case "Btn_Start":
                                button.Text = SettingsString.TxtExtractBtn;
                                break;
                            case "Btn_Merge":
                                button.Text = SettingsString.TxtMergetBtn;
                                break;
                            case "Btn_Exit":
                                button.Text = SettingsString.TxtExitBtn;
                                break;
                        }
                    }
                    else if (control.FieldType == typeof(PictureBox))
                    {
                        var pictureBox = (PictureBox)control.GetValue(frmMain);
                        if (pictureBox.Name == "pB_ICO")
                        {
                            pictureBox.Image = Resources.PDFUti;
                        }
                    }
                }
            }
        }

}
