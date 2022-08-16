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
            { "PreIntro", "" }, { "Intro", "" },
            { "BeforePickaxe", "" }, { "AfterPickaxe", "" },
            { "IfNoGold", "" }, { "MuleKilled", "" }
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
    }
}