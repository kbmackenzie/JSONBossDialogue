using BepInEx;
using System;
using System.Collections.Generic;
using UnityEngine;
using InscryptionAPI.Ascension;

namespace JSONBossDialogue
{
    [AscensionScreenSort(AscensionScreenSort.Direction.PrefersStart)]
    public class DialogueSelectScreen : AscensionRunSetupScreenBase
    {
        public static DialogueSelectScreen Instance;

        public override string headerText => "Select a Dialogue File";
        public override bool showCardDisplayer => true; // Show card info
        public override bool showCardPanel => false; // Show selectable cards

        // DIALOGUE ICONS -- FILEPATH (no longer using this)
        private readonly static string[] imgName = { "dialogueicon_kcm3_off-1.png",
                                            "dialogueicon_kcm3_skull-1.png",
                                            "menu_arrow2-1.png"};

        // DIALOGUE ICONS -- RESOURCES
        public static Dictionary<string, byte[]> imgRes = new Dictionary<string, byte[]>()
        {
            { "Off", Properties.Resources.DialogueOff },
            { "Skull", Properties.Resources.DialogueSkull },
            { "MenuArrow", Properties.Resources.MenuArrow },
        };

        public readonly static Dictionary<string, Sprite> dialogueSprites = new Dictionary<string, Sprite> {
            { "Off", LoadTexture.MakeSprite(imgRes["Off"], true, "red") },
            { "Hover", LoadTexture.MakeSprite(imgRes["Off"], true, "light", true) },
            { "Chosen", LoadTexture.MakeSprite(imgRes["Skull"], true, "red") },
            { "ChosenHover", LoadTexture.MakeSprite(imgRes["Skull"], true, "light", true) },
            { "Arrow", LoadTexture.MakeSprite(imgRes["MenuArrow"], true, "red") },
            { "ArrowHover", LoadTexture.MakeSprite(imgRes["MenuArrow"], true, "light", true) } };

        // OFFSET BASICS
        private readonly static Vector3 offsetBwnCards = new Vector3(0.6f, 0, 0);
        private readonly static Vector3 offsetStart = new Vector3(-1.5f, 0.15f, 0);

        // LIST OF DIALOGUE ICONS
        public List<DialogueIcon> dialogueIcons = new List<DialogueIcon>();

        // ARROWS DICTIONARY
        public Dictionary<string, DialogueArrows> arrowButtons = new Dictionary<string, DialogueArrows>();

        // ICON CONTAINER
        public ScreenHelper dialogueBox;

        // BLINK ANIMATION TIME
        public const float blinkTime = 0.4f;


        // Bool for "Has a dialogue file been chosen?"
        public bool dialogueChosen = false;
        // Selected icon index
        public int hoverIndex = -1, chosenIndex = -1;


        // * DESCRIPTION *
        private int currentPage { get { return PageNumber + 1; } }
        private int totalPages { get { return maxPages + 1; } }

        // Page string:
        public string pageStr { get { return $"Page {currentPage} of {totalPages}"; } }

        // Default strings:
        private readonly static string[] basicStr = { "No File Selected", "",
                                                    "Unnamed File", "No description."};
        // Variables:
        public string nameStr = basicStr[0], descriptionStr = ""; // basicStr[1];



        // * PAGES *
        private int maxPages { get { return (int)Math.Ceiling((float)Plugin.dialogueInstances.Count / 6) - 1; } }

        // PAGE NUMBER -- Page numbering starts at 0.
        public int PageNumber = 0;

        public override void InitializeScreen(GameObject partialScreen)
        {
            IconContainer(partialScreen);
        }

        // Create the IconContainer object to hold the dialogue icons (just to be organized):
        public void IconContainer(GameObject parent)
        {
            GameObject obj = new GameObject();
            obj.name = "DialogueIconBox";
            obj.layer = LayerMask.NameToLayer("GBCUI");
            obj.transform.SetParent(parent.transform);

            dialogueBox = obj.AddComponent<ScreenHelper>();
            dialogueBox.screen = this;

            for (int i = 0; i < 6; i++)
            {
                BuildIcons(i, obj);
            }

            MakeItemsClickable();
            BuildArrows(parent);
        }

