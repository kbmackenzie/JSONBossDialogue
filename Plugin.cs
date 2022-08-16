using BepInEx;
using System.IO;
using HarmonyLib;
using System;
using InscryptionAPI.Ascension;
using BepInEx.Logging;
using System.Collections.Generic;

namespace JSONBossDialogue
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "Kel.inscryption.jsonbossdialogue";
        private const string PluginName = "JSONBossDialogue";
        private const string PluginVersion = "1.0.0.0";

        internal static ManualLogSource myLogger; // Log source.
        public static string dialogueMod
        {
            get { return PluginGuid; }
        }

        // LOAD JSON - Variables
        public static string directory, filename;
        public static string[] getFile;

        // DIALOGUE LIST -- Arrays don't work because I don't know how many items I'll have.
        public static List<JSONHandler> dialogueArray = new List<JSONHandler>();

        private void Awake()
        {
            myLogger = Logger;

            Logger.LogInfo($"Loaded {PluginName} successfully!");

            Harmony harmony = new Harmony("kel.harmony.jsonbossdialogue");
            harmony.PatchAll();

            // REGISTER SCREEN
            AscensionScreenManager.RegisterScreen<DialogueSelectScreen>();

            // LOAD JSON
            directory = FileHandler.GetDirectory();
            getFile = FileHandler.SearchDirectory(directory);

            // CREATE JSON DIALOGUE ARRAY
            // JSONHandler dialogueObj;

            if (!FileHandler.isArrayEmpty(getFile))
            {
                // Repeat for every item in array, 
                // creating JSONHandler array for each '_bd.json' file in getFile[].

                for(int i = 0; i < getFile.Length; i++)
                {
                    try
                    {
                        filename = Path.GetFileName(getFile[i]);

                        dialogueArray.Add(FileHandler.JSONLoadIntoObject(FileHandler.ReadFile(getFile, i)));

                        // No loading JSON here; that's for the screen to do!
                        // The screen will do something like this:
                        // JSONInput.LoadJSON(dialogueArray[i]);

                        Logger.LogInfo($"Loaded JSON string from file \"{filename}\" successfully!");
                    }
                    catch (Exception)
                    {
                        Logger.LogError($"Could not load JSON from \"{filename}\"! Please double check your file!");
                        //throw;
                    }
                }

                /*if (!FileHandler.isArrayMany(getFile)) {
                } else
                {
                    // Plans: Add KCM screen that lets you choose a dialogue file.
                    // Screen will display each file's name except for the "_bd.json" part. :D 
                    // So test_bd.json would be displayed as "test".

                    // When I do implenet it, I will comment out this error and log a warning or regular message in the console instead simply making it clear what happened.

                    Logger.LogError("Could not load custom dialogue: There's more than one \"_bd.json\" file in the plugin directory." + Environment.NewLine + "Please choose one custom dialogue file to keep in the plugin folder.");
                }*/
            } else {
                Logger.LogError("Could not load custom dialogue: There's no \"_bd.json\" file in the \'plugins\' directory. Please make sure your file's name ends in \"_bd.json\".");
            }

        }
    }
}
