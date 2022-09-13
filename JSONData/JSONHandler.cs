using System;
using System.Collections.Generic;

namespace JSONBossDialogue
{
    [Serializable]
    public class JSONHandler
    {
        public string FileName = "", Description = "";

        public Dictionary<string, string> Prospector = new Dictionary<string, string>()
        {
            { "PreIntro", "" }, { "Intro", "" }, { "BeforePickaxe", "" },
            { "AfterPickaxe", "" }, { "IfNoGold", "" }, { "MuleKilled", "" }
        };

        public Dictionary<string, string> Angler = new Dictionary<string, string>()
        {
            { "PreIntro", "" }, { "Intro", "" }, { "GoFish", "" },
            { "AimingHook", "" }, { "EasyChoose", "" }, { "HookPull", "" }
        };

        public Dictionary<string, string> TrapperTrader = new Dictionary<string, string>()
        {
            { "PreIntro", "" }, { "Intro", "" },
            { "PrePhase2", "" }, { "Phase2", "" },
            { "PreTrade", "" }, { "Trade", "" }, { "PostTrade", "" }
        };

        public Dictionary<string, string> Leshy = new Dictionary<string, string>()
        {
            { "Intro", "" }, { "AddCandle", "" },
            { "Deathcards_Intro", "" }, { "Deathcards_Outro", "" },
            { "PreMoon", "" }, { "MoonPlayed", "" }, { "StinkyMoon", "" },
        };

        public Dictionary<string, string> Royal = new Dictionary<string, string>()
        {
            { "LeshyConfusion", "" }, { "WakeUp", "" },
            { "PreIntro", "" }, { "Intro", "" },
            { "Cannons", "" }, { "CannonFire", "" }, { "Rodents", "" },
            { "Limoncello_Charge", "" }, { "LeshyStop", "" }, { "Limoncello_Intro", "" },
            { "Limoncello_Mutinee", "" }, { "Limoncello_NoCards", "" },
            { "Limoncello_Sunk", "" }, { "Defeated", "" }, { "Farewell", "" },
        };
    }
}