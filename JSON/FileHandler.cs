using BepInEx;
using System.IO;

namespace JSONBossDialogue
{
    public static class FileHandler
    {
        // Get directory. (Yes, I know this is very redundant. Bear with me.)
        public static string GetDirectory()
        {
            return Paths.PluginPath;
        }

        // Search directory. Method should take "GetDirectory()" as argument.
        public static string[] SearchDirectory(string dir)
        {
            return Directory.GetFiles(dir, "*_bd.json", SearchOption.AllDirectories);
        }

        // Takes SearchDirectory() as argument. Checks if array is empty.
        // If array is empty, then no "*_bd.json" files could be found.
        // No patches should be made then.
        public static bool isArrayEmpty(string[] array)
        {
            return array.Length == 0;
        }

        // Takes SearchDirectory() as argument. Checks if array has more than one item.
        // This means there's more than one "*_bd.json" file. That can't happen.
        // No changes should be made then, and an error should be printed to console.
        public static bool isArrayMany(string[] array)
        {
            return array.Length > 1;
        }

        // Read file. Method should take "SearchDirectory()" as argument.
        // Should only be called if "isArrayEmpty" is false.
        public static string ReadFile(string[] foo)
        {
            string ReadText = File.ReadAllText(foo[0]);
            return ReadText;
        }

        // Parse JSON to class. This concludes file management.
        // Method should take "ReadFile()" as argument.
        public static JSONHandler JSONLoadIntoObject(string jsontext)
        {
            return TinyJSON.JSON.Load(jsontext).Make<JSONHandler>();
        }
    }
}