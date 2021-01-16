using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace SteamTool
{
    // C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo
    class CSGO
    {
        public long files_copied = 0;
        public bool move_files = true;
        public List<string> files = new List<string>();
        public long maxnum = 0;
        public string csgo_folder = string.Empty;
        string screenshots_folder = string.Empty;
        public string output_folder = string.Empty;

        public CSGO(string game_folder, string output_folder)
        {
            this.output_folder = output_folder;
            csgo_folder = Path.Combine(game_folder, "csgo");
            if (!Directory.Exists(csgo_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", csgo_folder));
            screenshots_folder = Path.Combine(csgo_folder, "screenshots");

            // Get .dem and .txt files
            string[] filelist = Directory.GetFiles(csgo_folder);
            foreach (string filename in filelist)
            {
                string ext = Path.GetExtension(filename).ToLower();
                if (ext == ".dem")
                {
                    files.Add(filename);
                    string basename = Path.GetFileNameWithoutExtension(filename);
                    // Assume a single letter is used as prefix
                    if (basename.Length > 1)
                    {
                        string rest = basename.Substring(1);
                        if (Std.IsDigitsOnly(rest))
                        {
                            long newnum = long.Parse(Std.TrimLeadingDigits(rest));
                            if (maxnum < newnum)
                                maxnum = newnum;
                        }
                    }
                }
                else if (ext == ".txt" && Path.GetFileName(filename).StartsWith("condump"))
                    files.Add(filename);
            }
            // Get screenshots
            if (Directory.Exists(screenshots_folder))
            {
                filelist = Directory.GetFiles(screenshots_folder);
                foreach (string filename in filelist)
                    files.Add(filename);
            }
        } // method

        public StringBuilder CreateRecordScript()
        {
            long newmax = maxnum + 20;
            StringBuilder buf = new StringBuilder();
            buf.WriteLine("echo \"// Executing Record\"");
            buf.WriteLine("alias rc r1");
            long step = 1;
            // Use a single letter as prefix
            for (long idx = maxnum + 1; idx < newmax; ++idx, ++step)
                buf.WriteLine("alias r{0} \"record c{1};alias rc r{2}\"", step, idx.ToString("D5"), step + 1);
            buf.WriteLine("alias r{0} \"record c{1};alias rc nop\"", step, newmax.ToString("D5"));
            buf.WriteLine("bind F11 rc");
            string cfgname = Path.Combine(csgo_folder, "cfg", "myrecord.cfg");
            File.WriteAllText(cfgname, buf.ToString(), new UTF8Encoding(false));
            return buf;
        } // method

        public void CopyFiles()
        {
            if (files.Count() == 0)
                return;
            if (output_folder.Length == 0)
                return;

            // Create the specified folder
            if (!Directory.Exists(output_folder))
                Directory.CreateDirectory(output_folder);
            if (!Directory.Exists(output_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", output_folder));

            // Create a sub folder 
            string output_subfolder = Path.Combine(output_folder, DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (!Directory.Exists(output_subfolder))
                Directory.CreateDirectory(output_subfolder);
            if (!Directory.Exists(output_subfolder))
                throw new Exception(string.Format("Folder does not exist: {0}", output_subfolder));

            // Copy the files
            foreach (string filename in files)
            {
                string newname = Path.Combine(output_subfolder, Path.GetFileName(filename));
                Std.CopyFile(filename, newname, move_files);
                ++files_copied;
            }
        } // method
    } // class
} // namespace
