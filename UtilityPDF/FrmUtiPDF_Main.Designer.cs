namespace UtilityPDF
{
    internal partial class FrmUtiPDF_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUtiPDF_Main));
            this.oFD_PDF = new System.Windows.Forms.OpenFileDialog();
            this.fBD_TXT = new System.Windows.Forms.FolderBrowserDialog();
            this.pB_ICO = new System.Windows.Forms.PictureBox();
            this.lbl_Language = new System.Windows.Forms.Label();
            this.cmb_Language = new System.Windows.Forms.ComboBox();
            this.PnlOCR = new UtilityPDF.Controls.ModernCard();
            this.modernProgressExtract = new UtilityPDF.Controls.ModernProgressBar();
            this.lblLang = new System.Windows.Forms.Label();
            this.cmbLangConv = new System.Windows.Forms.ComboBox();
            this.Btn_Reset = new UtilityPDF.Controls.ModernButton();
            this.Btn_Start = new UtilityPDF.Controls.ModernButton();
            this.Btn_Abort = new UtilityPDF.Controls.ModernButton();
            this.lbl_TXT = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputTXT = new UtilityPDF.Controls.ModernButton();
            this.lbl_PDF = new System.Windows.Forms.Label();
            this.Btn_SelectPDF = new UtilityPDF.Controls.ModernButton();
            this.PnlMerge = new UtilityPDF.Controls.ModernCard();
            this.lbl_DIROutputMergePDF = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputMergedPDF = new UtilityPDF.Controls.ModernButton();
            this.Lstb_FileMerge = new System.Windows.Forms.ListBox();
            this.Btn_ResetMerge = new UtilityPDF.Controls.ModernButton();
            this.Btn_Merge = new UtilityPDF.Controls.ModernButton();
            this.Btn_SelectPDFToMerge = new UtilityPDF.Controls.ModernButton();
            this.PnlCompress = new UtilityPDF.Controls.ModernCard();
            this.lbl_DIROutputCompressPDF = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputCompressPDF = new UtilityPDF.Controls.ModernButton();
            this.lbl_ViewLvlCompres = new System.Windows.Forms.Label();
            this.lbl_LvlCompr = new System.Windows.Forms.Label();
            this.Tb_Compress = new System.Windows.Forms.TrackBar();
            this.lbl_PDFToCompress = new System.Windows.Forms.Label();
            this.Btn_SelectPDFToCompress = new UtilityPDF.Controls.ModernButton();
            this.Btn_ResetCompres = new UtilityPDF.Controls.ModernButton();
            this.Btn_Compress = new UtilityPDF.Controls.ModernButton();
            this.lbl_CompressInProgress = new System.Windows.Forms.Label();
            this.lbl_MergeInProgress = new System.Windows.Forms.Label();
            this.PnlConvert = new UtilityPDF.Controls.ModernCard();
            this.rBOutputFormat_2 = new System.Windows.Forms.RadioButton();
            this.rBOutputFormat_1 = new System.Windows.Forms.RadioButton();
            this.rBOutputFormat_0 = new System.Windows.Forms.RadioButton();
            this.lbl_DIROutputConvertPDF = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputConvertPDF = new UtilityPDF.Controls.ModernButton();
            this.lbl_PDFToConvert = new System.Windows.Forms.Label();
            this.Btn_SelectPDFToConvert = new UtilityPDF.Controls.ModernButton();
            this.Btn_ResetConvert = new UtilityPDF.Controls.ModernButton();
            this.Btn_Convert = new UtilityPDF.Controls.ModernButton();
            this.lbl_ConvertInProgress = new System.Windows.Forms.Label();
            this.Btn_Exit = new UtilityPDF.Controls.ModernButton();
            this.spinnerCompress = new UtilityPDF.Controls.LoadingSpinner();
            this.spinnerMerge = new UtilityPDF.Controls.LoadingSpinner();
            this.spinnerConvert = new UtilityPDF.Controls.LoadingSpinner();
            ((System.ComponentModel.ISupportInitialize)(this.pB_ICO)).BeginInit();
            this.PnlOCR.SuspendLayout();
            this.PnlMerge.SuspendLayout();
            this.PnlCompress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_Compress)).BeginInit();
            this.PnlConvert.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_Exit
            // 
            this.Btn_Exit.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Exit.BorderRadius = 8;
            this.Btn_Exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Exit.FlatAppearance.BorderSize = 0;
            this.Btn_Exit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Exit.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.Btn_Exit.ForeColor = System.Drawing.Color.White;
            this.Btn_Exit.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.Btn_Exit.Location = new System.Drawing.Point(903, 470);
            this.Btn_Exit.Name = "Btn_Exit";
            this.Btn_Exit.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.Btn_Exit.Size = new System.Drawing.Size(130, 55);
            this.Btn_Exit.TabIndex = 21;
            this.Btn_Exit.Text = "❌ Exit";
            this.Btn_Exit.UseVisualStyleBackColor = false;
            this.Btn_Exit.Click += new System.EventHandler(this.Btn_Exit_Click);
            // 
            // oFD_PDF
            // 
            this.oFD_PDF.Filter = "PDF Files (*.pdf)|*.pdf";
            // 
            // pB_ICO
            // 
            this.pB_ICO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pB_ICO.Image = global::UtilityPDF.Properties.Resources.PDFUti;
            this.pB_ICO.InitialImage = null;
            this.pB_ICO.Location = new System.Drawing.Point(900, 553);
            this.pB_ICO.Name = "pB_ICO";
            this.pB_ICO.Size = new System.Drawing.Size(140, 140);
            this.pB_ICO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pB_ICO.TabIndex = 13;
            this.pB_ICO.TabStop = false;
            // 
            // lbl_Language
            // 
            this.lbl_Language.AutoSize = true;
            this.lbl_Language.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Language.Location = new System.Drawing.Point(900, 390);
            this.lbl_Language.Name = "lbl_Language";
            this.lbl_Language.Size = new System.Drawing.Size(120, 17);
            this.lbl_Language.TabIndex = 19;
            this.lbl_Language.Text = "🌐 Language UI:";
            // 
            // cmb_Language
            // 
            this.cmb_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Language.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Language.FormattingEnabled = true;
            this.cmb_Language.Location = new System.Drawing.Point(903, 420);
            this.cmb_Language.Name = "cmb_Language";
            this.cmb_Language.Size = new System.Drawing.Size(130, 23);
            this.cmb_Language.TabIndex = 20;
            this.cmb_Language.SelectedIndexChanged += new System.EventHandler(this.Cmb_Language_SelectedIndexChanged);
            // 
            // PnlOCR
            // 
            this.PnlOCR.BackColor = System.Drawing.Color.White;
            this.PnlOCR.BorderRadius = 12;
            this.PnlOCR.Controls.Add(this.modernProgressExtract);
            this.PnlOCR.Controls.Add(this.lblLang);
            this.PnlOCR.Controls.Add(this.cmbLangConv);
            this.PnlOCR.Controls.Add(this.Btn_Reset);
            this.PnlOCR.Controls.Add(this.Btn_Start);
            this.PnlOCR.Controls.Add(this.Btn_Abort);
            this.PnlOCR.Controls.Add(this.lbl_TXT);
            this.PnlOCR.Controls.Add(this.Btn_SelectDIROutputTXT);
            this.PnlOCR.Controls.Add(this.lbl_PDF);
            this.PnlOCR.Controls.Add(this.Btn_SelectPDF);
            this.PnlOCR.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.PnlOCR.HeaderFont = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.PnlOCR.HeaderText = "📄 Extract Text (OCR)";
            this.PnlOCR.Location = new System.Drawing.Point(15, 15);
            this.PnlOCR.Name = "PnlOCR";
            this.PnlOCR.Padding = new System.Windows.Forms.Padding(15, 50, 15, 15);
            this.PnlOCR.Size = new System.Drawing.Size(520, 360);
            this.PnlOCR.TabIndex = 0;
            // 
            // modernProgressExtract
            // 
            this.modernProgressExtract.BackColor = System.Drawing.Color.Transparent;
            this.modernProgressExtract.Location = new System.Drawing.Point(10, 308);
            this.modernProgressExtract.Maximum = 100;
            this.modernProgressExtract.Name = "modernProgressExtract";
            this.modernProgressExtract.ProgressBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.modernProgressExtract.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.modernProgressExtract.ShowPercentage = true;
            this.modernProgressExtract.Size = new System.Drawing.Size(490, 35);
            this.modernProgressExtract.TabIndex = 11;
            this.modernProgressExtract.Value = 0;
            // 
            // lblLang
            // 
            this.lblLang.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLang.Location = new System.Drawing.Point(10, 220);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(180, 25);
            this.lblLang.TabIndex = 9;
            this.lblLang.Text = "🌍 Language";
            this.lblLang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbLangConv
            // 
            this.cmbLangConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLangConv.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbLangConv.FormattingEnabled = true;
            this.cmbLangConv.Location = new System.Drawing.Point(195, 220);
            this.cmbLangConv.Name = "cmbLangConv";
            this.cmbLangConv.Size = new System.Drawing.Size(305, 23);
            this.cmbLangConv.TabIndex = 8;
            // 
            // Btn_Reset
            // 
            this.Btn_Reset.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Reset.BorderRadius = 8;
            this.Btn_Reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Reset.Enabled = false;
            this.Btn_Reset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Reset.FlatAppearance.BorderSize = 0;
            this.Btn_Reset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Reset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Reset.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_Reset.ForeColor = System.Drawing.Color.White;
            this.Btn_Reset.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_Reset.Location = new System.Drawing.Point(177, 255);
            this.Btn_Reset.Name = "Btn_Reset";
            this.Btn_Reset.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.Btn_Reset.Size = new System.Drawing.Size(155, 45);
            this.Btn_Reset.TabIndex = 7;
            this.Btn_Reset.Text = "🔄 Reset";
            this.Btn_Reset.UseVisualStyleBackColor = false;
            this.Btn_Reset.Click += new System.EventHandler(this.Btn_Reset_Click);
            // 
            // Btn_Start
            // 
            this.Btn_Start.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Start.BorderRadius = 8;
            this.Btn_Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Start.Enabled = false;
            this.Btn_Start.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Start.FlatAppearance.BorderSize = 0;
            this.Btn_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Start.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_Start.ForeColor = System.Drawing.Color.White;
            this.Btn_Start.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.Btn_Start.Location = new System.Drawing.Point(10, 255);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.Btn_Start.Size = new System.Drawing.Size(155, 45);
            this.Btn_Start.TabIndex = 6;
            this.Btn_Start.Text = "▶ Start";
            this.Btn_Start.UseVisualStyleBackColor = false;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // Btn_Abort
            // 
            this.Btn_Abort.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Abort.BorderRadius = 8;
            this.Btn_Abort.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Abort.Enabled = false;
            this.Btn_Abort.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Abort.FlatAppearance.BorderSize = 0;
            this.Btn_Abort.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Abort.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Abort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Abort.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_Abort.ForeColor = System.Drawing.Color.White;
            this.Btn_Abort.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.Btn_Abort.Location = new System.Drawing.Point(345, 255);
            this.Btn_Abort.Name = "Btn_Abort";
            this.Btn_Abort.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.Btn_Abort.Size = new System.Drawing.Size(155, 45);
            this.Btn_Abort.TabIndex = 11;
            this.Btn_Abort.Text = "⏹ Abort";
            this.Btn_Abort.UseVisualStyleBackColor = false;
            this.Btn_Abort.Click += new System.EventHandler(this.Btn_Abort_Click);
            // 
            // lbl_TXT
            // 
            this.lbl_TXT.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_TXT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_TXT.Location = new System.Drawing.Point(145, 143);
            this.lbl_TXT.Name = "lbl_TXT";
            this.lbl_TXT.Size = new System.Drawing.Size(355, 65);
            this.lbl_TXT.TabIndex = 5;
            this.lbl_TXT.Text = "Output directory...";
            // 
            // Btn_SelectDIROutputTXT
            // 
            this.Btn_SelectDIROutputTXT.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputTXT.BorderRadius = 8;
            this.Btn_SelectDIROutputTXT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectDIROutputTXT.Enabled = false;
            this.Btn_SelectDIROutputTXT.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectDIROutputTXT.FlatAppearance.BorderSize = 0;
            this.Btn_SelectDIROutputTXT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputTXT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputTXT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectDIROutputTXT.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectDIROutputTXT.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectDIROutputTXT.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.Btn_SelectDIROutputTXT.Location = new System.Drawing.Point(10, 143);
            this.Btn_SelectDIROutputTXT.Name = "Btn_SelectDIROutputTXT";
            this.Btn_SelectDIROutputTXT.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.Btn_SelectDIROutputTXT.Size = new System.Drawing.Size(125, 65);
            this.Btn_SelectDIROutputTXT.TabIndex = 4;
            this.Btn_SelectDIROutputTXT.Text = "📁 Output\r\nTXT";
            this.Btn_SelectDIROutputTXT.UseVisualStyleBackColor = false;
            this.Btn_SelectDIROutputTXT.Click += new System.EventHandler(this.Btn_SelectDIROutputTXT_Click);
            // 
            // lbl_PDF
            // 
            this.lbl_PDF.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_PDF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_PDF.Location = new System.Drawing.Point(145, 60);
            this.lbl_PDF.Name = "lbl_PDF";
            this.lbl_PDF.Size = new System.Drawing.Size(355, 65);
            this.lbl_PDF.TabIndex = 3;
            this.lbl_PDF.Text = "Select PDF file...";
            // 
            // Btn_SelectPDF
            // 
            this.Btn_SelectPDF.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDF.BorderRadius = 8;
            this.Btn_SelectPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectPDF.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectPDF.FlatAppearance.BorderSize = 0;
            this.Btn_SelectPDF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectPDF.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectPDF.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectPDF.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.Btn_SelectPDF.Location = new System.Drawing.Point(10, 60);
            this.Btn_SelectPDF.Name = "Btn_SelectPDF";
            this.Btn_SelectPDF.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.Btn_SelectPDF.Size = new System.Drawing.Size(125, 65);
            this.Btn_SelectPDF.TabIndex = 2;
            this.Btn_SelectPDF.Text = "📄 Select\r\nPDF";
            this.Btn_SelectPDF.UseVisualStyleBackColor = false;
            this.Btn_SelectPDF.Click += new System.EventHandler(this.Btn_SelectPDF_Click);
            // 
            // PnlMerge
            // 
            this.PnlMerge.BackColor = System.Drawing.Color.White;
            this.PnlMerge.BorderRadius = 12;
            this.PnlMerge.Controls.Add(this.lbl_DIROutputMergePDF);
            this.PnlMerge.Controls.Add(this.Btn_SelectDIROutputMergedPDF);
            this.PnlMerge.Controls.Add(this.Lstb_FileMerge);
            this.PnlMerge.Controls.Add(this.Btn_ResetMerge);
            this.PnlMerge.Controls.Add(this.Btn_Merge);
            this.PnlMerge.Controls.Add(this.Btn_SelectPDFToMerge);
            this.PnlMerge.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.PnlMerge.HeaderFont = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.PnlMerge.HeaderText = "🔗 Merge PDFs";
            this.PnlMerge.Location = new System.Drawing.Point(550, 15);
            this.PnlMerge.Name = "PnlMerge";
            this.PnlMerge.Padding = new System.Windows.Forms.Padding(15, 50, 15, 15);
            this.PnlMerge.Size = new System.Drawing.Size(480, 360);
            this.PnlMerge.TabIndex = 11;
            // 
            // lbl_DIROutputMergePDF
            // 
            this.lbl_DIROutputMergePDF.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_DIROutputMergePDF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_DIROutputMergePDF.Location = new System.Drawing.Point(141, 230);
            this.lbl_DIROutputMergePDF.Name = "lbl_DIROutputMergePDF";
            this.lbl_DIROutputMergePDF.Size = new System.Drawing.Size(324, 55);
            this.lbl_DIROutputMergePDF.TabIndex = 10;
            this.lbl_DIROutputMergePDF.Text = "Output merged PDF...";
            // 
            // Btn_SelectDIROutputMergedPDF
            // 
            this.Btn_SelectDIROutputMergedPDF.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputMergedPDF.BorderRadius = 8;
            this.Btn_SelectDIROutputMergedPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectDIROutputMergedPDF.Enabled = false;
            this.Btn_SelectDIROutputMergedPDF.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectDIROutputMergedPDF.FlatAppearance.BorderSize = 0;
            this.Btn_SelectDIROutputMergedPDF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputMergedPDF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputMergedPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectDIROutputMergedPDF.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectDIROutputMergedPDF.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectDIROutputMergedPDF.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.Btn_SelectDIROutputMergedPDF.Location = new System.Drawing.Point(10, 230);
            this.Btn_SelectDIROutputMergedPDF.Name = "Btn_SelectDIROutputMergedPDF";
            this.Btn_SelectDIROutputMergedPDF.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.Btn_SelectDIROutputMergedPDF.Size = new System.Drawing.Size(125, 65);
            this.Btn_SelectDIROutputMergedPDF.TabIndex = 9;
            this.Btn_SelectDIROutputMergedPDF.Text = "📁 Output\r\nPDF";
            this.Btn_SelectDIROutputMergedPDF.UseVisualStyleBackColor = false;
            this.Btn_SelectDIROutputMergedPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputMergedPDF_Click);
            // 
            // Lstb_FileMerge
            // 
            this.Lstb_FileMerge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lstb_FileMerge.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.Lstb_FileMerge.FormattingEnabled = true;
            this.Lstb_FileMerge.ItemHeight = 15;
            this.Lstb_FileMerge.Location = new System.Drawing.Point(141, 60);
            this.Lstb_FileMerge.Name = "Lstb_FileMerge";
            this.Lstb_FileMerge.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.Lstb_FileMerge.Size = new System.Drawing.Size(324, 152);
            this.Lstb_FileMerge.TabIndex = 8;
            // 
            // Btn_ResetMerge
            // 
            this.Btn_ResetMerge.BackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetMerge.BorderRadius = 8;
            this.Btn_ResetMerge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_ResetMerge.Enabled = false;
            this.Btn_ResetMerge.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_ResetMerge.FlatAppearance.BorderSize = 0;
            this.Btn_ResetMerge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetMerge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ResetMerge.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_ResetMerge.ForeColor = System.Drawing.Color.White;
            this.Btn_ResetMerge.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_ResetMerge.Location = new System.Drawing.Point(138, 300);
            this.Btn_ResetMerge.Name = "Btn_ResetMerge";
            this.Btn_ResetMerge.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.Btn_ResetMerge.Size = new System.Drawing.Size(155, 45);
            this.Btn_ResetMerge.TabIndex = 7;
            this.Btn_ResetMerge.Text = "🔄 Reset";
            this.Btn_ResetMerge.UseVisualStyleBackColor = false;
            this.Btn_ResetMerge.Click += new System.EventHandler(this.Btn_ResetMerge_Click);
            // 
            // Btn_Merge
            // 
            this.Btn_Merge.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Merge.BorderRadius = 8;
            this.Btn_Merge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Merge.Enabled = false;
            this.Btn_Merge.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Merge.FlatAppearance.BorderSize = 0;
            this.Btn_Merge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Merge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Merge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Merge.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_Merge.ForeColor = System.Drawing.Color.White;
            this.Btn_Merge.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.Btn_Merge.Location = new System.Drawing.Point(298, 300);
            this.Btn_Merge.Name = "Btn_Merge";
            this.Btn_Merge.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.Btn_Merge.Size = new System.Drawing.Size(155, 45);
            this.Btn_Merge.TabIndex = 6;
            this.Btn_Merge.Text = "🔗 Merge";
            this.Btn_Merge.UseVisualStyleBackColor = false;
            this.Btn_Merge.Click += new System.EventHandler(this.Btn_Merge_Click);
            // 
            // Btn_SelectPDFToMerge
            // 
            this.Btn_SelectPDFToMerge.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToMerge.BorderRadius = 8;
            this.Btn_SelectPDFToMerge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectPDFToMerge.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectPDFToMerge.FlatAppearance.BorderSize = 0;
            this.Btn_SelectPDFToMerge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToMerge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToMerge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectPDFToMerge.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectPDFToMerge.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectPDFToMerge.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(68)))), ((int)(((byte)(173)))));
            this.Btn_SelectPDFToMerge.Location = new System.Drawing.Point(10, 60);
            this.Btn_SelectPDFToMerge.Name = "Btn_SelectPDFToMerge";
            this.Btn_SelectPDFToMerge.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.Btn_SelectPDFToMerge.Size = new System.Drawing.Size(125, 65);
            this.Btn_SelectPDFToMerge.TabIndex = 2;
            this.Btn_SelectPDFToMerge.Text = "📄 Add\r\nPDF";
            this.Btn_SelectPDFToMerge.UseVisualStyleBackColor = false;
            this.Btn_SelectPDFToMerge.Click += new System.EventHandler(this.Btn_SelectPDFToMerge_Click);
            // 
            // PnlCompress
            // 
            this.PnlCompress.BackColor = System.Drawing.Color.White;
            this.PnlCompress.BorderRadius = 12;
            this.PnlCompress.Controls.Add(this.lbl_DIROutputCompressPDF);
            this.PnlCompress.Controls.Add(this.Btn_SelectDIROutputCompressPDF);
            this.PnlCompress.Controls.Add(this.lbl_ViewLvlCompres);
            this.PnlCompress.Controls.Add(this.lbl_LvlCompr);
            this.PnlCompress.Controls.Add(this.Tb_Compress);
            this.PnlCompress.Controls.Add(this.lbl_PDFToCompress);
            this.PnlCompress.Controls.Add(this.Btn_SelectPDFToCompress);
            this.PnlCompress.Controls.Add(this.Btn_ResetCompres);
            this.PnlCompress.Controls.Add(this.Btn_Compress);
            this.PnlCompress.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.PnlCompress.HeaderFont = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.PnlCompress.HeaderText = "🗜 Compress PDF";
            this.PnlCompress.Location = new System.Drawing.Point(390, 390);
            this.PnlCompress.Name = "PnlCompress";
            this.PnlCompress.Padding = new System.Windows.Forms.Padding(15, 50, 15, 15);
            this.PnlCompress.Size = new System.Drawing.Size(500, 300);
            this.PnlCompress.TabIndex = 12;
            // 
            // lbl_DIROutputCompressPDF
            // 
            this.lbl_DIROutputCompressPDF.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_DIROutputCompressPDF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_DIROutputCompressPDF.Location = new System.Drawing.Point(145, 115);
            this.lbl_DIROutputCompressPDF.Name = "lbl_DIROutputCompressPDF";
            this.lbl_DIROutputCompressPDF.Size = new System.Drawing.Size(340, 50);
            this.lbl_DIROutputCompressPDF.TabIndex = 15;
            this.lbl_DIROutputCompressPDF.Text = "Output compressed PDF...";
            // 
            // Btn_SelectDIROutputCompressPDF
            // 
            this.Btn_SelectDIROutputCompressPDF.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputCompressPDF.BorderRadius = 8;
            this.Btn_SelectDIROutputCompressPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectDIROutputCompressPDF.Enabled = false;
            this.Btn_SelectDIROutputCompressPDF.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectDIROutputCompressPDF.FlatAppearance.BorderSize = 0;
            this.Btn_SelectDIROutputCompressPDF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputCompressPDF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputCompressPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectDIROutputCompressPDF.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectDIROutputCompressPDF.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectDIROutputCompressPDF.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this.Btn_SelectDIROutputCompressPDF.Location = new System.Drawing.Point(10, 95);
            this.Btn_SelectDIROutputCompressPDF.Name = "Btn_SelectDIROutputCompressPDF";
            this.Btn_SelectDIROutputCompressPDF.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_SelectDIROutputCompressPDF.Size = new System.Drawing.Size(125, 55);
            this.Btn_SelectDIROutputCompressPDF.TabIndex = 14;
            this.Btn_SelectDIROutputCompressPDF.Text = "📁 Output\r\nPDF";
            this.Btn_SelectDIROutputCompressPDF.UseVisualStyleBackColor = false;
            this.Btn_SelectDIROutputCompressPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputCompressPDF_Click);
            // 
            // lbl_ViewLvlCompres
            // 
            this.lbl_ViewLvlCompres.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_ViewLvlCompres.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lbl_ViewLvlCompres.Location = new System.Drawing.Point(128, 210);
            this.lbl_ViewLvlCompres.Name = "lbl_ViewLvlCompres";
            this.lbl_ViewLvlCompres.Size = new System.Drawing.Size(353, 20);
            this.lbl_ViewLvlCompres.TabIndex = 13;
            this.lbl_ViewLvlCompres.Text = "Printer Quality";
            this.lbl_ViewLvlCompres.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_LvlCompr
            // 
            this.lbl_LvlCompr.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F);
            this.lbl_LvlCompr.Location = new System.Drawing.Point(10, 175);
            this.lbl_LvlCompr.Name = "lbl_LvlCompr";
            this.lbl_LvlCompr.Size = new System.Drawing.Size(179, 40);
            this.lbl_LvlCompr.TabIndex = 12;
            this.lbl_LvlCompr.Text = "⚙ Level Compression:";
            this.lbl_LvlCompr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Tb_Compress
            // 
            this.Tb_Compress.Enabled = false;
            this.Tb_Compress.LargeChange = 1;
            this.Tb_Compress.Location = new System.Drawing.Point(195, 170);
            this.Tb_Compress.Maximum = 3;
            this.Tb_Compress.Name = "Tb_Compress";
            this.Tb_Compress.Size = new System.Drawing.Size(290, 45);
            this.Tb_Compress.TabIndex = 11;
            this.Tb_Compress.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Tb_Compress.Value = 1;
            this.Tb_Compress.ValueChanged += new System.EventHandler(this.Tb_Compress_ValueChanged);
            // 
            // lbl_PDFToCompress
            // 
            this.lbl_PDFToCompress.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_PDFToCompress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_PDFToCompress.Location = new System.Drawing.Point(145, 60);
            this.lbl_PDFToCompress.Name = "lbl_PDFToCompress";
            this.lbl_PDFToCompress.Size = new System.Drawing.Size(340, 40);
            this.lbl_PDFToCompress.TabIndex = 10;
            this.lbl_PDFToCompress.Text = "Select PDF to compress...";
            // 
            // Btn_SelectPDFToCompress
            // 
            this.Btn_SelectPDFToCompress.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToCompress.BorderRadius = 8;
            this.Btn_SelectPDFToCompress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectPDFToCompress.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectPDFToCompress.FlatAppearance.BorderSize = 0;
            this.Btn_SelectPDFToCompress.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToCompress.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToCompress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectPDFToCompress.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectPDFToCompress.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectPDFToCompress.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this.Btn_SelectPDFToCompress.Location = new System.Drawing.Point(10, 47);
            this.Btn_SelectPDFToCompress.Name = "Btn_SelectPDFToCompress";
            this.Btn_SelectPDFToCompress.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_SelectPDFToCompress.Size = new System.Drawing.Size(125, 40);
            this.Btn_SelectPDFToCompress.TabIndex = 9;
            this.Btn_SelectPDFToCompress.Text = "📄 Select PDF";
            this.Btn_SelectPDFToCompress.UseVisualStyleBackColor = false;
            this.Btn_SelectPDFToCompress.Click += new System.EventHandler(this.Btn_SelectPDFToCompress_Click);
            // 
            // Btn_ResetCompres
            // 
            this.Btn_ResetCompres.BackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetCompres.BorderRadius = 8;
            this.Btn_ResetCompres.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_ResetCompres.Enabled = false;
            this.Btn_ResetCompres.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_ResetCompres.FlatAppearance.BorderSize = 0;
            this.Btn_ResetCompres.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetCompres.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetCompres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ResetCompres.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_ResetCompres.ForeColor = System.Drawing.Color.White;
            this.Btn_ResetCompres.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_ResetCompres.Location = new System.Drawing.Point(166, 240);
            this.Btn_ResetCompres.Name = "Btn_ResetCompres";
            this.Btn_ResetCompres.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.Btn_ResetCompres.Size = new System.Drawing.Size(155, 45);
            this.Btn_ResetCompres.TabIndex = 8;
            this.Btn_ResetCompres.Text = "🔄 Reset";
            this.Btn_ResetCompres.UseVisualStyleBackColor = false;
            this.Btn_ResetCompres.Click += new System.EventHandler(this.Btn_ResetCompres_Click);
            // 
            // Btn_Compress
            // 
            this.Btn_Compress.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Compress.BorderRadius = 8;
            this.Btn_Compress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Compress.Enabled = false;
            this.Btn_Compress.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Compress.FlatAppearance.BorderSize = 0;
            this.Btn_Compress.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Compress.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Compress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Compress.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_Compress.ForeColor = System.Drawing.Color.White;
            this.Btn_Compress.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this.Btn_Compress.Location = new System.Drawing.Point(327, 241);
            this.Btn_Compress.Name = "Btn_Compress";
            this.Btn_Compress.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_Compress.Size = new System.Drawing.Size(155, 45);
            this.Btn_Compress.TabIndex = 3;
            this.Btn_Compress.Text = "🗜 Compress";
            this.Btn_Compress.UseVisualStyleBackColor = false;
            this.Btn_Compress.Click += new System.EventHandler(this.Btn_Compress_Click);
            // 
            // lbl_CompressInProgress
            // 
            this.lbl_CompressInProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_CompressInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CompressInProgress.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CompressInProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lbl_CompressInProgress.Location = new System.Drawing.Point(390, 390);
            this.lbl_CompressInProgress.Name = "lbl_CompressInProgress";
            this.lbl_CompressInProgress.Size = new System.Drawing.Size(500, 300);
            this.lbl_CompressInProgress.TabIndex = 15;
            this.lbl_CompressInProgress.Text = "⏳ Compression in progress...\r\n\r\nPlease wait...";
            this.lbl_CompressInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_CompressInProgress.Visible = false;
            // 
            // lbl_MergeInProgress
            // 
            this.lbl_MergeInProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_MergeInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_MergeInProgress.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MergeInProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.lbl_MergeInProgress.Location = new System.Drawing.Point(550, 15);
            this.lbl_MergeInProgress.Name = "lbl_MergeInProgress";
            this.lbl_MergeInProgress.Size = new System.Drawing.Size(480, 360);
            this.lbl_MergeInProgress.TabIndex = 16;
            this.lbl_MergeInProgress.Text = "⏳ Merge in progress...\r\n\r\nPlease wait...";
            this.lbl_MergeInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_MergeInProgress.Visible = false;
            // 
            // PnlConvert
            // 
            this.PnlConvert.BackColor = System.Drawing.Color.White;
            this.PnlConvert.BorderRadius = 12;
            this.PnlConvert.Controls.Add(this.rBOutputFormat_2);
            this.PnlConvert.Controls.Add(this.rBOutputFormat_1);
            this.PnlConvert.Controls.Add(this.rBOutputFormat_0);
            this.PnlConvert.Controls.Add(this.lbl_DIROutputConvertPDF);
            this.PnlConvert.Controls.Add(this.Btn_SelectDIROutputConvertPDF);
            this.PnlConvert.Controls.Add(this.lbl_PDFToConvert);
            this.PnlConvert.Controls.Add(this.Btn_SelectPDFToConvert);
            this.PnlConvert.Controls.Add(this.Btn_ResetConvert);
            this.PnlConvert.Controls.Add(this.Btn_Convert);
            this.PnlConvert.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.PnlConvert.HeaderFont = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.PnlConvert.HeaderText = "🔄 Convert to DOCX/RTF";
            this.PnlConvert.Location = new System.Drawing.Point(15, 390);
            this.PnlConvert.Name = "PnlConvert";
            this.PnlConvert.Padding = new System.Windows.Forms.Padding(15, 50, 15, 15);
            this.PnlConvert.Size = new System.Drawing.Size(360, 300);
            this.PnlConvert.TabIndex = 17;
            // 
            // rBOutputFormat_2
            // 
            this.rBOutputFormat_2.AutoSize = true;
            this.rBOutputFormat_2.Enabled = false;
            this.rBOutputFormat_2.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.rBOutputFormat_2.Location = new System.Drawing.Point(10, 210);
            this.rBOutputFormat_2.Name = "rBOutputFormat_2";
            this.rBOutputFormat_2.Size = new System.Drawing.Size(122, 20);
            this.rBOutputFormat_2.TabIndex = 18;
            this.rBOutputFormat_2.Text = "📝 DOCX and RTF";
            this.rBOutputFormat_2.UseVisualStyleBackColor = true;
            // 
            // rBOutputFormat_1
            // 
            this.rBOutputFormat_1.AutoSize = true;
            this.rBOutputFormat_1.Enabled = false;
            this.rBOutputFormat_1.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.rBOutputFormat_1.Location = new System.Drawing.Point(10, 185);
            this.rBOutputFormat_1.Name = "rBOutputFormat_1";
            this.rBOutputFormat_1.Size = new System.Drawing.Size(90, 20);
            this.rBOutputFormat_1.TabIndex = 17;
            this.rBOutputFormat_1.Text = "📝 RTF only";
            this.rBOutputFormat_1.UseVisualStyleBackColor = true;
            // 
            // rBOutputFormat_0
            // 
            this.rBOutputFormat_0.AutoSize = true;
            this.rBOutputFormat_0.Checked = true;
            this.rBOutputFormat_0.Enabled = false;
            this.rBOutputFormat_0.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.rBOutputFormat_0.Location = new System.Drawing.Point(10, 160);
            this.rBOutputFormat_0.Name = "rBOutputFormat_0";
            this.rBOutputFormat_0.Size = new System.Drawing.Size(102, 20);
            this.rBOutputFormat_0.TabIndex = 16;
            this.rBOutputFormat_0.TabStop = true;
            this.rBOutputFormat_0.Text = "📝 DOCX only";
            this.rBOutputFormat_0.UseVisualStyleBackColor = true;
            // 
            // lbl_DIROutputConvertPDF
            // 
            this.lbl_DIROutputConvertPDF.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_DIROutputConvertPDF.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_DIROutputConvertPDF.Location = new System.Drawing.Point(145, 115);
            this.lbl_DIROutputConvertPDF.Name = "lbl_DIROutputConvertPDF";
            this.lbl_DIROutputConvertPDF.Size = new System.Drawing.Size(200, 40);
            this.lbl_DIROutputConvertPDF.TabIndex = 15;
            this.lbl_DIROutputConvertPDF.Text = "Output DOCX/RTF...";
            // 
            // Btn_SelectDIROutputConvertPDF
            // 
            this.Btn_SelectDIROutputConvertPDF.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputConvertPDF.BorderRadius = 8;
            this.Btn_SelectDIROutputConvertPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectDIROutputConvertPDF.Enabled = false;
            this.Btn_SelectDIROutputConvertPDF.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectDIROutputConvertPDF.FlatAppearance.BorderSize = 0;
            this.Btn_SelectDIROutputConvertPDF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputConvertPDF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectDIROutputConvertPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectDIROutputConvertPDF.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectDIROutputConvertPDF.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectDIROutputConvertPDF.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.Btn_SelectDIROutputConvertPDF.Location = new System.Drawing.Point(10, 102);
            this.Btn_SelectDIROutputConvertPDF.Name = "Btn_SelectDIROutputConvertPDF";
            this.Btn_SelectDIROutputConvertPDF.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.Btn_SelectDIROutputConvertPDF.Size = new System.Drawing.Size(125, 40);
            this.Btn_SelectDIROutputConvertPDF.TabIndex = 14;
            this.Btn_SelectDIROutputConvertPDF.Text = "📁 Output";
            this.Btn_SelectDIROutputConvertPDF.UseVisualStyleBackColor = false;
            this.Btn_SelectDIROutputConvertPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputConvertPDF_Click);
            // 
            // lbl_PDFToConvert
            // 
            this.lbl_PDFToConvert.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.lbl_PDFToConvert.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lbl_PDFToConvert.Location = new System.Drawing.Point(145, 60);
            this.lbl_PDFToConvert.Name = "lbl_PDFToConvert";
            this.lbl_PDFToConvert.Size = new System.Drawing.Size(200, 40);
            this.lbl_PDFToConvert.TabIndex = 10;
            this.lbl_PDFToConvert.Text = "Select PDF to convert...";
            // 
            // Btn_SelectPDFToConvert
            // 
            this.Btn_SelectPDFToConvert.BackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToConvert.BorderRadius = 8;
            this.Btn_SelectPDFToConvert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_SelectPDFToConvert.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_SelectPDFToConvert.FlatAppearance.BorderSize = 0;
            this.Btn_SelectPDFToConvert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToConvert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_SelectPDFToConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_SelectPDFToConvert.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_SelectPDFToConvert.ForeColor = System.Drawing.Color.White;
            this.Btn_SelectPDFToConvert.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.Btn_SelectPDFToConvert.Location = new System.Drawing.Point(10, 47);
            this.Btn_SelectPDFToConvert.Name = "Btn_SelectPDFToConvert";
            this.Btn_SelectPDFToConvert.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.Btn_SelectPDFToConvert.Size = new System.Drawing.Size(125, 40);
            this.Btn_SelectPDFToConvert.TabIndex = 9;
            this.Btn_SelectPDFToConvert.Text = "📄 Select PDF";
            this.Btn_SelectPDFToConvert.UseVisualStyleBackColor = false;
            this.Btn_SelectPDFToConvert.Click += new System.EventHandler(this.Btn_SelectPDFToConvert_Click);
            // 
            // Btn_ResetConvert
            // 
            this.Btn_ResetConvert.BackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetConvert.BorderRadius = 8;
            this.Btn_ResetConvert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_ResetConvert.Enabled = false;
            this.Btn_ResetConvert.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_ResetConvert.FlatAppearance.BorderSize = 0;
            this.Btn_ResetConvert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetConvert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_ResetConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_ResetConvert.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_ResetConvert.ForeColor = System.Drawing.Color.White;
            this.Btn_ResetConvert.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.Btn_ResetConvert.Location = new System.Drawing.Point(10, 240);
            this.Btn_ResetConvert.Name = "Btn_ResetConvert";
            this.Btn_ResetConvert.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.Btn_ResetConvert.Size = new System.Drawing.Size(155, 45);
            this.Btn_ResetConvert.TabIndex = 8;
            this.Btn_ResetConvert.Text = "🔄 Reset";
            this.Btn_ResetConvert.UseVisualStyleBackColor = false;
            this.Btn_ResetConvert.Click += new System.EventHandler(this.Btn_ResetConvert_Click);
            // 
            // Btn_Convert
            // 
            this.Btn_Convert.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Convert.BorderRadius = 8;
            this.Btn_Convert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Convert.Enabled = false;
            this.Btn_Convert.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Btn_Convert.FlatAppearance.BorderSize = 0;
            this.Btn_Convert.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.Btn_Convert.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.Btn_Convert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Convert.Font = new System.Drawing.Font("Segoe UI Emoji", 8.5F);
            this.Btn_Convert.ForeColor = System.Drawing.Color.White;
            this.Btn_Convert.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(160)))), ((int)(((byte)(133)))));
            this.Btn_Convert.Location = new System.Drawing.Point(183, 240);
            this.Btn_Convert.Name = "Btn_Convert";
            this.Btn_Convert.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.Btn_Convert.Size = new System.Drawing.Size(155, 45);
            this.Btn_Convert.TabIndex = 3;
            this.Btn_Convert.Text = "🔄 Convert";
            this.Btn_Convert.UseVisualStyleBackColor = false;
            this.Btn_Convert.Click += new System.EventHandler(this.Btn_Convert_Click);
            // 
            // spinnerCompress
            // 
            this.spinnerCompress.BackColor = System.Drawing.Color.Transparent;
            this.spinnerCompress.Location = new System.Drawing.Point(640, 500);
            this.spinnerCompress.Name = "spinnerCompress";
            this.spinnerCompress.Size = new System.Drawing.Size(50, 50);
            this.spinnerCompress.SpinnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.spinnerCompress.TabIndex = 21;
            this.spinnerCompress.Visible = false;
            // 
            // spinnerMerge
            // 
            this.spinnerMerge.BackColor = System.Drawing.Color.Transparent;
            this.spinnerMerge.Location = new System.Drawing.Point(790, 180);
            this.spinnerMerge.Name = "spinnerMerge";
            this.spinnerMerge.Size = new System.Drawing.Size(50, 50);
            this.spinnerMerge.SpinnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.spinnerMerge.TabIndex = 22;
            this.spinnerMerge.Visible = false;
            // 
            // spinnerConvert
            // 
            this.spinnerConvert.BackColor = System.Drawing.Color.Transparent;
            this.spinnerConvert.Location = new System.Drawing.Point(187, 440);
            this.spinnerConvert.Name = "spinnerConvert";
            this.spinnerConvert.Size = new System.Drawing.Size(50, 50);
            this.spinnerConvert.SpinnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.spinnerConvert.TabIndex = 23;
            this.spinnerConvert.Visible = false;
            // 
            // lbl_CompressInProgress
            // 
            this.lbl_CompressInProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_CompressInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CompressInProgress.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CompressInProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.lbl_CompressInProgress.Location = new System.Drawing.Point(390, 390);
            this.lbl_CompressInProgress.Name = "lbl_CompressInProgress";
            this.lbl_CompressInProgress.Size = new System.Drawing.Size(500, 300);
            this.lbl_CompressInProgress.TabIndex = 15;
            this.lbl_CompressInProgress.Text = "";
            this.lbl_CompressInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_CompressInProgress.Visible = false;
            // 
            // lbl_MergeInProgress
            // 
            this.lbl_MergeInProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_MergeInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_MergeInProgress.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MergeInProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.lbl_MergeInProgress.Location = new System.Drawing.Point(550, 15);
            this.lbl_MergeInProgress.Name = "lbl_MergeInProgress";
            this.lbl_MergeInProgress.Size = new System.Drawing.Size(480, 360);
            this.lbl_MergeInProgress.TabIndex = 16;
            this.lbl_MergeInProgress.Text = "";
            this.lbl_MergeInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_MergeInProgress.Visible = false;
            // 
            // lbl_ConvertInProgress
            // 
            this.lbl_ConvertInProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lbl_ConvertInProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_ConvertInProgress.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ConvertInProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.lbl_ConvertInProgress.Location = new System.Drawing.Point(15, 390);
            this.lbl_ConvertInProgress.Name = "lbl_ConvertInProgress";
            this.lbl_ConvertInProgress.Size = new System.Drawing.Size(360, 300);
            this.lbl_ConvertInProgress.TabIndex = 18;
            this.lbl_ConvertInProgress.Text = "";
            this.lbl_ConvertInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_ConvertInProgress.Visible = false;
            // 
            // FrmUtiPDF_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1045, 696);
            this.Controls.Add(this.cmb_Language);
            this.Controls.Add(this.lbl_Language);
            this.Controls.Add(this.PnlConvert);
            this.Controls.Add(this.Btn_Exit);
            this.Controls.Add(this.pB_ICO);
            this.Controls.Add(this.PnlCompress);
            this.Controls.Add(this.PnlMerge);
            this.Controls.Add(this.PnlOCR);
            this.Controls.Add(this.lbl_MergeInProgress);
            this.Controls.Add(this.lbl_ConvertInProgress);
            this.Controls.Add(this.lbl_CompressInProgress);
            this.Controls.Add(this.spinnerConvert);
            this.Controls.Add(this.spinnerMerge);
            this.Controls.Add(this.spinnerCompress);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmUtiPDF_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "📄 PDF Utility - Modern UI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUtiPDF_Main_FormClosing);
            this.Load += new System.EventHandler(this.FrmUtiPDF_Main_Load);
            this.Shown += new System.EventHandler(this.FrmUtiPDF_Main_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pB_ICO)).EndInit();
            this.PnlOCR.ResumeLayout(false);
            this.PnlMerge.ResumeLayout(false);
            this.PnlCompress.ResumeLayout(false);
            this.PnlCompress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_Compress)).EndInit();
            this.PnlConvert.ResumeLayout(false);
            this.PnlConvert.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ModernCard PnlOCR;
        private Controls.ModernButton Btn_SelectPDF;
        private System.Windows.Forms.OpenFileDialog oFD_PDF;
        private System.Windows.Forms.Label lbl_PDF;
        private System.Windows.Forms.Label lbl_TXT;
        private Controls.ModernButton Btn_SelectDIROutputTXT;
        private System.Windows.Forms.FolderBrowserDialog fBD_TXT;
        private Controls.ModernButton Btn_Start;
        private Controls.ModernButton Btn_Reset;
        private System.Windows.Forms.ComboBox cmbLangConv;
        private System.Windows.Forms.Label lblLang;
        private Controls.ModernCard PnlMerge;
        private System.Windows.Forms.Label lbl_DIROutputMergePDF;
        private Controls.ModernButton Btn_SelectDIROutputMergedPDF;
        private System.Windows.Forms.ListBox Lstb_FileMerge;
        private Controls.ModernButton Btn_ResetMerge;
        private Controls.ModernButton Btn_Merge;
        private Controls.ModernButton Btn_SelectPDFToMerge;
        private Controls.ModernCard PnlCompress;
        private Controls.ModernButton Btn_Compress;
        private System.Windows.Forms.Label lbl_PDFToCompress;
        private Controls.ModernButton Btn_SelectPDFToCompress;
        private Controls.ModernButton Btn_ResetCompres;
        private System.Windows.Forms.TrackBar Tb_Compress;
        private System.Windows.Forms.Label lbl_LvlCompr;
        private System.Windows.Forms.Label lbl_ViewLvlCompres;
        private System.Windows.Forms.Label lbl_DIROutputCompressPDF;
        private Controls.ModernButton Btn_SelectDIROutputCompressPDF;
        private System.Windows.Forms.PictureBox pB_ICO;
        private Controls.ModernProgressBar modernProgressExtract;
        private Controls.ModernButton Btn_Exit;
        private Controls.ModernButton Btn_Abort;
        private System.Windows.Forms.Label lbl_CompressInProgress;
        private System.Windows.Forms.Label lbl_MergeInProgress;
        private Controls.ModernCard PnlConvert;
        private System.Windows.Forms.Label lbl_DIROutputConvertPDF;
        private Controls.ModernButton Btn_SelectDIROutputConvertPDF;
        private System.Windows.Forms.Label lbl_PDFToConvert;
        private Controls.ModernButton Btn_SelectPDFToConvert;
        private Controls.ModernButton Btn_ResetConvert;
        private Controls.ModernButton Btn_Convert;
        private System.Windows.Forms.Label lbl_ConvertInProgress;
        private System.Windows.Forms.RadioButton rBOutputFormat_0;
        private System.Windows.Forms.RadioButton rBOutputFormat_2;
        private System.Windows.Forms.RadioButton rBOutputFormat_1;
        private System.Windows.Forms.Label lbl_Language;
        private System.Windows.Forms.ComboBox cmb_Language;
        private UtilityPDF.Controls.LoadingSpinner spinnerCompress;
        private UtilityPDF.Controls.LoadingSpinner spinnerMerge;
        private UtilityPDF.Controls.LoadingSpinner spinnerConvert;
    }
}