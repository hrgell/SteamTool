using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// See
// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.listviewitem?view=netframework-4.7.1#examples
// https://stackoverflow.com/questions/37791149/c-sharp-show-file-and-folder-icons-in-listview

namespace SteamTool
{
    public partial class FormSelectFolders : Form
    {
        public bool Status = false;
        public List<string> SelectedFileNames = new List<string>();

        public FormSelectFolders()
        {
            InitializeComponent();
            var lv = LsvFolders;
            lv.View = View.Details;
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.Sorting = SortOrder.Ascending;

            // Create columns for the items and subitems. Width of -2 indicates auto-size.
            lv.Columns.Add("Folder", -2, HorizontalAlignment.Left);
            lv.Columns.Add("Count", -2, HorizontalAlignment.Right);
            lv.Columns.Add("Path", -2, HorizontalAlignment.Left);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Status = false;
            SelectedFileNames.Clear();
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Status = true;
            RememberSelectedFilenames();
            Close();
        }

        public void AddFilenames(string[] filenames)
        {
            var lv = LsvFolders;
            lv.BeginUpdate();
            foreach (string filename in filenames)
                AddFilename(filename);
            lv.EndUpdate();
        }

        // Add filename to listbox
        public void AddFilename(string filename)
        {
            var lv = LsvFolders;
            string count = "?";
            try { count = Directory.EnumerateFiles(filename).Count().ToString(); } catch { }
            string basename = Path.GetFileName(filename);
            //ListViewItem item = new ListViewItem(new string[] { basename, count, filename });
            ListViewItem item = new ListViewItem(basename, 0);
            item.SubItems.Add(count);
            item.SubItems.Add(filename);
            lv.Items.Add(item);
        }

        // Remember the selected files
        public void RememberSelectedFilenames()
        {
            SelectedFileNames.Clear();
            foreach (ListViewItem item in LsvFolders.SelectedItems)
            {
                var subitem = item.SubItems[2];
                SelectedFileNames.Add(subitem.Text);
            }
            //Debug.WriteLine("-- Remember {0}", SelectedFileNames.Count);
        }
    } // class
} // namespace
