using System;
using Microsoft.Win32;

namespace SteamTool
{
    public class MyRegistry
    {
        // The key
        //     @"HKEY_CURRENT_USER\MySteamToolX"
        // gets mapped to this key on my development machine (the digits are replaced with #'s)
        //     @"Computer\HKEY_USERS\S-#-#-##-#########-#########-##########-####\MySteamToolX"
        // Currently we only use "MaxCfg" to hold a long integer.
        const string root_key = @"HKEY_CURRENT_USER\MySteamToolX";

        public static void TestMe()
        {
            //Uninstall();
            //SetDefault(0);
            //SetMaxCfg(5);
            long result = GetMaxCfg();
            System.Diagnostics.Debug.WriteLine("MaxCfg: {0}", result);
            //Uninstall();
        }

        public static void Install(long default_value = 0)
        {
            try { Registry.SetValue(root_key, "", default_value, RegistryValueKind.QWord); } catch { }
        }

        public static void Uninstall()
        {
            // TODO make sure it works
            try { Registry.CurrentUser.DeleteSubKeyTree(root_key); } catch { }
        }

        public static void SetMaxCfg(long maxfg)
        {
            try { Registry.SetValue(root_key, "MaxCfg", maxfg, RegistryValueKind.QWord); } catch { }
        }

        public static long GetMaxCfg(long default_value = 0)
        {
            try
            {
                var result = Registry.GetValue(root_key, "MaxCfg", default_value);
                return (result == null) ? default_value : (long)result;
            } catch
            {
                return default_value;
            }
        }

        public static void SetFolder(string folder)
        {
            try { Registry.SetValue(root_key, "Folder", folder, RegistryValueKind.String); } catch { }
        }

        public static string GetFolder()
        {
            try
            {
                var result = Registry.GetValue(root_key, "Folder", string.Empty);
                if (result != null)
                    return result as string;
            }
            catch
            {
            }
            return string.Empty;
        }
    } // class MyRegistry
} // namespace
