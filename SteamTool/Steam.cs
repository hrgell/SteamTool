using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace SteamTool
{
    class Steam
    {
        public string steam_folder = string.Empty;
        public string apps_folder = string.Empty;
        public string libs_file = string.Empty;
        Dictionary<string, VifEntry> manifests = new Dictionary<string, VifEntry>();

        public Steam()
        {
            // Get paths of folders and files
            steam_folder = GetSteamFolder();
            if (string.IsNullOrEmpty(steam_folder))
                throw new Exception("Steam is not installed.");
            if (!Directory.Exists(steam_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", steam_folder));
            apps_folder = Path.Combine(steam_folder, "steamapps");
            if (!Directory.Exists(apps_folder))
                throw new Exception(string.Format("Folder does not exist: {0}", apps_folder));
            libs_file = Path.Combine(apps_folder, "libraryfolders.vdf");
            if (!File.Exists(libs_file))
                throw new Exception(string.Format("File does not exist: {0}", libs_file));
            // Read the libfolders file
            string contents = File.ReadAllText(libs_file);
            VifEntry libraries = VifEntry.Scan(ref contents);
            // Read the manifest files
            string[] filelist = Directory.GetFiles(apps_folder);
            foreach (string filename in filelist)
            {
                string ext = Path.GetExtension(filename).ToLower();
                const string prefix = "appmanifest_";
                int prelen = prefix.Length;
                if (ext == ".acf" && Path.GetFileName(filename).StartsWith(prefix)) // eg. "appmanifest_228980.acf"
                {
                    contents = File.ReadAllText(filename);
                    VifEntry manifest = VifEntry.Scan(ref contents);
                    string appid = Path.GetFileNameWithoutExtension(filename).Substring(prelen);
                    manifests[appid] = manifest;
                }
            }
        } // constructor

        private static string GetSteamFolder()
        {
            string val = Std.GetRegistryString(RegistryHive.LocalMachine, @"SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath");
            if (val.Length == 0)
                val = Std.GetRegistryString(RegistryHive.LocalMachine, @"SOFTWARE\Valve\Steam", "InstallPath");
            if (val.Length == 0)
                return string.Empty;
            return val;
        } // method

        public VifEntry GetManifest(string appid)
        {
            if (!manifests.ContainsKey(appid))
                return null;
            return manifests[appid];
        }
    } // class
} // namespace
