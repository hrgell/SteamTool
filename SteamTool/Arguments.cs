using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace SteamTool
{
    public class Arguments
    {
        public string folder = string.Empty;
        public bool dostorebackup = false;
        public bool docopy = false;
        public bool domove = false;
        public bool docfg = false;
        public bool dostore = false;
        public List<string> filenames = new List<string>();

        public Arguments(string[] args)
        {
            string usage = string.Format("Usage: {0} [-b] [-c] [-m] [-r] [-s] folder [filenames...]", AppDomain.CurrentDomain.FriendlyName);
            bool sawfolder = false;
            foreach (string arg in args) {
                if (arg.Length == 0)
                    throw new Exception("Empty command line argument is not allowed.");
                if (arg[0] != '-')
                {
                    if (sawfolder)
                        filenames.Add(arg);
                    else
                        folder = arg;
                    sawfolder = true;
                    continue;
                }
                if (arg.Length == 1)
                    throw new Exception("A command line option must not be empty.");
                foreach (char ch in arg.Substring(1))
                {
                    if (ch == 'b')
                        dostorebackup = true;
                    else if (ch == 'c')
                        docopy = true;
                    else if (ch == 'm')
                        domove = true;
                    else if (ch == 'r')
                        docfg = true;
                    else if (ch == 's')
                        dostore = true;
                    else if (ch == '?' || ch == 'h' || ch == '-')
                        throw new Exception($"{usage}");
                    else
                        throw new Exception($"{usage}: Unknown command line option -{ch}.");
                }
            } // foreach command line argument

            // Normalize the output path name
            if (folder.Length > 0)
                folder = Path.GetFullPath(folder);
        } // method
    } // class
}
