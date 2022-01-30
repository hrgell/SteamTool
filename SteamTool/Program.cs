using System;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace SteamTool
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            StringBuilder buf = null;
            if (args.Length > 0)
                buf = DoMyTask(args);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 frm = new Form1();
            if (buf != null)
                frm.SetMessages(buf.ToString());
            Application.Run(frm);
        } // Main()

        public static StringBuilder DoMyTask(string[] args)
        {
            Arguments options = new Arguments(args);
            return ProcessCSGO(options);
        } // method

        public static StringBuilder ProcessCSGO(Arguments options)
        {
            StringBuilder buf = new StringBuilder();
            Steam steam = new Steam();
            string appid = "730";
            VifEntry manifest = steam.GetManifest(appid);
            if(manifest == null)
                throw new Exception(string.Format("Failed to read the manifest for csgo (appid={0}) from \"{1}\"", appid, steam.apps_folder));
            string game_folder = Path.Combine(steam.apps_folder, "common", manifest.Get("AppState", "installdir"));
            string game_name = manifest.Get("AppState", "name");
            if (!Directory.Exists(game_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", game_folder));
            buf.WriteLine("Appid {0}", appid);
            buf.WriteLine(game_name);
            buf.WriteLine(game_folder);
            //manifest.ToStringBuilder(buf);
            CSGO csgo = new CSGO(game_folder, options.output_folder);
            if (options.docfg)
            {
                buf.WriteLine("Created Script:");
                buf.Write(csgo.CreateRecordScript().ToString());
            }
            buf.WriteLine("{0}{1}", "Files found: ", csgo.files.Count);
            buf.WriteLine("{0}{1}", "Next demo number: ", csgo.maxnum + 1);
            if (options.dostore)
            {
                csgo.StorePlayers();
                buf.WriteLine("Stored players.");
            }
            if (options.docopy)
            {
                csgo.CopyFiles();
                buf.WriteLine("Copied {0} files to {1}", csgo.files_copied, csgo.output_folder);
            }
            return buf;
        } // method
    } // class
} // namespace
