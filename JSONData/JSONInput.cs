using System;
using System.Linq;
using System.Collections.Generic;

namespace JSONBossDialogue
{
    internal static class JSONInput
    {

        public static Dictionary<string, string> strPatch = new Dictionary<string, string>()
        {
            // KEY = Id, VALUE = String to replace it with
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

        // Custom JSON strings - ShowUntilInput
        public static string[] strDialogue2 = new string[4];

        // Custom JSON strings - ShowMessage
        public static string[] strDialogue3 = new string[1];

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

        public static void UnloadJSON()
        {
            // Unload custom dialogue lines from strPatch dictionary:
            ClearDictionary(strPatch);

            // Unload custom dialogue lines from strDialogue2:
            for(int i = 0; i < strDialogue2.Length; i++)
            {
                strDialogue2[i] = "";
            }

            // Unload custom dialogue lines from strDialogue3:
            for(int i = 0; i < strDialogue3.Length; i++)
            {
                strDialogue3[i] = "";
            }
        }

        private static void ClearDictionary(Dictionary<string, string> obj)
        {
            try
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    var item = obj.ElementAt(i);
                    string key = item.Key;
                    obj[key] = "";
                }
            }
            catch (Exception)
            {
                Plugin.myLogger.LogError("Exception caught: ClearDictionary method.");
                //throw;
            }
        }
    }
}