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

            // Prospector:
            { "ProspectorPreIntro", "" },
            { "ProspectorIntro", "" },
            { "ProspectorMuleKilled", "" },

            // Angler:
            { "AnglerPreIntro", "" },
            { "AnglerIntro", "" },
            { "TeachFishHookAimRandom", "" },
            { "TeachFishHookAimNew", "" },
            { "TeachFishHookPull", "" },

            // Trapper / Trader:
            { "TrapperTraderPreIntro", "" },
            { "TrapperTraderIntro", "" },
            { "TrapperTraderPrePhase2", "" },
            { "TrapperTraderPhase2", "" },
            { "TrapperTraderPreTrade", "" },
            { "TrapperTraderPostTrade", "" },

            // Leshy:
            { "LeshyBossIntro1", "" },
            { "LeshyBossAddCandle", "" },
            { "LeshyBossDeathcards1", "" },
            { "LeshyBossDeathcards2", "" },
            { "LeshyBossMoon1", "" },
            { "LeshyBossMoon2", "" },
            { "LeshyBossStinkyMoon", "" },

            // Royal:
            
            { "PirateSkullIntro1", "" }, // Leshy
            { "PirateSkullIntro2", "" }, // Royal wakes up
            { "PirateSkullIntro3", "" }, // Pre-intro
            { "PirateSkullIntro4", "" },
            { "PirateSkullAimCannons", "" },
            { "PirateSkullRodentPack", "" },
            { "PirateSkullPreCharge", "" }, // Limoncello
            { "PirateSkullPostCharge", "" }, // Leshy
            { "PirateSkullShipSpawned", "" }, // Royal again
            { "PirateSkullShipMutinee", "" },
            { "Part1CardsExhaustedShip", "" },
            { "PirateSkullShipDestroyed", "" },

        };

        // Custom JSON strings - ShowUntilInput
        public static string[] strDialogue2 = new string[6];

        // Custom JSON strings - ShowMessage
        public static string[] strDialogue3 = new string[1];

        // Custom JSON strings - ShowThenClear
        public static string[] strDialogue4 = new string[1];

        public static void LoadJSON(JSONHandler obj)
        {
            // Load custom dialogue lines into...
            
            // ======= strPatch Dictionary =======

            // Prospector
            strPatch["ProspectorPreIntro"] = obj.Prospector["PreIntro"];
            strPatch["ProspectorIntro"] = obj.Prospector["Intro"];
            strPatch["ProspectorMuleKilled"] = obj.Prospector["MuleKilled"];

            // Angler
            strPatch["AnglerPreIntro"] = obj.Angler["PreIntro"];
            strPatch["AnglerIntro"] = obj.Angler["Intro"];
            strPatch["TeachFishHookAimRandom"] = obj.Angler["AimingHook"];
            strPatch["TeachFishHookAimNew"] = obj.Angler["EasyChoose"];
            strPatch["TeachFishHookPull"] = obj.Angler["HookPull"];

            // Trapper / Trader
            strPatch["TrapperTraderPreIntro"] = obj.TrapperTrader["PreIntro"];
            strPatch["TrapperTraderIntro"] = obj.TrapperTrader["Intro"];
            strPatch["TrapperTraderPrePhase2"] = obj.TrapperTrader["PrePhase2"];
            strPatch["TrapperTraderPhase2"] = obj.TrapperTrader["Phase2"];
            strPatch["TrapperTraderPreTrade"] = obj.TrapperTrader["PreTrade"];
            strPatch["TrapperTraderPostTrade"] = obj.TrapperTrader["PostTrade"];

            // Leshy
            strPatch["LeshyBossIntro1"] = obj.Leshy["Intro"];
            strPatch["LeshyBossAddCandle"] = obj.Leshy["AddCandle"];
            strPatch["LeshyBossDeathcards1"] = obj.Leshy["Deathcards_Intro"];
            strPatch["LeshyBossDeathcards2"] = obj.Leshy["Deathcards_Outro"];
            strPatch["LeshyBossMoon1"] = obj.Leshy["PreMoon"];
            strPatch["LeshyBossMoon2"] = obj.Leshy["MoonPlayed"];
            strPatch["LeshyBossStinkyMoon"] = obj.Leshy["StinkyMoon"];

            // Royal (yep. A LOT.)
            strPatch["PirateSkullIntro1"] = obj.Royal["LeshyConfusion"];
            strPatch["PirateSkullIntro2"] = obj.Royal["WakeUp"];
            strPatch["PirateSkullIntro3"] = obj.Royal["PreIntro"];
            strPatch["PirateSkullIntro4"] = obj.Royal["Intro"];
            strPatch["PirateSkullAimCannons"] = obj.Royal["Cannons"];
            strPatch["PirateSkullRodentPack"] = obj.Royal["Rodents"];
            strPatch["PirateSkullPreCharge"] = obj.Royal["Limoncello_Charge"];
            strPatch["PirateSkullPostCharge"] = obj.Royal["LeshyStop"];
            strPatch["PirateSkullShipSpawned"] = obj.Royal["Limoncello_Intro"];
            strPatch["PirateSkullShipMutinee"] = obj.Royal["Limoncello_Mutinee"];
            strPatch["Part1CardsExhaustedShip"] = obj.Royal["Limoncello_NoCards"];
            strPatch["PirateSkullShipDestroyed"] = obj.Royal["Limoncello_Sunk"];


            // ======= strDialogue2 Array =======
            strDialogue2[0] = obj.Prospector["BeforePickaxe"];
            strDialogue2[1] = obj.Prospector["AfterPickaxe"];
            strDialogue2[2] = obj.Prospector["IfNoGold"];
            strDialogue2[3] = obj.Angler["GoFish"];
            strDialogue2[4] = obj.Royal["Defeated"];
            strDialogue2[5] = obj.Royal["Farewell"];

            // ======= strDialogue3 Array =======
            strDialogue3[0] = obj.TrapperTrader["Trade"];

            // ======= strDialogue4 Array =======
            strDialogue4[0] = obj.Royal["CannonFire"];
        }

        public static void UnloadJSON()
        {
            // Unload custom dialogue lines from strPatch dictionary:
            ClearDictionary(strPatch);

            // Unload custom dialogue lines from strDialogue2 array:
            for(int i = 0; i < strDialogue2.Length; i++)
            {
                strDialogue2[i] = "";
            }

            // Unload custom dialogue lines from strDialogue3 array:
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