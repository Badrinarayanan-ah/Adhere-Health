namespace USPSAddressVerifierDesktop
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnProcessFile = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlErrors = new System.Windows.Forms.Panel();
            this.lblReturnFile = new System.Windows.Forms.Label();
            this.btnCopyResultsToClipboard = new System.Windows.Forms.Button();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnViewSample = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnViewReturnFile = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.txtPMDClientID = new System.Windows.Forms.TextBox();
            this.pnlErrors.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select a File: ";
            this.toolTip1.SetToolTip(this.label1, "Double-click here to select a file");
            this.label1.Click += new System.EventHandler(this.label1_Click);
            this.label1.DoubleClick += new System.EventHandler(this.label1_DoubleClick);
            // 
            // txtFile
            // 
            this.errorProvider1.SetError(this.txtFile, "You must select a file");
            this.txtFile.Location = new System.Drawing.Point(203, 122);
            this.txtFile.Multiline = true;
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(372, 43);
            this.txtFile.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtFile, "Double-click here to select a file");
            this.txtFile.DoubleClick += new System.EventHandler(this.txtFile_DoubleClick);
            // 
            // btnProcessFile
            // 
            this.btnProcessFile.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnProcessFile.Location = new System.Drawing.Point(8, 216);
            this.btnProcessFile.Name = "btnProcessFile";
            this.btnProcessFile.Size = new System.Drawing.Size(143, 46);
            this.btnProcessFile.TabIndex = 2;
            this.btnProcessFile.Text = "Process File";
            this.btnProcessFile.UseVisualStyleBackColor = false;
            this.btnProcessFile.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // pnlErrors
            // 
            this.pnlErrors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlErrors.Controls.Add(this.lblReturnFile);
            this.pnlErrors.Controls.Add(this.btnCopyResultsToClipboard);
            this.pnlErrors.Controls.Add(this.txtErrors);
            this.pnlErrors.Controls.Add(this.label2);
            this.pnlErrors.Location = new System.Drawing.Point(15, 303);
            this.pnlErrors.Name = "pnlErrors";
            this.pnlErrors.Size = new System.Drawing.Size(970, 212);
            this.pnlErrors.TabIndex = 3;
            this.pnlErrors.Visible = false;
            // 
            // lblReturnFile
            // 
            this.lblReturnFile.AutoSize = true;
            this.lblReturnFile.Location = new System.Drawing.Point(829, 138);
            this.lblReturnFile.Name = "lblReturnFile";
            this.lblReturnFile.Size = new System.Drawing.Size(0, 17);
            this.lblReturnFile.TabIndex = 4;
            this.lblReturnFile.Visible = false;
            // 
            // btnCopyResultsToClipboard
            // 
            this.btnCopyResultsToClipboard.CausesValidation = false;
            this.btnCopyResultsToClipboard.Location = new System.Drawing.Point(850, 5);
            this.btnCopyResultsToClipboard.Name = "btnCopyResultsToClipboard";
            this.btnCopyResultsToClipboard.Size = new System.Drawing.Size(107, 71);
            this.btnCopyResultsToClipboard.TabIndex = 3;
            this.btnCopyResultsToClipboard.Text = "Copy Results to Clipboard";
            this.btnCopyResultsToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyResultsToClipboard.Click += new System.EventHandler(this.btnCopyResultsToClipboard_Click);
            // 
            // txtErrors
            // 
            this.txtErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErrors.Location = new System.Drawing.Point(9, 32);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrors.Size = new System.Drawing.Size(835, 163);
            this.txtErrors.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "ERRORS and OUTPUT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(406, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "NOTE: Files can be either pipe or tab delimited";
            // 
            // btnViewSample
            // 
            this.btnViewSample.CausesValidation = false;
            this.btnViewSample.Location = new System.Drawing.Point(157, 216);
            this.btnViewSample.Name = "btnViewSample";
            this.btnViewSample.Size = new System.Drawing.Size(152, 46);
            this.btnViewSample.TabIndex = 5;
            this.btnViewSample.Text = "View Sample File(s)";
            this.btnViewSample.UseVisualStyleBackColor = true;
            this.btnViewSample.Click += new System.EventHandler(this.btnViewSample_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.pnlMain.Controls.Add(this.txtPMDClientID);
            this.pnlMain.Controls.Add(this.label9);
            this.pnlMain.Controls.Add(this.btnExit);
            this.pnlMain.Controls.Add(this.label8);
            this.pnlMain.Controls.Add(this.label7);
            this.pnlMain.Controls.Add(this.btnViewReturnFile);
            this.pnlMain.Controls.Add(this.label6);
            this.pnlMain.Controls.Add(this.label5);
            this.pnlMain.Controls.Add(this.comboBox1);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.btnViewSample);
            this.pnlMain.Controls.Add(this.txtFile);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.btnProcessFile);
            this.pnlMain.Controls.Add(this.pnlErrors);
            this.pnlMain.Location = new System.Drawing.Point(24, 12);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1003, 538);
            this.pnlMain.TabIndex = 6;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Location = new System.Drawing.Point(460, 213);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(92, 49);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Info;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(10, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(480, 25);
            this.label8.TabIndex = 13;
            this.label8.Text = "Double-click the \"Select a File\" textbox to choose a file";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.LawnGreen;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(592, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(382, 74);
            this.label7.TabIndex = 12;
            this.label7.Text = "NOTE: Return files will be delivered in a \'||\' delimited format. Here you can see" +
    " the differences between the input data and the USPS data.";
            this.label7.Visible = false;
            // 
            // btnViewReturnFile
            // 
            this.btnViewReturnFile.CausesValidation = false;
            this.btnViewReturnFile.Location = new System.Drawing.Point(315, 213);
            this.btnViewReturnFile.Name = "btnViewReturnFile";
            this.btnViewReturnFile.Size = new System.Drawing.Size(139, 49);
            this.btnViewReturnFile.TabIndex = 11;
            this.btnViewReturnFile.Text = "View Output/Return File";
            this.btnViewReturnFile.UseVisualStyleBackColor = true;
            this.btnViewReturnFile.Visible = false;
            this.btnViewReturnFile.Click += new System.EventHandler(this.btnViewReturnFile_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.LawnGreen;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(980, 230);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 55);
            this.label6.TabIndex = 10;
            this.label6.Text = "NOTE: Response/Return Files will be stored with a date and time stamp in a folder" +
    " called \"c:\\usps\"";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(581, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(386, 189);
            this.label5.TabIndex = 9;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider1.SetError(this.comboBox1, "File Mode is required");
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Standard Address File",
            "Account Based Address File",
            "PharmMD ETLCore Patient"});
            this.comboBox1.Location = new System.Drawing.Point(203, 171);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 24);
            this.comboBox1.TabIndex = 8;
            this.toolTip1.SetToolTip(this.comboBox1, "There are 3 modes - one (1) takes a standard address and another(2) takes an addr" +
        "ess attached to account/client/provider, etc. The last and special one runs for " +
        "PharmMD from a preset database setup");
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Select a File Mode: ";
            this.toolTip1.SetToolTip(this.label4, "There are 2 modes - one takes a standard address and the other takes an address a" +
        "ttached to account/client/provider, etc");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Text Files|*.txt|CSV Files|*.csv";
            this.openFileDialog1.InitialDirectory = "C:\\usps";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1054, 57);
            this.progressBar1.Minimum = 1;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(42, 396);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Value = 1;
            this.progressBar1.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(146, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "Enter PMDClientID:";
            // 
            // txtPMDClientID
            // 
            this.errorProvider1.SetError(this.txtPMDClientID, "You must enter a numeric PMDClientID");
            this.txtPMDClientID.Location = new System.Drawing.Point(203, 73);
            this.txtPMDClientID.Multiline = true;
            this.txtPMDClientID.Name = "txtPMDClientID";
            this.txtPMDClientID.Size = new System.Drawing.Size(77, 28);
            this.txtPMDClientID.TabIndex = 16;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1108, 562);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pnlMain);
            this.Name = "frmMain";
            this.Text = "USPS Address Verifier";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlErrors.ResumeLayout(false);
            this.pnlErrors.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnProcessFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel pnlErrors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnViewSample;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCopyResultsToClipboard;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnViewReturnFile;
        private System.Windows.Forms.Label lblReturnFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPMDClientID;
    }
}

