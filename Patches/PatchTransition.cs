using BepInEx;
using System;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Saves;

namespace JSONBossDialogue
{
    [HarmonyPatch]
    internal static class PatchTransition
    {
        public static JSONHandler chosenDialogue;

        private static string dialogueSave // Syncs up to save.
        {
            get { return ModdedSaveManager.SaveData.GetValue(Plugin.dialogueMod, "DialogueData"); }
            set { ModdedSaveManager.SaveData.SetValue(Plugin.dialogueMod, "DialogueData", value); }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AscensionMenuScreens), nameof(AscensionMenuScreens.TransitionToGame))]
        static void PatchStartRun(ref bool newRun)
        {
            JSONInput.UnloadJSON(); // This cleans up any leftover JSON data, just in case.

            if (!newRun) // If not a new run, load dialogue data from save file
            {
                if (dialogueSave.IsNullOrWhiteSpace()) // If true, no JSON data exists in the save file
                {
                    Plugin.myLogger.LogInfo("No custom dialogue loaded.");
                    return; // Don't do anything else.
                }

                try
                {
                    // FileLog.Log(dialogueSave);

                    // Load data from save
                    JSONHandler obj = FileHandler.JSONLoadIntoObject(dialogueSave);
                    string fileName = obj.FileName;
                    Plugin.myLogger.LogInfo("Dialogue JSON string loaded!");
                    Plugin.myLogger.LogInfo($"Name: {fileName}");

                    JSONInput.LoadJSON(obj);
                    Plugin.myLogger.LogInfo("Custom dialogue loaded successfully!");
                }
                catch (Exception)
                {
                    Plugin.myLogger.LogError("Couldn't load dialogue from JSON string! No dialogue will be loaded.");

                    JSONInput.UnloadJSON(); // Make sure no weird glitched dialogue will be loaded!
                    // throw;
                }

                return;
            }

            // If a new run, do this:

            if (chosenDialogue != null)
            {
                bool nameEmpty = chosenDialogue.FileName.IsNullOrWhiteSpace();
                string qtm = "\"";

                string fileName = nameEmpty ? "No name found!" : qtm + chosenDialogue.FileName + qtm;
                Plugin.myLogger.LogInfo("Loading custom dialogue from JSON string.");
                Plugin.myLogger.LogInfo($"Name: {fileName}");

                try
                {
                    JSONInput.LoadJSON(chosenDialogue);
                }
                catch (Exception)
                {
                    Plugin.myLogger.LogError("There was an error while loading dialogue from JSON.");
                    Plugin.myLogger.LogError($"Name: {fileName}");
                    Plugin.myLogger.LogError("Please double-check that file or choose a different one. No custom dialogue will be loaded in this run.");

                    return; // Leave before saving any of the JSON data to the modded savefile.
                }

                // After this, save JSON dialogue data to save file.
                string JSONdata = FileHandler.JSONWriteAsString(chosenDialogue);
                dialogueSave = JSONdata;

            } else
            {
                Plugin.myLogger.LogInfo("No custom dialogue loaded.");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AscensionSaveData), nameof(AscensionSaveData.EndRun))]
        static void PatchEndRun()
        {
            chosenDialogue = null;
            JSONInput.UnloadJSON();

            dialogueSave = "";
        }
    }
}