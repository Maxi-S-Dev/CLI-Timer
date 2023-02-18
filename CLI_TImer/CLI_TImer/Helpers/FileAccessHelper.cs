using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI_TImer.Helpers
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
