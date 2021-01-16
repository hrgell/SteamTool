using System;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace SteamTool
{
    static class Std
    {
        public static string GetRegistryString(RegistryHive hkey, string keyname, string itemname)
        {
            try
            {
                using (var basekey = RegistryKey.OpenBaseKey(hkey, RegistryView.Registry64))
                using (var key = basekey.OpenSubKey(keyname, false))
                {
                    if (key != null)
                    {
                        object val = key.GetValue(itemname);
                        if (val != null)
                            return (val as string);
                    }
                }
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine("GetKeyValue exception");
            }
            return string.Empty;
        } // method

        public static void SetRegistryString(RegistryHive hkey, string keyname, string itemname, string itemvalue)
        {
            using (var basekey = RegistryKey.OpenBaseKey(hkey, RegistryView.Registry64))
            using (var key = basekey.CreateSubKey(keyname, true))
            {
                key.SetValue(itemname, itemvalue);
            }
        }

        public static void CopyFile(string src, string dst, bool move_file = false)
        {
            if (move_file)
                File.Move(src, dst);
            else
                File.Copy(src, dst, false);
        } // method

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        } // method

        public static string TrimLeadingDigits(string str)
        {
            if (str.Length == 0)
                return string.Empty;
            int idx = -1;
            foreach (char c in str)
            {
                if (c != '0')
                    break;
                ++idx;
            }
            if (idx == -1)
                return str;
            if (idx == str.Length - 1)
                return "0";
            return str.Substring(idx + 1);
        } // method

        public static void Write(this StringBuilder buf, string text, params object[] args)
        {
            buf.Append(string.Format(text, args));
        } // method

        public static void WriteLine(this StringBuilder buf, string text, params object[] args)
        {
            buf.AppendLine(string.Format(text, args));
        } // method
    } // class
} // namespace
