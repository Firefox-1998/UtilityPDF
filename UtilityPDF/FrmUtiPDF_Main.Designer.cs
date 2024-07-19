namespace UtilityPDF
{
    partial class FrmUtiPDF_Main
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnlOCR = new System.Windows.Forms.Panel();
            this.PbConvert = new System.Windows.Forms.ProgressBar();
            this.lblLang = new System.Windows.Forms.Label();
            this.cmbLangConv = new System.Windows.Forms.ComboBox();
            this.Btn_Reset = new System.Windows.Forms.Button();
            this.Btn_Start = new System.Windows.Forms.Button();
            this.lbl_TXT = new System.Windows.Forms.Label();
            this.Btn_SelectTXT = new System.Windows.Forms.Button();
            this.lbl_PDF = new System.Windows.Forms.Label();
            this.Btn_SelectPDF = new System.Windows.Forms.Button();
            this.lblOCR = new System.Windows.Forms.Label();
            this.oFD_PDF = new System.Windows.Forms.OpenFileDialog();
            this.fBD_TXT = new System.Windows.Forms.FolderBrowserDialog();
            this.PnlMerge = new System.Windows.Forms.Panel();
            this.lbl_DIROutputMergePDF = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputMergedPDF = new System.Windows.Forms.Button();
            this.Lstb_FileMerge = new System.Windows.Forms.ListBox();
            this.Btn_ResetMerge = new System.Windows.Forms.Button();
            this.Btn_Merge = new System.Windows.Forms.Button();
            this.Btn_SelectPDFToMerge = new System.Windows.Forms.Button();
            this.lblMerge = new System.Windows.Forms.Label();
            this.PnlCompress = new System.Windows.Forms.Panel();
            this.lblCompr = new System.Windows.Forms.Label();
            this.Btn_Compress = new System.Windows.Forms.Button();
            this.Btn_ResetCompres = new System.Windows.Forms.Button();
            this.lbl_PDFToCompress = new System.Windows.Forms.Label();
            this.Btn_PDFToCompress = new System.Windows.Forms.Button();
            this.tb_Compress = new System.Windows.Forms.TrackBar();
            this.lbl_LvlCompr = new System.Windows.Forms.Label();
            this.PnlOCR.SuspendLayout();
            this.PnlMerge.SuspendLayout();
            this.PnlCompress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Compress)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlOCR
            // 
            this.PnlOCR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlOCR.Controls.Add(this.PbConvert);
            this.PnlOCR.Controls.Add(this.lblLang);
            this.PnlOCR.Controls.Add(this.cmbLangConv);
            this.PnlOCR.Controls.Add(this.Btn_Reset);
            this.PnlOCR.Controls.Add(this.Btn_Start);
            this.PnlOCR.Controls.Add(this.lbl_TXT);
            this.PnlOCR.Controls.Add(this.Btn_SelectTXT);
            this.PnlOCR.Controls.Add(this.lbl_PDF);
            this.PnlOCR.Controls.Add(this.Btn_SelectPDF);
            this.PnlOCR.Controls.Add(this.lblOCR);
            this.PnlOCR.Location = new System.Drawing.Point(14, 14);
            this.PnlOCR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlOCR.Name = "PnlOCR";
            this.PnlOCR.Size = new System.Drawing.Size(396, 297);
            this.PnlOCR.TabIndex = 0;
            // 
            // PbConvert
            // 
            this.PbConvert.Location = new System.Drawing.Point(7, 270);
            this.PbConvert.Name = "PbConvert";
            this.PbConvert.Size = new System.Drawing.Size(380, 20);
            this.PbConvert.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PbConvert.TabIndex = 10;
            // 
            // lblLang
            // 
            this.lblLang.AutoSize = true;
            this.lblLang.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLang.Location = new System.Drawing.Point(4, 214);
            this.lblLang.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(231, 15);
            this.lblLang.TabIndex = 9;
            this.lblLang.Text = "Select language OCR recognize to convert";
            // 
            // cmbLangConv
            // 
            this.cmbLangConv.FormattingEnabled = true;
            this.cmbLangConv.Location = new System.Drawing.Point(241, 211);
            this.cmbLangConv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbLangConv.Name = "cmbLangConv";
            this.cmbLangConv.Size = new System.Drawing.Size(57, 23);
            this.cmbLangConv.TabIndex = 8;
            // 
            // Btn_Reset
            // 
            this.Btn_Reset.Location = new System.Drawing.Point(115, 239);
            this.Btn_Reset.Name = "Btn_Reset";
            this.Btn_Reset.Size = new System.Drawing.Size(100, 22);
            this.Btn_Reset.TabIndex = 7;
            this.Btn_Reset.Text = "Reset";
            this.Btn_Reset.UseVisualStyleBackColor = true;
            this.Btn_Reset.Click += new System.EventHandler(this.Btn_Reset_Click);
            // 
            // Btn_Start
            // 
            this.Btn_Start.Enabled = false;
            this.Btn_Start.Location = new System.Drawing.Point(7, 239);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(100, 22);
            this.Btn_Start.TabIndex = 6;
            this.Btn_Start.Text = "Extract";
            this.Btn_Start.UseVisualStyleBackColor = true;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // lbl_TXT
            // 
            this.lbl_TXT.Location = new System.Drawing.Point(112, 122);
            this.lbl_TXT.Name = "lbl_TXT";
            this.lbl_TXT.Size = new System.Drawing.Size(275, 75);
            this.lbl_TXT.TabIndex = 5;
            this.lbl_TXT.Text = "Directory Output TXT Selected";
            // 
            // Btn_SelectTXT
            // 
            this.Btn_SelectTXT.Enabled = false;
            this.Btn_SelectTXT.Location = new System.Drawing.Point(7, 122);
            this.Btn_SelectTXT.Name = "Btn_SelectTXT";
            this.Btn_SelectTXT.Size = new System.Drawing.Size(100, 22);
            this.Btn_SelectTXT.TabIndex = 4;
            this.Btn_SelectTXT.Text = "Select DIR TXT";
            this.Btn_SelectTXT.UseVisualStyleBackColor = true;
            this.Btn_SelectTXT.Click += new System.EventHandler(this.Btn_SelectTXT_Click);
            // 
            // lbl_PDF
            // 
            this.lbl_PDF.Location = new System.Drawing.Point(112, 33);
            this.lbl_PDF.Name = "lbl_PDF";
            this.lbl_PDF.Size = new System.Drawing.Size(275, 75);
            this.lbl_PDF.TabIndex = 3;
            this.lbl_PDF.Text = "PDF Selected";
            // 
            // Btn_SelectPDF
            // 
            this.Btn_SelectPDF.Location = new System.Drawing.Point(7, 33);
            this.Btn_SelectPDF.Name = "Btn_SelectPDF";
            this.Btn_SelectPDF.Size = new System.Drawing.Size(100, 22);
            this.Btn_SelectPDF.TabIndex = 2;
            this.Btn_SelectPDF.Text = "Select PDF";
            this.Btn_SelectPDF.UseVisualStyleBackColor = true;
            this.Btn_SelectPDF.Click += new System.EventHandler(this.Btn_SelectPDF_Click);
            // 
            // lblOCR
            // 
            this.lblOCR.AutoSize = true;
            this.lblOCR.Location = new System.Drawing.Point(4, 5);
            this.lblOCR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOCR.Name = "lblOCR";
            this.lblOCR.Size = new System.Drawing.Size(124, 15);
            this.lblOCR.TabIndex = 1;
            this.lblOCR.Text = "Extract Text From PDF";
            // 
            // oFD_PDF
            // 
            this.oFD_PDF.FileName = "*.pdf";
            this.oFD_PDF.Filter = "PDF Files|*.pdf";
            // 
            // PnlMerge
            // 
            this.PnlMerge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlMerge.Controls.Add(this.lbl_DIROutputMergePDF);
            this.PnlMerge.Controls.Add(this.Btn_SelectDIROutputMergedPDF);
            this.PnlMerge.Controls.Add(this.Lstb_FileMerge);
            this.PnlMerge.Controls.Add(this.Btn_ResetMerge);
            this.PnlMerge.Controls.Add(this.Btn_Merge);
            this.PnlMerge.Controls.Add(this.Btn_SelectPDFToMerge);
            this.PnlMerge.Controls.Add(this.lblMerge);
            this.PnlMerge.Location = new System.Drawing.Point(461, 14);
            this.PnlMerge.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlMerge.Name = "PnlMerge";
            this.PnlMerge.Size = new System.Drawing.Size(396, 297);
            this.PnlMerge.TabIndex = 11;
            // 
            // lbl_DIROutputMergePDF
            // 
            this.lbl_DIROutputMergePDF.Location = new System.Drawing.Point(108, 207);
            this.lbl_DIROutputMergePDF.Name = "lbl_DIROutputMergePDF";
            this.lbl_DIROutputMergePDF.Size = new System.Drawing.Size(275, 53);
            this.lbl_DIROutputMergePDF.TabIndex = 10;
            this.lbl_DIROutputMergePDF.Text = "Directory Output Merged PDF";
            // 
            // Btn_SelectDIROutputMergedPDF
            // 
            this.Btn_SelectDIROutputMergedPDF.Enabled = false;
            this.Btn_SelectDIROutputMergedPDF.Location = new System.Drawing.Point(5, 207);
            this.Btn_SelectDIROutputMergedPDF.Name = "Btn_SelectDIROutputMergedPDF";
            this.Btn_SelectDIROutputMergedPDF.Size = new System.Drawing.Size(100, 22);
            this.Btn_SelectDIROutputMergedPDF.TabIndex = 9;
            this.Btn_SelectDIROutputMergedPDF.Text = "Select DIR PDF";
            this.Btn_SelectDIROutputMergedPDF.UseVisualStyleBackColor = true;
            this.Btn_SelectDIROutputMergedPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputMergedPDF_Click);
            // 
            // Lstb_FileMerge
            // 
            this.Lstb_FileMerge.FormattingEnabled = true;
            this.Lstb_FileMerge.ItemHeight = 15;
            this.Lstb_FileMerge.Location = new System.Drawing.Point(113, 33);
            this.Lstb_FileMerge.Name = "Lstb_FileMerge";
            this.Lstb_FileMerge.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.Lstb_FileMerge.Size = new System.Drawing.Size(254, 169);
            this.Lstb_FileMerge.TabIndex = 8;
            // 
            // Btn_ResetMerge
            // 
            this.Btn_ResetMerge.Location = new System.Drawing.Point(113, 268);
            this.Btn_ResetMerge.Name = "Btn_ResetMerge";
            this.Btn_ResetMerge.Size = new System.Drawing.Size(100, 22);
            this.Btn_ResetMerge.TabIndex = 7;
            this.Btn_ResetMerge.Text = "Reset";
            this.Btn_ResetMerge.UseVisualStyleBackColor = true;
            this.Btn_ResetMerge.Click += new System.EventHandler(this.Btn_ResetMerge_Click);
            // 
            // Btn_Merge
            // 
            this.Btn_Merge.Enabled = false;
            this.Btn_Merge.Location = new System.Drawing.Point(5, 268);
            this.Btn_Merge.Name = "Btn_Merge";
            this.Btn_Merge.Size = new System.Drawing.Size(100, 22);
            this.Btn_Merge.TabIndex = 6;
            this.Btn_Merge.Text = "Merge";
            this.Btn_Merge.UseVisualStyleBackColor = true;
            this.Btn_Merge.Click += new System.EventHandler(this.Btn_Merge_Click);
            // 
            // Btn_SelectPDFToMerge
            // 
            this.Btn_SelectPDFToMerge.Location = new System.Drawing.Point(7, 33);
            this.Btn_SelectPDFToMerge.Name = "Btn_SelectPDFToMerge";
            this.Btn_SelectPDFToMerge.Size = new System.Drawing.Size(100, 22);
            this.Btn_SelectPDFToMerge.TabIndex = 2;
            this.Btn_SelectPDFToMerge.Text = "Select PDF";
            this.Btn_SelectPDFToMerge.UseVisualStyleBackColor = true;
            this.Btn_SelectPDFToMerge.Click += new System.EventHandler(this.Btn_SelectPDFToMerge_Click);
            // 
            // lblMerge
            // 
            this.lblMerge.AutoSize = true;
            this.lblMerge.Location = new System.Drawing.Point(4, 5);
            this.lblMerge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMerge.Name = "lblMerge";
            this.lblMerge.Size = new System.Drawing.Size(65, 15);
            this.lblMerge.TabIndex = 1;
            this.lblMerge.Text = "Merge PDF";
            // 
            // PnlCompress
            // 
            this.PnlCompress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlCompress.Controls.Add(this.lbl_LvlCompr);
            this.PnlCompress.Controls.Add(this.tb_Compress);
            this.PnlCompress.Controls.Add(this.lbl_PDFToCompress);
            this.PnlCompress.Controls.Add(this.Btn_PDFToCompress);
            this.PnlCompress.Controls.Add(this.Btn_ResetCompres);
            this.PnlCompress.Controls.Add(this.Btn_Compress);
            this.PnlCompress.Controls.Add(this.lblCompr);
            this.PnlCompress.Location = new System.Drawing.Point(229, 330);
            this.PnlCompress.Name = "PnlCompress";
            this.PnlCompress.Size = new System.Drawing.Size(420, 199);
            this.PnlCompress.TabIndex = 12;
            // 
            // lblCompr
            // 
            this.lblCompr.AutoSize = true;
            this.lblCompr.Location = new System.Drawing.Point(4, 6);
            this.lblCompr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompr.Name = "lblCompr";
            this.lblCompr.Size = new System.Drawing.Size(85, 15);
            this.lblCompr.TabIndex = 2;
            this.lblCompr.Text = "Compress PDF";
            // 
            // Btn_Compress
            // 
            this.Btn_Compress.Enabled = false;
            this.Btn_Compress.Location = new System.Drawing.Point(9, 168);
            this.Btn_Compress.Name = "Btn_Compress";
            this.Btn_Compress.Size = new System.Drawing.Size(75, 22);
            this.Btn_Compress.TabIndex = 3;
            this.Btn_Compress.Text = "Compress";
            this.Btn_Compress.UseVisualStyleBackColor = true;
            this.Btn_Compress.Click += new System.EventHandler(this.Btn_Compress_Click);
            // 
            // Btn_ResetCompres
            // 
            this.Btn_ResetCompres.Location = new System.Drawing.Point(96, 168);
            this.Btn_ResetCompres.Name = "Btn_ResetCompres";
            this.Btn_ResetCompres.Size = new System.Drawing.Size(100, 22);
            this.Btn_ResetCompres.TabIndex = 8;
            this.Btn_ResetCompres.Text = "Reset";
            this.Btn_ResetCompres.UseVisualStyleBackColor = true;
            this.Btn_ResetCompres.Click += new System.EventHandler(this.Btn_ResetCompres_Click);
            // 
            // lbl_PDFToCompress
            // 
            this.lbl_PDFToCompress.Location = new System.Drawing.Point(114, 27);
            this.lbl_PDFToCompress.Name = "lbl_PDFToCompress";
            this.lbl_PDFToCompress.Size = new System.Drawing.Size(275, 75);
            this.lbl_PDFToCompress.TabIndex = 10;
            this.lbl_PDFToCompress.Text = "PDF file to COMPRESS";
            // 
            // Btn_PDFToCompress
            // 
            this.Btn_PDFToCompress.Location = new System.Drawing.Point(9, 27);
            this.Btn_PDFToCompress.Name = "Btn_PDFToCompress";
            this.Btn_PDFToCompress.Size = new System.Drawing.Size(100, 22);
            this.Btn_PDFToCompress.TabIndex = 9;
            this.Btn_PDFToCompress.Text = "Select PDF";
            this.Btn_PDFToCompress.UseVisualStyleBackColor = true;
            this.Btn_PDFToCompress.Click += new System.EventHandler(this.Btn_PDFToCompress_Click);
            // 
            // tb_Compress
            // 
            this.tb_Compress.Enabled = false;
            this.tb_Compress.LargeChange = 1;
            this.tb_Compress.Location = new System.Drawing.Point(117, 118);
            this.tb_Compress.Maximum = 9;
            this.tb_Compress.Name = "tb_Compress";
            this.tb_Compress.Size = new System.Drawing.Size(294, 45);
            this.tb_Compress.TabIndex = 11;
            this.tb_Compress.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // lbl_LvlCompr
            // 
            this.lbl_LvlCompr.Location = new System.Drawing.Point(6, 128);
            this.lbl_LvlCompr.Name = "lbl_LvlCompr";
            this.lbl_LvlCompr.Size = new System.Drawing.Size(116, 21);
            this.lbl_LvlCompr.TabIndex = 12;
            this.lbl_LvlCompr.Text = "Compression LEVEL";
            // 
            // FrmUtiPDF_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 541);
            this.Controls.Add(this.PnlCompress);
            this.Controls.Add(this.PnlMerge);
            this.Controls.Add(this.PnlOCR);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "FrmUtiPDF_Main";
            this.Text = "PDF Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUtiPDF_Main_FormClosing);
            this.Load += new System.EventHandler(this.FrmUtiPDF_Main_Load);
            this.PnlOCR.ResumeLayout(false);
            this.PnlOCR.PerformLayout();
            this.PnlMerge.ResumeLayout(false);
            this.PnlMerge.PerformLayout();
            this.PnlCompress.ResumeLayout(false);
            this.PnlCompress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_Compress)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlOCR;
        private System.Windows.Forms.Button Btn_SelectPDF;
        private System.Windows.Forms.Label lblOCR;
        private System.Windows.Forms.OpenFileDialog oFD_PDF;
        private System.Windows.Forms.Label lbl_PDF;
        private System.Windows.Forms.Label lbl_TXT;
        private System.Windows.Forms.Button Btn_SelectTXT;
        private System.Windows.Forms.FolderBrowserDialog fBD_TXT;
        private System.Windows.Forms.Button Btn_Start;
        private System.Windows.Forms.Button Btn_Reset;
        private System.Windows.Forms.ComboBox cmbLangConv;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.ProgressBar PbConvert;
        private System.Windows.Forms.Panel PnlMerge;
        private System.Windows.Forms.Label lbl_DIROutputMergePDF;
        private System.Windows.Forms.Button Btn_SelectDIROutputMergedPDF;
        private System.Windows.Forms.ListBox Lstb_FileMerge;
        private System.Windows.Forms.Button Btn_ResetMerge;
        private System.Windows.Forms.Button Btn_Merge;
        private System.Windows.Forms.Button Btn_SelectPDFToMerge;
        private System.Windows.Forms.Label lblMerge;
        private System.Windows.Forms.Panel PnlCompress;
        private System.Windows.Forms.Button Btn_Compress;
        private System.Windows.Forms.Label lblCompr;
        private System.Windows.Forms.Label lbl_PDFToCompress;
        private System.Windows.Forms.Button Btn_PDFToCompress;
        private System.Windows.Forms.Button Btn_ResetCompres;
        private System.Windows.Forms.TrackBar tb_Compress;
        private System.Windows.Forms.Label lbl_LvlCompr;
    }
}