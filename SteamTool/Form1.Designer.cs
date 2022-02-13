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
            this.TxtBackupFolder = new System.Windows.Forms.TextBox();
            this.BtnOutputFolder = new System.Windows.Forms.Button();
            this.BtnStore = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.LblBackupFolder = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnStoreArchive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnCopy
            // 
            this.BtnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCopy.BackColor = System.Drawing.Color.LightCoral;
            this.BtnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCopy.Location = new System.Drawing.Point(645, 188);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(75, 23);
            this.BtnCopy.TabIndex = 5;
            this.BtnCopy.Text = "Archive";
            this.BtnCopy.UseVisualStyleBackColor = false;
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // BtnCfg
            // 
            this.BtnCfg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCfg.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnCfg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCfg.Location = new System.Drawing.Point(645, 130);
            this.BtnCfg.Name = "BtnCfg";
            this.BtnCfg.Size = new System.Drawing.Size(75, 23);
            this.BtnCfg.TabIndex = 3;
            this.BtnCfg.Text = "Cfg";
            this.BtnCfg.UseVisualStyleBackColor = false;
            this.BtnCfg.Click += new System.EventHandler(this.BtnCfg_Click);
            // 
            // BtnTest
            // 
            this.BtnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnTest.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnTest.Location = new System.Drawing.Point(645, 101);
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
            this.TxtMessages.Location = new System.Drawing.Point(15, 101);
            this.TxtMessages.Multiline = true;
            this.TxtMessages.Name = "TxtMessages";
            this.TxtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtMessages.Size = new System.Drawing.Size(611, 555);
            this.TxtMessages.TabIndex = 7;
            // 
            // TxtBackupFolder
            // 
            this.TxtBackupFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtBackupFolder.Location = new System.Drawing.Point(15, 37);
            this.TxtBackupFolder.Name = "TxtBackupFolder";
            this.TxtBackupFolder.Size = new System.Drawing.Size(611, 20);
            this.TxtBackupFolder.TabIndex = 0;
            // 
            // BtnOutputFolder
            // 
            this.BtnOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOutputFolder.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnOutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOutputFolder.Location = new System.Drawing.Point(645, 35);
            this.BtnOutputFolder.Name = "BtnOutputFolder";
            this.BtnOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.BtnOutputFolder.TabIndex = 1;
            this.BtnOutputFolder.Text = "Browse";
            this.BtnOutputFolder.UseVisualStyleBackColor = false;
            this.BtnOutputFolder.Click += new System.EventHandler(this.BtnOutputFolder_Click);
            // 
            // BtnStore
            // 
            this.BtnStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStore.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStore.Location = new System.Drawing.Point(645, 159);
            this.BtnStore.Name = "BtnStore";
            this.BtnStore.Size = new System.Drawing.Size(75, 23);
            this.BtnStore.TabIndex = 4;
            this.BtnStore.Text = "Store";
            this.BtnStore.UseVisualStyleBackColor = false;
            this.BtnStore.Click += new System.EventHandler(this.BtnStore_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Location = new System.Drawing.Point(645, 633);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 23);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // LblBackupFolder
            // 
            this.LblBackupFolder.AutoSize = true;
            this.LblBackupFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblBackupFolder.Location = new System.Drawing.Point(12, 21);
            this.LblBackupFolder.Name = "LblBackupFolder";
            this.LblBackupFolder.Size = new System.Drawing.Size(89, 13);
            this.LblBackupFolder.TabIndex = 8;
            this.LblBackupFolder.Text = "Backup Folder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Log";
            // 
            // BtnStoreArchive
            // 
            this.BtnStoreArchive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStoreArchive.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.BtnStoreArchive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStoreArchive.Location = new System.Drawing.Point(645, 217);
            this.BtnStoreArchive.Name = "BtnStoreArchive";
            this.BtnStoreArchive.Size = new System.Drawing.Size(75, 23);
            this.BtnStoreArchive.TabIndex = 10;
            this.BtnStoreArchive.Text = "Store Arc";
            this.BtnStoreArchive.UseVisualStyleBackColor = false;
            this.BtnStoreArchive.Click += new System.EventHandler(this.BtnStoreArchive_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.BtnExit;
            this.ClientSize = new System.Drawing.Size(736, 678);
            this.Controls.Add(this.BtnStoreArchive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LblBackupFolder);
            this.Controls.Add(this.BtnOutputFolder);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.TxtBackupFolder);
            this.Controls.Add(this.BtnStore);
            this.Controls.Add(this.TxtMessages);
            this.Controls.Add(this.BtnTest);
            this.Controls.Add(this.BtnCfg);
            this.Controls.Add(this.BtnCopy);
            this.MinimumSize = new System.Drawing.Size(640, 450);
            this.Name = "Form1";
            this.Text = "Steam Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCopy;
        private System.Windows.Forms.Button BtnCfg;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.TextBox TxtMessages;
        private System.Windows.Forms.TextBox TxtBackupFolder;
        private System.Windows.Forms.Button BtnOutputFolder;
        private System.Windows.Forms.Button BtnStore;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label LblBackupFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnStoreArchive;
    }
}

