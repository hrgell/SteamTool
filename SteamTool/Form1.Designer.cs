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
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtMessages = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtOutputFolder = new System.Windows.Forms.TextBox();
            this.BtnOutputFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnCopy
            // 
            this.BtnCopy.Location = new System.Drawing.Point(174, 110);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(75, 23);
            this.BtnCopy.TabIndex = 4;
            this.BtnCopy.Text = "Copy";
            this.BtnCopy.UseVisualStyleBackColor = true;
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // BtnCfg
            // 
            this.BtnCfg.Location = new System.Drawing.Point(93, 110);
            this.BtnCfg.Name = "BtnCfg";
            this.BtnCfg.Size = new System.Drawing.Size(75, 23);
            this.BtnCfg.TabIndex = 3;
            this.BtnCfg.Text = "Cfg";
            this.BtnCfg.UseVisualStyleBackColor = true;
            this.BtnCfg.Click += new System.EventHandler(this.BtnCfg_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(642, 110);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnTest
            // 
            this.BtnTest.Location = new System.Drawing.Point(12, 110);
            this.BtnTest.Name = "BtnTest";
            this.BtnTest.Size = new System.Drawing.Size(75, 23);
            this.BtnTest.TabIndex = 2;
            this.BtnTest.Text = "Test";
            this.BtnTest.UseVisualStyleBackColor = true;
            this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Actions";
            // 
            // TxtMessages
            // 
            this.TxtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtMessages.Location = new System.Drawing.Point(12, 139);
            this.TxtMessages.Multiline = true;
            this.TxtMessages.Name = "TxtMessages";
            this.TxtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtMessages.Size = new System.Drawing.Size(705, 327);
            this.TxtMessages.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Output Folder";
            // 
            // TxtOutputFolder
            // 
            this.TxtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutputFolder.Location = new System.Drawing.Point(15, 36);
            this.TxtOutputFolder.Name = "TxtOutputFolder";
            this.TxtOutputFolder.Size = new System.Drawing.Size(621, 20);
            this.TxtOutputFolder.TabIndex = 0;
            // 
            // BtnOutputFolder
            // 
            this.BtnOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOutputFolder.Location = new System.Drawing.Point(642, 34);
            this.BtnOutputFolder.Name = "BtnOutputFolder";
            this.BtnOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.BtnOutputFolder.TabIndex = 1;
            this.BtnOutputFolder.Text = "Browse";
            this.BtnOutputFolder.UseVisualStyleBackColor = true;
            this.BtnOutputFolder.Click += new System.EventHandler(this.BtnOutputFolder_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(729, 478);
            this.Controls.Add(this.BtnOutputFolder);
            this.Controls.Add(this.TxtOutputFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMessages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnTest);
            this.Controls.Add(this.BtnCancel);
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
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtMessages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtOutputFolder;
        private System.Windows.Forms.Button BtnOutputFolder;
    }
}

