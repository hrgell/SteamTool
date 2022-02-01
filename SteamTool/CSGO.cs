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
        // TODO Add move_files to Arguments, use -m to set it to true.
        // TODO Change the default to false when move_files have been added to Arguments.
        public bool move_files = true;
        public List<string> files = null;
        public long maxnum = 0;
        // Marked these readonly because the compiler suggested it - we assign to them here, but we also assign to some of them in the constructor. /shrug.
        // Need to update to .Net6 to get better initialization - I don't want them to be null at any time, because it makes the other code easier to maintain.
        public readonly string csgo_folder = string.Empty;
        public readonly string screenshots_folder = string.Empty;
        public readonly string output_folder = string.Empty;
        public readonly string db_filename = string.Empty;
        public readonly string db_connection_string = string.Empty;
        // These are used while processing files
        private readonly Regex rx_steamid = null;
        private readonly Dictionary<string, string> players = null;
        private readonly List<Tuple<string, string>> abusers = null;
        private readonly char[] charsToTrim = { '\t', ' ', '"', ',' };

        public CSGO(string game_folder, string output_folder)
        {
            //string pattern = "(STEAM_[0-9]:[0-9]+:[0-9]+)(?:([ \"]|$))";
            string pattern = "(STEAM_[0-9]:[0-9]+:[0-9]+)(?:[ \"]|$)";
            rx_steamid = new Regex(pattern);
            players = new Dictionary<string, string>();
            abusers = new List<Tuple<string, string>>();

            this.output_folder = output_folder;
            db_filename = Path.Combine(output_folder, "playerdb.sqlite");
            db_connection_string = String.Format("Data Source={0};Version=3;", db_filename);

            csgo_folder = Path.Combine(game_folder, "csgo");
            if (!Directory.Exists(csgo_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", csgo_folder));
            screenshots_folder = Path.Combine(csgo_folder, "screenshots");

            files = new List<string>();
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

        private bool CheckFilesAndOuputFolder()
        {
            if (files.Count() == 0)
                return false;
            if (output_folder.Length == 0)
                return false;

            // Create the specified folder
            if (!Directory.Exists(output_folder))
                Directory.CreateDirectory(output_folder);
            if (!Directory.Exists(output_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", output_folder));
            return true;
        }

        private void ProcessConsoleLine(SQLiteConnection connection, string line)
        {
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
                // The line contains a steamid at the start of the line. We don't like that.
                System.Diagnostics.Debug.WriteLine("[START_OF_LINE] " + line);
                return;

            }
            if (steamid_start < 0)
            {
                // The line does not contain a steamid, and may be empty.
                //System.Diagnostics.Debug.WriteLine(line);
                return;
            }

            string steamid = match.Groups[1].Value;

            // Is it s a status line?
            char ch = line[0];
            if (ch == '#')
            {
                System.Diagnostics.Debug.WriteLine("[STAT] " + line);
                string namepart = line.Substring(0, steamid_start);
                System.Diagnostics.Debug.WriteLine("[NAMEPART] " + namepart);
                int nick_start = namepart.IndexOf('"') + 1;
                if(nick_start > 0)
                {
                    int nick_length = namepart.Length - nick_start - 2;
                    if (nick_length >= nick_start)
                    {
                        if (namepart[nick_start + nick_length] == '"')
                        {
                            string nick = namepart.Substring(nick_start, nick_length);
                            System.Diagnostics.Debug.WriteLine("[NICK] " + steamid + "-> '" + nick + "'");
                            if (!players.ContainsKey(steamid))
                            {
                                //System.Diagnostics.Debug.WriteLine("[ADD] " + steamid + " -> " + nick);
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
                System.Diagnostics.Debug.WriteLine("[ABUSE_CONSOLE" + line);
                System.Diagnostics.Debug.WriteLine("[FOUND] " + steamid + " -> '" + abuse + "'");
                // Filter out abuses that stat lines like: 22:09 90 0 active 196608
                bool has_digit = abuse.Length > 0 && char.IsDigit(abuse[0]);
                if (!has_digit)
                    abusers.Add(new Tuple<string, string>(steamid, abuse));
                return;
            }

            // It's not a status line or a console command. Is it a say or say_team command?
            // Process these AFTER all the status lines (from all the files) have been processed.
            int pos2 = line.IndexOf(" : ");
            if (pos2 >= 0 && pos2 < steamid_start)
            {
                System.Diagnostics.Debug.WriteLine("[ABUSE_SAY] " + line);
                System.Diagnostics.Debug.WriteLine("[FOUND] " + steamid + " -> '" + abuse + "'");
                // Filter out abuses that stat lines like: 22:09 90 0 active 196608
                bool has_digit = abuse.Length > 0 && char.IsDigit(abuse[0]);
                if (!has_digit)
                    abusers.Add(new Tuple<string, string>(steamid, abuse));
                return;
            }

            // We have a steamid that fails to meet our standard, a spelling mistake or the like.
            // TODO Delete the Debug.WriteLine when we have debugged a lot of condumps
            System.Diagnostics.Debug.WriteLine("[IGNORED] " + line);
            return;
        }

        private void ProcessConsoleFiles(SQLiteConnection connection)
        {
            foreach (var filename in files)
            {
                var ext = Path.GetExtension(filename).ToLower();
                if (!ext.Equals(".txt"))
                    continue;
                const Int32 siz = 1024;
                using (var stream = File.OpenRead(filename))
                using (var reader = new StreamReader(stream, Encoding.UTF8, true, siz))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                        ProcessConsoleLine(connection, line);
                }
            }

            // Process the collected abusers.
            foreach(var tuble in abusers)
            {
                string steamid = tuble.Item1;
                string abuse = tuble.Item2;
                if (!players.ContainsKey(steamid))
                {
                    // Check if the player is in the db already.
                    int playerid = MySQLite.Lookup_Players(connection, steamid);
                    if (playerid != -1)
                    {
                        if (MySQLite.Lookup_Abuses(connection, playerid, abuse) == -1)
                            MySQLite.Insert_Abuse(connection, playerid, abuse);
                    }
                    continue;
                }
                string nick = players[steamid];
                MySQLite.InsertPlayer(connection, steamid, nick, (abuse.Length == 0) ? "afk" : abuse);
            }
            // TODO Insert players we have met, that don't abuse?
        }

        public void StorePlayers()
        {
            if (!CheckFilesAndOuputFolder())
                return;
            MySQLite.CreateDb(db_filename, db_connection_string);
            using (SQLiteConnection connection = new SQLiteConnection(db_connection_string))
            {
                connection.Open();
                try
                {
                    ProcessConsoleFiles(connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void CopyFiles()
        {
            if (!CheckFilesAndOuputFolder())
                return;

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
