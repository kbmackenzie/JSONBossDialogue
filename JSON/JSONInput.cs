using System;

namespace JSONBossDialogue
{
    public static class JSONInput
    {
        // Custom JSON strings - IDs
        public static string[] strDialogue = new string[11];

        // Custom JSON strings - ShowUntilInput
        public static string[] strDialogue2 = new string[4];

        // Load JSON
        public static void LoadJSON(JSONHandler obj)
        {
            // Load custom dialogue lines into strDialogue:
            strDialogue[0] = obj.Prospector["PreIntro"];
            strDialogue[1] = obj.Prospector["Intro"];
            strDialogue[2] = obj.Angler["PreIntro"];
            strDialogue[3] = obj.Angler["Intro"];
            strDialogue[4] = obj.TrapperTrader["PreIntro"];
            strDialogue[5] = obj.TrapperTrader["Intro"];
            strDialogue[6] = obj.TrapperTrader["PrePhase2"];
            strDialogue[7] = obj.TrapperTrader["Phase2"];
            strDialogue[8] = obj.Angler["AimingHook"];
            strDialogue[9] = obj.Angler["EasyChoose"];
            strDialogue[10] = obj.Angler["HookPull"];

            // Load custom dialogue lines into strDialogue2:
            strDialogue2[0] = obj.Prospector["BeforePickaxe"];
            strDialogue2[1] = obj.Prospector["AfterPickaxe"];
            strDialogue2[2] = obj.Prospector["IfNoGold"];
            strDialogue2[3] = obj.Angler["GoFish"];
        }
    }
}