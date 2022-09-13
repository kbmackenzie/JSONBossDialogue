using BepInEx;
using System;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace JSONBossDialogue
{
    // HARMONY PATCHES - Prospector, Angler and Trapper/Trader dialogue events

    [HarmonyPatch]
    internal static class PatchDialogue
    {
        public static bool bossDialogue = false, getDialogue = false;

        public static string dialogueID;

        // Array of strings of dialogue (without ID) passed through the ShowUntilInput method:
        readonly static string[] bossDialogueStrings2 = {
            "THAR'S GOLD IN THEM CARDS!",
            "G-G-GOLD! I'VE STRUCK GOLD!",
            "N-... NO GOLD?",
            "Go fish.",
            "Avast ye!",
            "Farewell."
        };

        /* The JSON name for each of these is, respectively:
         * - Prospector -- BeforePickaxe
         * - Prospector -- AfterPickaxe
         * - Angler -- IfNoGold
         * - Angler -- GoFish
         * - Royal -- Goodbye1
         * - Royal -- Goodbye2
         */

        // Array of strings of dialogue (without ID) passed through the ShowMessage method:
        readonly static string[] bossDialogueStrings3 = {
            "Trade for what you can, but know this: the rest will stay and fight for me.", // Trade
        };

        // Array of strings of dialogue (without ID) passed through the ShowThenClear method:
        readonly static string[] bossDialogueStrings4 =
        {
            "FIRE!" // CannonFire
        };

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.PlayDialogueEvent))]
        static void PatchIDs(ref string eventId)
        {
            // If bossDialogue = True, player is in a boss fight.

            if (bossDialogue && JSONInput.strPatch.ContainsKey(eventId))
            {
                // eventId = "id" of dialogue data in dialogue_data file
                // If id is in this array, it can be replaced.

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


        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.ShowUntilInput))]
        static void PatchShowUntilInput(ref string message)
        {
            // message = String passed to be shown as dialogue.

            if (bossDialogue)
            {
                if (!getDialogue && bossDialogueStrings2.Contains(message))
                {
                    // Find index of string in array
                    int index = Array.IndexOf(bossDialogueStrings2, message);

                    // See if the index in the custom string array is empty.
                    bool isEmpty = JSONInput.strDialogue2[index].IsNullOrWhiteSpace();

                    // Patch dialogue.
                    message = isEmpty ? message : JSONInput.strDialogue2[index];

                }
                else if (getDialogue)
                {
                    // Patch dialogue.
                    message = JSONInput.strPatch[dialogueID];

                    getDialogue = false;
                }
            }
        }



        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.ShowMessage))]
        static void PatchShowMessage(ref string message)
        {
            // message = String passed to be shown as dialogue.

            if (bossDialogue && bossDialogueStrings3.Contains(message))
            {
                // Find index of string in array
                int index = Array.IndexOf(bossDialogueStrings3, message);

                // See if the index in the custom string array is empty.
                bool isEmpty = JSONInput.strDialogue3[index].IsNullOrWhiteSpace();

                // Patch dialogue.
                message = isEmpty ? message : JSONInput.strDialogue3[index];
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.ShowThenClear))]
        static void PatchShowThenClear(ref string message)
        {
            // message = String passed to be shown as dialogue.

            if (bossDialogue && bossDialogueStrings4.Contains(message))
            {
                // Find index of string in array
                int index = Array.IndexOf(bossDialogueStrings4, message);

                // See if the index in the custom string array is empty.
                bool isEmpty = JSONInput.strDialogue4[index].IsNullOrWhiteSpace();

                // Patch dialogue.
                message = isEmpty ? message : JSONInput.strDialogue4[index];
            }
        }
    }
}