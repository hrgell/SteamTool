using System;
using System.IO;
using Microsoft.Win32;

namespace SteamTool
{
    public class Arguments
    {
        public string output_folder = string.Empty;
        public bool docopy = false;
        public bool docfg = false;

        public Arguments(string[] args)
        {
            bool sawoutput = false;
            foreach (string arg in args) {
                if (arg.Length == 0)
                {
                    if (sawoutput)
                        throw new Exception("Too many command line options.");
                    sawoutput = true;
                    continue;
                }
                if (arg[0] != '-')
                {
                    if (sawoutput)
                        throw new Exception("Too many command line options.");
                    sawoutput = true;
                    output_folder = arg;
                    continue;
                }
                if (arg.Length == 1)
                    throw new Exception("A command line option must not be empty.");
                foreach (char ch in arg.Substring(1))
                {
                    if (ch == 'c')
                        docopy = true;
                    else if (ch == 'r')
                        docfg = true;
                    else if (ch == '?' || ch == 'h' || ch == '-')
                        throw new Exception(string.Format("Usage: {0} [-c] [-r] [outputfolder]", AppDomain.CurrentDomain.FriendlyName));
                    else
                        throw new Exception(string.Format("Unknown command line option -{0}.", ch));
                }
            } // foreach command line argument

            // Normalize the output path name
            if (output_folder.Length > 0)
                output_folder = Path.GetFullPath(output_folder);
        } // method
    } // class
}
