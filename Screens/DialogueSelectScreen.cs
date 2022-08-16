using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using InscryptionAPI.Ascension;
using GBC;

namespace JSONBossDialogue
{
    [AscensionScreenSort(AscensionScreenSort.Direction.PrefersStart)]
    public class DialogueSelectScreen : AscensionRunSetupScreenBase
    {
        public static DialogueSelectScreen Instance;

        public override string headerText => "Select a Dialogue File";
        public override bool showCardDisplayer => true; // Show card info
        public override bool showCardPanel => false; // Show selectable cards

        // DIALOGUE ICONS
        private readonly static string[] imgName = { "dialogueicon_kcm3_off-1.png",
                                            "dialogueicon_kcm3_skull-1.png",
                                            "menu_arrow2-1.png"};
        public static readonly Dictionary<string, Sprite> dialogueSprites = new Dictionary<string, Sprite> {
            { "Off", LoadTexture.MakeSprite(imgName[0], true, "red") },
            { "Hover", LoadTexture.MakeSprite(imgName[0], true, "light", true) },
            { "Chosen", LoadTexture.MakeSprite(imgName[1], true, "red") },
            { "ChosenHover", LoadTexture.MakeSprite(imgName[1], true, "light", true) },
            { "Arrow", LoadTexture.MakeSprite(imgName[2], true, "red") },
            { "ArrowHover", LoadTexture.MakeSprite(imgName[2], true, "light", true) } };

        // OFFSET BASICS
        private readonly static Vector3 offsetBwnCards = new Vector3(0.6f, 0, 0);
        private readonly static Vector3 offsetStart = new Vector3(-1.5f, 0.15f, 0);

        // ARRAY OF DIALOGUE ICONS
        // public static DialogueIcon[] dialogueIcons;
        public List<DialogueIcon> dialogueIcons = new List<DialogueIcon>();

        // ARROWS DICTIONARY
        public Dictionary<string, DialogueArrows> arrowButtons = new Dictionary<string, DialogueArrows>();

        // ICON CONTAINER
        public ScreenHelper dialogueBox;

        // CHOSEN DIALOGUE
        public JSONHandler chosenDialogue;

        // Bool for "Has a dialogue file been chosen?", important for showing descriptions
        public bool dialogueChosen = false;
        // Selected icon index
        public int selectedIndex = -1, chosenIndex = -1;


        // * DESCRIPTION*

        // Default strings:
        private readonly static string[] basicStr = { "No File Selected", "",
                                                    "No filename", "No description."};
        // Variables:
        public string nameStr = basicStr[0], descriptionStr = basicStr[1];


        // PAGES
        private readonly int maxPages = (int)Math.Floor((float)Plugin.dialogueArray.Count / 6);

        // PAGE NUMBER
        public int PageNumber = 0;
        /* Page numbering starts at 0. Next page is 1.
         * 
         * 
         * This is important because PAGINATORS will take code like the following:
            num = this.dialogueID + 6 * PageNumber // "6" is the number of DialogueIcon instances.
            getFiles[num];
         * So having them start at 1 makes sure this whole thing even works. This code reads strings from getFiles[0] according to the dialogue icon and the page. */

        public override void InitializeScreen(GameObject partialScreen)
        {
            /* string[] text = { "a", "b" };
            cardInfoLines.ShowText(1, text, true); */
            // ^^ These only work if I set showCardDisplayer to 'true'.

            IconContainer(partialScreen);
        }

        public void IconContainer(GameObject parent)
        {
            GameObject obj = new GameObject();
            obj.name = "DialogueIconBox";
            obj.layer = LayerMask.NameToLayer("GBCUI");
            obj.transform.SetParent(parent.transform);

            // Add DialogueBoxController script to obj DialogueIconBox:
            dialogueBox = obj.AddComponent<ScreenHelper>();
            dialogueBox.screen = this;

            for (int i = 0; i < 6; i++)
            {
                BuildIcons(i, obj);
            }

            MakeItemsClickable();
            BuildArrows(parent);
        }

