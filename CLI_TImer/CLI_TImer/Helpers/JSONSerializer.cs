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
            string JsonString = JsonSerializer.Serialize(list);

            return JsonString;
        }

        internal static List<Profile> JSONToList(string jsonString)
        {
            List<Profile>? list = JsonSerializer.Deserialize<List<Profile>>(jsonString);

            return list;
        }
    }
}
