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
            this.PnlOCR = new System.Windows.Forms.Panel();
            this.Btn_Abort = new System.Windows.Forms.Button();
            this.pBProgressExtract = new System.Windows.Forms.PictureBox();
            this.lblLang = new System.Windows.Forms.Label();
            this.cmbLangConv = new System.Windows.Forms.ComboBox();
            this.Btn_Reset = new System.Windows.Forms.Button();
            this.Btn_Start = new System.Windows.Forms.Button();
            this.lbl_TXT = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputTXT = new System.Windows.Forms.Button();
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
            this.lbl_DIROutputCompressPDF = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputCompressPDF = new System.Windows.Forms.Button();
            this.lbl_ViewLvlCompres = new System.Windows.Forms.Label();
            this.lbl_LvlCompr = new System.Windows.Forms.Label();
            this.Tb_Compress = new System.Windows.Forms.TrackBar();
            this.lbl_PDFToCompress = new System.Windows.Forms.Label();
            this.Btn_SelectPDFToCompress = new System.Windows.Forms.Button();
            this.Btn_ResetCompres = new System.Windows.Forms.Button();
            this.Btn_Compress = new System.Windows.Forms.Button();
            this.lblCompr = new System.Windows.Forms.Label();
            this.pB_ICO = new System.Windows.Forms.PictureBox();
            this.Btn_Exit = new System.Windows.Forms.Button();
            this.lbl_CompressInProgress = new System.Windows.Forms.Label();
            this.lbl_MergeInProgress = new System.Windows.Forms.Label();
            this.PnlConvert = new System.Windows.Forms.Panel();
            this.lbl_DIROutputConvertPDF = new System.Windows.Forms.Label();
            this.Btn_SelectDIROutputConvertPDF = new System.Windows.Forms.Button();
            this.lbl_PDFToConvert = new System.Windows.Forms.Label();
            this.Btn_SelectPDFToConvert = new System.Windows.Forms.Button();
            this.Btn_ResetConvert = new System.Windows.Forms.Button();
            this.Btn_Convert = new System.Windows.Forms.Button();
            this.lbl_ConvDOCX = new System.Windows.Forms.Label();
            this.lbl_ConvertInProgress = new System.Windows.Forms.Label();
            this.PnlOCR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBProgressExtract)).BeginInit();
            this.PnlMerge.SuspendLayout();
            this.PnlCompress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_Compress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pB_ICO)).BeginInit();
            this.PnlConvert.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlOCR
            // 
            this.PnlOCR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlOCR.Controls.Add(this.Btn_Abort);
            this.PnlOCR.Controls.Add(this.pBProgressExtract);
            this.PnlOCR.Controls.Add(this.lblLang);
            this.PnlOCR.Controls.Add(this.cmbLangConv);
            this.PnlOCR.Controls.Add(this.Btn_Reset);
            this.PnlOCR.Controls.Add(this.Btn_Start);
            this.PnlOCR.Controls.Add(this.lbl_TXT);
            this.PnlOCR.Controls.Add(this.Btn_SelectDIROutputTXT);
            this.PnlOCR.Controls.Add(this.lbl_PDF);
            this.PnlOCR.Controls.Add(this.Btn_SelectPDF);
            this.PnlOCR.Controls.Add(this.lblOCR);
            this.PnlOCR.Location = new System.Drawing.Point(14, 14);
            this.PnlOCR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlOCR.Name = "PnlOCR";
            this.PnlOCR.Size = new System.Drawing.Size(439, 297);
            this.PnlOCR.TabIndex = 0;
            // 
            // Btn_Abort
            // 
            this.Btn_Abort.Enabled = false;
            this.Btn_Abort.Location = new System.Drawing.Point(221, 239);
            this.Btn_Abort.Name = "Btn_Abort";
            this.Btn_Abort.Size = new System.Drawing.Size(104, 22);
            this.Btn_Abort.TabIndex = 11;
            this.Btn_Abort.UseVisualStyleBackColor = true;
            this.Btn_Abort.Click += new System.EventHandler(this.Btn_Abort_Click);
            // 
            // pBProgressExtract
            // 
            this.pBProgressExtract.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pBProgressExtract.Location = new System.Drawing.Point(5, 267);
            this.pBProgressExtract.Name = "pBProgressExtract";
            this.pBProgressExtract.Size = new System.Drawing.Size(428, 21);
            this.pBProgressExtract.TabIndex = 10;
            this.pBProgressExtract.TabStop = false;
            // 
            // lblLang
            // 
            this.lblLang.AutoSize = true;
            this.lblLang.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLang.Location = new System.Drawing.Point(4, 214);
            this.lblLang.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(0, 15);
            this.lblLang.TabIndex = 9;
            // 
            // cmbLangConv
            // 
            this.cmbLangConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLangConv.FormattingEnabled = true;
            this.cmbLangConv.Location = new System.Drawing.Point(223, 210);
            this.cmbLangConv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbLangConv.Name = "cmbLangConv";
            this.cmbLangConv.Size = new System.Drawing.Size(210, 23);
            this.cmbLangConv.TabIndex = 8;
            // 
            // Btn_Reset
            // 
            this.Btn_Reset.Enabled = false;
            this.Btn_Reset.Location = new System.Drawing.Point(113, 239);
            this.Btn_Reset.Name = "Btn_Reset";
            this.Btn_Reset.Size = new System.Drawing.Size(104, 22);
            this.Btn_Reset.TabIndex = 7;
            this.Btn_Reset.UseVisualStyleBackColor = true;
            this.Btn_Reset.Click += new System.EventHandler(this.Btn_Reset_Click);
            // 
            // Btn_Start
            // 
            this.Btn_Start.Enabled = false;
            this.Btn_Start.Location = new System.Drawing.Point(5, 239);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(104, 22);
            this.Btn_Start.TabIndex = 6;
            this.Btn_Start.UseVisualStyleBackColor = true;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // lbl_TXT
            // 
            this.lbl_TXT.Location = new System.Drawing.Point(112, 122);
            this.lbl_TXT.Name = "lbl_TXT";
            this.lbl_TXT.Size = new System.Drawing.Size(275, 75);
            this.lbl_TXT.TabIndex = 5;
            // 
            // Btn_SelectDIROutputTXT
            // 
            this.Btn_SelectDIROutputTXT.Enabled = false;
            this.Btn_SelectDIROutputTXT.Location = new System.Drawing.Point(5, 122);
            this.Btn_SelectDIROutputTXT.Name = "Btn_SelectDIROutputTXT";
            this.Btn_SelectDIROutputTXT.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectDIROutputTXT.TabIndex = 4;
            this.Btn_SelectDIROutputTXT.UseVisualStyleBackColor = true;
            this.Btn_SelectDIROutputTXT.Click += new System.EventHandler(this.Btn_SelectDIROutputTXT_Click);
            // 
            // lbl_PDF
            // 
            this.lbl_PDF.Location = new System.Drawing.Point(112, 27);
            this.lbl_PDF.Name = "lbl_PDF";
            this.lbl_PDF.Size = new System.Drawing.Size(275, 75);
            this.lbl_PDF.TabIndex = 3;
            // 
            // Btn_SelectPDF
            // 
            this.Btn_SelectPDF.Location = new System.Drawing.Point(5, 27);
            this.Btn_SelectPDF.Name = "Btn_SelectPDF";
            this.Btn_SelectPDF.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectPDF.TabIndex = 2;
            this.Btn_SelectPDF.UseVisualStyleBackColor = true;
            this.Btn_SelectPDF.Click += new System.EventHandler(this.Btn_SelectPDF_Click);
            // 
            // lblOCR
            // 
            this.lblOCR.AutoSize = true;
            this.lblOCR.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOCR.Location = new System.Drawing.Point(5, 3);
            this.lblOCR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOCR.Name = "lblOCR";
            this.lblOCR.Size = new System.Drawing.Size(0, 18);
            this.lblOCR.TabIndex = 1;
            // 
            // oFD_PDF
            // 
            this.oFD_PDF.Filter = "PDF Files (*.pdf)|*.pdf";
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
            // 
            // Btn_SelectDIROutputMergedPDF
            // 
            this.Btn_SelectDIROutputMergedPDF.Enabled = false;
            this.Btn_SelectDIROutputMergedPDF.Location = new System.Drawing.Point(5, 207);
            this.Btn_SelectDIROutputMergedPDF.Name = "Btn_SelectDIROutputMergedPDF";
            this.Btn_SelectDIROutputMergedPDF.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectDIROutputMergedPDF.TabIndex = 9;
            this.Btn_SelectDIROutputMergedPDF.UseVisualStyleBackColor = true;
            this.Btn_SelectDIROutputMergedPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputMergedPDF_Click);
            // 
            // Lstb_FileMerge
            // 
            this.Lstb_FileMerge.FormattingEnabled = true;
            this.Lstb_FileMerge.ItemHeight = 15;
            this.Lstb_FileMerge.Location = new System.Drawing.Point(113, 27);
            this.Lstb_FileMerge.Name = "Lstb_FileMerge";
            this.Lstb_FileMerge.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.Lstb_FileMerge.Size = new System.Drawing.Size(254, 169);
            this.Lstb_FileMerge.TabIndex = 8;
            // 
            // Btn_ResetMerge
            // 
            this.Btn_ResetMerge.Enabled = false;
            this.Btn_ResetMerge.Location = new System.Drawing.Point(113, 268);
            this.Btn_ResetMerge.Name = "Btn_ResetMerge";
            this.Btn_ResetMerge.Size = new System.Drawing.Size(104, 22);
            this.Btn_ResetMerge.TabIndex = 7;
            this.Btn_ResetMerge.UseVisualStyleBackColor = true;
            this.Btn_ResetMerge.Click += new System.EventHandler(this.Btn_ResetMerge_Click);
            // 
            // Btn_Merge
            // 
            this.Btn_Merge.Enabled = false;
            this.Btn_Merge.Location = new System.Drawing.Point(5, 268);
            this.Btn_Merge.Name = "Btn_Merge";
            this.Btn_Merge.Size = new System.Drawing.Size(104, 22);
            this.Btn_Merge.TabIndex = 6;
            this.Btn_Merge.UseVisualStyleBackColor = true;
            this.Btn_Merge.Click += new System.EventHandler(this.Btn_Merge_Click);
            // 
            // Btn_SelectPDFToMerge
            // 
            this.Btn_SelectPDFToMerge.Location = new System.Drawing.Point(5, 27);
            this.Btn_SelectPDFToMerge.Name = "Btn_SelectPDFToMerge";
            this.Btn_SelectPDFToMerge.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectPDFToMerge.TabIndex = 2;
            this.Btn_SelectPDFToMerge.UseVisualStyleBackColor = true;
            this.Btn_SelectPDFToMerge.Click += new System.EventHandler(this.Btn_SelectPDFToMerge_Click);
            // 
            // lblMerge
            // 
            this.lblMerge.AutoSize = true;
            this.lblMerge.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMerge.Location = new System.Drawing.Point(5, 3);
            this.lblMerge.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMerge.Name = "lblMerge";
            this.lblMerge.Size = new System.Drawing.Size(0, 18);
            this.lblMerge.TabIndex = 1;
            // 
            // PnlCompress
            // 
            this.PnlCompress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlCompress.Controls.Add(this.lbl_DIROutputCompressPDF);
            this.PnlCompress.Controls.Add(this.Btn_SelectDIROutputCompressPDF);
            this.PnlCompress.Controls.Add(this.lbl_ViewLvlCompres);
            this.PnlCompress.Controls.Add(this.lbl_LvlCompr);
            this.PnlCompress.Controls.Add(this.Tb_Compress);
            this.PnlCompress.Controls.Add(this.lbl_PDFToCompress);
            this.PnlCompress.Controls.Add(this.Btn_SelectPDFToCompress);
            this.PnlCompress.Controls.Add(this.Btn_ResetCompres);
            this.PnlCompress.Controls.Add(this.Btn_Compress);
            this.PnlCompress.Controls.Add(this.lblCompr);
            this.PnlCompress.Location = new System.Drawing.Point(325, 330);
            this.PnlCompress.Name = "PnlCompress";
            this.PnlCompress.Size = new System.Drawing.Size(420, 251);
            this.PnlCompress.TabIndex = 12;
            // 
            // lbl_DIROutputCompressPDF
            // 
            this.lbl_DIROutputCompressPDF.Location = new System.Drawing.Point(112, 103);
            this.lbl_DIROutputCompressPDF.Name = "lbl_DIROutputCompressPDF";
            this.lbl_DIROutputCompressPDF.Size = new System.Drawing.Size(290, 53);
            this.lbl_DIROutputCompressPDF.TabIndex = 15;
            // 
            // Btn_SelectDIROutputCompressPDF
            // 
            this.Btn_SelectDIROutputCompressPDF.Enabled = false;
            this.Btn_SelectDIROutputCompressPDF.Location = new System.Drawing.Point(5, 103);
            this.Btn_SelectDIROutputCompressPDF.Name = "Btn_SelectDIROutputCompressPDF";
            this.Btn_SelectDIROutputCompressPDF.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectDIROutputCompressPDF.TabIndex = 14;
            this.Btn_SelectDIROutputCompressPDF.UseVisualStyleBackColor = true;
            this.Btn_SelectDIROutputCompressPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputCompressPDF_Click);
            // 
            // lbl_ViewLvlCompres
            // 
            this.lbl_ViewLvlCompres.Location = new System.Drawing.Point(117, 193);
            this.lbl_ViewLvlCompres.Name = "lbl_ViewLvlCompres";
            this.lbl_ViewLvlCompres.Size = new System.Drawing.Size(288, 19);
            this.lbl_ViewLvlCompres.TabIndex = 13;
            this.lbl_ViewLvlCompres.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_LvlCompr
            // 
            this.lbl_LvlCompr.Location = new System.Drawing.Point(5, 173);
            this.lbl_LvlCompr.Name = "lbl_LvlCompr";
            this.lbl_LvlCompr.Size = new System.Drawing.Size(116, 21);
            this.lbl_LvlCompr.TabIndex = 12;
            // 
            // Tb_Compress
            // 
            this.Tb_Compress.Enabled = false;
            this.Tb_Compress.LargeChange = 1;
            this.Tb_Compress.Location = new System.Drawing.Point(120, 163);
            this.Tb_Compress.Maximum = 3;
            this.Tb_Compress.Name = "Tb_Compress";
            this.Tb_Compress.Size = new System.Drawing.Size(285, 45);
            this.Tb_Compress.TabIndex = 11;
            this.Tb_Compress.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.Tb_Compress.Value = 1;
            this.Tb_Compress.ValueChanged += new System.EventHandler(this.Tb_Compress_ValueChanged);
            // 
            // lbl_PDFToCompress
            // 
            this.lbl_PDFToCompress.Location = new System.Drawing.Point(114, 27);
            this.lbl_PDFToCompress.Name = "lbl_PDFToCompress";
            this.lbl_PDFToCompress.Size = new System.Drawing.Size(288, 65);
            this.lbl_PDFToCompress.TabIndex = 10;
            // 
            // Btn_SelectPDFToCompress
            // 
            this.Btn_SelectPDFToCompress.Location = new System.Drawing.Point(5, 27);
            this.Btn_SelectPDFToCompress.Name = "Btn_SelectPDFToCompress";
            this.Btn_SelectPDFToCompress.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectPDFToCompress.TabIndex = 9;
            this.Btn_SelectPDFToCompress.UseVisualStyleBackColor = true;
            this.Btn_SelectPDFToCompress.Click += new System.EventHandler(this.Btn_SelectPDFToCompress_Click);
            // 
            // Btn_ResetCompres
            // 
            this.Btn_ResetCompres.Enabled = false;
            this.Btn_ResetCompres.Location = new System.Drawing.Point(113, 224);
            this.Btn_ResetCompres.Name = "Btn_ResetCompres";
            this.Btn_ResetCompres.Size = new System.Drawing.Size(104, 22);
            this.Btn_ResetCompres.TabIndex = 8;
            this.Btn_ResetCompres.UseVisualStyleBackColor = true;
            this.Btn_ResetCompres.Click += new System.EventHandler(this.Btn_ResetCompres_Click);
            // 
            // Btn_Compress
            // 
            this.Btn_Compress.Enabled = false;
            this.Btn_Compress.Location = new System.Drawing.Point(5, 224);
            this.Btn_Compress.Name = "Btn_Compress";
            this.Btn_Compress.Size = new System.Drawing.Size(104, 22);
            this.Btn_Compress.TabIndex = 3;
            this.Btn_Compress.UseVisualStyleBackColor = true;
            this.Btn_Compress.Click += new System.EventHandler(this.Btn_Compress_Click);
            // 
            // lblCompr
            // 
            this.lblCompr.AutoSize = true;
            this.lblCompr.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompr.Location = new System.Drawing.Point(5, 3);
            this.lblCompr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompr.Name = "lblCompr";
            this.lblCompr.Size = new System.Drawing.Size(0, 18);
            this.lblCompr.TabIndex = 2;
            // 
            // pB_ICO
            // 
            this.pB_ICO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pB_ICO.InitialImage = null;
            this.pB_ICO.Location = new System.Drawing.Point(751, 475);
            this.pB_ICO.Name = "pB_ICO";
            this.pB_ICO.Size = new System.Drawing.Size(128, 116);
            this.pB_ICO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pB_ICO.TabIndex = 13;
            this.pB_ICO.TabStop = false;
            // 
            // Btn_Exit
            // 
            this.Btn_Exit.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Exit.Location = new System.Drawing.Point(770, 375);
            this.Btn_Exit.Name = "Btn_Exit";
            this.Btn_Exit.Size = new System.Drawing.Size(75, 48);
            this.Btn_Exit.TabIndex = 14;
            this.Btn_Exit.UseVisualStyleBackColor = true;
            this.Btn_Exit.Click += new System.EventHandler(this.Btn_Exit_Click);
            // 
            // lbl_CompressInProgress
            // 
            this.lbl_CompressInProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_CompressInProgress.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CompressInProgress.Location = new System.Drawing.Point(751, 314);
            this.lbl_CompressInProgress.Name = "lbl_CompressInProgress";
            this.lbl_CompressInProgress.Size = new System.Drawing.Size(50, 50);
            this.lbl_CompressInProgress.TabIndex = 15;
            this.lbl_CompressInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_CompressInProgress.Visible = false;
            // 
            // lbl_MergeInProgress
            // 
            this.lbl_MergeInProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_MergeInProgress.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MergeInProgress.Location = new System.Drawing.Point(807, 314);
            this.lbl_MergeInProgress.Name = "lbl_MergeInProgress";
            this.lbl_MergeInProgress.Size = new System.Drawing.Size(50, 50);
            this.lbl_MergeInProgress.TabIndex = 16;
            this.lbl_MergeInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_MergeInProgress.Visible = false;
            // 
            // PnlConvert
            // 
            this.PnlConvert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnlConvert.Controls.Add(this.lbl_DIROutputConvertPDF);
            this.PnlConvert.Controls.Add(this.Btn_SelectDIROutputConvertPDF);
            this.PnlConvert.Controls.Add(this.lbl_PDFToConvert);
            this.PnlConvert.Controls.Add(this.Btn_SelectPDFToConvert);
            this.PnlConvert.Controls.Add(this.Btn_ResetConvert);
            this.PnlConvert.Controls.Add(this.Btn_Convert);
            this.PnlConvert.Controls.Add(this.lbl_ConvDOCX);
            this.PnlConvert.Location = new System.Drawing.Point(14, 330);
            this.PnlConvert.Name = "PnlConvert";
            this.PnlConvert.Size = new System.Drawing.Size(300, 251);
            this.PnlConvert.TabIndex = 17;
            // 
            // lbl_DIROutputConvertPDF
            // 
            this.lbl_DIROutputConvertPDF.Location = new System.Drawing.Point(112, 130);
            this.lbl_DIROutputConvertPDF.Name = "lbl_DIROutputConvertPDF";
            this.lbl_DIROutputConvertPDF.Size = new System.Drawing.Size(170, 85);
            this.lbl_DIROutputConvertPDF.TabIndex = 15;
            // 
            // Btn_SelectDIROutputConvertPDF
            // 
            this.Btn_SelectDIROutputConvertPDF.Enabled = false;
            this.Btn_SelectDIROutputConvertPDF.Location = new System.Drawing.Point(5, 130);
            this.Btn_SelectDIROutputConvertPDF.Name = "Btn_SelectDIROutputConvertPDF";
            this.Btn_SelectDIROutputConvertPDF.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectDIROutputConvertPDF.TabIndex = 14;
            this.Btn_SelectDIROutputConvertPDF.UseVisualStyleBackColor = true;
            this.Btn_SelectDIROutputConvertPDF.Click += new System.EventHandler(this.Btn_SelectDIROutputConvertPDF_Click);
            // 
            // lbl_PDFToConvert
            // 
            this.lbl_PDFToConvert.Location = new System.Drawing.Point(114, 27);
            this.lbl_PDFToConvert.Name = "lbl_PDFToConvert";
            this.lbl_PDFToConvert.Size = new System.Drawing.Size(170, 85);
            this.lbl_PDFToConvert.TabIndex = 10;
            // 
            // Btn_SelectPDFToConvert
            // 
            this.Btn_SelectPDFToConvert.Location = new System.Drawing.Point(5, 27);
            this.Btn_SelectPDFToConvert.Name = "Btn_SelectPDFToConvert";
            this.Btn_SelectPDFToConvert.Size = new System.Drawing.Size(104, 22);
            this.Btn_SelectPDFToConvert.TabIndex = 9;
            this.Btn_SelectPDFToConvert.UseVisualStyleBackColor = true;
            this.Btn_SelectPDFToConvert.Click += new System.EventHandler(this.Btn_SelectPDFToConvert_Click);
            // 
            // Btn_ResetConvert
            // 
            this.Btn_ResetConvert.Enabled = false;
            this.Btn_ResetConvert.Location = new System.Drawing.Point(113, 224);
            this.Btn_ResetConvert.Name = "Btn_ResetConvert";
            this.Btn_ResetConvert.Size = new System.Drawing.Size(104, 22);
            this.Btn_ResetConvert.TabIndex = 8;
            this.Btn_ResetConvert.UseVisualStyleBackColor = true;
            this.Btn_ResetConvert.Click += new System.EventHandler(this.Btn_ResetConvert_Click);
            // 
            // Btn_Convert
            // 
            this.Btn_Convert.Enabled = false;
            this.Btn_Convert.Location = new System.Drawing.Point(5, 224);
            this.Btn_Convert.Name = "Btn_Convert";
            this.Btn_Convert.Size = new System.Drawing.Size(104, 22);
            this.Btn_Convert.TabIndex = 3;
            this.Btn_Convert.UseVisualStyleBackColor = true;
            this.Btn_Convert.Click += new System.EventHandler(this.Btn_Convert_Click);
            // 
            // lbl_ConvDOCX
            // 
            this.lbl_ConvDOCX.AutoSize = true;
            this.lbl_ConvDOCX.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ConvDOCX.Location = new System.Drawing.Point(5, 3);
            this.lbl_ConvDOCX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_ConvDOCX.Name = "lbl_ConvDOCX";
            this.lbl_ConvDOCX.Size = new System.Drawing.Size(0, 18);
            this.lbl_ConvDOCX.TabIndex = 2;
            // 
            // lbl_ConvertInProgress
            // 
            this.lbl_ConvertInProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_ConvertInProgress.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ConvertInProgress.Location = new System.Drawing.Point(751, 423);
            this.lbl_ConvertInProgress.Name = "lbl_ConvertInProgress";
            this.lbl_ConvertInProgress.Size = new System.Drawing.Size(50, 50);
            this.lbl_ConvertInProgress.TabIndex = 18;
            this.lbl_ConvertInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_ConvertInProgress.Visible = false;
            // 
            // FrmUtiPDF_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 593);
            this.Controls.Add(this.lbl_ConvertInProgress);
            this.Controls.Add(this.PnlConvert);
            this.Controls.Add(this.lbl_CompressInProgress);
            this.Controls.Add(this.lbl_MergeInProgress);
            this.Controls.Add(this.Btn_Exit);
            this.Controls.Add(this.pB_ICO);
            this.Controls.Add(this.PnlCompress);
            this.Controls.Add(this.PnlMerge);
            this.Controls.Add(this.PnlOCR);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "FrmUtiPDF_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDF Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUtiPDF_Main_FormClosing);
            this.Load += new System.EventHandler(this.FrmUtiPDF_Main_Load);
            this.Shown += new System.EventHandler(this.FrmUtiPDF_Main_Shown);
            this.PnlOCR.ResumeLayout(false);
            this.PnlOCR.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBProgressExtract)).EndInit();
            this.PnlMerge.ResumeLayout(false);
            this.PnlMerge.PerformLayout();
            this.PnlCompress.ResumeLayout(false);
            this.PnlCompress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tb_Compress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pB_ICO)).EndInit();
            this.PnlConvert.ResumeLayout(false);
            this.PnlConvert.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlOCR;
        private System.Windows.Forms.Button Btn_SelectPDF;
        private System.Windows.Forms.Label lblOCR;
        private System.Windows.Forms.OpenFileDialog oFD_PDF;
        private System.Windows.Forms.Label lbl_PDF;
        private System.Windows.Forms.Label lbl_TXT;
        private System.Windows.Forms.Button Btn_SelectDIROutputTXT;
        private System.Windows.Forms.FolderBrowserDialog fBD_TXT;
        private System.Windows.Forms.Button Btn_Start;
        private System.Windows.Forms.Button Btn_Reset;
        private System.Windows.Forms.ComboBox cmbLangConv;
        private System.Windows.Forms.Label lblLang;
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
        private System.Windows.Forms.Button Btn_SelectPDFToCompress;
        private System.Windows.Forms.Button Btn_ResetCompres;
        private System.Windows.Forms.TrackBar Tb_Compress;
        private System.Windows.Forms.Label lbl_LvlCompr;
        private System.Windows.Forms.Label lbl_ViewLvlCompres;
        private System.Windows.Forms.Label lbl_DIROutputCompressPDF;
        private System.Windows.Forms.Button Btn_SelectDIROutputCompressPDF;
        private System.Windows.Forms.PictureBox pB_ICO;
        private System.Windows.Forms.PictureBox pBProgressExtract;
        private System.Windows.Forms.Button Btn_Exit;
        private System.Windows.Forms.Button Btn_Abort;
        private System.Windows.Forms.Label lbl_CompressInProgress;
        private System.Windows.Forms.Label lbl_MergeInProgress;
        private System.Windows.Forms.Panel PnlConvert;
        private System.Windows.Forms.Label lbl_DIROutputConvertPDF;
        private System.Windows.Forms.Button Btn_SelectDIROutputConvertPDF;
        private System.Windows.Forms.Label lbl_PDFToConvert;
        private System.Windows.Forms.Button Btn_SelectPDFToConvert;
        private System.Windows.Forms.Button Btn_ResetConvert;
        private System.Windows.Forms.Button Btn_Convert;
        private System.Windows.Forms.Label lbl_ConvDOCX;
        private System.Windows.Forms.Label lbl_ConvertInProgress;
    }
}