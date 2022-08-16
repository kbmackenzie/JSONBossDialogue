using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace JSONBossDialogue
{
    public class DialogueIcon : MonoBehaviour
    {
        public bool isSelected, isChosen;
        public SpriteRenderer theSR;

        public DialogueSelectScreen screen;

        public int iconID; // Dialogue index! Corresponds to an index of the dialogueArray list! c:

        public bool clickable = false;
        // Whether this dialogue icon is clickable or not (if it maps to an existing index).

        private bool coroutineStart = false;
        private float blink = DialogueSelectScreen.blinkTime;

        public Dictionary<string, Sprite> sprites = DialogueSelectScreen.dialogueSprites;

        private void Update()
        {
            // Consider moving all of this to the MouseEnter / MouseExit / MouseDown functions!

            if(!clickable)
            {
                if (theSR.color.a > 0)
                {
                    theSR.color = new Color(theSR.color.r, theSR.color.b, theSR.color.g, 0.5f);
                }

                return;
            }

            // ... I'm very sorry for the 'if' tree. I'm sorry.
            if (isSelected)
            {
                if (isChosen)
                {
                    if (!coroutineStart)
                    {
                        StartCoroutine(BlinkAnimation(sprites["Chosen"], sprites["ChosenHover"]));
                        coroutineStart = true;
                    }
                } else
                {
                    if (!coroutineStart)
                    {
                        StartCoroutine(BlinkAnimation(sprites["Off"], sprites["Hover"]));
                        coroutineStart = true;
                    }
                }
            } else
            {
                if (coroutineStart)
                {
                    StopAllCoroutines();
                    coroutineStart = false;
                }

                if (isChosen)
                {
                    theSR.sprite = sprites["Chosen"];
                } else
                {
                    theSR.sprite = sprites["Off"];
                }
            }

            if (theSR.color.a < 1)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.b, theSR.color.g, 1f);
            }
        }

        // Cursor no longer hovering over icon
        public void OnMouseExit()
        {
            if (clickable)
            {
                isSelected = false;

                screen.selectedIndex = -1;

                screen.SetDescription();

                // Clear changes to cursor:
                Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
            }

        }

        public void OnMouseEnter()
        {
            if (clickable)
            {
                isSelected = true;

                screen.selectedIndex = iconID;

                screen.SetDescriptionHover();

                // Change cursor to Point:
                Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Point);
            }
        }

        public void OnMouseDown()
        {
            if (clickable)
            {
                StopAllCoroutines();
                coroutineStart = false;

                if (!isChosen)
                {
                    // Clear selection for all
                    screen.ClearChoices();
                }

                // Toggle "isChosen" boolean
                isChosen = !isChosen;

                // Set "dialogueSelected" (important for showing descriptions and such later)
                screen.dialogueChosen = isChosen;

                // Save index if chosen:
                screen.chosenIndex = isChosen ? iconID : -1;

                screen.ValidateChoice(isChosen, iconID);
                screen.SetDescription();
            }
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
}