using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using InscryptionAPI.Ascension;

namespace JSONBossDialogue
{
    public class ScreenHelper : MonoBehaviour
    {
        public DialogueSelectScreen screen;

        private void Update()
        {
            if(PatchButtons.isTextEmpty)
            {
                screen.SetDescription();
                PatchButtons.isTextEmpty = false;
            }
        }
    }

    public class DialogueArrows : MonoBehaviour
    {
        public DialogueSelectScreen screen;
        public GameObject arrow;

        private Dictionary<string, Sprite> sprites = DialogueSelectScreen.dialogueSprites;

        public bool isLeft, isActive; // I'm unsure if I'll need 'isActive'?

        private bool isSelected;

        public SpriteRenderer theSR;

        private void Update()
        {
            theSR.sprite = isSelected ? sprites["ArrowHover"] : sprites["Arrow"];
        }

        // Cursor no longer hovering over icon
        public void OnMouseExit()
        {
            isSelected = false;
        }

        public void OnMouseEnter()
        {
            isSelected = true;
            CommandLineTextDisplayer.PlayCommandLineClickSound();
        }

        public void OnMouseDown()
        {
            screen.UpdatePage(isLeft);
            CommandLineTextDisplayer.PlayCommandLineClickSound();
        }
    }


    // Patch "Back" and "Continue" buttons so that the screen text doesn't disappear after they're hovered over:
    [HarmonyPatch]
    internal static class PatchButtons
    {
        /*[HarmonyPostfix]
        [HarmonyPatch(typeof(AscensionMenuBackButton), nameof(AscensionMenuBackButton.OnCursorExit))]
        static void PatchBackButton()
        {

        }*/ // Redundant.

        public static bool isTextEmpty;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AscensionMenuInteractable), nameof(AscensionMenuInteractable.OnCursorExit))]
        static void PatchContinueButton()
        {
            isTextEmpty = true;
        }
    }
}