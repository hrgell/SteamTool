using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Win32;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace SteamTool
{
    // C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo
    class CSGO
    {
        public long files_copied = 0;
        public List<string> files = null;
        public List<string> files_condumps = null;
        public List<string> files_demos = null;
        public List<string> files_screenshots = null;
        public long demo_max = 0;
        // Marked these readonly because the compiler suggested it - we assign to them here, but we also assign to some of them in the constructor. /shrug.
        // Need to update to .Net6 to get better initialization - I don't want them to be null at any time, because it makes the other code easier to maintain.
        public readonly string csgo_folder = string.Empty;
        public string cfgname = string.Empty;
        public readonly string screenshots_folder = string.Empty;
        public readonly string backup_folder = string.Empty;
        public readonly string db_filename = string.Empty;
        public readonly string db_connection_string = string.Empty;
        // These are used while processing files
        private readonly Regex rx_steamid = null;
        private readonly Dictionary<string, string> players = null;
        //private readonly List<Tuple<string, string>> abusers = null;
        private readonly HashSet<Tuple<string, string, string>> abusers = null;
        private readonly char[] charsToTrim = { '\t', ' ', '"', ',' };

        public CSGO(string game_folder, string output_folder)
        {
            //string pattern = "(STEAM_[0-9]:[0-9]+:[0-9]+)(?:([ \"]|$))";
            string pattern = "(STEAM_[0-9]:[0-9]+:[0-9]+)(?:[ \"]|$)";
            rx_steamid = new Regex(pattern);
            players = new Dictionary<string, string>();
            abusers = new HashSet<Tuple<string, string, string>>();

            this.backup_folder = output_folder;
            db_filename = Path.Combine(output_folder, "playerdb.sqlite");
            db_connection_string = String.Format("Data Source={0};Version=3;", db_filename);

            csgo_folder = Path.Combine(game_folder, "csgo");
            if (!Directory.Exists(csgo_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", csgo_folder));

            cfgname = Path.Combine(csgo_folder, "cfg", "myrecord.cfg");

            screenshots_folder = Path.Combine(csgo_folder, "screenshots");

            files = new List<string>();
            files_condumps = new List<string>();
            files_demos = new List<string>();
            files_screenshots = new List<string>();

            // Get .dem and .txt files
            string[] filelist = Directory.GetFiles(csgo_folder);
            foreach (string filename in filelist)
            {
                string ext = Path.GetExtension(filename).ToLower();
                if (ext == ".dem")
                {
                    files.Add(filename);
                    files_demos.Add(filename);
                    string basename = Path.GetFileNameWithoutExtension(filename);
                    // Assume a single letter is used as prefix
                    if (basename.Length > 1)
                    {
                        string rest = basename.Substring(1);
                        if (Std.IsDigitsOnly(rest))
                        {
                            long num = long.Parse(Std.TrimLeadingDigits(rest));
                            if (demo_max < num)
                                demo_max = num;
                        }
                    }
                }
                else if (ext == ".txt" && Path.GetFileName(filename).StartsWith("condump"))
                {
                    files.Add(filename);
                    files_condumps.Add(filename);
                }
            }
            // Get screenshots
            if (Directory.Exists(screenshots_folder))
            {
                filelist = Directory.GetFiles(screenshots_folder);
                foreach (string filename in filelist)
                {
                    files.Add(filename);
                    files_screenshots.Add(filename);
                }
            }

            // Sync demo_max with the registry
            long max = MyRegistry.GetMaxCfg();
            if (max > demo_max)
                demo_max = max;
            else
                MyRegistry.SetMaxCfg(demo_max);
        } // method

        public StringBuilder CreateRecordScript()
        {

            MyRegistry.TestMe();

            long newmax = demo_max + 20;
            StringBuilder buf = new StringBuilder();
            buf.WriteLine("echo \"// Executing Record\"");
            buf.WriteLine("alias ra \"echo Cant record. The record config needs to be updated.\"");
            buf.WriteLine("alias rc r1");
            long step = 1;
            // Use a single letter as prefix
            for (long idx = demo_max + 1; idx < newmax; ++idx, ++step)
                buf.WriteLine("alias r{0} \"record c{1};alias rc r{2}\"", step, idx.ToString("D5"), step + 1);
            buf.WriteLine("alias r{0} \"record c{1};alias rc ra", step, newmax.ToString("D5"));
            buf.WriteLine("bind F11 rc");
            File.WriteAllText(cfgname, buf.ToString(), new UTF8Encoding(false));
            return buf;
        } // method

        private void EnsureOutputFolderExists()
        {
            // Create the specified folder
            if (!Directory.Exists(backup_folder))
                Directory.CreateDirectory(backup_folder);
            if (!Directory.Exists(backup_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", backup_folder));
        }

        private bool CheckFilesAndOuputFolder()
        {
            if (files.Count() == 0)
                return false;
            if (backup_folder.Length == 0)
                return false;
            EnsureOutputFolderExists();
            return true;
        }

        private void ProcessConsoleLine(StringBuilder buf, string line)
        {
            bool debug = false;
            if (string.IsNullOrEmpty(line))
                return;
            // Locate the steamid STEAM_Y:XXX:ZZZZZZZZZ
            int steamid_start = -1;
            Match match = rx_steamid.Match(line);
            if (match.Success)
            {
                steamid_start = match.Groups[1].Index;
            }

            if (steamid_start == 0)
            {
                // We don't like steamids at the start of the line.
                if (debug) buf.WriteLine("[START_OF_LINE] {0}", line);
                return;

            }
            if (steamid_start < 0)
            {
                // The line does not contain a steamid, and may be empty.
                //if (debug) buf.WriteLine("{0}", line);
                return;
            }

            string steamid = match.Groups[1].Value;

            // Is it s a status line?
            char ch = line[0];
            if (ch == '#')
            {
                if (debug) buf.WriteLine("[STAT] {0}", line);
                string namepart = line.Substring(0, steamid_start);
                if (debug) buf.WriteLine("[NAMEPART] {0}", namepart);
                int nick_start = namepart.IndexOf('"') + 1;
                if (nick_start > 0)
                {
                    int nick_length = namepart.Length - nick_start - 2;
                    if (nick_length >= 0)
                    {
                        if (namepart[nick_start + nick_length] == '"')
                        {
                            string nick = namepart.Substring(nick_start, nick_length);
                            if (debug) buf.WriteLine("[NICK] {0}-> '{1}'", steamid, nick);
                            if (!players.ContainsKey(steamid))
                            {
                                if (debug) buf.WriteLine("[ADD] {0}-> '{1}'", steamid, nick);
                                players.Add(steamid, nick);
                            }
                        }
                    }
                }
                return;
            }

            int pos_abuse = steamid_start + steamid.Length + 1;
            string abuse = (pos_abuse < line.Length) ? line.Substring(pos_abuse).Trim(charsToTrim) : string.Empty;
            // Is it a console command?
            if (ch == ']')
            {
                if (debug) buf.WriteLine("[ABUSE_CONSOLE] {0}", line);
                if (debug) buf.WriteLine("[FOUND] {0} -> '{1}'", steamid, abuse);
                var tuple = new Tuple<string, string, string>(steamid, abuse, line);
                // Filter out abuses that stat lines that start with a digit, like: 22:09 90 0 active 196608
                if (abuse.Length == 0 || !(char.IsDigit(abuse[0]) || abuse.Contains(':')))
                {
                    if (!abusers.Contains(tuple))
                        abusers.Add(tuple);
                }
                return;
            }

            // It's not a status line or a console command. Is it a say or say_team command?
            // Process these AFTER all the status lines (from all the files) have been processed.
            int pos2 = line.IndexOf(" : ");
            if (pos2 >= 0 && pos2 < steamid_start)
            {
                //if (debug) buf.WriteLine("[ABUSE_SAY] {0}", line);
                //if (debug) buf.WriteLine("[FOUND] {0} -> '{1}'", steamid, abuse);
                // Filter out abuses that stat lines like: 22:09 90 0 active 196608
                if (abuse.Length == 0 || !(char.IsDigit(abuse[0]) || abuse.Contains(':')))
                    {
                    var tuple = new Tuple<string, string, string>(steamid, abuse, line);
                    if (!abusers.Contains(tuple))
                        abusers.Add(tuple);
                }
                return;
            }

            // We have a steamid that fails to meet our standard, a spelling mistake or the like.
            if (debug) buf.WriteLine("[IGNORED] {0}", line);
            return;
        }

        private void ProcessConsoleFiles(StringBuilder buf, SQLiteConnection connection, List<string> filenames)
        {
            PreProcess(buf, filenames);
            PostProcess(buf, connection);
            // TODO Insert all players we meet?
        }

        private void PreProcess(StringBuilder buf, List<string> filenames)
        {
            foreach (var filename in filenames)
            {
                var ext = Path.GetExtension(filename).ToLower();
                //if (!ext.Equals(".txt"))
                //    continue;
                const Int32 siz = 1024;
                using (var stream = File.OpenRead(filename))
                using (var reader = new StreamReader(stream, Encoding.UTF8, true, siz))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                        ProcessConsoleLine(buf, line);
                }
            }
        }

        // TODO Add new nicks to the db.
        // TODO Use MySQLite.Lookup_Nicks(connection, playerid, nick) and MySQLite.Insert_Nick(connection, playerid, nick)
        private void PostProcess(StringBuilder buf, SQLiteConnection connection)
        {
            // Process the collected abusers.
            buf.WriteLine("Stats/Store: \"nick\": steamid -> abuse");
            buf.WriteLine("-------------------------------------------------------------");
            //Dictionary<string, int> dct = new Dictionary<string, int>();
            foreach (var tuble in abusers)
            {
                string steamid = tuble.Item1;
                string nick = string.Empty;
                string abuse = (tuble.Item2.Length == 0) ? "afk" : tuble.Item2;

                bool hasstat = players.ContainsKey(steamid);
                bool store = false;
                bool failed = false;

                if (!hasstat)
                {
                    // Check if the player is in the db already.
                    int playerid = MySQLite.Lookup_Players(connection, steamid);
                    if (playerid != -1)
                    {
                        store = MySQLite.Lookup_Abuses(connection, playerid, abuse) == -1;
                    }
                    else
                    {
                        failed = true;
                    }
                }
                else
                {
                    store = true;
                    nick = players[steamid];
                }

                if (failed)
                {
                    buf.WriteLine("[Failed] {0} -> {1}", steamid, abuse);
                    MySQLite.Insert_Fail(connection, steamid, tuble.Item3);
                }
                else
                {
                    buf.WriteLine("{0}/{1}: \"{2}\": {3} -> {4}", hasstat, store, nick, steamid, abuse);
                    if (store)
                        MySQLite.InsertPlayer(connection, steamid, nick, abuse);
                }
            }
        }

        private void ProcessOldBackup(StringBuilder buf, SQLiteConnection connection, string folder)
        {
            //List<string> lst = Directory.GetFiles(folder)
            var files = Directory.EnumerateFiles(folder, "con*.txt");
            PreProcess(buf, files.ToList<string>());
            PostProcess(buf, connection);
        }

        public void StoreOldBackup(StringBuilder buf, List<string> filenames)
        {
            EnsureOutputFolderExists();
            MySQLite.EnsureDatabaseCreated(db_filename, db_connection_string);
            using (SQLiteConnection connection = new SQLiteConnection(db_connection_string))
            {
                connection.Open();
                try
                {
                    foreach (string filename in filenames)
                    {
                        ProcessOldBackup(buf, connection, filename);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void StorePlayers(StringBuilder buf)
        {
            if (!CheckFilesAndOuputFolder())
                return;
            MySQLite.EnsureDatabaseCreated(db_filename, db_connection_string);
            using (SQLiteConnection connection = new SQLiteConnection(db_connection_string))
            {
                connection.Open();
                try
                {
                    ProcessConsoleFiles(buf, connection, files_condumps);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void BackupFiles(bool do_move)
        {
            if (!CheckFilesAndOuputFolder())
                return;

            // Create a sub folder 
            string output_subfolder = Path.Combine(backup_folder, DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (!Directory.Exists(output_subfolder))
                Directory.CreateDirectory(output_subfolder);
            if (!Directory.Exists(output_subfolder))
                throw new Exception(string.Format("Folder does not exist: {0}", output_subfolder));

            // Copy the files
            foreach (string filename in files)
            {
                string newname = Path.Combine(output_subfolder, Path.GetFileName(filename));
                Std.CopyFile(filename, newname, do_move);
                ++files_copied;
            }
        } // method
    } // class
} // namespace
