using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

using Verse;
using VerseBase;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    public class ColonistCreationMenu : Layer
    {
        private const float TopAreaHeight = 80f;
        private static Colonist col;
        private static Common common = new Common();
        private static int ChildStoryIndex = 0;
        private static int AdultStoryIndex = 0;
        private static int Trait1Index = 0;
        private static int Trait2Index = 0;
        private static int WeaponIndex = 0;
        private static string MajorSkill = "";
        private static string MinorSkill1 = "";
        private static string MinorSkill2 = "";
        private static readonly Vector2 WinSize = new Vector2(900f, 750f);
        private static readonly Vector2 BottomButSize = new Vector2(150f, 40f);
        private static readonly Texture2D HighlightColTex = MaterialMaker.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));
        private static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);
        private static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);
        private static readonly Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);
        private static List<StoryTrait> allTraits = new List<StoryTrait>();
        private static List<ColHairDef> MaleHairList = new List<ColHairDef>();
        private static List<ColHairDef> FemaleHairList = new List<ColHairDef>();

        public ColonistCreationMenu(Colonist c)
        {
            col = c;
            base.SetCentered(ColonistCreationMenu.WinSize);
            category = LayerCategory.GameDialog;
            clearNonEditDialogs = true;
            absorbAllInput = true;
            forcePause = true;

            //Gen Traits and sort
            allTraits.Clear();
            foreach (TraitDef trait in DefDatabase<TraitDef>.AllDefsListForReading)
            {
                StoryTrait storytrait = new StoryTrait();
                storytrait.TraitName = trait.label;
                storytrait.Effect = trait.effect;
                allTraits.Add(storytrait);
            }
            List<StoryTrait> SortedTraits = allTraits.OrderBy(o => o.TraitName).ToList();
            allTraits = SortedTraits;

            //Set Trait1 index
            for (int i = 0; i < allTraits.Count - 1; i++)
            {
                if (col.Traits[0].TraitName == allTraits[i].TraitName)
                {
                    Trait1Index = i;
                }
            }
            //Set Trait2 index
            for (int a = 0; a < allTraits.Count - 1; a++)
            {
                if (col.Traits[1].TraitName == allTraits[a].TraitName)
                {
                    Trait2Index = a;
                }
            }

            //Set Childhood index
            for (int b = 0; b < ColonistManager.ChildStories.Count - 1; b++)
            {
                if (col.Backstory[0].StoryName == ColonistManager.ChildStories[b].StoryName)
                {
                    ChildStoryIndex = b;
                }
            }
            //Set Adulthood index
            for (int d = 0; d < ColonistManager.AdultStories.Count - 1; d++)
            {
                if (col.Backstory[1].StoryName == ColonistManager.AdultStories[d].StoryName)
                {
                    AdultStoryIndex = d;
                }
            }

            //Set Weapon Index
            for (int d = 0; d < ColonistManager.WeaponList.Count - 1; d++)
            {
                if (col.Weapon == ColonistManager.WeaponList[d])
                {
                    WeaponIndex = d;
                }
            }
        }

        protected override void FillWindow(Rect inRect)
        {
            DrawTabs(inRect, col);
            DrawButtons();
        }

        private AcceptanceReport CanStart()
        {
            AcceptanceReport result;
            foreach (Pawn current in ModdedMapInitParams.colonists)
            {
                if (!current.Name.Valid)
                {
                    result = new AcceptanceReport(Translator.Translate("EveryoneNeedsValidName"));
                    return result;
                }
            }
            result = AcceptanceReport.WasAccepted;
            return result;
        }

        public void SelectColonistConfig(Colonist c)
        {
            if (c != col)
            {
                col = c;
            }
        }

        private static void FillCard(Colonist colonist)
        {
            Rect rect = new Rect(0f, 0f, 300f, 30f);

            DrawEnterName(rect, colonist);
            Display_GenderRaceAge(colonist);

            Rect rect2 = new Rect(0f, 40f, 300f, 500f);
            Rect rect3 = new Rect(rect2.xMax + 17f, 40f, 280f, 385f);
            Rect rect4 = new Rect(rect3.xMax, 40f, 280f, 500f);
            Rect rect5 = new Rect(rect2.xMax + 17f, 385f, 280f, 160f);
            Rect innerRect = rect2.GetInnerRect(10f);
            Rect innerRect2 = rect3.GetInnerRect(10f);
            Rect innerRect3 = rect4.GetInnerRect(10f);
            Rect innerRect4 = rect5.GetInnerRect(10f);

            DrawBackstory(innerRect, colonist);
            DrawSkills(innerRect2, colonist);
            DrawStyle(innerRect3, colonist);
            DrawPassions(innerRect4, colonist);
        }

        private void DrawTabs(Rect inRect, Colonist colonist)
        {
            GenFont.SetFontMedium();
            GUI.Label(new Rect(0f, 0f, 300f, 300f), Language.FindText(LoadLanguage.language, "Enhanced Colonist Creation"));
            GenFont.SetFontSmall();
            Rect rect = new Rect(0f, 80f, inRect.width, inRect.height - 60f - 80f);
            Widgets.DrawMenuSection(rect);
            TabDrawer.DrawTabs(rect,
                from c in ColonistManager.Population
                select new TabRecord(c.FirstName + " " + c.LastName, delegate
                {
                    SelectColonistConfig(c);
                }, c == colonist));
            Rect innerRect = GenUI.GetInnerRect(rect, 17f);

            GUI.BeginGroup(innerRect);
            FillCard(colonist);
            GUI.EndGroup();
        }

        private void DrawButtons()
        {
            Rect innerRect2 = winRect.GetInnerRect(17f);
            float top = innerRect2.height - BottomButSize.y;
            Rect butRect = new Rect(0f, top, BottomButSize.x, BottomButSize.y);
            if (Widgets.TextButton(butRect, Language.FindText(LoadLanguage.language, "Back")))
            {
                Application.LoadLevel("Entry");
            }
            Rect butRect2 = new Rect(275f, top, BottomButSize.x, BottomButSize.y);
            if (Widgets.TextButton(butRect2, Language.FindText(LoadLanguage.language, "Export")))
            {
                Find.LayerStack.Add(new DialogSave());
            }
            Rect butRect3 = new Rect((innerRect2.width - BottomButSize.x) - 275f, top, BottomButSize.x, BottomButSize.y);
            if (Widgets.TextButton(butRect3, Language.FindText(LoadLanguage.language, "Import")))
            {
                Find.LayerStack.Add(new DialogLoad());
            }
            Rect butRect4 = new Rect(innerRect2.width - BottomButSize.x, top, BottomButSize.x, BottomButSize.y);
            if (Widgets.TextButton(butRect4, Language.FindText(LoadLanguage.language, "Continue")))
            {
                AcceptanceReport acceptanceReport = CanStart();
                if (acceptanceReport.accepted)
                {
                    ColonistToPawn();
                    base.Close();
                    Genstep_ColonistCreationMod.SpawnStuff();
                }
                else
                {
                    Messages.Message(acceptanceReport.reasonText);
                }
            }
        }

        private static void DrawEnterName(Rect rect, Colonist colonist)
        {
            Rect rect2 = new Rect(rect);
            rect2.width *= 0.333f;
            Rect rect3 = new Rect(rect);
            rect3.width *= 0.333f;
            rect3.x += rect3.width;
            Rect rect4 = new Rect(rect);
            rect4.width *= 0.333f;
            rect4.x += rect3.width * 2f;

            string firstName = colonist.FirstName;
            string nickName = colonist.NickName;
            string lastName = colonist.LastName;

            DoNameInputRect(rect2, ref firstName, 12);
            colonist.FirstName = firstName;

            if (colonist.NickName == colonist.FirstName || colonist.NickName == colonist.LastName)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
            }

            DoNameInputRect(rect3, ref nickName, 9);
            colonist.NickName = nickName;

            GUI.color = Color.white;

            DoNameInputRect(rect4, ref lastName, 12);
            colonist.LastName = lastName;

            TooltipHandler.TipRegion(rect2, "FirstNameDesc".Translate());
            TooltipHandler.TipRegion(rect3, "ShortIdentifierDesc".Translate());
            TooltipHandler.TipRegion(rect4, "LastNameDesc".Translate());
        }

        private static void DoNameInputRect(Rect rect, ref string name, int maxLength)
        {
            GUI.skin.settings.doubleClickSelectsWord = true;
            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
            GUI.skin.textField.contentOffset = new Vector2(12f, 0f);
            string text = GUI.TextField(rect, name);
            Regex regex = new Regex("^[a-zA-Z '\\-]*$");
            if (text.Length <= maxLength && regex.IsMatch(text))
            {
                name = text;
            }
        }

        private static void Display_GenderRaceAge(Colonist colonist)
        {
            Rect position = new Rect(327f, 5f, 50f, 24f);
            Rect position2 = new Rect((position.x + position.width) + 2, 5f, 24f, 24f);
            Rect position3 = new Rect((position2.x + position2.width) + 5, 5f, 50f, 24f);
            Rect position4 = new Rect((position3.x + position3.width) + 2, 5f, 24f, 24f);
            Rect position5 = new Rect((position4.x + position4.width) + 20, 5f, 30f, 24f);
            Rect position6 = new Rect((position5.x + position5.width) + 2, 5f, 24f, 24f);
            Rect position7 = new Rect((position6.x + position6.width) + 5, 5f, 20f, 24f);
            Rect position8 = new Rect(position7.x + position7.width, 5f, 24f, 24f);

            string gender = "";
            if (colonist.Gender == 1)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            GUI.Label(position, Language.FindText(LoadLanguage.language, "Gender") + ":");
            if (Widgets.TextButton(position2, "<"))
            {
                GenderChange(position3, colonist);
            }
            GUI.Label(position3, gender.Translate());
            if (Widgets.TextButton(position4, ">"))
            {
                GenderChange(position3, colonist);
            }

            GUI.Label(position5, Language.FindText(LoadLanguage.language, "Age") + ":");
            if (Widgets.TextButton(position6, "-"))
            {
                AgeDown(position7, colonist);
            }
            GUI.Label(position7, colonist.Age.ToString().Translate());
            if (Widgets.TextButton(position8, "+"))
            {
                AgeUp(position7, colonist);
            }
        }

        private static void DrawBackstory(Rect innerRect, Colonist colonist)
        {
            GUI.BeginGroup(innerRect);

            float num = 0f;
            
            GenFont.SetFontMedium();
            GUI.Label(new Rect(0f, 0f, 200f, 30f), Language.FindText(LoadLanguage.language, "Backstory"));

            num += 30f;            

            //Childhood Story
            GenFont.SetFontSmall();
            Rect rect7 = new Rect(0f, num, innerRect.width, 24f);
            if (rect7.Contains(Event.current.mousePosition))
            {
                Widgets.DrawHighlight(rect7);
            }
            TooltipHandler.TipRegion(rect7, colonist.Backstory[0].FullDescriptionFor(colonist.Backstory[0].SkillGains, colonist.Backstory[0].WorkRestrictions, colonist.Backstory[0].BaseDescription, colonist));
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.Label(rect7, Language.FindText(LoadLanguage.language, "Childhood") + ":");
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            Rect position2 = new Rect(rect7);
            position2.x += 108f;
            position2.width -= 90f;
            string title = colonist.Backstory[0].StoryName;
            GUI.Label(position2, title.Translate());

            Rect rect9 = new Rect(position2.x - 30f, num, 24f, 24f);
            Rect position4 = new Rect(rect9);
            if (Widgets.TextButton(position4, "<"))
            {
                ChildStoryPrevious(position2, colonist, rect7, 0);
            }
            position4.x = innerRect.width - 24f;
            if (Widgets.TextButton(position4, ">"))
            {
                ChildStoryNext(position2, colonist, rect7, 0);
            }
            num += rect7.height + 2f;

            //Adulthood Story
            GenFont.SetFontSmall();
            Rect rect8 = new Rect(0f, num, innerRect.width, 24f);
            if (rect8.Contains(Event.current.mousePosition))
            {
                Widgets.DrawHighlight(rect8);
            }
            TooltipHandler.TipRegion(rect8, colonist.Backstory[1].FullDescriptionFor(colonist.Backstory[1].SkillGains, colonist.Backstory[1].WorkRestrictions, colonist.Backstory[1].BaseDescription, colonist));
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.Label(rect8, Language.FindText(LoadLanguage.language, "Adulthood") + ":");
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            Rect position3 = new Rect(rect8);
            position3.x += 108f;
            position3.width -= 90f;
            string title2 = colonist.Backstory[1].StoryName;
            GUI.Label(position3, title2.Translate());

            Rect rect10 = new Rect(position3.x - 30f, num, 24f, 24f);
            Rect position5 = new Rect(rect10);
            if (Widgets.TextButton(position5, "<"))
            {
                AdultStoryPrevious(position3, colonist, rect8, 1);
            }
            position5.x = innerRect.width - 24f;
            if (Widgets.TextButton(position5, ">"))
            {
                AdultStoryNext(position3, colonist, rect8, 1);
            }
            num += rect8.height + 2f;

            num += 5f;
            GenFont.SetFontMedium();
            GUI.Label(new Rect(0f, num, 200f, 30f), Language.FindText(LoadLanguage.language, "Incapable Of"));

            
            num += 30f;
            float num2 = num;
            num2 += 280f;
            GenFont.SetFontSmall();
            List<string> list = new List<string>();
            
            if (colonist.Backstory[0].WorkRestrictions.Count > 0)
            {
                for (int a = 0; a < colonist.Backstory[0].WorkRestrictions.Count; a++)
                {
                    list.Add(colonist.Backstory[0].WorkRestrictions[a]);
                }
            }

            if (colonist.Backstory[1].WorkRestrictions.Count > 0)
            {
                for (int b = 0; b < colonist.Backstory[1].WorkRestrictions.Count; b++)
                {
                    if (!list.Contains(colonist.Backstory[1].WorkRestrictions[b]))
                    {
                        list.Add(colonist.Backstory[1].WorkRestrictions[b]);
                    }
                }
            }
            
            if (list.Count == 0)
            {
                string text = Language.FindText(LoadLanguage.language, "(none)");
                Rect position6 = new Rect(0f, num, innerRect.width, 20f);
                GUI.Label(position6, text.Translate());
            }
            else
            {
                for (int c = 0; c < list.Count; c++)
                {
                    string text = list[c];
                    Rect position6 = new Rect(0f, num, innerRect.width, 20f);
                    GUI.Label(position6, Language.FindText(LoadLanguage.language, text));
                    num += 20f;
                }
            }
            
            //Traits
            GenFont.SetFontMedium();
            GUI.Label(new Rect(0f, num2, 200f, 30f), Language.FindText(LoadLanguage.language, "Traits"));

            num2 += 30f;
            GenFont.SetFontSmall();

            //Trait1
            Rect rect11 = new Rect(0f, num2, 24f, 24f);
            Rect rect12 = new Rect(30f, num2, innerRect.width, 24f);
            if (rect12.Contains(Event.current.mousePosition))
            {
                Widgets.DrawHighlight(rect11);
            }
            TooltipHandler.TipRegion(rect11, "TraitsDoNothing".Translate());
            if (Widgets.TextButton(rect11, "<"))
            {
                Trait1Down(rect12, colonist);
            }
            rect12.width = innerRect.width - 96f;
            rect11.x = innerRect.width - 96f;
            if (Widgets.TextButton(rect11, ">"))
            {
                Trait1Up(rect12, colonist);
            }
            GUI.Label(rect12, colonist.Traits[0].TraitName.Translate());
            num2 += rect11.height + 2f;

            //Trait2
            Rect rect13 = new Rect(0f, num2, 24f, 24f);
            Rect rect14 = new Rect(30f, num2, innerRect.width, 24f);
            if (rect12.Contains(Event.current.mousePosition))
            {
                Widgets.DrawHighlight(rect13);
            }
            TooltipHandler.TipRegion(rect14, "TraitsDoNothing".Translate());
            if (Widgets.TextButton(rect13, "<"))
            {
                Trait2Down(rect14, colonist);
            }
            rect14.width = innerRect.width - 96f;
            rect13.x = innerRect.width - 96f;
            if (Widgets.TextButton(rect13, ">"))
            {
                Trait2Up(rect14, colonist);
            }
            GUI.Label(rect14, colonist.Traits[1].TraitName.Translate());

            GUI.EndGroup();
        }

        private static void ChildStoryNext(Rect position, Colonist colonist, Rect rect7, int slot)
        {
            ChildStoryIndex = ChildStoryIndex + 1;
            if (ChildStoryIndex > ColonistManager.ChildStories.Count - 1)
            {
                ChildStoryIndex = 0;
            }
            colonist.Backstory[0] = ColonistManager.ChildStories[ChildStoryIndex];
            if (Genstep_ColonistCreationMod.BaseStats[0] == false)
            {
                ResetSkills(colonist);
            }
            GUI.Label(position, colonist.Backstory[0].StoryName.Translate());
        }

        private static void ChildStoryPrevious(Rect position, Colonist colonist, Rect rect7, int slot)
        {
            ChildStoryIndex = ChildStoryIndex - 1;
            if (ChildStoryIndex < 0)
            {
                ChildStoryIndex = ColonistManager.ChildStories.Count - 1;
            }
            colonist.Backstory[0] = ColonistManager.ChildStories[ChildStoryIndex];
            if (Genstep_ColonistCreationMod.BaseStats[0] == false)
            {
                ResetSkills(colonist);
            }
            GUI.Label(position, colonist.Backstory[0].StoryName.Translate());
        }

        private static void AdultStoryNext(Rect position, Colonist colonist, Rect rect7, int slot)
        {
            AdultStoryIndex = AdultStoryIndex + 1;
            if (AdultStoryIndex > ColonistManager.AdultStories.Count - 1)
            {
                AdultStoryIndex = 0;
            }
            colonist.Backstory[1] = ColonistManager.AdultStories[AdultStoryIndex];
            if (Genstep_ColonistCreationMod.BaseStats[0] == false)
            {
                ResetSkills(colonist);
            }
            GUI.Label(position, colonist.Backstory[1].StoryName.Translate());
        }

        private static void AdultStoryPrevious(Rect position, Colonist colonist, Rect rect7, int slot)
        {
            AdultStoryIndex = AdultStoryIndex - 1;
            if (AdultStoryIndex < 0)
            {
                AdultStoryIndex = ColonistManager.AdultStories.Count - 1;
            }
            colonist.Backstory[1] = ColonistManager.AdultStories[AdultStoryIndex];
            if (Genstep_ColonistCreationMod.BaseStats[0] == false)
            {
                ResetSkills(colonist);
            }
            GUI.Label(position, colonist.Backstory[1].StoryName.Translate());
        }

        private static void DrawSkills(Rect innerRect, Colonist colonist)
        {
            GUI.BeginGroup(innerRect);
            GenFont.SetFontMedium();
            GUI.Label(new Rect(0f, 0f, 200f, 30f), Language.FindText(LoadLanguage.language, "Skills"));
            GUI.Label(new Rect(0f, 35f, 200f, 30f), Language.FindText(LoadLanguage.language, "Skill Points") + ":" + colonist.SkillPool.ToString().Translate());
            DrawSkillsOf(colonist, new Vector2(0f, 70f));
            GUI.EndGroup();
        }

        private static void DrawSkillsOf(Colonist c, Vector2 Offset)
        {
            GenFont.SetFontSmall();
            int num = 0;
            
            for (int i = 0; i < 11; i++)
            {
                float y = (float)num * 27f + Offset.y;
                DrawSkill(i, new Vector2(Offset.x, y), c);
                num++;
            }
        }

        private static void DrawSkill(int index, Vector2 topLeft, Colonist colonist)
        {
            float num = 0f;
            num = 180f;
            float width = num - 180f - 6f;
            Rect rect = new Rect(topLeft.x, topLeft.y, num, 24f);
            if (rect.Contains(Event.current.mousePosition))
            {
                GUI.DrawTexture(rect, HighlightColTex);
            }

            GUI.BeginGroup(rect);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            Rect position2 = new Rect(0f, 0f, 100f, rect.height);
            GUI.Label(position2, Language.FindText(LoadLanguage.language, colonist.Skills[index].SkillName) + ": ");

            Rect position3 = new Rect(120f, 0f, rect.height, rect.height);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            string text;

            text = colonist.Skills[index].SkillValue.ToString();
            int SkillUpdate = 0;

            try
            {
                foreach (StorySkillGain a in colonist.Backstory[0].SkillGains)
                {
                    if (colonist.Skills[index].SkillName == a.SkillName)
                    {
                        SkillUpdate = SkillUpdate + a.SkillValue;
                        text = SkillUpdate.ToString();
                    }
                }

                foreach (StorySkillGain b in colonist.Backstory[1].SkillGains)
                {
                    if (colonist.Skills[index].SkillName == b.SkillName)
                    {
                        SkillUpdate = SkillUpdate + b.SkillValue;
                        text = SkillUpdate.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Message(e.Message);
            }

            List<string> list = new List<string>();
            if (colonist.Backstory[0].WorkRestrictions.Count > 0)
            {
                for (int a = 0; a < colonist.Backstory[0].WorkRestrictions.Count; a++)
                {
                    if (colonist.Backstory[0].WorkRestrictions[a] == "Art")
                    {
                        string Restriction = "Artistic";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Warden")
                    {
                        string Restriction = "Social";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Doctor")
                    {
                        string Restriction = "Medicine";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Caring")
                    {
                        string Restriction = "Medicine";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Plant work")
                    {
                        string Restriction = "Growing";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Violent")
                    {
                        string Restriction = "Melee";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                        string Restriction2 = "Shooting";
                        if (!list.Contains(Restriction2))
                        {
                            list.Add(Restriction2);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Hunting")
                    {
                        string Restriction = "Melee";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                        string Restriction2 = "Shooting";
                        if (!list.Contains(Restriction2))
                        {
                            list.Add(Restriction2);
                        }
                    }
                    else if (colonist.Backstory[0].WorkRestrictions[a] == "Intellectual")
                    {
                        string Restriction = "Research";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else
                    {
                        list.Add(colonist.Backstory[0].WorkRestrictions[a]);
                    }
                }
            }

            if (colonist.Backstory[1].WorkRestrictions.Count > 0)
            {
                for (int a = 0; a < colonist.Backstory[1].WorkRestrictions.Count; a++)
                {
                    if (colonist.Backstory[1].WorkRestrictions[a] == "Art")
                    {
                        string Restriction = "Artistic";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Warden")
                    {
                        string Restriction = "Social";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Doctor")
                    {
                        string Restriction = "Medicine";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Caring")
                    {
                        string Restriction = "Medicine";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Plant work")
                    {
                        string Restriction = "Growing";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Violent")
                    {
                        string Restriction = "Melee";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                        string Restriction2 = "Shooting";
                        if (!list.Contains(Restriction2))
                        {
                            list.Add(Restriction2);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Hunting")
                    {
                        string Restriction = "Melee";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                        string Restriction2 = "Shooting";
                        if (!list.Contains(Restriction2))
                        {
                            list.Add(Restriction2);
                        }
                    }
                    else if (colonist.Backstory[1].WorkRestrictions[a] == "Intellectual")
                    {
                        string Restriction = "Research";
                        if (!list.Contains(Restriction))
                        {
                            list.Add(Restriction);
                        }
                    }
                    else
                    {
                        list.Add(colonist.Backstory[1].WorkRestrictions[a]);
                    }
                }
            }

            if (list.Contains(colonist.Skills[index].SkillName))
            {
                GUI.color = DisabledSkillColor;
                text = "-";
            }
            else
            {
                text = (colonist.Skills[index].SkillValue + SkillUpdate).ToString();
                Rect position4 = new Rect(85f, 0f, 24f, 24f);
                if (Widgets.TextButton(position4, "-"))
                {
                    SkillDown(rect, index, colonist, SkillUpdate);
                }
                position3.x += 10;
                Rect position5 = new Rect(155f, 0f, 24f, 24f);
                if (Widgets.TextButton(position5, "+"))
                {
                    SkillUp(rect, index, colonist, SkillUpdate);
                }
            }

            GUI.Label(position3, text.Translate());
            GUI.color = Color.white;
            GUI.EndGroup();

            //TooltipHandler.TipRegion(rect, colonist.Skills[index].SkillDescription);
        }

        private static void SkillUp(Rect rect, int index, Colonist colonist, int SkillUpdate)
        {
            if (Genstep_ColonistCreationMod.BaseStats[0] == false)
            {
                if (colonist.SkillPool > 0 && colonist.Skills[index].SkillValue + SkillUpdate < 15)
                {
                    Rect pos = new Rect(120f, 0f, rect.height, rect.height);
                    colonist.Skills[index].SkillValue = colonist.Skills[index].SkillValue + 1;
                    colonist.SkillPool = colonist.SkillPool - 1;
                    GUI.Label(new Rect(0f, 35f, 200f, 30f), "Skill Points: ".Translate() + colonist.SkillPool.ToString());
                    string text = GenString.NumberString(colonist.Skills[index].SkillValue);
                    GUI.Label(pos, text);
                }
            }
            else
            {
                if (colonist.SkillPool > 0)
                {
                    Rect pos = new Rect(120f, 0f, rect.height, rect.height);
                    colonist.Skills[index].SkillValue = colonist.Skills[index].SkillValue + 1;
                    colonist.SkillPool = colonist.SkillPool - 1;
                    GUI.Label(new Rect(0f, 35f, 200f, 30f), "Skill Points: ".Translate() + colonist.SkillPool.ToString());
                    string text = GenString.NumberString(colonist.Skills[index].SkillValue);
                    GUI.Label(pos, text);
                }
            }
        }

        private static void SkillDown(Rect rect, int index, Colonist colonist, int SkillUpdate)
        {
            if (colonist.Skills[index].SkillValue > 0)
            {
                Rect pos = new Rect(120f, 0f, rect.height, rect.height);
                colonist.Skills[index].SkillValue = colonist.Skills[index].SkillValue - 1;
                colonist.SkillPool = colonist.SkillPool + 1;
                GUI.Label(new Rect(0f, 35f, 200f, 30f), "Skill Points: ".Translate() + colonist.SkillPool.ToString());
                string text = GenString.NumberString(colonist.Skills[index].SkillValue);
                GUI.Label(pos, text);
            }
        }

        private static void DrawStyle(Rect innerRect3, Colonist colonist)
        {
            GUI.BeginGroup(innerRect3);
            GenFont.SetFontMedium();
            GUI.Label(new Rect(0f, 0f, 200f, 30f), Language.FindText(LoadLanguage.language, "Style"));

            //Preview
            Rect position = new Rect(40f, 60f, 120f, 160f);
            GUI.Box(position, "");
            common.CreatePawnPreview(colonist, position.x, position.y + 20, colonist.BodyType.ToString(), colonist.HeadGraphicPath, colonist.Clothing[0], colonist.Clothing[1], colonist.HairDef.GraphicPath, colonist.SkinColor, colonist.Clothing[0].Color, colonist.Clothing[1].Color, colonist.HairColor);

            //Buttons
            GUI.color = Color.white;
            if (Widgets.TextButton(new Rect(0f, position.height + 90f, 200f, 30f), Language.FindText(LoadLanguage.language, "Change Clothing")))
            {
                Find.LayerStack.Add(new ChangeClothing(colonist));
            }
            if (Widgets.TextButton(new Rect(0f, position.height + 140f, 200f, 30f), Language.FindText(LoadLanguage.language, "Change Head")))
            {
                Find.LayerStack.Add(new ChangeHead(colonist));
            }
            if (Widgets.TextButton(new Rect(0f, position.height + 190f, 200f, 30f), Language.FindText(LoadLanguage.language, "Change Body")))
            {
                Find.LayerStack.Add(new ChangeBody(colonist));
            }

            for (int d = 0; d < ColonistManager.WeaponList.Count - 1; d++)
            {
                if (col.Weapon == ColonistManager.WeaponList[d])
                {
                    WeaponIndex = d;
                }
            }

            //Weapon
            float posHeight = position.height + 245f;
            GenFont.SetFontSmall();
            position = new Rect(0f, posHeight, 60f, 20f);
            GUI.Label(position, Language.FindText(LoadLanguage.language, "Weapon") + ":");

            Rect position2 = new Rect(94f, posHeight, 100f, 24f);
            position = new Rect(65f, posHeight, 24f, 24f);
            if (Widgets.TextButton(position, "<"))
            {
                WeaponDown(position2, colonist);
            }
            GUI.Label(position2, ColonistManager.WeaponList[WeaponIndex].Translate());
            position = new Rect(198f, posHeight, 24f, 24f);
            if (Widgets.TextButton(position, ">"))
            {
                WeaponUp(position2, colonist);
            }
            GUI.EndGroup();
        }

        private static void DrawPassions(Rect rect, Colonist colonist)
        {
            GUI.BeginGroup(rect);

            GenFont.SetFontMedium();
            Rect rect2 = new Rect(0f, 30f, 100f, 30f);
            Rect rect3 = new Rect(96f, 60f, 24f, 24f);
            Rect rect4 = new Rect(125f, 60f, 115f, 24f);
            Rect rect5 = new Rect(211f, 60f, 24f, 24f);
            Rect rect6 = new Rect(125f, 86f, 115f, 24f);
            Rect rect7 = new Rect(125f, 112f, 115f, 24f);

            GUI.Label(rect2, Language.FindText(LoadLanguage.language, "Passions"));
            rect2.y += 30;
            rect2.height = 24;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorSkill = colonist.Skills[i].SkillName;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorSkill1 = colonist.Skills[i].SkillName;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorSkill2 = colonist.Skills[i].SkillName;
                }
            }

            GenFont.SetFontSmall();
            GUI.Label(rect2, Language.FindText(LoadLanguage.language, "Major Passion") + ":");
            if (Widgets.TextButton(rect3, "<"))
            {
                MajorSkillDown(rect4, colonist);
            }
            GUI.Label(rect4, Language.FindText(LoadLanguage.language, MajorSkill));
            if (Widgets.TextButton(rect5, ">"))
            {
                MajorSkillUp(rect4, colonist);
            }

            rect2.y += 26;
            rect3.y += 26;
            rect5.y += 26;
            GUI.Label(rect2, Language.FindText(LoadLanguage.language, "Minor Passion") + ":");
            if (Widgets.TextButton(rect3, "<"))
            {
                MinorSkill1Down(rect6, colonist);
            }
            GUI.Label(rect6, Language.FindText(LoadLanguage.language, MinorSkill1));
            if (Widgets.TextButton(rect5, ">"))
            {
                MinorSkill1Up(rect6, colonist);
            }

            rect2.y += 26;
            rect3.y += 26;
            rect5.y += 26;
            GUI.Label(rect2, Language.FindText(LoadLanguage.language, "Minor Passion") + ":");
            if (Widgets.TextButton(rect3, "<"))
            {
                MinorSkill2Down(rect7, colonist);
            }
            GUI.Label(rect7, Language.FindText(LoadLanguage.language, MinorSkill2));
            if (Widgets.TextButton(rect5, ">"))
            {
                MinorSkill2Up(rect7, colonist);
            }

            GUI.EndGroup();
        }

        private static void MajorSkillUp(Rect rect4, Colonist colonist)
        {
            int MajorIndex = 0;
            int MinorIndex1 = 0;
            int MinorIndex2 = 0;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorIndex = i;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorIndex1 = i;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorIndex2 = i;
                }
            }

            do
            {
                if (colonist.Skills[MajorIndex].SkillPassion == 1)
                {
                    colonist.Skills[MajorIndex].SkillPassion = 0;
                }
                MajorIndex = MajorIndex + 1;
                if (MajorIndex > 10)
                {
                    MajorIndex = 0;
                }
                if (MajorIndex != MinorIndex1 && MajorIndex != MinorIndex2)
                {
                    colonist.Skills[MajorIndex].SkillPassion = 1;
                }
            } while (MajorIndex == MinorIndex1 || MajorIndex == MinorIndex2);
            
            GUI.Label(rect4, Language.FindText(LoadLanguage.language, colonist.Skills[MajorIndex].SkillName));
        }

        private static void MajorSkillDown(Rect rect4, Colonist colonist)
        {
            int MajorIndex = 0;
            int MinorIndex1 = 0;
            int MinorIndex2 = 0;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorIndex = i;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorIndex1 = i;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorIndex2 = i;
                }
            }

            do
            {
                if (colonist.Skills[MajorIndex].SkillPassion == 1)
                {
                    colonist.Skills[MajorIndex].SkillPassion = 0;
                }
                MajorIndex = MajorIndex - 1;
                if (MajorIndex < 0)
                {
                    MajorIndex = 10;
                }
                if (MajorIndex != MinorIndex1 && MajorIndex != MinorIndex2)
                {
                    colonist.Skills[MajorIndex].SkillPassion = 1;
                }
            } while (MajorIndex == MinorIndex1 || MajorIndex == MinorIndex2);

            GUI.Label(rect4, Language.FindText(LoadLanguage.language, colonist.Skills[MajorIndex].SkillName));
        }

        private static void MinorSkill1Up(Rect rect6, Colonist colonist)
        {
            int MajorIndex = 0;
            int MinorIndex1 = 0;
            int MinorIndex2 = 0;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorIndex = i;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorIndex1 = i;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorIndex2 = i;
                }
            }

            do
            {
                if (colonist.Skills[MinorIndex1].SkillPassion == 2)
                {
                    colonist.Skills[MinorIndex1].SkillPassion = 0;
                }
                MinorIndex1 = MinorIndex1 + 1;
                if (MinorIndex1 > 10)
                {
                    MinorIndex1 = 0;
                }
                if (MinorIndex1 != MajorIndex && MinorIndex1 != MinorIndex2)
                {
                    colonist.Skills[MinorIndex1].SkillPassion = 2;
                }
            } while (MinorIndex1 == MajorIndex || MinorIndex1 == MinorIndex2);

            GUI.Label(rect6, Language.FindText(LoadLanguage.language, colonist.Skills[MinorIndex1].SkillName));
        }

        private static void MinorSkill1Down(Rect rect6, Colonist colonist)
        {
            int MajorIndex = 0;
            int MinorIndex1 = 0;
            int MinorIndex2 = 0;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorIndex = i;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorIndex1 = i;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorIndex2 = i;
                }
            }

            do
            {
                if (colonist.Skills[MinorIndex1].SkillPassion == 2)
                {
                    colonist.Skills[MinorIndex1].SkillPassion = 0;
                }
                MinorIndex1 = MinorIndex1 - 1;
                if (MinorIndex1 < 0)
                {
                    MinorIndex1 = 10;
                }
                if (MinorIndex1 != MajorIndex && MinorIndex1 != MinorIndex2)
                {
                    colonist.Skills[MinorIndex1].SkillPassion = 2;
                }
            } while (MinorIndex1 == MajorIndex || MinorIndex1 == MinorIndex2);

            GUI.Label(rect6, Language.FindText(LoadLanguage.language, colonist.Skills[MinorIndex1].SkillName));
        }

        private static void MinorSkill2Up(Rect rect7, Colonist colonist)
        {
            int MajorIndex = 0;
            int MinorIndex1 = 0;
            int MinorIndex2 = 0;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorIndex = i;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorIndex1 = i;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorIndex2 = i;
                }
            }

            do
            {
                if (colonist.Skills[MinorIndex2].SkillPassion == 3)
                {
                    colonist.Skills[MinorIndex2].SkillPassion = 0;
                }
                MinorIndex2 = MinorIndex2 + 1;
                if (MinorIndex2 > 10)
                {
                    MinorIndex2 = 0;
                }
                if (MinorIndex2 != MajorIndex && MinorIndex2 != MinorIndex1)
                {
                    colonist.Skills[MinorIndex2].SkillPassion = 3;
                }
            } while (MinorIndex2 == MajorIndex || MinorIndex2 == MinorIndex1);

            GUI.Label(rect7, Language.FindText(LoadLanguage.language, colonist.Skills[MinorIndex2].SkillName));
        }

        private static void MinorSkill2Down(Rect rect7, Colonist colonist)
        {
            int MajorIndex = 0;
            int MinorIndex1 = 0;
            int MinorIndex2 = 0;

            for (int i = 0; i < colonist.Skills.Count; i++)
            {
                if (colonist.Skills[i].SkillPassion == 1)
                {
                    MajorIndex = i;
                }
                if (colonist.Skills[i].SkillPassion == 2)
                {
                    MinorIndex1 = i;
                }
                if (colonist.Skills[i].SkillPassion == 3)
                {
                    MinorIndex2 = i;
                }
            }

            do
            {
                if (colonist.Skills[MinorIndex2].SkillPassion == 3)
                {
                    colonist.Skills[MinorIndex2].SkillPassion = 0;
                }
                MinorIndex2 = MinorIndex2 - 1;
                if (MinorIndex2 < 0)
                {
                    MinorIndex2 = 10;
                }
                if (MinorIndex2 != MajorIndex && MinorIndex2 != MinorIndex1)
                {
                    colonist.Skills[MinorIndex2].SkillPassion = 3;
                }
            } while (MinorIndex2 == MajorIndex || MinorIndex2 == MinorIndex1);

            GUI.Label(rect7, Language.FindText(LoadLanguage.language, colonist.Skills[MinorIndex2].SkillName));
        }

        private static void GenderChange(Rect position, Colonist colonist)
        {
            string gender = "";
            if (colonist.Gender == 1)
            {
                colonist.Gender = 2;
                gender = "Female";
            }
            else
            {
                colonist.Gender = 1;
                gender = "Male";
            }
            GUI.Label(position, Language.FindText(LoadLanguage.language, gender));

            //Reset Body Shape
            if (colonist.Gender == 1)
            {
                int body = UnityEngine.Random.Range(1, 4);
                if (body == 1)
                {
                    colonist.BodyType = BodyType.Male;
                }
                else if (body == 2)
                {
                    colonist.BodyType = BodyType.Thin;
                }
                else if (body == 3)
                {
                    colonist.BodyType = BodyType.Hulk;
                }
                else if (body == 4)
                {
                    colonist.BodyType = BodyType.Fat;
                }
            }
            else
            {
                int body = UnityEngine.Random.Range(1, 4);
                if (body == 1)
                {
                    colonist.BodyType = BodyType.Female;
                }
                else if (body == 2)
                {
                    colonist.BodyType = BodyType.Thin;
                }
                else if (body == 3)
                {
                    colonist.BodyType = BodyType.Hulk;
                }
                else if (body == 4)
                {
                    colonist.BodyType = BodyType.Fat;
                }
            }

            //Reset Hair
            GenHairList();
            ColHairDef hairDef = new ColHairDef();
            if (colonist.Gender == 1)
            {
                hairDef = MaleHairList.RandomListElement<ColHairDef>();
            }
            else if (colonist.Gender == 2)
            {
                hairDef = FemaleHairList.RandomListElement<ColHairDef>();
            }
            colonist.HairDef = hairDef;

            //Reset Adult Backstory
            ResetStoryList(colonist);
        }

        private static void AgeUp(Rect position, Colonist colonist)
        {
            if (colonist.Age < 70)
            {
                colonist.Age = colonist.Age + 1;
                GUI.Label(position, colonist.Age.ToString());
            }
        }

        private static void AgeDown(Rect position, Colonist colonist)
        {
            if (colonist.Age > 16)
            {
                colonist.Age = colonist.Age - 1;
                GUI.Label(position, colonist.Age.ToString());
            }
        }

        private static void Trait1Up(Rect position, Colonist colonist)
        {
            Trait1Index = Trait1Index + 1;
            if (Trait1Index > allTraits.Count - 1)
            {
                Trait1Index = 0;
            }
            if (allTraits[Trait1Index].TraitName == colonist.Traits[1].TraitName)
            {
                Trait1Index = Trait1Index + 1;
            }
            colonist.Traits[0].TraitName = allTraits[Trait1Index].TraitName;
            GUI.Label(position, colonist.Traits[0].TraitName);
        }

        private static void Trait1Down(Rect position, Colonist colonist)
        {
            Trait1Index = Trait1Index - 1;
            if (Trait1Index < 0)
            {
                Trait1Index = allTraits.Count - 1;
            }
            if (allTraits[Trait1Index].TraitName == colonist.Traits[1].TraitName)
            {
                Trait1Index = Trait1Index - 1;
            }
            colonist.Traits[0].TraitName = allTraits[Trait1Index].TraitName;
            GUI.Label(position, colonist.Traits[0].TraitName);
        }

        private static void Trait2Up(Rect position, Colonist colonist)
        {
            Trait2Index = Trait2Index + 1;
            if (Trait2Index > allTraits.Count - 1)
            {
                Trait2Index = 0;
            }
            if (colonist.Traits[0].TraitName == allTraits[Trait2Index].TraitName)
            {
                Trait2Index = Trait2Index + 1;
            }
            colonist.Traits[1].TraitName = allTraits[Trait2Index].TraitName;
            GUI.Label(position, colonist.Traits[1].TraitName);
        }

        private static void Trait2Down(Rect position, Colonist colonist)
        {
            Trait2Index = Trait2Index - 1;
            if (Trait2Index < 0)
            {
                Trait2Index = allTraits.Count - 1;
            }
            if (colonist.Traits[0].TraitName == allTraits[Trait2Index].TraitName)
            {
                Trait2Index = Trait2Index - 1;
            }
            colonist.Traits[1].TraitName = allTraits[Trait2Index].TraitName;
            GUI.Label(position, colonist.Traits[1].TraitName);
        }

        private static void WeaponUp(Rect position, Colonist colonist)
        {
            WeaponIndex = WeaponIndex + 1;
            if (WeaponIndex > ColonistManager.WeaponList.Count - 1)
            {
                WeaponIndex = 0;
            }
            colonist.Weapon = ColonistManager.WeaponList[WeaponIndex];
            GUI.Label(position, colonist.Weapon);
        }

        private static void WeaponDown(Rect position, Colonist colonist)
        {
            WeaponIndex = WeaponIndex - 1;
            if (WeaponIndex < 0 )
            {
                WeaponIndex = ColonistManager.WeaponList.Count - 1;
            }
            colonist.Weapon = ColonistManager.WeaponList[WeaponIndex];
            GUI.Label(position, colonist.Weapon);
        }

        private static void ResetStoryList(Colonist colonist)
        {
            ColonistManager.AdultStories.Clear();
            int adultStoryIndex = 0;
            for (int a = 0; a < ColonistManager.AllBackstories.Count - 1; a++)
            {
                bool AdultStoryFound = false;

                Colonist_Backstory story = new Colonist_Backstory();

                if (colonist.Gender == 1)
                {
                    if (ColonistManager.AllBackstories[a].slot == BackstorySlot.Adulthood)
                    {
                        if (ColonistManager.AllBackstories[a].bodyTypeMale == colonist.BodyType || ColonistManager.AllBackstories[a].bodyTypeGlobal == colonist.BodyType)
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
                else if (colonist.Gender == 2)
                {
                    if (ColonistManager.AllBackstories[a].slot == BackstorySlot.Adulthood)
                    {
                        if (ColonistManager.AllBackstories[a].bodyTypeFemale == colonist.BodyType || ColonistManager.AllBackstories[a].bodyTypeGlobal == colonist.BodyType)
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
            }
            List<Colonist_Backstory> ColonistStory = new List<Colonist_Backstory>();
            ColonistStory.Add(ColonistManager.ChildStories[colonist.Backstory[0].StoryIndex]);
            ColonistStory.Add(ColonistManager.AdultStories[UnityEngine.Random.Range(0, ColonistManager.AdultStories.Count - 1)]);
            colonist.Backstory = ColonistStory;
            AdultStoryIndex = colonist.Backstory[1].StoryIndex;
        }

        private static void ResetSkills(Colonist colonist)
        {
            for (int i = 0; i < colonist.Skills.Count - 1; i++)
            {
                colonist.Skills[i].SkillValue = 0;
            }

            if (ColonistDifficulty.Difficulty == "Easy")
            {
                colonist.SkillPool = 40;
            }
            else if (ColonistDifficulty.Difficulty == "Normal")
            {
                colonist.SkillPool = 20;
            }
            else
            {
                colonist.SkillPool = 0;
            }
        }

        private static void GenHairList()
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

        private static void ColonistToPawn()
        {
            for (int p = 0; p < ModdedMapInitParams.colonists.Count; p++)
            {
                ModdedMapInitParams.colonists[p].age = ColonistManager.Population[p].Age;
                ModdedMapInitParams.colonists[p].gender = (Gender)ColonistManager.Population[p].Gender;
                ModdedMapInitParams.colonists[p].Name.first = ColonistManager.Population[p].FirstName;
                ModdedMapInitParams.colonists[p].Name.nick = ColonistManager.Population[p].NickName;
                ModdedMapInitParams.colonists[p].Name.last = ColonistManager.Population[p].LastName;
                ModdedMapInitParams.colonists[p].story.skinColor = ColonistManager.Population[p].SkinColor;
                ModdedMapInitParams.colonists[p].story.crownType = ColonistManager.Population[p].CrownType;
                ModdedMapInitParams.colonists[p].story.headGraphicPath = ColonistManager.Population[p].HeadGraphicPath;
                ModdedMapInitParams.colonists[p].story.hairColor = ColonistManager.Population[p].HairColor;

                foreach (TraitDef trait in DefDatabase<TraitDef>.AllDefsListForReading)
                {
                    if (trait.label == ColonistManager.Population[p].Traits[0].TraitName)
                    {
                        ModdedMapInitParams.colonists[p].story.traits.allTraits[0].def = trait;
                    }
                    else if (trait.label == ColonistManager.Population[p].Traits[1].TraitName)
                    {
                        ModdedMapInitParams.colonists[p].story.traits.allTraits[1].def = trait;
                    }
                }

                IEnumerable<HairDef> enumerable =
                from hair in DefDatabase<HairDef>.AllDefs
                select hair;
                foreach (HairDef current in enumerable)
                {
                    if (current.defName == ColonistManager.Population[p].HairDef.DefName)
                    {
                        ModdedMapInitParams.colonists[p].story.hairDef = current;
                    }
                }

                foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
                {
                    if (current.apparel != null)
                    {
                        Apparel apparel = (Apparel)ThingMaker.MakeThing(current);
                        if (apparel.Label == ColonistManager.Population[p].Clothing[0].Label && apparel.def.apparel.Layer.ToString() == "OnSkin")
                        {
                            apparel.color = ColonistManager.Population[p].Clothing[0].Color;
                            ModdedMapInitParams.colonists[p].apparel.SetApparel(apparel);
                            break;
                        }
                    }
                }

                foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
                {
                    if (current.apparel != null)
                    {
                        Apparel apparel = (Apparel)ThingMaker.MakeThing(current);
                        if (apparel.Label == ColonistManager.Population[p].Clothing[1].Label && apparel.def.apparel.Layer.ToString() == "Shell")
                        {
                            apparel.color = ColonistManager.Population[p].Clothing[1].Color;
                            ModdedMapInitParams.colonists[p].apparel.SetApparel(apparel);
                            break;
                        }
                    }
                }
                
                foreach (StorySkillGain a in ColonistManager.Population[p].Backstory[0].SkillGains)
                {
                    for (int s = 0; s < ColonistManager.Population[p].Skills.Count; s++)
                    {
                        if (ColonistManager.Population[p].Skills[s].SkillName == a.SkillName)
                        {
                            ColonistManager.Population[p].Skills[s].SkillValue += a.SkillValue;
                            if (ColonistManager.Population[p].Skills[s].SkillValue < 0)
                            {
                                ColonistManager.Population[p].Skills[s].SkillValue = 0;
                            }
                        }
                    }

                }

                foreach (StorySkillGain b in ColonistManager.Population[p].Backstory[1].SkillGains)
                {
                    for (int s = 0; s < ColonistManager.Population[p].Skills.Count; s++)
                    {
                        if (ColonistManager.Population[p].Skills[s].SkillName == b.SkillName)
                        {
                            ColonistManager.Population[p].Skills[s].SkillValue += b.SkillValue;
                            if (ColonistManager.Population[p].Skills[s].SkillValue < 0)
                            {
                                ColonistManager.Population[p].Skills[s].SkillValue = 0;
                            }
                        }
                    }
                }

                for (int i = 0; i < ColonistManager.Population[p].Skills.Count; i++)
                {
                    if (ColonistManager.Population[p].Skills[i].SkillPassion == 1)
                    {
                        MajorSkill = ColonistManager.Population[p].Skills[i].SkillName;
                    }
                    if (ColonistManager.Population[p].Skills[i].SkillPassion == 2)
                    {
                        MinorSkill1 = ColonistManager.Population[p].Skills[i].SkillName;
                    }
                    if (ColonistManager.Population[p].Skills[i].SkillPassion == 3)
                    {
                        MinorSkill2 = ColonistManager.Population[p].Skills[i].SkillName;
                    }
                }

                for (int i = 0; i < ModdedMapInitParams.colonists[p].skills.skills.Count; i++)
                {
                    if (ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel == ColonistManager.Population[p].Skills[i].SkillName)
                    {
                        ModdedMapInitParams.colonists[p].skills.skills[i].level = ColonistManager.Population[p].Skills[i].SkillValue;
                    }

                    if (ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel == MajorSkill)
                    {
                        ModdedMapInitParams.colonists[p].skills.skills[i].passion = (Passion)2;
                    }
                    if (ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel == MinorSkill1)
                    {
                        ModdedMapInitParams.colonists[p].skills.skills[i].passion = (Passion)1;
                    }
                    if (ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel == MinorSkill2)
                    {
                        ModdedMapInitParams.colonists[p].skills.skills[i].passion = (Passion)1;
                    }
                    if (ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel != MajorSkill && ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel != MinorSkill1 && ModdedMapInitParams.colonists[p].skills.skills[i].def.skillLabel != MinorSkill2)
                    {
                        ModdedMapInitParams.colonists[p].skills.skills[i].passion = (Passion)0;
                    }
                }

                for (int i = 0; i < BackstoryDatabase.allBackstories.Count; i++)
                {
                    if (BackstoryDatabase.allBackstories.ElementAt(i).Value.title == ColonistManager.Population[p].Backstory[0].StoryName)
                    {
                        ModdedMapInitParams.colonists[p].story.childhood = BackstoryDatabase.allBackstories.ElementAt(i).Value;
                    }
                    
                }

                for (int i = 0; i < BackstoryDatabase.allBackstories.Count; i++)
                {
                    if (BackstoryDatabase.allBackstories.ElementAt(i).Value.title == ColonistManager.Population[p].Backstory[1].StoryName)
                    {
                        ModdedMapInitParams.colonists[p].story.adulthood = BackstoryDatabase.allBackstories.ElementAt(i).Value;
                    }
                }

                foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
                {
                    if (current.isGun == true && current.equipmentType == EquipmentType.Primary && current.canBeSpawningInventory)
                    {
                        Equipment equipment = new Equipment();
                        equipment = (Equipment)ThingMaker.MakeThing(current);
                        if (ColonistManager.Population[p].Weapon == equipment.LabelShort)
                        {
                            ModdedMapInitParams.colonists[p].equipment.AddEquipment(equipment);
                        }
                    }
                }
            }
        }
    }
}
