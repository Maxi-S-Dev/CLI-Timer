using CLI_TImer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        internal static string DataToJSON(AppData data) 
        {
            string jsonString = JsonSerializer.Serialize(data.profileList);
            jsonString += "&&";
            jsonString += JsonSerializer.Serialize(data.settings);
            jsonString += "&&";
            jsonString += JsonSerializer.Serialize(data.gradientList);
            return jsonString;
        }

        internal static AppData JSONToData(string jsonString) 
        { 
            AppData data = new();
            string dataString = jsonString;

            data.profileList = JsonSerializer.Deserialize<List<Profile>>(dataString.Split("&&")[0]);
            data.settings = JsonSerializer.Deserialize<Settings>(dataString.Split("&&")[1]);
            data.gradientList = JsonSerializer.Deserialize<List<Gradient>>(dataString.Split("&&")[2]);

            return data;
        }
    }
}
