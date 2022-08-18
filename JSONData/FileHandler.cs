using BepInEx;
using System.IO;
using Newtonsoft.Json;

namespace JSONBossDialogue
{
    internal static class FileHandler
    {
        // Get directory. (Yes, I know this is very redundant. Bear with me!)
        public static string GetDirectory()
        {
            return Paths.PluginPath;
        }

        // Search directory. Method should take "GetDirectory()" as argument.
        public static string[] SearchDirectory(string dir)
        {
            return Directory.GetFiles(dir, "*_bd.json", SearchOption.AllDirectories);
        }

        public static bool isArrayEmpty(string[] array)
        {
            return array.Length == 0;
        }

        // Checks if array has more than one item. (Not useful anymore.)
        public static bool isArrayMany(string[] array)
        {
            return array.Length > 1;
        }

        // Read file. Method should take "SearchDirectory()" as argument.
        // Should only be called if "isArrayEmpty" is false.
        public static string ReadFile(string[] foo, int a = 0)
        {
            string ReadText = File.ReadAllText(foo[a]);
            return ReadText;
        }

        // Parse JSON to class. This concludes file management.
        // Method should take "ReadFile()" as argument.
        public static JSONHandler JSONLoadIntoObject(string jsontext)
        {
            return JsonConvert.DeserializeObject<JSONHandler>(jsontext);
        }

        // Parse JSON to string. This is for saving the JSON data in the ModdedSaveFile.
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