        // Build the dialogue icons. This method should preferably be called inside a 'for' loop.
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

        // Build screen arrows.
        // ... Also yes, I'm making my own instead of using the arrows AscensionRunSetupScreenBase gives me.
        // I felt more comfortable doing it like this. I don't think it'll cause any issues!
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

        // Update visibility of arrows.
        public void ArrowUpdate()
        {
            // Two-way arrows:
            bool arrowVisibility = maxPages > 0;
            arrowButtons["Left"].arrow.SetActive(arrowVisibility);
            arrowButtons["Right"].arrow.SetActive(arrowVisibility);
        }

        // Make ALL items unclickable.
        public void MakeItemsNotClickable()
        {
            for(int i = 0; i < dialogueIcons.Count; i++)
            {
                dialogueIcons[i].clickable = false;
            }
        }

        // Make items clickable if they map to an existing JSONHandler item in the list.
        public void MakeItemsClickable()
        {
            int lengthInPage = Plugin.dialogueInstances.Count - 6 * PageNumber;

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

            dialogueChosen = false;
            chosenIndex = -1;
        }

        // Update page when arrows are clicked!
        public void UpdatePage(bool previous)
        {
            ClearChoices();
            MakeItemsNotClickable();
            PatchTransition.chosenDialogue = null;

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
            SetDescription();

            // Note: Maybe make it so that the selected dialogue option stays selected even when changing pages?
            // As of right now, selection instantly resets when you change pages.
            // Maybe I could store the iconID of the dialigue icon selected and just try to match it to any of the visible ones...? 
        }

        public override void OnEnable()
        {
            base.OnEnable();

            challengeHeaderDisplay.UpdateText();
            ClearChoices();
            PatchTransition.chosenDialogue = null;

            SetDescription();
        }



        // * LOAD JSON *

        // Validate choice of JSON string for loading.
        // This method should be called by DialogueIcons on MouseDown!
        public void ValidateChoice(bool chosen, int id)
        {
            if (!chosen)
            {
                PatchTransition.chosenDialogue = null;
                return;
            }

            PatchTransition.chosenDialogue = Plugin.dialogueInstances[id];

            string logMessage = chosen ? "Dialogue file selected." : "Dialogue file deselected.";
            Plugin.myLogger.LogInfo(logMessage);
        }



        // * DESCRIPTION *

        // This method should be called by DialogueIcons on MouseDown and on MouseExit.
        public void SetDescription()
        {
            if (dialogueChosen && chosenIndex >= 0)
            {
                bool nameCheck = Plugin.dialogueInstances[chosenIndex].FileName.IsNullOrWhiteSpace();
                bool descCheck = Plugin.dialogueInstances[chosenIndex].Description.IsNullOrWhiteSpace();

                nameStr = nameCheck ? basicStr[2] : "Selected: " + Plugin.dialogueInstances[chosenIndex].FileName;
                descriptionStr = descCheck ? basicStr[3] : Plugin.dialogueInstances[chosenIndex].Description;

            } else
            {
                nameStr = basicStr[0];
                descriptionStr = pageStr; //basicStr[1];
            }

            // Display info:
            DisplayCardInfo(null, nameStr, descriptionStr);
        }

        // This method should be called by DialogueIcons on MouseEnter.
        public void SetDescriptionHover()
        {
            if(hoverIndex >= 0)
            {
                bool nameCheck = Plugin.dialogueInstances[hoverIndex].FileName.IsNullOrWhiteSpace();
                bool descCheck = Plugin.dialogueInstances[hoverIndex].Description.IsNullOrWhiteSpace();

                nameStr = nameCheck ? basicStr[2] : Plugin.dialogueInstances[hoverIndex].FileName;
                descriptionStr = descCheck ? basicStr[3] : Plugin.dialogueInstances[hoverIndex].Description;
            }

            // Display info:
            DisplayCardInfo(null, nameStr, descriptionStr);
        }
    }
}