using System;
using System.Collections.Generic;

namespace JSONBossDialogue
{
    public static class JSONInput
    {

        public static Dictionary<string, string> strPatch = new Dictionary<string, string>()
        {
            // TKEY = Id, TVALUE = String to replace it with
            // If I'm changing to this I have to pass the eventId from PatchDialogue to ShowUntilInput btw
            { "ProspectorPreIntro", "" },
            { "ProspectorIntro", "" },
            { "ProspectorMuleKilled", "" },
            { "AnglerPreIntro", "" },
            { "AnglerIntro", "" },
            { "TeachFishHookAimRandom", "" },
            { "TeachFishHookAimNew", "" },
            { "TeachFishHookPull", "" },
            { "TrapperTraderPreIntro", "" },
            { "TrapperTraderIntro", "" },
            { "TrapperTraderPrePhase2", "" },
            { "TrapperTraderPhase2", "" },
            { "TrapperTraderPreTrade", "" },
            { "TrapperTraderPostTrade", "" }
        };

        // Custom JSON strings - IDs - Array no longer needed.
        // public static string[] strDialogue = new string[14];

        // Custom JSON strings - ShowUntilInput
        public static string[] strDialogue2 = new string[4];

        // Custom JSON strings - ShowMessage
        public static string[] strDialogue3 = new string[1];

        // Load JSON
        public static void LoadJSON(JSONHandler obj)
        {
            // Load custom dialogue lines into strPatch dictionary:
            strPatch["ProspectorPreIntro"] = obj.Prospector["PreIntro"];
            strPatch["ProspectorIntro"] = obj.Prospector["Intro"];
            strPatch["ProspectorMuleKilled"] = obj.Prospector["MuleKilled"];
            strPatch["AnglerPreIntro"] = obj.Angler["PreIntro"];
            strPatch["AnglerIntro"] = obj.Angler["Intro"];
            strPatch["TeachFishHookAimRandom"] = obj.Angler["AimingHook"];
            strPatch["TeachFishHookAimNew"] = obj.Angler["EasyChoose"];
            strPatch["TeachFishHookPull"] = obj.Angler["HookPull"];
            strPatch["TrapperTraderPreIntro"] = obj.TrapperTrader["PreIntro"];
            strPatch["TrapperTraderIntro"] = obj.TrapperTrader["Intro"];
            strPatch["TrapperTraderPrePhase2"] = obj.TrapperTrader["PrePhase2"];
            strPatch["TrapperTraderPhase2"] = obj.TrapperTrader["Phase2"];
            strPatch["TrapperTraderPreTrade"] = obj.TrapperTrader["PreTrade"];
            strPatch["TrapperTraderPostTrade"] = obj.TrapperTrader["PostTrade"];

            // Load custom dialogue lines into strDialogue2:
            strDialogue2[0] = obj.Prospector["BeforePickaxe"];
            strDialogue2[1] = obj.Prospector["AfterPickaxe"];
            strDialogue2[2] = obj.Prospector["IfNoGold"];
            strDialogue2[3] = obj.Angler["GoFish"];

            // Loads custom dialogue lines into strDialogue3:
            strDialogue3[0] = obj.TrapperTrader["Trade"];
        }
    }
}