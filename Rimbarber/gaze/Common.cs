using System;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    public class Common
    {
        private Texture2D voidTexture = new Texture2D(0, 0);
        private Texture2D ctex = new Texture2D(100, 20);

        public void CreatePawnPreview(Colonist colonist, float left, float top, string Body, string Head, Clothing l1, Clothing l2, string Hair, Color Skin, Color color1, Color color2, Color HairColor)
        {
            Rect position = new Rect(left, top, 128f, 128f);
            string text = "Things/Pawn/Humanoid/Bodies/Naked_" + Body + "_Front";
            Texture2D image = ContentFinder<Texture2D>.Get(text, true);
            GUI.color = Skin;
            GUI.Label(position, image);

            Texture2D image2 = voidTexture;
            if (l1 != null)
            {
                image2 = ContentFinder<Texture2D>.Get(l1.GraphicPath + "_" + Body + "_front", true);
                GUI.color = color1;
                GUI.Label(position, image2);
            }

            Texture2D image3 = voidTexture;
            if (l2 != null)
            {
                image3 = ContentFinder<Texture2D>.Get(l2.GraphicPath + "_" + Body + "_front", true);
                GUI.color = color2;
                GUI.Label(position, image3);
            }

            Rect position2 = new Rect(left, top - 30f, 128f, 128f);
            string arg_12B_0 = colonist.HeadGraphicPath;
            Texture2D image5 = ContentFinder<Texture2D>.Get(Head + "_front", true);
            GUI.color = Skin;
            GUI.Label(position2, image5);
            position2 = new Rect(left, top - 30f, 128f, 128f);
            string arg_179_0 = colonist.HairDef.GraphicPath;
            Texture2D image6 = ContentFinder<Texture2D>.Get(Hair + "_front", true);
            GUI.color = HairColor;
            GUI.Label(position2, image6);
            
        }

        public void CreateColorSelector(ref Color c, float left, float top)
        {
            GenFont.SetFontTiny();
            Rect position = new Rect(left + 10f, top, 100f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Choose Color"));
            position = new Rect(left, top + 20f, 100f, 20f);
            GUI.color = c;
            GUI.Box(position, ctex);
            GUI.color = Color.white;
            position = new Rect(left + 15f, top + 42f, 20f, 20f);
            GUI.color = Color.red;
            GUI.Label(position, "R");
            position = new Rect(left + 45f, top + 42f, 20f, 20f);
            GUI.color = Color.green;
            GUI.Label(position, "G");
            position = new Rect(left + 75f, top + 42f, 20f, 20f);
            GUI.color = Color.blue;
            GUI.Label(position, "B");
            GUI.color = Color.white;
            position = new Rect(left + 10f, top + 65f, 20f, 70f);
            float num = GUI.VerticalSlider(position, c.r, 1f, 0f);
            if (c.r != num)
            {
                c.r = num;
            }
            position = new Rect(left + 40f, top + 65f, 20f, 70f);
            float num2 = GUI.VerticalSlider(position, c.g, 1f, 0f);
            if (c.g != num2)
            {
                c.g = num2;
            }
            position = new Rect(left + 70f, top + 65f, 20f, 70f);
            float num3 = GUI.VerticalSlider(position, c.b, 1f, 0f);
            if (c.b != num3)
            {
                c.b = num3;
            }
        }
    }
}
