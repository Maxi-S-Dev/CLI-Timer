using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CLI_TImer.Helpers
{
    internal static class DataManager
    {
        internal static void SaveData(string data)
        {
            File.WriteAllText(Path.Combine(FileAccessHelper.MainDirectory(), "Profiles.json"), data);
        }

        internal static string LoadData(string data) 
        {

            return null;
        }
    }
}
