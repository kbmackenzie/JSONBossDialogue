using BepInEx;
using System;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
//using System.Collections.Generic;

namespace JSONBossDialogue
{
    // HARMONY PATCHES - Prospector, Angler and Trapper/Trader dialogue events

    [HarmonyPatch]
    internal static class PatchDialogue
    {
        public static bool bossDialogue = false, getDialogue = false;

        public static string dialogueID;

        // Array of strings of dialogue (without ID) passed through the ShowUntilInput method:
        readonly static string[] bossDialogueStrings = {
            "THAR'S GOLD IN THEM CARDS!", // BeforePickaxe
            "G-G-GOLD! I'VE STRUCK GOLD!", // AfterPickaxe
            "N-... NO GOLD?", // IfNoGold
            "Go fish." // GoFish
        };

        // Array of strings of dialogue (without ID) passed through the ShowMessage method:
        readonly static string[] bossDialogueStrings2 = {
            "Trade for what you can, but know this: the rest will stay and fight for me." // Trade 
        };

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.PlayDialogueEvent))]
        static void PatchIDs(ref string eventId)
        {
            // If bossDialogue = True, player is in a boss fight.

            // If in boss battle:
            if (bossDialogue)
            {
                // eventId = "id" of dialogue data in dialogue_data file
                // If id is in this array, it can be replaced.
                bool stringInArray = JSONInput.strPatch.ContainsKey(eventId);

                // If 'id' string is in array:
                if (stringInArray)
                {
                    // See if matching key has empty/null value in strPatch dictionary:
                    bool isEmpty = JSONInput.strPatch[eventId].IsNullOrWhiteSpace();

                    // If not, fetch dialogue
                    getDialogue = !isEmpty;

                    // Store dialogue ID in this string variable for use in ShowUntilInput
                    dialogueID = eventId;

                    eventId = getDialogue ? "Hint_CantSacrificeTerrain" : eventId;
                    // ^ All this does is ensure dialogue is kept to a single line.
                    // Don't worry too much about it.
                }
            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.ShowUntilInput))]
        static void PatchShowUntilInput(ref string message)
        {
            // message = String passed to be shown as dialogue.

            if (bossDialogue)
            {
                bool strInArray2 = bossDialogueStrings.Contains(message);
                // ^ This is for patching the dialogue strings that don't have IDs.

                if (strInArray2)
                {
                    // Find index of string in array
                    int index = Array.IndexOf(bossDialogueStrings, message);

                    // See if the index in the custom string array is empty.
                    bool isEmpty = JSONInput.strDialogue2[index].IsNullOrWhiteSpace();

                    // Patch dialogue.
                    message = isEmpty ? message : JSONInput.strDialogue2[index];

                }
                else if (getDialogue)
                {
                    // Patch dialogue.
                    message = JSONInput.strPatch[dialogueID];
                }
            }
        }



        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.ShowMessage))]
        static void PatchShowMessage(ref string message)
        {
            // message = String passed to be shown as dialogue.

            if (bossDialogue)
            {
                bool strInArray3 = bossDialogueStrings2.Contains(message);
                // ^ This is for patching the dialogue strings that don't have IDs.

                if (strInArray3)
                {
                    // Find index of string in array
                    int index = Array.IndexOf(bossDialogueStrings2, message);

                    // See if the index in the custom string array is empty.
                    bool isEmpty = JSONInput.strDialogue3[index].IsNullOrWhiteSpace();

                    // Patch dialogue.
                    message = isEmpty ? message : JSONInput.strDialogue3[index];

                }
            }
        }

    }
}