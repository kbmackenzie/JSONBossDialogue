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
        [HarmonyPatch(typeof(LeshyBossOpponent), nameof(LeshyBossOpponent.IntroSequence))]
        static void PatchLeshy()
        {
            PatchDialogue.bossDialogue = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PirateSkullBossOpponent), nameof(PirateSkullBossOpponent.IntroSequence))]
        static void PatchRoyal()
        {
            PatchDialogue.bossDialogue = true;
            PatchDialogue.isRoyal = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.BossDefeatedSequence))]
        static void Prefix()
        {
            PatchDialogue.bossDialogue = false;
            PatchDialogue.isRoyal = false;
        }
    }
}