namespace SteamTool
{
    partial class Form1
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
            this.BtnCopy = new System.Windows.Forms.Button();
            this.BtnCfg = new System.Windows.Forms.Button();
            this.BtnTest = new System.Windows.Forms.Button();
            this.TxtMessages = new System.Windows.Forms.TextBox();
            this.TxtOutputFolder = new System.Windows.Forms.TextBox();
            this.BtnOutputFolder = new System.Windows.Forms.Button();
            this.BtnStore = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnExit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnCopy
            // 
            this.BtnCopy.BackColor = System.Drawing.Color.LightCoral;
            this.BtnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCopy.Location = new System.Drawing.Point(255, 139);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(75, 23);
            this.BtnCopy.TabIndex = 5;
            this.BtnCopy.Text = "Archive";
            this.BtnCopy.UseVisualStyleBackColor = false;
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // BtnCfg
            // 
            this.BtnCfg.BackColor = System.Drawing.Color.PaleGreen;
            this.BtnCfg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCfg.Location = new System.Drawing.Point(93, 139);
            this.BtnCfg.Name = "BtnCfg";
            this.BtnCfg.Size = new System.Drawing.Size(75, 23);
            this.BtnCfg.TabIndex = 3;
            this.BtnCfg.Text = "Cfg";
            this.BtnCfg.UseVisualStyleBackColor = false;
            this.BtnCfg.Click += new System.EventHandler(this.BtnCfg_Click);
            // 
            // BtnTest
            // 
            this.BtnTest.BackColor = System.Drawing.Color.PaleGreen;
            this.BtnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTest.Location = new System.Drawing.Point(12, 139);
            this.BtnTest.Name = "BtnTest";
            this.BtnTest.Size = new System.Drawing.Size(75, 23);
            this.BtnTest.TabIndex = 2;
            this.BtnTest.Text = "Test";
            this.BtnTest.UseVisualStyleBackColor = false;
            this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // TxtMessages
            // 
            this.TxtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMessages.Location = new System.Drawing.Point(12, 168);
            this.TxtMessages.Multiline = true;
            this.TxtMessages.Name = "TxtMessages";
            this.TxtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtMessages.Size = new System.Drawing.Size(705, 507);
            this.TxtMessages.TabIndex = 7;
            // 
            // TxtOutputFolder
            // 
            this.TxtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutputFolder.Location = new System.Drawing.Point(12, 38);
            this.TxtOutputFolder.Name = "TxtOutputFolder";
            this.TxtOutputFolder.Size = new System.Drawing.Size(598, 20);
            this.TxtOutputFolder.TabIndex = 0;
            // 
            // BtnOutputFolder
            // 
            this.BtnOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOutputFolder.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnOutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOutputFolder.Location = new System.Drawing.Point(616, 35);
            this.BtnOutputFolder.Name = "BtnOutputFolder";
            this.BtnOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.BtnOutputFolder.TabIndex = 1;
            this.BtnOutputFolder.Text = "Browse";
            this.BtnOutputFolder.UseVisualStyleBackColor = false;
            this.BtnOutputFolder.Click += new System.EventHandler(this.BtnOutputFolder_Click);
            // 
            // BtnStore
            // 
            this.BtnStore.BackColor = System.Drawing.Color.PaleGreen;
            this.BtnStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStore.Location = new System.Drawing.Point(174, 139);
            this.BtnStore.Name = "BtnStore";
            this.BtnStore.Size = new System.Drawing.Size(75, 23);
            this.BtnStore.TabIndex = 4;
            this.BtnStore.Text = "Store";
            this.BtnStore.UseVisualStyleBackColor = false;
            this.BtnStore.Click += new System.EventHandler(this.BtnStore_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.BtnOutputFolder);
            this.groupBox1.Controls.Add(this.TxtOutputFolder);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(705, 77);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Backup Folder";
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Location = new System.Drawing.Point(628, 139);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.BtnExit;
            this.ClientSize = new System.Drawing.Size(729, 687);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnStore);
            this.Controls.Add(this.TxtMessages);
            this.Controls.Add(this.BtnTest);
            this.Controls.Add(this.BtnCfg);
            this.Controls.Add(this.BtnCopy);
            this.MinimumSize = new System.Drawing.Size(640, 450);
            this.Name = "Form1";
            this.Text = "Steam Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCopy;
        private System.Windows.Forms.Button BtnCfg;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.TextBox TxtMessages;
        private System.Windows.Forms.TextBox TxtOutputFolder;
        private System.Windows.Forms.Button BtnOutputFolder;
        private System.Windows.Forms.Button BtnStore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnExit;
    }
}

