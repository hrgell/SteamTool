using Microsoft.Win32;
using Microsoft.Win32;
using System;
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
            TxtOutputFolder.Text = Std.GetRegistryString(RegistryHive.CurrentUser, @"SOFTWARE\SteamTool", "OutputFolder");
        } // method

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            string output_folder = TxtOutputFolder.Text;
            if(output_folder.Length == 0)
            {
                MessageBox.Show("You must select the folder to copy files to.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string[] args = { "-c", output_folder };
            StringBuilder buf = Program.DoMyTask(args);
            TxtMessages.Text = buf.ToString();
        } // method

        private void BtnCfg_Click(object sender, EventArgs e)
        {
            string[] args = { "-r" };
            StringBuilder buf = Program.DoMyTask(args);
            TxtMessages.Text = buf.ToString();
        } // method

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        } // method

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string[] args = { };
            StringBuilder buf = Program.DoMyTask(args);
            TxtMessages.Text = buf.ToString();
        } // method

        private void BtnOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select the output folder.";
            //dlg.RootFolder = Environment.SpecialFolder.Personal;
            DialogResult rst = dlg.ShowDialog();
            if (rst == DialogResult.OK)
            {
                TxtOutputFolder.Text = dlg.SelectedPath;
                Std.SetRegistryString(RegistryHive.CurrentUser, @"Software\SteamTool", "OutputFolder", TxtOutputFolder.Text);
            }
        } // method

        public void SetMessages(string msgs)
        {
            TxtMessages.Text = msgs;
        } // method
    } // class
} // namespace