        // This method should preferably be called inside a 'for' loop.
        public void BuildIcons(int a, GameObject parent)
        {
            GameObject obj = new GameObject();
            obj.layer = LayerMask.NameToLayer("GBCUI");
            obj.transform.SetParent(parent.transform);
            obj.name = "Dialogue" + a.ToString();
            obj.transform.position = offsetStart + offsetBwnCards * (float)a;

            SpriteRenderer img = obj.AddComponent<SpriteRenderer>();
            img.sprite = dialogueSprites["Off"];

            BoxCollider2D box2D = obj.AddComponent<BoxCollider2D>();
            box2D.size = img.size;

            DialogueIcon dialogue = obj.AddComponent<DialogueIcon>();
            dialogue.iconID = a;
            dialogue.screen = this;
            dialogue.theSR = img;

            dialogueIcons.Add(dialogue);
        }

        public void BuildArrows(GameObject parent)
        {
            float arrowY = 0.16f;

            // Left Arrow
            GameObject leftArrow = new GameObject();
            leftArrow.layer = LayerMask.NameToLayer("GBCUI");
            leftArrow.transform.SetParent(parent.transform);
            leftArrow.name = "LeftArrow";
            leftArrow.transform.position = new Vector3(-1.9f, arrowY, 0);

            SpriteRenderer img = leftArrow.AddComponent<SpriteRenderer>();
            img.sprite = dialogueSprites["Arrow"];

            BoxCollider2D box2D = leftArrow.AddComponent<BoxCollider2D>();
            box2D.size = img.size;

            DialogueArrows arr1 = leftArrow.AddComponent<DialogueArrows>();
            arr1.screen = this;
            arr1.isLeft = true;
            arr1.arrow = leftArrow;
            arr1.theSR = img;

            // Add left arrow to dictionary arrowButton:
            arrowButtons.Add("Left", arr1);


            // Right arrow
            GameObject rightArrow = new GameObject();
            rightArrow.layer = LayerMask.NameToLayer("GBCUI");
            rightArrow.transform.SetParent(parent.transform);
            rightArrow.name = "RightArrow";
            rightArrow.transform.position = new Vector3(1.9f,arrowY, 0);

            SpriteRenderer img2 = rightArrow.AddComponent<SpriteRenderer>();
            img2.sprite = dialogueSprites["Arrow"];
            img2.flipX = true; // Flip sprite

            BoxCollider2D box2D2 = rightArrow.AddComponent<BoxCollider2D>();
            box2D2.size = img2.size;

            DialogueArrows arr2 = rightArrow.AddComponent<DialogueArrows>();
            arr2.screen = this;
            arr2.isLeft = false;
            arr2.arrow = rightArrow;
            arr2.theSR = img2;

            // Add right arrow to dictionary arrowButton:
            arrowButtons.Add("Right", arr2);

            ArrowUpdate();
        }

        public void ArrowUpdate()
        {
            // arrowButtons["Left"].arrow.SetActive(PageNumber > 0); // Visibility
            // arrowButtons["Right"].arrow.SetActive(PageNumber < maxPages); // Visibility

            // A different way of handling errors (two-way arrows):
            bool arrowVisibility = maxPages > 0;
            arrowButtons["Left"].arrow.SetActive(arrowVisibility);
            arrowButtons["Right"].arrow.SetActive(arrowVisibility);
        }

        // This method should be called when moving to a new page.
        // MakeItemsClickable() should be called after this one.
        public void MakeItemsNotClickable()
        {
            // Iterate through entire list of dialogueIcons, make them all unclickable:
            for(int i = 0; i < dialogueIcons.Count; i++)
            {
                dialogueIcons[i].clickable = false;
            }
        }

