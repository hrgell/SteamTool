using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Data.SQLite;

namespace SteamTool
{
    // C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive\csgo
    class CSGO
    {
        public long files_copied = 0;
        public bool move_files = true;
        public List<string> files = new List<string>();
        public long maxnum = 0;
        // Marked these readonly because the compiler suggested it - but we assign to them here AND in the constructor.
        public readonly string csgo_folder = string.Empty;
        public readonly string screenshots_folder = string.Empty;
        public readonly string output_folder = string.Empty;
        public readonly string db_filename = string.Empty;
        public readonly string db_connection_string = string.Empty;

        public CSGO(string game_folder, string output_folder)
        {
            this.output_folder = output_folder;
            db_filename = Path.Combine(output_folder, "playerdb.sqlite");
            db_connection_string = String.Format("Data Source={0};Version=3;", db_filename);

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

        private void SQLite_CreateTable_Players(SQLiteConnection connection)
        {
            string sql = "create table players (id INTEGER PRIMARY KEY AUTOINCREMENT, steamid varchar(80) NOT NULL)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE UNIQUE INDEX idx_players_steamid ON players(steamid)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void SQLite_CreateTable_Nicks(SQLiteConnection connection)
        {
            string sql = "create table nicks (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "playerid INTEGER NOT NULL," +
                    //"nick varchar(100) NOT NULL," +
                    "nick varchar(100)," +
                    "FOREIGN KEY (playerid) " +
                    "REFERENCES players (id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ")";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE INDEX idx_nicks_playerid ON nicks(playerid)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE INDEX idx_nicks_nick ON nicks(nick)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE UNIQUE INDEX idx_nicks_playerid_nick ON nicks(playerid, nick)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void SQlite_CreateTable_Abuses(SQLiteConnection connection)
        {
            string sql = "create table abuses (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "playerid INTEGER NOT NULL, " +
                    //"abuse varchar(40) NOT NULL, " +
                    "abuse varchar(40), " +
                    "FOREIGN KEY (playerid) " +
                    "REFERENCES players (id) ON UPDATE CASCADE ON DELETE CASCADE" +
                    ")";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE INDEX idx_abuses_playerid ON abuses(playerid)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE INDEX idx_abuses_abuse ON abuses(abuse)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            sql = "CREATE UNIQUE INDEX idx_abuses_playerid_abuse ON abuses(playerid, abuse)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private int SQLite_Lookup_Players(SQLiteConnection connection, string steamid)
        {
            int id = -1;
            string sql = "SELECT id FROM players WHERE steamid = @steamid";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@steamid", steamid);
                object field = command.ExecuteScalar();
                if (field != null)
                {
                    //playerid = (int)ob;
                    // TODO I think sqlite uses 64 bit int - detect if 32-bit or 64-bit, and use the right int type.
                    id = Convert.ToInt32(field);
                }
            }
            return id;
        }

        private int SQLite_Lookup_Nicks(SQLiteConnection connection, int playerid, string nick)
        {
            int id = -1;
            string sql = "SELECT id FROM nicks WHERE playerid = @playerid and nick = @nick";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@playerid", playerid);
                command.Parameters.AddWithValue("@nick", nick);
                object field = command.ExecuteScalar();
                if (field != null)
                {
                    id = Convert.ToInt32(field);
                }
            }
            return id;
        }

        private int SQLite_Lookup_Abuses(SQLiteConnection connection, int playerid, string abuse)
        {
            int id = -1;
            string sql = "SELECT id FROM abuses WHERE playerid = @playerid and abuse = @abuse";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@playerid", playerid);
                command.Parameters.AddWithValue("@abuse", abuse);
                object field = command.ExecuteScalar();
                if (field != null)
                {
                    id = Convert.ToInt32(field);
                }
            }
            return id;
        }

        private void SQLite_Insert_Steamid(SQLiteConnection connection, string steamid)
        {
            //string sql = "INSERT OR IGNORE INTO players(steamid) VALUES(@steamid)";
            string sql = "INSERT INTO players(steamid) VALUES(@steamid)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@steamid", steamid);
                command.ExecuteNonQuery();
            }
        }

        private void SQLite_Insert_Nick(SQLiteConnection connection, int playerid, string nick)
        {
            string sql = "INSERT INTO nicks(playerid, nick) VALUES(@playerid, @nick)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@playerid", playerid);
                command.Parameters.AddWithValue("@nick", nick);
                command.ExecuteNonQuery();
            }
        }

        private void SQLite_Insert_Abuse(SQLiteConnection connection, int playerid, string abuse)
        {
            string sql = "INSERT INTO abuses(playerid, abuse) VALUES(@playerid, @abuse)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@playerid", playerid);
                command.Parameters.AddWithValue("@abuse", abuse);
                command.ExecuteNonQuery();
            }
        }

        private void SQLite_InsertPlayer(SQLiteConnection connection, string steamid, string nick, string abuse)
        {
            int playerid = SQLite_Lookup_Players(connection, steamid);
            if (playerid == -1)
                SQLite_Insert_Steamid(connection, steamid);
            playerid = SQLite_Lookup_Players(connection, steamid);
            if (playerid == -1)
            {
                // TODO error
                return;
            }
            if(SQLite_Lookup_Nicks(connection, playerid, nick) == -1)
                SQLite_Insert_Nick(connection, playerid, nick);
            if (SQLite_Lookup_Abuses(connection, playerid, abuse) == -1)
                SQLite_Insert_Abuse(connection, playerid, abuse);
        }

        private void SQLite_InsertPlayers(SQLiteConnection connection)
        {
            foreach (var filename in files)
            {
                var ext = Path.GetExtension(filename).ToLower();
                if (!ext.Equals(".txt"))
                    continue;
                // TODO Scan the file and insert the players
            }
            // TODO Remove this when we have written code for scanning the files in the foreach above.
            SQLite_InsertPlayer(connection, "steamid1", "nickname1", "afk1");
            SQLite_InsertPlayer(connection, "steamid1", "nickname1", "afk1");
            SQLite_InsertPlayer(connection, "steamid2", "nickname2", "afk2");
            SQLite_InsertPlayer(connection, "steamid2", "nickname2", "afk2b");
            SQLite_InsertPlayer(connection, "steamid3", "nickname3", "afk1");
            SQLite_InsertPlayer(connection, "steamid3", "nickname3", "afk2");
            SQLite_InsertPlayer(connection, "steamid3", "nickname3", "afk2b");
            SQLite_InsertPlayer(connection, "steamid3", "nickname3", "afk3");
            SQLite_InsertPlayer(connection, "steamid3", "nickname3b", "afk3");
        }

        private void SQLite_CreateDb()
        {
            if (File.Exists(db_filename))
                return;
            SQLiteConnection.CreateFile(db_filename);
            using (SQLiteConnection connection = new SQLiteConnection(db_connection_string))
            {
                connection.Open();
                try
                {
                    SQLite_CreateTable_Players(connection);
                    SQLite_CreateTable_Nicks(connection);
                    SQlite_CreateTable_Abuses(connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void StorePlayers()
        {
            if (!CheckFilesAndOuputFolder())
                return;
            SQLite_CreateDb();
            using (SQLiteConnection connection = new SQLiteConnection(db_connection_string))
            {
                connection.Open();
                try
                {
                    SQLite_InsertPlayers(connection);
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
