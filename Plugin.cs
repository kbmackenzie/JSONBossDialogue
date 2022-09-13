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
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "Kel.inscryption.jsonbossdialogue";
        private const string PluginName = "JSONBossDialogue";
        private const string PluginVersion = "1.1.0";

        internal static ManualLogSource myLogger; // Log source.
        public static string dialogueMod
        {
            get { return PluginGuid; }
        }

        // LOAD FILES -- Array
        public static string[] getFile;

        // DIALOGUE LIST -- Dialogue files
        public static List<JSONHandler> dialogueInstances = new List<JSONHandler>();

        private void Awake()
        {
            myLogger = Logger;

            Logger.LogInfo($"Loaded {PluginName} successfully!");

            Harmony harmony = new Harmony("kel.harmony.jsonbossdialogue");
            harmony.PatchAll();

            // REGISTER SCREEN
            AscensionScreenManager.RegisterScreen<DialogueSelectScreen>();

            // LOAD JSON FILES
            getFile = FileHandler.SearchDirectory(Paths.PluginPath);

            // CREATE DIALOGUE LIST
            if (getFile.Length > 0)
            {
                // Repeat for every "_bd.json" file in getFile[]

                for(int i = 0; i < getFile.Length; i++)
                {
                    string filename = Path.GetFileName(getFile[i]);

                    try
                    {
                        dialogueInstances.Add(FileHandler.JSONLoadIntoObject(FileHandler.ReadFile(getFile, i)));

                        Logger.LogInfo($"Loaded JSON string from file \"{filename}\" successfully!");
                    }
                    catch (Exception)
                    {
                        Logger.LogError($"Could not load JSON from \"{filename}\"! Please double check your file!");
                        //throw;
                    }
                }

            } else {
                Logger.LogWarning("No custom dialogue files found in the \'plugins\' directory!");
            }

        }
    }
}
