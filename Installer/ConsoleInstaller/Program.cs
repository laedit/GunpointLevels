using Gunpoint.Common;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace GunpointLevels.ConsoleInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GunPoint Levels Console Installer");
            Console.WriteLine();

            if (args != null && args.Length == 0)
            {
                Console.WriteLine("Error, I need at least the url to the .lvl file to install it!");
            }
            else
            {
                // TODO merge with GunpointLevelInstallationProcess
                string levelFilePath = args[0];
                if (levelFilePath.StartsWith(UrlProtocol.Prefix))
                {
                    levelFilePath = levelFilePath.Replace(UrlProtocol.Prefix, string.Empty);
                }

                bool isInError = false;
                try
                {
                    LevelsUtils.CheckGunpointLevelFile(levelFilePath);
                }
                catch(Exception ex)
                {
                    isInError = true;
                    Console.WriteLine(ex.Message);
                }

                if (!isInError)
                {
                    string gunpointInstallFolder = GetGunpointInstallFolder();
                    string gunpointLevelsFolder = Path.Combine(gunpointInstallFolder, "Levels");

                    if (!Directory.Exists(gunpointLevelsFolder))
                    {
                        Directory.CreateDirectory(gunpointLevelsFolder);
                    }

                    WebClient wc = new WebClient();
                    wc.DownloadFile(args[0], Path.GetFileName(args[0]));
                }
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        private static string GetGunpointInstallFolder()
        {
            string gunpointInstallFolder = ConfigurationManager.AppSettings["gunpointInstallFolder"];
            if (string.IsNullOrEmpty(gunpointInstallFolder))
            {
                // TODO use GunpointLevelInstallationProcess
                var subkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 206190");
                if (subkey != null)
                {
                    gunpointInstallFolder = (string)subkey.GetValue("InstallLocation");
                }

                if (string.IsNullOrEmpty(gunpointInstallFolder))
                {
                    subkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 206190");
                    if (subkey != null)
                    {
                        gunpointInstallFolder = (string)subkey.GetValue("InstallLocation");
                    }

                    if (string.IsNullOrEmpty(gunpointInstallFolder))
                    {
                        gunpointInstallFolder = GetUserProvidedGunpointInstallationFolder();
                    }
                }

                ConfigurationManager.AppSettings["gunpointInstallFolder"] = gunpointInstallFolder;
            }
            return gunpointInstallFolder;
        }

        private static string GetUserProvidedGunpointInstallationFolder()
        {
            Console.WriteLine("I can't find the Gunpoint installation folder, can you tell me what it is?");
            string gunpointInstallFolder = Console.ReadLine();
            if (!Directory.Exists(gunpointInstallFolder))
            {
                gunpointInstallFolder = GetUserProvidedGunpointInstallationFolder();
            }
            return gunpointInstallFolder;
        }
    }
}
