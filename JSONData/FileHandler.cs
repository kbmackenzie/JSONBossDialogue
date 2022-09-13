using BepInEx;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace JSONBossDialogue
{
    internal static class FileHandler
    {
        // Search directory. Method should take "GetDirectory()" as argument.
        public static string[] SearchDirectory(string dir)
        {
            return Directory.GetFiles(dir, "*_bd.json", SearchOption.AllDirectories);
        }

        // Read file. Method should take "SearchDirectory()" as argument.
        public static string ReadFile(string[] foo, int a = 0)
        {
            string ReadText = File.ReadAllText(foo[a]);
            return ReadText;
        }

        // Parse JSON to class. Takes ReadFile() as argument.
        public static JSONHandler JSONLoadIntoObject(string jsontext)
        {
            return JsonConvert.DeserializeObject<JSONHandler>(jsontext);
        }

        // Parse JSONHandler to string. This is for saving the JSON data in the ModdedSaveFile.
        // Indentation is optional and I'm only including it in case it's useful later.
        public static string JSONWriteAsString(JSONHandler obj, bool indentation = false)
        {
            if(indentation)
            {
                return JsonConvert.SerializeObject(obj, Formatting.Indented);
            }

            return JsonConvert.SerializeObject(obj);
        }
    }
}