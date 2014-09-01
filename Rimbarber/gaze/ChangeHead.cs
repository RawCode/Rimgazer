using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    class ChangeHead : Layer
    {
        private Colonist colonist;
        private Common common = new Common();
        private Color hairColor = Color.grey;
        private ColHairDef thisHair = new ColHairDef();
        private int hairindex = 0;
        private int face1index = 0;
        private int face2index = 0;
        private string selectedhair = "";
        private string selectedface1 = "";
        private string selectedface2 = "";
        private string thisFace = "";
        private List<ColHairDef> MaleHairList = new List<ColHairDef>();
        private List<ColHairDef> FemaleHairList = new List<ColHairDef>();
        private List<string> Tags = new List<string>();
        private List<string> Face1List = new List<string>();
        private List<string> Face2List = new List<string>();

        public ChangeHead(Colonist c)
        {
            colonist = c;
            base.SetCentered(600f, 440f);
            category = LayerCategory.GameDialog;
            clearNonEditDialogs = false;
            absorbAllInput = true;
            forcePause = true;
            thisHair = colonist.HairDef;
            CreateHairList();
            CreateFaceList();
            SetDefaults();
        }

        private void CreateFaceList()
        {
            Face1List.Clear();
            Face2List.Clear();

            Face1List.Add("Average");
            Face1List.Add("Narrow");

            Face2List.Add("Normal");
            Face2List.Add("Pointy");
            Face2List.Add("Wide");
        }

        private void CreateHairList()
        {
            MaleHairList.Clear();
            FemaleHairList.Clear();

            foreach (HairDef hair in DefDatabase<HairDef>.AllDefs)
            {
                ColHairDef thisHair = new ColHairDef();
                List<string> Tags = new List<string>();
                thisHair.DefName = hair.defName;
                thisHair.GraphicPath = hair.graphicPath;
                thisHair.HairGender = hair.hairGender;
                thisHair.Label = hair.label;
                foreach (string tag in hair.hairTags)
                {
                    Tags.Add(tag);
                }
                if (hair.hairGender == HairGender.Any)
                {
                    MaleHairList.Add(thisHair);
                    FemaleHairList.Add(thisHair);
                }
                else if (hair.hairGender == HairGender.Male)
                {
                    MaleHairList.Add(thisHair);
                }
                else if (hair.hairGender == HairGender.Female)
                {
                    FemaleHairList.Add(thisHair);
                }
                else if (hair.hairGender == HairGender.MaleUsually)
                {
                    MaleHairList.Add(thisHair);
                    FemaleHairList.Add(thisHair);
                }
                else if (hair.hairGender == HairGender.FemaleUsually)
                {
                    MaleHairList.Add(thisHair);
                    FemaleHairList.Add(thisHair);
                }
            }
        }

        private void SetDefaults()
        {
            hairColor = colonist.HairColor;
            thisHair = new ColHairDef();

            if (colonist.Gender == 1)
            {
                for (int i = 0; i < MaleHairList.Count; i++)
                {
                    if (colonist.HairDef.Label == MaleHairList[i].Label)
                    {
                        selectedhair = MaleHairList[i].Label;
                        hairindex = i;
                    }
                }

                foreach (ColHairDef hair in MaleHairList)
                {
                    if (selectedhair == hair.Label)
                    {
                        thisHair = hair;
                    }
                }
            }
            else if (colonist.Gender == 2)
            {
                for (int i = 0; i < FemaleHairList.Count; i++)
                {
                    if (colonist.HairDef.Label == FemaleHairList[i].Label)
                    {
                        selectedhair = FemaleHairList[i].Label;
                        hairindex = i;
                    }
                }

                foreach (ColHairDef hair in FemaleHairList)
                {
                    if (selectedhair == hair.Label)
                    {
                        thisHair = hair;
                    }
                }
            }
            

            string face = colonist.HeadGraphicPath;
            if (face.Contains("_"))
            {
                int index = face.IndexOf("_");
                face = face.Remove(0, index + 1);
            }
            string[] faceArray = face.Split('_');
            selectedface1 = faceArray[0];
            selectedface2 = faceArray[1];
            for (int i = 0; i < Face1List.Count; i++)
            {
                if (selectedface1 == Face1List[i])
                {
                    face1index = i;
                }
            }

            for (int i = 0; i < Face2List.Count; i++)
            {
                if (selectedface2 == Face2List[i])
                {
                    face2index = i;
                }
            }

            thisFace = "Things/Pawn/Humanoid/Heads/" + ((Gender)colonist.Gender).ToString() + "/" + ((Gender)colonist.Gender).ToString() + "_" + selectedface1 + "_" + selectedface2;
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
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Change Head"), gUIStyle);

            //Face
            GenFont.SetFontTiny();
            position = new Rect(0f, 50f, 200f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Face"));

            //Face1
            Rect position2 = new Rect(30f, 70f, 100f, 24f);
            position = new Rect(0f, 70f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                Face1Down(position2);
            }
            GUI.Label(position2, Language.FindText(LoadLanguage.language, Face1List[face1index]));
            position = new Rect(140f, 70f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                Face1Up(position2);
            }

            //Face2
            position2 = new Rect(30f, 100f, 100f, 24f);
            position = new Rect(0f, 100f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                Face2Down(position2);
            }
            GUI.Label(position2, Language.FindText(LoadLanguage.language, Face2List[face2index]));
            position = new Rect(140f, 100f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                Face2Up(position2);
            }

            //Hair
            position = new Rect(0f, 190f, 200f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Hair"));
            position2 = new Rect(30f, 210f, 100f, 24f);

            position = new Rect(0f, 210f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                HairDown(position2);
            }

            if (colonist.Gender == 1)
            {
                GUI.Label(position2, Language.FindText(LoadLanguage.language, MaleHairList[hairindex].Label));
            }
            else if (colonist.Gender == 2)
            {
                GUI.Label(position2, Language.FindText(LoadLanguage.language, FemaleHairList[hairindex].Label));
            }
            
            position = new Rect(140f, 210f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                HairUp(position2);
            }

            common.CreateColorSelector(ref hairColor, 210f, 190f);

            Clothing l = null;
            Clothing l2 = null;
            Color color = Color.gray;
            Color color2 = Color.gray;
            foreach (Clothing clothing in colonist.Clothing)
            {
                if (clothing.Layer.ToString() == "OnSkin")
                {
                    l = clothing;
                    color = clothing.Color;
                }
                else
                {
                    if (clothing.Layer.ToString() == "Shell")
                    {
                        l2 = clothing;
                        color2 = clothing.Color;
                    }
                }
            }

            position = new Rect(320f, 50f, 240f, 280f);
            GUI.Box(position, "");
            common.CreatePawnPreview(colonist, 390f, 120f, colonist.BodyType.ToString(), thisFace, l, l2, thisHair.GraphicPath, colonist.SkinColor, color, color2, hairColor);

            GenFont.SetFontMedium();
            GUI.color = Color.white;
            if (Widgets.TextButton(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Confirm".Translate()))
            {
                colonist.HairColor = hairColor;
                colonist.HairDef = thisHair;
                colonist.HeadGraphicPath = thisFace;
                base.Close();
            }
            if (Widgets.TextButton(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Back".Translate()))
            {
                base.Close();
            }
        }

        private void HairDown(Rect position)
        {
            if (colonist.Gender == 1)
            {
                hairindex = hairindex - 1;
                if (hairindex < 0)
                {
                    hairindex = MaleHairList.Count - 1;
                }

                GUI.Label(position, Language.FindText(LoadLanguage.language, MaleHairList[hairindex].Label));

                selectedhair = MaleHairList[hairindex].Label;
                foreach (ColHairDef hair in MaleHairList)
                {
                    if (hair.Label == selectedhair)
                    {
                        thisHair = hair;
                        break;
                    }
                }
            }
            else if (colonist.Gender == 2)
            {
                hairindex = hairindex - 1;
                if (hairindex < 0)
                {
                    hairindex = FemaleHairList.Count - 1;
                }

                GUI.Label(position, Language.FindText(LoadLanguage.language, FemaleHairList[hairindex].Label));

                selectedhair = FemaleHairList[hairindex].Label;
                foreach (ColHairDef hair in FemaleHairList)
                {
                    if (hair.Label == selectedhair)
                    {
                        thisHair = hair;
                        break;
                    }
                }
            }
        }

        private void HairUp(Rect position)
        {
            if (colonist.Gender == 1)
            {
                hairindex = hairindex + 1;
                if (hairindex > MaleHairList.Count - 1)
                {
                    hairindex = 0;
                }

                GUI.Label(position, Language.FindText(LoadLanguage.language, MaleHairList[hairindex].Label));

                selectedhair = MaleHairList[hairindex].Label;
                foreach (ColHairDef hair in MaleHairList)
                {
                    if (hair.Label == selectedhair)
                    {
                        thisHair = hair;
                        break;
                    }
                }
            }
            else if (colonist.Gender == 2)
            {
                hairindex = hairindex + 1;
                if (hairindex > FemaleHairList.Count - 1)
                {
                    hairindex = 0;
                }

                GUI.Label(position, Language.FindText(LoadLanguage.language, FemaleHairList[hairindex].Label));

                selectedhair = FemaleHairList[hairindex].Label;
                foreach (ColHairDef hair in FemaleHairList)
                {
                    if (hair.Label == selectedhair)
                    {
                        thisHair = hair;
                        break;
                    }
                }
            }
        }

        private void Face1Down(Rect position)
        {
            face1index = face1index - 1;
            if (face1index < 0)
            {
                face1index = Face1List.Count - 1;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, Face1List[face1index]));
            selectedface1 = Face1List[face1index];
            thisFace = "Things/Pawn/Humanoid/Heads/" + ((Gender)colonist.Gender).ToString() + "/" + ((Gender)colonist.Gender).ToString() + "_" + selectedface1 + "_" + selectedface2;
        }

        private void Face1Up(Rect position)
        {
            face1index = face1index + 1;
            if (face1index > Face1List.Count - 1)
            {
                face1index = 0;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, Face1List[face1index]));
            selectedface1 = Face1List[face1index];
            thisFace = "Things/Pawn/Humanoid/Heads/" + ((Gender)colonist.Gender).ToString() + "/" + ((Gender)colonist.Gender).ToString() + "_" + selectedface1 + "_" + selectedface2;
        }

        private void Face2Down(Rect position)
        {
            face2index = face2index - 1;
            if (face2index < 0)
            {
                face2index = Face2List.Count - 1;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, Face2List[face2index]));
            selectedface2 = Face2List[face2index];
            thisFace = "Things/Pawn/Humanoid/Heads/" + ((Gender)colonist.Gender).ToString() + "/" + ((Gender)colonist.Gender).ToString() + "_" + selectedface1 + "_" + selectedface2;
        }

        private void Face2Up(Rect position)
        {
            face2index = face2index + 1;
            if (face2index > Face2List.Count - 1)
            {
                face2index = 0;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, Face2List[face2index]));
            selectedface2 = Face2List[face2index];
            thisFace = "Things/Pawn/Humanoid/Heads/" + ((Gender)colonist.Gender).ToString() + "/" + ((Gender)colonist.Gender).ToString() + "_" + selectedface1 + "_" + selectedface2;
        }
    }
}
