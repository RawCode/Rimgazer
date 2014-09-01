using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    class ChangeBody : Layer
    {
        private Colonist colonist;
        private Common common = new Common();
        private Color skinColor = Color.grey;
        private int bodyindex = 0;
        private int skinindex = 0;
        private string selectedBody = "";
        private List<BodyType> BodyList = new List<BodyType>();
        private List<string> SkinList = new List<string>();

        public ChangeBody(Colonist c)
        {
            colonist = c;
            base.SetCentered(600f, 440f);
            category = LayerCategory.GameDialog;
            clearNonEditDialogs = false;
            absorbAllInput = true;
            forcePause = true;
            CreateBodyList();
            CreateSkinList();
            SetDefaults();
        }

        private void CreateSkinList()
        {
            SkinList.Clear();

            SkinList.Add("Pale White");
            SkinList.Add("White");
            SkinList.Add("Mid");
            SkinList.Add("Light Black");
            SkinList.Add("Dark Black");
        }

        private void CreateBodyList()
        {
            BodyList.Clear();

            if (colonist.Gender == 1)
            {
                BodyList.Add(BodyType.Male);
                BodyList.Add(BodyType.Thin);
                BodyList.Add(BodyType.Hulk);
                BodyList.Add(BodyType.Fat);
            }
            else if (colonist.Gender == 2)
            {
                BodyList.Add(BodyType.Female);
                BodyList.Add(BodyType.Thin);
                BodyList.Add(BodyType.Hulk);
                BodyList.Add(BodyType.Fat);
            }
        }

        private void SetDefaults()
        {
            selectedBody = colonist.BodyType.ToString();
            for (int i = 0; i < BodyList.Count; i++)
            {
                if (selectedBody == BodyList[i].ToString())
                {
                    bodyindex = i;
                }
            }

            skinColor = colonist.SkinColor;
            if (skinColor == PawnSkinColors.PaleWhiteSkin)
            {
                for (int i = 0; i < SkinList.Count; i++)
                {
                    if (SkinList[i].Equals("Pale White"))
                    {
                        skinindex = i;
                    }
                }
            }
            else if (skinColor == PawnSkinColors.WhiteSkin)
            {
                for (int i = 0; i < SkinList.Count; i++)
                {
                    if (SkinList[i].Equals("White"))
                    {
                        skinindex = i;
                    }
                }
            }
            else if (skinColor == PawnSkinColors.MidSkin)
            {
                for (int i = 0; i < SkinList.Count; i++)
                {
                    if (SkinList[i].Equals("Mid"))
                    {
                        skinindex = i;
                    }
                }
            }
            else if (skinColor == PawnSkinColors.LightBlackSkin)
            {
                for (int i = 0; i < SkinList.Count; i++)
                {
                    if (SkinList[i].Equals("Light Black"))
                    {
                        skinindex = i;
                    }
                }
            }
            else if (skinColor == PawnSkinColors.DarkBlackSkin)
            {
                for (int i = 0; i < SkinList.Count; i++)
                {
                    if (SkinList[i].Equals("Dark Black"))
                    {
                        skinindex = i;
                    }
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
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Change Skin"), gUIStyle);

            //Body
            GenFont.SetFontTiny();
            position = new Rect(0f, 50f, 200f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Body Shape"));

            Rect position2 = new Rect(30f, 70f, 100f, 24f);
            position = new Rect(0f, 70f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                BodyDown(position2);
            }
            GUI.Label(position2, Language.FindText(LoadLanguage.language, BodyList[bodyindex].ToString()));
            position = new Rect(140f, 70f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                BodyUp(position2);
            }

            //Skin
            GenFont.SetFontTiny();
            position = new Rect(0f, 190f, 200f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Skin Color"));

            position2 = new Rect(30f, 210f, 100f, 24f);
            position = new Rect(0f, 210f, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                SkinDown(position2);
            }
            GUI.Label(position2, Language.FindText(LoadLanguage.language, SkinList[skinindex].ToString()));
            position = new Rect(140f, 210f, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                SkinUp(position2);
            }

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
            common.CreatePawnPreview(colonist, 390f, 120f, selectedBody, colonist.HeadGraphicPath, l, l2, colonist.HairDef.GraphicPath, skinColor, color, color2, colonist.HairColor);

            GenFont.SetFontMedium();
            GUI.color = Color.white;
            if (Widgets.TextButton(new Rect(inRect.width / 2f + 20f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Confirm".Translate()))
            {
                colonist.SkinColor = skinColor;
                base.Close();
            }
            if (Widgets.TextButton(new Rect(0f, inRect.height - 35f, inRect.width / 2f - 20f, 35f), "Back".Translate()))
            {
                base.Close();
            }
        }

        private void BodyUp(Rect position)
        {
            bodyindex = bodyindex + 1;
            if (bodyindex > BodyList.Count - 1)
            {
                bodyindex = 0;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, BodyList[bodyindex].ToString()));
            selectedBody = BodyList[bodyindex].ToString();
            colonist.BodyType = BodyList[bodyindex];
            ResetStoryList();
        }

        private void BodyDown(Rect position)
        {
            bodyindex = bodyindex - 1;
            if (bodyindex < 0)
            {
                bodyindex = BodyList.Count - 1;
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, BodyList[bodyindex].ToString()));
            selectedBody = BodyList[bodyindex].ToString();
            colonist.BodyType = BodyList[bodyindex];
            ResetStoryList();
        }

        private void SkinUp(Rect position)
        {
            skinindex = skinindex + 1;
            if (skinindex > SkinList.Count - 1)
            {
                skinindex = 0;
            }
            
            if (SkinList[skinindex] == "Pale White")
            {
                skinColor = PawnSkinColors.PaleWhiteSkin;
            }
            else if (SkinList[skinindex] == "White")
            {
                skinColor = PawnSkinColors.WhiteSkin;
            }
            else if (SkinList[skinindex] == "Mid")
            {
                skinColor = PawnSkinColors.MidSkin;
            }
            else if (SkinList[skinindex] == "Light Black")
            {
                skinColor = PawnSkinColors.LightBlackSkin;
            }
            else if (SkinList[skinindex] == "Dark Black")
            {
                skinColor = PawnSkinColors.DarkBlackSkin;
            }

            GUI.Label(position, Language.FindText(LoadLanguage.language, SkinList[skinindex]));
        }

        private void SkinDown(Rect position)
        {
            skinindex = skinindex - 1;
            if (skinindex < 0)
            {
                skinindex = SkinList.Count - 1;
            }
            
            if (SkinList[skinindex] == "Pale White")
            {
                skinColor = PawnSkinColors.PaleWhiteSkin;
            }
            else if (SkinList[skinindex] == "White")
            {
                skinColor = PawnSkinColors.WhiteSkin;
            }
            else if (SkinList[skinindex] == "Mid")
            {
                skinColor = PawnSkinColors.MidSkin;
            }
            else if (SkinList[skinindex] == "Light Black")
            {
                skinColor = PawnSkinColors.LightBlackSkin;
            }
            else if (SkinList[skinindex] == "Dark Black")
            {
                skinColor = PawnSkinColors.DarkBlackSkin;
            }

            GUI.Label(position, Language.FindText(LoadLanguage.language, SkinList[skinindex]));
        }

        private void ResetStoryList()
        {
            ColonistManager.AdultStories.Clear();
            int adultStoryIndex = 0;
            for (int a = 0; a < ColonistManager.AllBackstories.Count - 1; a++)
            {
                bool AdultStoryFound = false;

                Colonist_Backstory story = new Colonist_Backstory();

                if (colonist.Gender == 1)
                {
                    if (ColonistManager.AllBackstories[a].slot == BackstorySlot.Adulthood && (ColonistManager.AllBackstories[a].bodyTypeMale == colonist.BodyType || ColonistManager.AllBackstories[a].bodyTypeGlobal == colonist.BodyType))
                    {
                        story.StoryName = ColonistManager.AllBackstories[a].title;
                        story.StoryAge = 1;
                        story.BaseDescription = ColonistManager.AllBackstories[a].baseDesc;
                        story.BodyTypeFemale = ColonistManager.AllBackstories[a].bodyTypeFemale;
                        story.BodyTypeMale = ColonistManager.AllBackstories[a].bodyTypeMale;
                        story.BodyTypeGlobal = ColonistManager.AllBackstories[a].bodyTypeGlobal;
                        story.StoryIndex = adultStoryIndex;

                        List<StorySkillGain> skillGains = new List<StorySkillGain>();
                        for (int b = 0; b < ColonistManager.AllBackstories[a].skillGainsResolved.Count; b++)
                        {
                            StorySkillGain skillGain = new StorySkillGain();
                            skillGain.SkillName = ColonistManager.AllBackstories[a].skillGainsResolved.ElementAt(b).Key.skillLabel;
                            skillGain.SkillValue = ColonistManager.AllBackstories[a].skillGainsResolved.ElementAt(b).Value;
                            skillGains.Add(skillGain);
                        }
                        story.SkillGains = skillGains;

                        List<string> workRestrictions = new List<string>();
                        foreach (WorkTypeDef work in ColonistManager.AllBackstories[a].DisabledWorkTypes)
                        {
                            workRestrictions.Add(work.defName);
                        }
                        story.WorkRestrictions = workRestrictions;

                        for (int b = 0; b < ColonistManager.AdultStories.Count; b++)
                        {
                            if (ColonistManager.AdultStories[b].StoryName == story.StoryName)
                            {
                                AdultStoryFound = true;
                            }
                        }

                        if (AdultStoryFound == false)
                        {
                            ColonistManager.AdultStories.Add(story);
                            adultStoryIndex = adultStoryIndex + 1;
                        }
                    }
                }
                else if (colonist.Gender == 2)
                {
                    if (ColonistManager.AllBackstories[a].slot == BackstorySlot.Adulthood && (ColonistManager.AllBackstories[a].bodyTypeFemale == colonist.BodyType || ColonistManager.AllBackstories[a].bodyTypeGlobal == colonist.BodyType))
                    {
                        story.StoryName = ColonistManager.AllBackstories[a].title;
                        story.StoryAge = 1;
                        story.BaseDescription = ColonistManager.AllBackstories[a].baseDesc;
                        story.BodyTypeFemale = ColonistManager.AllBackstories[a].bodyTypeFemale;
                        story.BodyTypeMale = ColonistManager.AllBackstories[a].bodyTypeMale;
                        story.BodyTypeGlobal = ColonistManager.AllBackstories[a].bodyTypeGlobal;
                        story.StoryIndex = adultStoryIndex;

                        List<StorySkillGain> skillGains = new List<StorySkillGain>();
                        for (int b = 0; b < ColonistManager.AllBackstories[a].skillGainsResolved.Count; b++)
                        {
                            StorySkillGain skillGain = new StorySkillGain();
                            skillGain.SkillName = ColonistManager.AllBackstories[a].skillGainsResolved.ElementAt(b).Key.skillLabel;
                            skillGain.SkillValue = ColonistManager.AllBackstories[a].skillGainsResolved.ElementAt(b).Value;
                            skillGains.Add(skillGain);
                        }
                        story.SkillGains = skillGains;

                        List<string> workRestrictions = new List<string>();
                        foreach (WorkTypeDef work in ColonistManager.AllBackstories[a].DisabledWorkTypes)
                        {
                            workRestrictions.Add(work.defName);
                        }
                        story.WorkRestrictions = workRestrictions;

                        for (int b = 0; b < ColonistManager.AdultStories.Count; b++)
                        {
                            if (ColonistManager.AdultStories[b].StoryName == story.StoryName)
                            {
                                AdultStoryFound = true;
                            }
                        }

                        if (AdultStoryFound == false)
                        {
                            ColonistManager.AdultStories.Add(story);
                            adultStoryIndex = adultStoryIndex + 1;
                        }
                    }
                }
            }
            List<Colonist_Backstory> ColonistStory = new List<Colonist_Backstory>();
            ColonistStory.Add(ColonistManager.ChildStories[colonist.Backstory[0].StoryIndex]);
            ColonistStory.Add(ColonistManager.AdultStories[UnityEngine.Random.Range(0, ColonistManager.AdultStories.Count - 1)]);
            colonist.Backstory = ColonistStory;
        }
    }
}
