using Freeware;
using Ghostscript.NET;
using Ghostscript.NET.Processor;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Tesseract;

namespace UtilityPDF
{
    public partial class FrmUtiPDF_Main : Form
    {
        private bool bConvert = false;
        private string LevelCompress = "/prepress";
        private static readonly string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string gsDllPath = Path.Combine(binPath, Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
        private readonly GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(gsDllPath);

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
            PnlMerge.Enabled = false;
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

                    if (pages.Count <= 100)
                    {
                        PbConvert.Maximum = 100;
                    }
                    else
                    {
                        PbConvert.Maximum = 1000;
                    }

                    using (var engine = new TesseractEngine(@"./tessdata", selectedLanguage, EngineMode.LstmOnly))
                    {
                        for (int i = 0; i < pages.Count; i++)
                        {
                            if (pages.Count <= 100)
                            {
                                PbConvert.Value = 100 / (i + 1);
                            }
                            else
                            {
                                PbConvert.Value = 1000 / (i + 1);
                            }

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
                MessageBox.Show("Conversion completed!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Display the exception message
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            PbConvert.Value = PbConvert.Maximum;            
            PbConvert.Value = 0;
            PbConvert.Maximum = 100;
            PnlMerge.Enabled = true;
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
            lbl_DIROutputMergePDF.Text = "Directory Output Merged PDF";
            Btn_SelectDIROutputMergedPDF.Enabled = false;
            Btn_Merge.Enabled = false;
            Btn_SelectPDFToMerge.Enabled = true;
        }

        private void Btn_Merge_Click(object sender, EventArgs e)
        {
            bConvert = true;
            PnlMerge.Enabled = false;
            PnlOCR.Enabled = false;
            Application.DoEvents();
            string pdfPath = lbl_DIROutputMergePDF.Text;

            try
            {
                using (PdfDocument outputDocument = new PdfDocument())
                {
                    outputDocument.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
                    outputDocument.Options.NoCompression = false;
                    outputDocument.Options.CompressContentStreams = true;
                    outputDocument.Options.EnableCcittCompressionForBilevelImages = true;

                    foreach (string path in Lstb_FileMerge.Items)
                    {
                        PdfDocument inputDocument = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                        for (int i = 0; i < inputDocument.PageCount; i++)
                        {
                            outputDocument.AddPage(inputDocument.Pages[i]);
                        }
                    }

                    outputDocument.Save(pdfPath);
                }
                MessageBox.Show("Merge completed!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Display the exception message
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            

            bConvert = false;
            PnlMerge.Enabled = true;
            PnlOCR.Enabled = true;
            Lstb_FileMerge.Items.Clear();
            lbl_DIROutputMergePDF.Text = "Directory Output Merged PDF";
            Btn_SelectDIROutputMergedPDF.Enabled = false;
            Btn_Merge.Enabled = false;
            Btn_SelectPDFToMerge.Enabled = true;
        }


        private void Btn_SelectDIROutputMergedPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
    "If a PDF file with the same name already exists (PDF_MERGED file will have the SAME NAME + MERGED as the first selected PDF file) in the folder you select, it will be overwritten!!!",
    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
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
            bConvert = true;
            PnlMerge.Enabled = false;
            PnlOCR.Enabled = false;
            PnlCompress.Enabled = false;
            Application.DoEvents();

            string pdfPath = lbl_PDFToCompress.Text;
            string outputPath = lbl_DIROutputCompressPDF.Text;

            try
            {
                using (GhostscriptProcessor processor = new GhostscriptProcessor(gvi))
                {
                    List<string> switches = new List<string>
                    {
                        $"gs",
                        $"-sDEVICE=pdfwrite",
                        $"-dPDFSETTINGS={LevelCompress}",
                        $"-dNOPAUSE",
                        $"-dQUIET",
                        $"-sOutputFile={outputPath}",
                        $"{pdfPath}"
                    };

                    processor.StartProcessing(switches.ToArray(), null);
                }
                MessageBox.Show("Compress completed!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Display the exception message
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                        
            bConvert = false;
            PnlMerge.Enabled = true;
            PnlOCR.Enabled = true;
            PnlCompress.Enabled = true;
            lbl_PDFToCompress.Text = "PDF file to COMPRESS.";
            lbl_DIROutputCompressPDF .Text = "Directory Output Compressed PDF";
            Btn_SelectDIROutputCompressPDF.Enabled = false;
            Btn_SelectPDFToCompress.Enabled = true;
            Btn_Compress.Enabled = false;
            Tb_Compress.Value = 0;
            Tb_Compress.Enabled = false;
        }

        private void Btn_PDFToCompress_Click(object sender, EventArgs e)
        {
            if (oFD_PDF.ShowDialog() == DialogResult.OK)
            {
                lbl_PDFToCompress.Text = oFD_PDF.FileName;
                Tb_Compress.Enabled = true;
                Btn_Compress.Enabled = true;
            }
        }

        private void Btn_ResetCompres_Click(object sender, EventArgs e)
        {
            lbl_PDFToCompress.Text = "PDF file to COMPRESS.";
            lbl_DIROutputCompressPDF.Text = "Directory Output Compressed PDF";            
            Tb_Compress.Value = 0;
            Tb_Compress.Enabled = false;
            Btn_Compress.Enabled = false;
            Btn_SelectPDFToCompress.Enabled = true;
            Btn_SelectDIROutputCompressPDF.Enabled = false;
        }

        private void Btn_SelectDIROutputCompressPDF_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
    "If a PDF file with the same name already exists (OUTPUT file will have the SAME NAME_COMPRESSED as the selected PDF file) in the folder you select, it will be overwritten!!!",
    "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
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
                    lbl_ViewLvlCompres.Text = "VERY LOW COMPRESS --> MAX QUALITY";
                    break;

                case 1:
                    LevelCompress = "/printer";
                    lbl_ViewLvlCompres.Text = "LOW COMPRESS --> HIGH QUALITY";
                    break;

                case 2:
                    LevelCompress = "/ebook";
                    lbl_ViewLvlCompres.Text = "MEDIUM COMPRESS --> MEDIUM QUALITY";
                    break;

                case 3:
                    LevelCompress = "/screen";
                    lbl_ViewLvlCompres.Text = "HIGH COMPRESS --> LOW QUALITY";
                    break;
            }
        }
    }
}
