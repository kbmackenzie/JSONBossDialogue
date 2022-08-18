using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using System.Collections;

namespace JSONBossDialogue
{
    public class ScreenHelper : MonoBehaviour
    {
        public DialogueSelectScreen screen;

        /*private void Start()
        {
            // Set default description.
            screen.descriptionStr = screen.pageStr;
        }*/

        private void Update()
        {
            // Update page description (for when the cursor exits an AscensionMenuInteractable object)!
            if (PatchButtons.isTextEmpty)
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

        public bool isLeft; // isActive; // I'm unsure if I'll need 'isActive'?

        private bool isSelected, coroutineStart;
        private float blink = DialogueSelectScreen.blinkTime;

        public SpriteRenderer theSR;

        private void Update()
        {
            if(isSelected)
            {
                if (!coroutineStart)
                {
                    StartCoroutine(BlinkAnimation(sprites["Arrow"], sprites["ArrowHover"]));
                    coroutineStart = true;
                }
            } else
            {
                if (coroutineStart)
                {
                    StopAllCoroutines();
                    coroutineStart = false;
                }

                theSR.sprite = sprites["Arrow"];
            }
        }

        // Cursor no longer hovering over icon
        public void OnMouseExit()
        {
            isSelected = false;

            // Clear changes to cursor
            Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
        }

        public void OnMouseEnter()
        {
            isSelected = true;
            CommandLineTextDisplayer.PlayCommandLineClickSound();

            // Change cursor to 'Point':
            Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Point);
        }

        public void OnMouseDown()
        {
            screen.UpdatePage(isLeft);
            CommandLineTextDisplayer.PlayCommandLineClickSound();
        }

        private IEnumerator BlinkAnimation(Sprite normal, Sprite hover)
        {
            while (isSelected)
            {
                theSR.sprite = hover;
                yield return new WaitForSeconds(blink);

                theSR.sprite = normal;
                yield return new WaitForSeconds(blink);
            }
        }
    }


    // Patch "Back" and "Continue" buttons so that the screen text doesn't disappear after they're hovered over:
    [HarmonyPatch]
    internal static class PatchButtons
    {
        public static bool isTextEmpty;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AscensionMenuInteractable), nameof(AscensionMenuInteractable.OnCursorExit))]
        static void PatchContinueButton()
        {
            isTextEmpty = true;
        }
    }
}