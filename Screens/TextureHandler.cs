using BepInEx;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace JSONBossDialogue
{
    internal static class LoadTexture
    {

        public readonly static Dictionary<string, Color> AscenscionColors = new Dictionary<string, Color>()
        {
            // Color32() can be implicitly converted to Color().
            { "red", new Color32(158, 38, 48, 255) },
            { "light", new Color32(247, 253, 207, 255) },
            { "default", new Color32(255, 255, 255, 255) }
        };

        public static byte[] ArtworkAsBytes(string fileName)
        {
            byte[] bytes;

            string[] imgs = Directory.GetFiles(Paths.PluginPath, fileName, SearchOption.AllDirectories);
            
            if (imgs.Length > 0)
            {
                if (imgs.Length > 1)
                {
                    Plugin.myLogger.LogError($"More than one image file named \"{fileName}\" found in the \'plugins\' directory. Weird behavior may come from this.");
                    
                }
            } else
            {
                Plugin.myLogger.LogError($"Could not load artwork: No image file named \"{fileName}\" found in the \'plugins\' directory.");
            }

            // Handle image-loading issues somewhere else! c:
            // Try-catching this method is a good idea.

            bytes = File.ReadAllBytes(imgs[0]);

            return bytes; // Handle null possibility later?
        }


        // This method should take ArtworkAsBytes as an argument.
        public static Texture2D TextureFromBytes(byte[] array, bool recolor = false, string colorName = "default", bool invertAlpha = false)
        {
            Texture2D tex = new Texture2D(1, 1);
            ImageConversion.LoadImage(tex, array);
            tex.filterMode = FilterMode.Point; // Pixel-perfect filter.

            // RECOLOR -- Replace all white pixels with colorName
            if(recolor)
            {
                // See if 'colorName' exists in the AscenscionColors dictionary:
                bool isValidColor = AscenscionColors.ContainsKey(colorName);

                // If not, set colorName to "default"
                colorName = isValidColor ? colorName : "default";

                if (!isValidColor)
                {
                    Plugin.myLogger.LogWarning("Invalid color name?");
                }

                // INVERT ALPHA -- This is helpful to create the "Hover" sprites in this mod!
                if (invertAlpha)
                {
                    Color transparent = new Color(1, 1, 1, 0);

                    for (int y = 0; y < tex.height; y++) // Go through the Y-Axis
                    {
                        for (int x = 0; x < tex.width; x++) // Go through the X-Axis
                        {
                            if(tex.GetPixel(x, y).a == 0)
                            {
                                // Change transparent pixels to this color
                                tex.SetPixel(x, y, AscenscionColors[colorName]);
                            } else
                            {
                                // Change white pixels to transparent pixels
                                tex.SetPixel(x, y, transparent);
                            }
                        }
                    }
                } else
                {
                    Color replaceColor = AscenscionColors["default"];

                    for (int y = 0; y < tex.height; y++) // Go through the Y-Axis
                    {
                        for (int x = 0; x < tex.width; x++) // Go through the X-Axis
                        {
                            if (tex.GetPixel(x, y) == replaceColor) // If pixel is white
                            {
                                // Change white pixels to this color
                                tex.SetPixel(x, y, AscenscionColors[colorName]);
                            }
                        }
                    }
                }
                tex.Apply();
            }

            return tex; // Return Texture2D c:
        }

        // This method should take TextureFromBytes as an argument.
        public static Sprite SpriteFromTexture(Texture2D tex)
        {
            Rect texRect = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            return Sprite.Create(tex, texRect, pivot);
        }

        // This method calls all of the above, making a Sprite object with them.
        public static Sprite MakeSprite(string name, bool recolor = false, string colorName = "default", bool invertAlpha = false)
        {
            return SpriteFromTexture(TextureFromBytes(ArtworkAsBytes(name), recolor, colorName, invertAlpha));
        }


    }
}