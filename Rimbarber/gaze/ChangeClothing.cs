using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    class ChangeClothing : Layer
    {
        private Colonist colonist = new Colonist();
        private Common common = new Common();
        private Color color1 = new Color();
        private Color color2 = new Color();
        private int shirtindex = 0;
        private int coatindex = 0;
        private string selectedshirt = "";
        private string selectedcoat = "";
        private static List<Clothing> ClothingList = new List<Clothing>();
        private List<Clothing> ShirtList = new List<Clothing>();
        private List<Clothing> CoatList = new List<Clothing>();
        private Clothing thisShirt = new Clothing();
        private Clothing thisCoat = new Clothing();

        public ChangeClothing(Colonist c)
        {
            colonist = c;
            base.SetCentered(600f, 440f);
            category = LayerCategory.GameDialog;
            clearNonEditDialogs = false;
            absorbAllInput = true;
            forcePause = true;
            CreateClothingList();
            SetClothingDefaults();
        }

        private void CreateClothingList()
        {
            ClothingList.Clear();
            ShirtList.Clear();
            CoatList.Clear();

            int onskinIndex = 0;
            int shellIndex = 0;

            foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
            {
                if (current.apparel != null)
                {
                    Apparel apparel = (Apparel)ThingMaker.MakeThing(current);
                    Clothing thisclothing = new Clothing();
                    bool ShirtOkay = true;
                    bool CoatOkay = true;

                    foreach (string exemption in Genstep_ColonistCreationMod.ExemptShirts)
                    {
                        if (apparel.Label == exemption)
                        {
                            ShirtOkay = false;
                        }
                    }
                    foreach (string exemption in Genstep_ColonistCreationMod.ExemptCoats)
                    {
                        if (apparel.Label == exemption)
                        {
                            CoatOkay = false;
                        }
                    }

                    if (apparel.def.apparel.Layer.ToString() == "OnSkin")
                    {
                        if (ShirtOkay == true)
                        {
                            thisclothing.Index = onskinIndex;
                            thisclothing.Layer = apparel.def.apparel.Layer.ToString();
                            thisclothing.Label = apparel.Label;
                            thisclothing.GraphicPath = apparel.def.apparel.graphicPath;
                            thisclothing.Color = Color.grey;
                            ShirtList.Add(thisclothing);
                            ClothingList.Add(thisclothing);
                            onskinIndex = onskinIndex + 1;
                        }
                    }
                    else if (apparel.def.apparel.Layer.ToString() == "Shell")
                    {
                        if (CoatOkay == true)
                        {
                            thisclothing.Index = shellIndex;
                            thisclothing.Layer = apparel.def.apparel.Layer.ToString();
                            thisclothing.Label = apparel.Label;
                            thisclothing.GraphicPath = apparel.def.apparel.graphicPath;
                            thisclothing.Color = Color.grey;
                            CoatList.Add(thisclothing);
                            ClothingList.Add(thisclothing);
                            shellIndex = shellIndex + 1;
                        }
                    }
                }
            }
        }

        private void SetClothingDefaults()
        {
            color1 = new Color();
            color2 = new Color();
            thisShirt = new Clothing();
            thisCoat = new Clothing();

            for (int i = 0; i < ShirtList.Count; i++)
            {
                if (colonist.Clothing[0].Label == ShirtList[i].Label)
                {
                    selectedshirt = ShirtList[i].Label;
                    shirtindex = i;
                }
            }

            for (int i = 0; i < CoatList.Count; i++)
            {
                if (colonist.Clothing[1].Label == CoatList[i].Label)
                {
                    selectedcoat = CoatList[i].Label;
                    coatindex = i;
                }
            }

            foreach (Clothing clothing in ClothingList)
            {
                if (selectedshirt == clothing.Label)
                {
                    thisShirt = clothing;
                }
                if (selectedcoat == clothing.Label)
                {
                    thisCoat = clothing;
                }
            }

            foreach (Clothing clothing in colonist.Clothing)
            {
                if (selectedshirt == clothing.Label)
                {
                    color1 = clothing.Color;
                }
                if (selectedcoat == clothing.Label)
                {
                    color2 = clothing.Color;
                }
            }
        }

        protected override void FillWindow(Rect inRect)
        {
            GenFont.SetFontMedium();
            GUI.contentColor = Color.white;
            Rect position = new Rect(0f, 2f, 520f, 20f);
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.alignment = TextAnchor.MiddleCenter;
            gUIStyle.onNormal.textColor = Color.yellow;
            gUIStyle.normal.textColor = Color.yellow;
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Change Clothing"), gUIStyle);

            //Shirt
            GenFont.SetFontTiny();
            position = new Rect(0f, 50f, 200f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Shirt"));

            Rect position2 = new Rect(30f, 70f, 100f, 24f);
            position = new Rect(0f, 70f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                ShirtDown(position2);
            }
            GUI.Label(position2, Language.FindText(LoadLanguage.language, ShirtList[shirtindex].Label));
            position = new Rect(140f, 70f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                ShirtUp(position2);
            }

            common.CreateColorSelector(ref color1, 210f, 50f);

            //Coat
            position = new Rect(0f, 190f, 200f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Coat"));
            position2 = new Rect(30f, 210f, 100f, 24f);

            position = new Rect(0f, 210f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                CoatDown(position2);
            }
            GUI.Label(position2, Language.FindText(LoadLanguage.language, CoatList[coatindex].Label));
            position = new Rect(140f, 210f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                CoatUp(position2);
            }

            common.CreateColorSelector(ref color2, 210f, 190f);

            //Preview
            position = new Rect(320f, 50f, 240f, 280f);
            GUI.Box(position, "");

            common.CreatePawnPreview(colonist, 380f, 140f, colonist.BodyType.ToString(), colonist.HeadGraphicPath, thisShirt, thisCoat, colonist.HairDef.GraphicPath, colonist.SkinColor, color1, color2, colonist.HairColor);

            GenFont.SetFontMedium();
            GUI.color = Color.white;
            if (Widgets.TextButton(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Confirm".Translate()))
            {
                thisShirt.Color = color1;
                thisCoat.Color = color2;
                colonist.Clothing[0] = thisShirt;
                colonist.Clothing[1] = thisCoat;
                base.Close();
            }
            if (Widgets.TextButton(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Back".Translate()))
            {
                base.Close();
            }
        }

        private void ShirtDown(Rect position)
        {
            shirtindex = shirtindex - 1;
            if (shirtindex < 0)
            {
                shirtindex = ShirtList.Count - 1;
            }

            GUI.Label(position, Language.FindText(LoadLanguage.language, ShirtList[shirtindex].Label));
            
            selectedshirt = ShirtList[shirtindex].Label;
            foreach (Clothing clothing in ClothingList)
            {
                if (clothing.Label == selectedshirt)
                {
                    thisShirt = clothing;
                    break;
                }
            }
        }

        private void ShirtUp(Rect position)
        {
            shirtindex = shirtindex + 1;
            if (shirtindex > ShirtList.Count - 1)
            {
                shirtindex = 0;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, ShirtList[shirtindex].Label));
            selectedshirt = ShirtList[shirtindex].Label;
            foreach (Clothing clothing in ClothingList)
            {
                if (clothing.Label == selectedshirt)
                {
                    thisShirt = clothing;
                    break;
                }
            }
        }

        private void CoatDown(Rect position)
        {
            coatindex = coatindex - 1;
            if (coatindex < 0)
            {
                coatindex = CoatList.Count - 1;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, CoatList[coatindex].Label));
            selectedcoat = CoatList[coatindex].Label;
            foreach (Clothing clothing in ClothingList)
            {
                if (clothing.Label == selectedcoat)
                {
                    thisCoat = clothing;
                    break;
                }
            }
        }

        private void CoatUp(Rect position)
        {
            coatindex = coatindex + 1;
            if (coatindex > CoatList.Count - 1)
            {
                coatindex = 0;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, CoatList[coatindex].Label));
            selectedcoat = CoatList[coatindex].Label;
            foreach (Clothing clothing in ClothingList)
            {
                if (clothing.Label == selectedcoat)
                {
                    thisCoat = clothing;
                    break;
                }
            }
        }
    }
}
