using System;
using System.IO;
using System.Net;

namespace Gunpoint.Common
{
    public static class LevelsUtils
    {
        /// <summary>
        /// Extension of a Gunpoint level file : .lvl
        /// Based on http://gunpointwiki.net/wiki/Level_editor
        /// </summary>
        public const string FileExtension = ".lvl";

        /// <summary>
        /// Check if the specified file is a valid Gunpoint level file
        /// </summary>
        /// <param name="levelFilePath"></param>
        /// <exception cref="System.ArgumentNullException">levelFilePath is null or empty.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="System.IO.IOException">levelFilePath includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
        public static void CheckGunpointLevelFile(string levelFilePath)
        {
            if (string.IsNullOrEmpty(levelFilePath))
            {
                throw new ArgumentNullException("levelFilePath");
            }

            CheckLvlExtension(levelFilePath);

            Uri uri;
            if(Uri.TryCreate(levelFilePath, UriKind.Absolute, out uri))
            {
                levelFilePath = DownloadFile(uri);
            }

            using (StreamReader reader = new StreamReader(levelFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int lineValue;
                    if (!Int32.TryParse(line, out lineValue))
                    {
                        throw new InvalidGunpointLevelFileException("Not a valid gunpoint level file: it should contains only numbers.");
                    }
                }
            }
        }

        private static string DownloadFile(Uri url)
        {
            string tempFilePath = Path.GetTempFileName();
            WebClient wc = new WebClient();
            wc.DownloadFile(url, tempFilePath);
            return tempFilePath;
        }

        private static void CheckLvlExtension(string levelFilePath)
        {
            if (Path.GetExtension(levelFilePath) != FileExtension)
            {
                throw new InvalidGunpointLevelFileException("Not a .lvl file");
            }
        }
    }
}
