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
            if (!FileHandler.isArrayEmpty(getFile))
            {
                // Repeat for every "_bd.json" file in getFile[]
                // This creates a list of JSONHandler objects!

                for(int i = 0; i < getFile.Length; i++)
                {
                    try
                    {
                        filename = Path.GetFileName(getFile[i]);

                        dialogueArray.Add(FileHandler.JSONLoadIntoObject(FileHandler.ReadFile(getFile, i)));

                        // No more JSON loading to be done here; the rest is for the screen to do!

                        Logger.LogInfo($"Loaded JSON string from file \"{filename}\" successfully!");
                    }
                    catch (Exception)
                    {
                        Logger.LogError($"Could not load JSON from \"{filename}\"! Please double check your file!");
                        //throw;
                    }
                }

            } else {
                Logger.LogError("Could not load custom dialogue: There's no \"_bd.json\" file in the \'plugins\' directory. Please make sure your file's name ends in \"_bd.json\".");
            }

        }
    }
}
