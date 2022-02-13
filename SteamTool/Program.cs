using System;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace SteamTool
{
    static class Program
    {
        static Form1 frm = null;

        [STAThread]
        static void Main(string[] args)
        {
            MyRegistry.Install();
            StringBuilder buf = null;
            if (args.Length > 0)
                buf = Doit(args);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frm = new Form1();
            if (buf != null)
                frm.SetMessages(buf.ToString());
            Application.Run(frm);
        } // Main()

        public static StringBuilder Doit(string[] args)
        {
            Arguments options = new Arguments(args);
            var rst = DoCSGO(options);
            return rst;
        } // method

        public static StringBuilder DoCSGO(Arguments options)
        {
            StringBuilder buf = new StringBuilder();
            Steam steam = new Steam();
            string appid = "730";
            VifEntry manifest = steam.GetManifest(appid);
            if (manifest == null)
                throw new Exception(string.Format("Failed to read the manifest for csgo (appid={0}) from \"{1}\"", appid, steam.apps_folder));
            string game_folder = Path.Combine(steam.apps_folder, "common", manifest.Get("AppState", "installdir"));
            string game_name = manifest.Get("AppState", "name");
            if (!Directory.Exists(game_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", game_folder));
            buf.WriteLine("Appid {0}: {1}", appid, game_name);
            buf.WriteLine(game_folder);

            //manifest.ToStringBuilder(buf);
            CSGO csgo = new CSGO(game_folder, options.folder);
            buf.WriteLine(string.Empty);
            buf.WriteLine("{0}{1}", "Files found: ", csgo.files.Count);
            string space = string.Empty;
            if (csgo.files.Count > 0)
            {
                if (csgo.files_condumps.Count > 0)
                {
                    buf.Write("Condumps({0})", csgo.files_condumps.Count);
                    space = " ";
                }
                if (csgo.files_demos.Count > 0)
                {
                    buf.Write("{0}Demos({1})", space, csgo.files_demos.Count);
                    space = " ";
                }
                if (csgo.files_screenshots.Count > 0)
                {
                    buf.Write("{0}Screenshots({1})", space, csgo.files_screenshots.Count);
                }
            }
            buf.WriteLine("{0}NextDemo({1})", space, csgo.demo_max + 1);
            if (options.docfg)
            {
                buf.WriteLine(string.Empty);
                //buf.Write(csgo.CreateRecordScript().ToString());
                csgo.CreateRecordScript();
                buf.WriteLine("Created record script {0}", Path.GetFileName(csgo.cfgname));
                //buf.WriteLine(Path.GetFileName(csgo.cfgname));
                buf.WriteLine(csgo.cfgname);
            }
            if (options.dostore)
            {
                buf.WriteLine(string.Empty);
                csgo.StorePlayers(buf);
                buf.WriteLine("Stored players.");
            }
            if (options.docopy || options.domove)
            {
                buf.WriteLine(string.Empty);
                csgo.BackupFiles(options.domove);
                buf.WriteLine("Moved {0} files to {1}", csgo.files_copied, csgo.backup_folder);
            }
            if (options.dostorebackup)
            {
                buf.WriteLine(string.Empty);
                csgo.StoreOldBackup(buf, options.filenames);
                buf.WriteLine("Stored old backup.");
            }
            return buf;
        } // method
    } // class
} // namespace
