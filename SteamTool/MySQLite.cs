using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTool
{
    public class MySQLite
    {
        public static void EnsureDatabaseCreated(string db_filename, string db_connection_string)
        {
            if (File.Exists(db_filename))
                return;
            SQLiteConnection.CreateFile(db_filename);
            using (SQLiteConnection connection = new SQLiteConnection(db_connection_string))
            {
                connection.Open();
                try
                {
                    CreateTable_Players(connection);
                    CreateTable_Nicks(connection);
                    CreateTable_Abuses(connection);
                    CreateTable_Fails(connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void CreateTable_Players(SQLiteConnection connection)
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

        public static void CreateTable_Nicks(SQLiteConnection connection)
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

        public static void CreateTable_Abuses(SQLiteConnection connection)
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

        public static void CreateTable_Fails(SQLiteConnection connection)
        {
            string sql = "create table unknowns (id INTEGER PRIMARY KEY AUTOINCREMENT, steamid varchar(80) NOT NULL, msg varchar(1000) NOT NULL)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
            sql = "CREATE INDEX idx_unknowns_steamid ON unknowns(steamid)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static int Lookup_Players(SQLiteConnection connection, string steamid)
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

        public static int Lookup_Nicks(SQLiteConnection connection, int playerid, string nick)
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

        public static int Lookup_Abuses(SQLiteConnection connection, int playerid, string abuse)
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

        public static void Insert_Steamid(SQLiteConnection connection, string steamid)
        {
            //string sql = "INSERT OR IGNORE INTO players(steamid) VALUES(@steamid)";
            string sql = "INSERT INTO players(steamid) VALUES(@steamid)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@steamid", steamid);
                command.ExecuteNonQuery();
            }
        }

        public static void Insert_Nick(SQLiteConnection connection, int playerid, string nick)
        {
            string sql = "INSERT INTO nicks(playerid, nick) VALUES(@playerid, @nick)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@playerid", playerid);
                command.Parameters.AddWithValue("@nick", nick);
                command.ExecuteNonQuery();
            }
        }

        public static void Insert_Abuse(SQLiteConnection connection, int playerid, string abuse)
        {
            string sql = "INSERT INTO abuses(playerid, abuse) VALUES(@playerid, @abuse)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@playerid", playerid);
                command.Parameters.AddWithValue("@abuse", abuse);
                command.ExecuteNonQuery();
            }
        }

        public static void Insert_Fail(SQLiteConnection connection, string steamid, string msg)
        {
            string sql = "INSERT INTO unknowns(steamid, msg) VALUES(@steamid, @msg)";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@steamid", steamid);
                command.Parameters.AddWithValue("@msg", msg);
                command.ExecuteNonQuery();
            }
        }

        public static void InsertPlayer(SQLiteConnection connection, string steamid, string nick, string abuse)
        {
            int playerid = Lookup_Players(connection, steamid);
            if (playerid == -1)
                Insert_Steamid(connection, steamid);
            playerid = Lookup_Players(connection, steamid);
            if (playerid == -1)
            {
                // TODO error
                return;
            }
            if (Lookup_Nicks(connection, playerid, nick) == -1)
                Insert_Nick(connection, playerid, nick);
            if (Lookup_Abuses(connection, playerid, abuse) == -1)
                Insert_Abuse(connection, playerid, abuse);
        }
    } // class
} // namespace
