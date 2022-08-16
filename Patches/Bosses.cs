using DiskCardGame;
using HarmonyLib;

namespace JSONBossDialogue
{
    // HARMONY PATCHES - Prospector, Angler and Trapper/Trader dialogue events

    [HarmonyPatch]
    internal class BossPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TrapperTraderBossOpponent), nameof(TrapperTraderBossOpponent.IntroSequence))]
        static void PatchTrader()
        {
            PatchDialogue.bossDialogue = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AnglerBossOpponent), nameof(AnglerBossOpponent.IntroSequence))]
        static void PatchAngler()
        {
            PatchDialogue.bossDialogue = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ProspectorBossOpponent), nameof(ProspectorBossOpponent.IntroSequence))]
        static void PatchProspector()
        {
            PatchDialogue.bossDialogue = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.BossDefeatedSequence))]
        static void Prefix()
        {
            PatchDialogue.bossDialogue = false;
        }
    }

    /*
    [HarmonyPatch(typeof(TrapperTraderBossOpponent), nameof(TrapperTraderBossOpponent.IntroSequence))]
    public static class PatchTrader
    {
        static void Prefix()
        {
            PatchDialogue.bossDialogue = true;
        }
    }

    [HarmonyPatch(typeof(AnglerBossOpponent), nameof(AnglerBossOpponent.IntroSequence))]
    public static class PatchAngler
    {
        static void Prefix()
        {
            PatchDialogue.bossDialogue = true;
        }
    }

    [HarmonyPatch(typeof(ProspectorBossOpponent), nameof(ProspectorBossOpponent.IntroSequence))]
    public static class PatchProspector
    {
        static void Prefix()
        {
            PatchDialogue.bossDialogue = true;
        }
    }

    [HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.BossDefeatedSequence))]
    public static class PatchDefeat
    {
        static void Prefix()
        {
            PatchDialogue.bossDialogue = false;
        }
    }*/
}