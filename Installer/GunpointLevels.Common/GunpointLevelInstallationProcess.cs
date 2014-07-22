using Microsoft.Win32;
using System.Configuration;
using System.IO;

namespace GunpointLevels.Common
{
    public class GunpointLevelInstallationProcess
    {
        private const string GunpointRegistryKeyPath = @"SOFTWARE\{0}Microsoft\Windows\CurrentVersion\Uninstall\Steam App 206190";
        private const string RegistryWow64Node = @"Wow6432Node\";
        private const string GunpointRegistryKeyName = "InstallLocation";

        public string OriginalFilePath { get; set; }
        public string TempFilePath { get; set; }
        public string InstalledFilePath { get; set; }


        private string GetGunpointInstallFolderFromRegistry()
        {
            string gunpointInstallFolder = null;
            var subkey = Registry.LocalMachine.OpenSubKey(string.Format(GunpointRegistryKeyPath, string.Empty));
            if (subkey != null)
            {
                gunpointInstallFolder = (string)subkey.GetValue(GunpointRegistryKeyName);
            }

            if (string.IsNullOrEmpty(gunpointInstallFolder))
            {
                subkey = Registry.LocalMachine.OpenSubKey(string.Format(GunpointRegistryKeyPath, RegistryWow64Node));
                if (subkey != null)
                {
                    gunpointInstallFolder = (string)subkey.GetValue(GunpointRegistryKeyName);
                }
            }
            return gunpointInstallFolder;
        }


    }
}
