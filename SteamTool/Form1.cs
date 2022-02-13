using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SteamTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        } // constructor

        private void Form1_Load(object sender, EventArgs e)
        {
            TxtBackupFolder.Text = Std.GetRegistryString(RegistryHive.CurrentUser, @"SOFTWARE\SteamTool", "OutputFolder");
        } // method

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            string output_folder = TxtBackupFolder.Text;
            if (output_folder.Length == 0)
            {
                MessageBox.Show("You must select a backup folder.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] args = { "-m", output_folder };
            StringBuilder buf = Program.Doit(args);
            TxtMessages.Text = buf.ToString();
            ScrollToBottom();
        } // method

        internal void ScrollToBottom()
        {
            TxtMessages.SelectionStart = TxtMessages.Text.Length;
            TxtMessages.SelectionLength = 0;
            TxtMessages.ScrollToCaret();
        }

        private void BtnCfg_Click(object sender, EventArgs e)
        {
            string[] args = { "-r" };
            StringBuilder buf = Program.Doit(args);
            TxtMessages.Text = buf.ToString();
            ScrollToBottom();
        } // method

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        } // method

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string[] args = { };
            StringBuilder buf = Program.Doit(args);
            TxtMessages.Text = buf.ToString();
            ScrollToBottom();
        } // method

        private void BtnOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog
            {
                Description = "Select the output folder."
                //RootFolder = Environment.SpecialFolder.Personal
            };
            DialogResult rst = dlg.ShowDialog();
            if (rst == DialogResult.OK)
            {
                TxtBackupFolder.Text = dlg.SelectedPath;
                Std.SetRegistryString(RegistryHive.CurrentUser, @"Software\SteamTool", "OutputFolder", TxtBackupFolder.Text);
            }
        } // method

        public void SetMessages(string msgs)
        {
            TxtMessages.Text = msgs;
        } // method

        private void BtnStore_Click(object sender, EventArgs e)
        {
            string output_folder = TxtBackupFolder.Text;
            if (output_folder.Length == 0)
            {
                MessageBox.Show("You must select an output folder.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] args = { "-s", output_folder };
            StringBuilder buf = Program.Doit(args);
            TxtMessages.Text = buf.ToString();
            ScrollToBottom();
        } // method

        private void BtnStoreArchive_Click(object sender, EventArgs ev)
        {
            var txtLog = new StringBuilder();
            string root = TxtBackupFolder.Text;
            bool exists = Directory.Exists(root);
            string[] folders = { };
            if (!exists)
            {
                TxtMessages.Text = "Error: The backup folder does not exist.";
                return;
            }
            try
            {
                folders = Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                txtLog.WriteLine("Error: Failed to read the backup folder: {0}", ex.ToString());
                return;
            }

            txtLog.WriteLine("The number of files is {0}.", folders.Length);
            using (var frm = new FormSelectFolders())
            {
                frm.Height = this.Height;
                frm.Width = this.Width;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.AddFilenames(folders);
                //MessageBox.Show("Press Ok.", "DEBUG", MessageBoxButtons.OK);
                frm.ShowDialog(this);
                txtLog.WriteLine("Selected Files: {0}", frm.SelectedFileNames.Count);
                if (frm.Status)
                {
                    // TODO Insert the selected file names into the database
                    string[] args = { "-b", root };
                    Arguments options = new Arguments(args);
                    options.filenames = frm.SelectedFileNames;
                    StringBuilder rst = Program.DoCSGO(options);
                    txtLog.Append(rst);
                }
            }
            TxtMessages.Text = txtLog.ToString();
        } // method
    } // class
} // namespace
