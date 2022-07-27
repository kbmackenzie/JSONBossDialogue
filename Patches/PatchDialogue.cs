using BepInEx;
using System;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace JSONBossDialogue
{
    // HARMONY PATCHES - Prospector, Angler and Trapper/Trader dialogue events

    [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.PlayDialogueEvent))]
    public static class PatchDialogue
    {
        public static bool bossDialogue = false;

        public static bool getDialogue = false;

        // String array with all the eventIds of the dialogue for the bosses. 
        public static string[] bossDialogueIDs = {
            "ProspectorPreIntro", // PreIntro
            "ProspectorIntro", // Intro
            "AnglerPreIntro", // PreIntro
            "AnglerIntro", // Intro
            "TrapperTraderPreIntro", // PreIntro
            "TrapperTraderIntro", // Intro
            "TrapperTraderPrePhase2", // PrePhase2
            "TrapperTraderPhase2", // Phase2
            "TeachFishHookAimRandom", // AimHookRandom
            "TeachFishHookAimNew", // AimHookNew
            "TeachFishHookPull", // AimHookPull
            "TrapperTraderPreTrade", //PreTrade
            "TrapperTraderPostTrade" //PostTrade
        };

        public static int arrayIndex;

        static void Prefix(ref string eventId)
        {
            // If bossDialogue = True, player is in a boss fight.

            // eventId = "id" of dialogue data in dialogue_data file
            // If id is in this array, it can be replaced.
            bool stringInArray = bossDialogueIDs.Contains(eventId);

            // If in boss battle and eventId string is in the dialogue array:
            if (bossDialogue && stringInArray)
            {
                // Find index of string item in ID array
                arrayIndex = Array.IndexOf(bossDialogueIDs, eventId);

                // See if matching index of custom dialogue array is empty.
                bool isEmpty = JSONInput.strDialogue[arrayIndex].IsNullOrWhiteSpace();

                // If not, fetch dialogue
                getDialogue = !isEmpty;

                eventId = "Hint_CantSacrificeTerrain";
                // ^ All this does is ensure dialogue is kept to a single line.
                // Don't worry too much about it.
            }
        }
    }

    [HarmonyPatch(typeof(TextDisplayer), nameof(TextDisplayer.ShowUntilInput))]
    public static class PatchShowUntilInput
    {
        // Patch dialogue shown.

        // Array of strings of dialogue without ID, passed directly through ShowUntilInput method:
        public static string[] bossDialogueStrings = {
            "THAR'S GOLD IN THEM CARDS!", // BeforePickaxe
            "G-G-GOLD! I'VE STRUCK GOLD!", // AfterPickaxe
            "N-... NO GOLD?", // IfNoGold
            "Go fish.", // GoFish
            "Trade for what you can, but know this: the rest will stay and fight for me." //Trade
        };

        // DIALOGUE PATCH - STRINGS
        static void Prefix(ref string message)
        {
            // message = String passed to be shown as dialogue.

            bool strInArray2 = bossDialogueStrings.Contains(message);
            // ^ This is for patching the dialogue strings that don't have IDs.

            if (PatchDialogue.bossDialogue)
            {
                if (strInArray2)
                {
                    // Find index of string in array
                    int index = Array.IndexOf(bossDialogueStrings, message);

                    // See if the index in the custom string array is empty.
                    bool isEmpty = JSONInput.strDialogue2[index].IsNullOrWhiteSpace();

                    // Patch dialogue.
                    message = isEmpty ? message : JSONInput.strDialogue2[index];

                }
                else if (PatchDialogue.getDialogue)
                {
                    // Patch dialogue.
                    message = JSONInput.strDialogue[PatchDialogue.arrayIndex];
                }
            }
        }
    }
}