        public void MakeItemsClickable()
        {
            // Assign which dialogueIcons are clickable and which aren't,
            // based on the size of the dialogueArray list. (I have to change the name of that list...)

            int lengthInPage = Plugin.dialogueArray.Count - 6 * PageNumber;
            // ^^ Note: I have to make sure this number is never negative.

            // Make sure the loop doesn't iterate more than 6 times (nor less than 0??):
            int listLength = lengthInPage > 6 ? 6 : (lengthInPage < 0 ? 0 : lengthInPage);

            // Clickability loop:
            for (int i = 0; i < listLength; i++)
            {
                dialogueIcons[i].clickable = true;
            }
        }


        // This method sets all of the DialogueIcon instances' "isChosen" booleans as false.
        // This should be done right before a new one is selected, to make sure only one can be selected at a time.
        public void ClearChoices()
        {
            for(int i = 0; i < dialogueIcons.Count; i++)
            {
                dialogueIcons[i].isChosen = false;
            }
        }

        // Update page when arrows are clicked!
        public void UpdatePage(bool previous)
        {
            ClearChoices();
            MakeItemsNotClickable();

            if (PageNumber <= 0 && previous) // If on first page and pressed "Previous",
            {
                PageNumber = maxPages;
            }
            else if (PageNumber >= maxPages && !previous) // If on last page and pressed "Next",
            {
                PageNumber = 0;
            }
            else
            {
                PageNumber += previous ? -1 : 1;
            }

            for (int i = 0; i < dialogueIcons.Count; i++)
            {
                dialogueIcons[i].iconID = i + 6 * PageNumber;
            }

            MakeItemsClickable();
            ArrowUpdate();

            // Note: Maybe make it so that the selected dialogue option stays selected even when changing pages?
            // Maybe I could store the iconID of whichever page was selected? 
        }




        // LOAD JSON

        // This method is very redundant. Consider getting rid of it later?
        public JSONHandler ChooseDialogue(int a)
        {
            // int dNum = a + 6 * PageNumber; // If PageNumber = 0, dNum = a.
            // chosenIndex = dNum;
            return Plugin.dialogueArray[a];
        }


        // This method should be called by DialogueIcons on MouseDown!
        public void ValidateChoice(bool chosen, int id)
        {
            if (!chosen)
            {
                PatchTransition.chosenDialogue = null;
                return;
            }

            PatchTransition.chosenDialogue = Plugin.dialogueArray[id];

            string logMessage = chosen ? "Dialogue file selected." : "Dialogue file deselected.";
            Plugin.myLogger.LogInfo(logMessage);
        }




        // This method should be called by DialogueIcons on MouseDown and on MouseExit.
        public void SetDescription()
        {
            if (dialogueChosen && chosenIndex >= 0)
            {
                bool nameCheck = Plugin.dialogueArray[chosenIndex].FileName.IsNullOrWhiteSpace();
                bool descCheck = Plugin.dialogueArray[chosenIndex].Description.IsNullOrWhiteSpace();

                nameStr = nameCheck ? basicStr[2] : "Selected: " + Plugin.dialogueArray[chosenIndex].FileName;
                descriptionStr = descCheck ? basicStr[3] : Plugin.dialogueArray[chosenIndex].Description;

            } else
            {
                nameStr = basicStr[0];
                descriptionStr = basicStr[1];
            }

            // Display info:
            DisplayCardInfo(null, nameStr, descriptionStr);
        }

        // This method should be called by DialogueIcons on MouseEnter.
        public void SetDescriptionHover()
        {
            if(selectedIndex >= 0)
            {
                bool nameCheck = Plugin.dialogueArray[selectedIndex].FileName.IsNullOrWhiteSpace();
                bool descCheck = Plugin.dialogueArray[selectedIndex].Description.IsNullOrWhiteSpace();

                nameStr = nameCheck ? basicStr[2] : Plugin.dialogueArray[selectedIndex].FileName;
                descriptionStr = descCheck ? basicStr[3] : Plugin.dialogueArray[selectedIndex].Description;
            }

            // Display info:
            DisplayCardInfo(null, nameStr, descriptionStr);
        }
    }


    /*
        public override void OnEnable()
        {
            nameStr = defaultStr[0];
            descriptionStr = defaultStr[1];
        }
     */

}