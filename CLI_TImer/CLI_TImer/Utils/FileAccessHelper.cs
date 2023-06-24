using System;
using System.IO;

namespace CLI_TImer.Utils
{
    internal static class FileAccessHelper
    {
        internal static string MainDirectory()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CLI-Timer/SaveData");

            if(!File.Exists(path)) 
            { 
                Directory.CreateDirectory(path);
            }   

            return path;
        }
    }
}
