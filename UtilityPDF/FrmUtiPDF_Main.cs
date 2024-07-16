using Freeware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;

namespace UtilityPDF
{
    public partial class FrmUtiPDF_Main : Form
    {
        private bool bConvert = false;

        public FrmUtiPDF_Main()
        {
            InitializeComponent();
        }

        private void Btn_SelectPDF_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                lbl_PDF.Text = oFD_PDF.FileName;
                Btn_SelectTXT.Enabled = true;
                Btn_SelectPDF.Enabled = false;
            }
        }

        private void Btn_SelectTXT_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "If a TXT file with the same name already exists (TXT file will have the SAME NAME as the selected PDF file) in the folder you select, it will be overwritten!!!",
                "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                if (fBD_TXT.ShowDialog() == DialogResult.OK)
                {
                    lbl_TXT.Text = fBD_TXT.SelectedPath + @"\" + Path.GetFileNameWithoutExtension(lbl_PDF.Text) + ".txt";
                    Btn_SelectTXT.Enabled = false;
                    Btn_Start.Enabled = true;
                }
            }
            else
            {
                lbl_PDF.Text = "PDF input file.";
                lbl_TXT.Text = "TXT output file.";
                Btn_SelectTXT.Enabled = false;
                Btn_SelectPDF.Enabled = true;
            }
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            bConvert = true;
            Btn_Start.Enabled = false;
            Btn_Reset.Enabled = false;
            cmbLangConv.Enabled = false;
            Application.DoEvents();

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
                using (Stream pdfStream = File.OpenRead(pdfPath))
                {
                    List<byte[]> pages = Pdf2Png.ConvertAllPages(pdfStream, 600);
                    PbConvert.Maximum = pages.Count * 100;

                    using (var engine = new TesseractEngine(@"./tessdata", selectedLanguage, EngineMode.LstmOnly))
                    {
                        for (int i = 0; i < pages.Count; i++)
                        {
                            PbConvert.Value = (i + 1) * 100;
                            Application.DoEvents();
                            using (var ms = new MemoryStream(pages[i]))
                            {
                                Image img = Image.FromStream(ms);

                                using (var imgPix = PixConverter.ToPix((Bitmap)img))
                                {
                                    using (var page = engine.Process(imgPix))
                                    {
                                        string text = page.GetText();

                                        // Append the text to the output file
                                        File.AppendAllText(txtPath, text);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Display the exception message
                MessageBox.Show("Si è verificato un errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show("Conversion completed!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            PbConvert.Value = 0;
            PbConvert.Maximum = 100;
            lbl_PDF.Text = "PDF input file.";
            lbl_TXT.Text = "TXT output file.";
            Btn_SelectTXT.Enabled = false;
            Btn_SelectPDF.Enabled = true;
            Btn_Reset.Enabled = true;
            cmbLangConv.Enabled = true;
            bConvert = false;
        }

        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            lbl_PDF.Text = "PDF input file.";
            lbl_TXT.Text = "TXT output file.";
            Btn_SelectTXT.Enabled = false;
            Btn_SelectPDF.Enabled = true;
            Btn_Start.Enabled = false;
            cmbLangConv.SelectedIndex = 0;
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
            cmbLangConv.SelectedIndex = 0;
        }
    }
}
