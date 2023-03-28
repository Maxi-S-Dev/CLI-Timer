using CLI_TImer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CLI_TImer.Helpers
{
    internal static class JSONSerializer
    {
        internal static string ListToJSON(List<Profile> list)
        {
            string jsonString = JsonSerializer.Serialize(list);

            return jsonString;
        }

        internal static List<Profile> JSONToList(string jsonString)
        {
            List<Profile>? list = JsonSerializer.Deserialize<List<Profile>>(jsonString);

            return list;
        }

        internal static string SettingsToJSON(Settings settings) 
        {
            string jsonString = JsonSerializer.Serialize(settings);
            return jsonString;
        }

        internal static AppData? JSONToData(string jsonString) 
        { 
            AppData data = JsonSerializer.Deserialize<AppData>(jsonString);

            return data;
        }
    }
}
