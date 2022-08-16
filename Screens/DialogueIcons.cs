using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace JSONBossDialogue
{
    public class DialogueIcon : MonoBehaviour
    {
        // public static DialogueIcon Instance;

        // private Sprite iconArt;
        // private Text iconText;

        public bool isSelected, isChosen;
        public SpriteRenderer theSR;

        public DialogueSelectScreen screen;

        public int iconID; // Dialogue index, corresponding to an index in the getFile[] array! c:

        public bool clickable = false;
        // Whether this dialogue icon is clickable or not (if it maps to an existing index).

        public Dictionary<string, Sprite> sprites = DialogueSelectScreen.dialogueSprites;

        private void Update()
        {
            // Consider moving all of this to the MouseEnter / MouseExit / MouseDown functions!

            if (clickable)
            {
                if (isChosen) // Is chosen?
                {
                    theSR.sprite = isSelected ? sprites["ChosenHover"] : sprites["Chosen"];
                }
                else // Isn't chosen?
                {
                    theSR.sprite = isSelected ? sprites["Hover"] : sprites["Off"];
                }

                if (theSR.color.a < 1)
                {
                    theSR.color = new Color(theSR.color.r, theSR.color.b, theSR.color.g, 1f);
                }
            } else
            {
                if (theSR.color.a > 0)
                {
                    theSR.color = new Color(theSR.color.r, theSR.color.b, theSR.color.g, 0.5f);
                }
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
            }

        }

        public void OnMouseEnter()
        {
            if (clickable)
            {
                isSelected = true;

                screen.selectedIndex = iconID;

                screen.SetDescriptionHover();
            }
        }

        public void OnMouseDown()
        {
            if (clickable)
            {
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
    }
}