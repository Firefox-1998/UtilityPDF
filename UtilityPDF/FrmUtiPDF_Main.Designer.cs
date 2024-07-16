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
            this.PnlOCR.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlOCR
            // 
            this.PnlOCR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.PnlOCR.Size = new System.Drawing.Size(396, 275);
            this.PnlOCR.TabIndex = 0;
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
            this.Btn_Reset.Enabled = false;
            this.Btn_Reset.Location = new System.Drawing.Point(115, 243);
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
            this.Btn_Start.Location = new System.Drawing.Point(7, 243);
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
            // FrmUtiPDF_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
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
    }
}