using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using Verse;

namespace ColonistCreationMod
{
    class LoadColonists
    {
        private static string loadDataPath = null;

        public static string FilePathForLoadchars(string groupName)
        {
            return Path.Combine(CharLoadGamesFolderPath, groupName + ".col");
        }

        public static string CharLoadGamesFolderPath
        {
            get
            {
                string text = Path.Combine(CharLoadDataFolderPath, "CharSaves");
                DirectoryInfo directoryInfo = new DirectoryInfo(text);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                return text;
            }
        }

        public static string CharLoadDataFolderPath
        {
            get
            {
                if (loadDataPath == null)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
                    if (Application.isEditor)
                    {
                        loadDataPath = Path.Combine(directoryInfo.Parent.ToString(), "LoadData");
                    }
                    else
                    {
                        loadDataPath = Application.persistentDataPath;
                    }
                    DirectoryInfo directoryInfo2 = new DirectoryInfo(loadDataPath);
                    if (!directoryInfo2.Exists)
                    {
                        directoryInfo2.Create();
                    }
                }
                return loadDataPath;
            }
        }

        public static void LoadFromFile(Map map, string groupName)
        {
            ColonistRegen(FilePathForLoadchars(groupName));
            Parse(FilePathForLoadchars(groupName));
            GC.Collect();
        }

        private static void ColonistRegen(string file)
        {
            using (XmlTextReader reader = new XmlTextReader(File.OpenRead(file)))
            {
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "ColonistAmount":
                            ColonistManager.RandomColonists();
                            ModdedMapInitParams.GenerateColonists();
                            break;
                    }
                }
            }
        }

        private static int VisitAmount(XmlTextReader reader)
        {
            int amount = 0;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "amount":
                        amount = int.Parse(reader.Value);
                        break;
                }
            }

            return amount;
        }

        private static void Parse(string file)
        {
            using (XmlTextReader reader = new XmlTextReader(File.OpenRead(file)))
            {
                int i = 0;
                while (reader.Read())
                {
                    if (i < ColonistNum.Amount)
                    {
                        switch (reader.Name)
                        {
                            case "Colonist":
                                VisitColonist(reader, i);
                                i++;
                                break;
                        }
                    }
                }
            }
        }

        private static void VisitColonist(XmlTextReader reader, int i)
        {
            while (reader.Read())
            {
                if (reader.Name == "Colonist" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                switch (reader.Name)
                {
                    case "BasicInfo":
                        VisitBasicInfo(reader, i);
                        break;

                    case "Graphics":
                        VisitGraphics(reader, i);
                        break;

                    case "HairDef":
                        VisitHairDef(reader, i);
                        break;

                    case "OnSkin":
                        VisitOnSkin(reader, i);
                        break;

                    case "Shell":
                        VisitShell(reader, i);
                        break;

                    case "Childhood":
                        VisitChildhood(reader, i);
                        break;

                    case "Adulthood":
                        VisitAdulthood(reader, i);
                        break;

                    case "Traits":
                        VisitTraits(reader, i);
                        break;

                    case "SkillPool":
                        VisitSkillPool(reader, i);
                        break;

                    case "Skill0":
                        VisitSkill0(reader, i);
                        break;

                    case "Skill1":
                        VisitSkill1(reader, i);
                        break;

                    case "Skill2":
                        VisitSkill2(reader, i);
                        break;

                    case "Skill3":
                        VisitSkill3(reader, i);
                        break;

                    case "Skill4":
                        VisitSkill4(reader, i);
                        break;

                    case "Skill5":
                        VisitSkill5(reader, i);
                        break;

                    case "Skill6":
                        VisitSkill6(reader, i);
                        break;

                    case "Skill7":
                        VisitSkill7(reader, i);
                        break;

                    case "Skill8":
                        VisitSkill8(reader, i);
                        break;

                    case "Skill9":
                        VisitSkill9(reader, i);
                        break;

                    case "Skill10":
                        VisitSkill10(reader, i);
                        break;

                    case "Weapon":
                        VisitWeapon(reader, i);
                        break;
                }
            }
        }

        private static void VisitBasicInfo(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Name":
                        var parts = reader.Value.Split(' ');
                        ColonistManager.Population[i].FirstName = parts[0];
                        ColonistManager.Population[i].NickName = parts[1];
                        ColonistManager.Population[i].LastName = parts[2];
                        break;

                    case "Age":
                        ColonistManager.Population[i].Age = int.Parse(reader.Value);
                        break;

                    case "Gender":
                        ColonistManager.Population[i].Gender = int.Parse(reader.Value);
                        break;
                }
            }
        }

        private static void VisitGraphics(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "BodyType":
                        string body = reader.Value;
                        int bodyType = 0;
                        if (body == "Male")
                        {
                            bodyType = 1;
                        }
                        else if (body == "Female")
                        {
                            bodyType = 2;
                        }
                        else if (body == "Thin")
                        {
                            bodyType = 3;
                        }
                        else if (body == "Hulk")
                        {
                            bodyType = 4;
                        }
                        else if (body == "Fat")
                        {
                            bodyType = 5;
                        }
                        ColonistManager.Population[i].BodyType = (BodyType)bodyType;
                        break;

                    case "SkinColor":
                        ColonistManager.Population[i].SkinColor = GetColor(reader.Value);
                        break;

                    case "CrownType":
                        string crown = reader.Value;
                        int crownType = 0;
                        if (crown == "Average")
                        {
                            crownType = 1;
                        }
                        else if (crown == "Narrow")
                        {
                            crownType = 2;
                        }
                        ColonistManager.Population[i].CrownType = (CrownType)crownType;
                        break;

                    case "HeadGraphicPath":
                        ColonistManager.Population[i].HeadGraphicPath = reader.Value;
                        break;

                    case "HairColor":
                        ColonistManager.Population[i].HairColor = GetColor(reader.Value);
                        break;
                }
            }
        }

        private static void VisitHairDef(XmlTextReader reader, int i)
        {
            ColHairDef hairDef = new ColHairDef();
            
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "DefName":
                        hairDef.DefName = reader.Value;
                        break;

                    case "GraphicPath":
                        hairDef.GraphicPath = reader.Value;
                        break;

                    case "HairGender":
                        string hair = reader.Value;
                        int hairGender = 0;
                        if (hair == "Any")
                        {
                            hairGender = 2;
                        }
                        else if (hair == "Female")
                        {
                            hairGender = 4;
                        }
                        else if (hair == "FemaleUsually")
                        {
                            hairGender = 3;
                        }
                        else if (hair == "MaleUsually")
                        {
                            hairGender = 1;
                        }
                        hairDef.HairGender = (HairGender)hairGender;
                        break;

                    case "Label":
                        hairDef.Label = reader.Value;
                        break;

                    case "Tags":
                        hairDef.HairTags = GetTags(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].HairDef = hairDef;
        }

        private static List<string> GetTags(string tagList)
        {
            List<string> tags = tagList.Split(',').ToList<string>();
            return tags;
        }

        private static void VisitOnSkin(XmlTextReader reader, int i)
        {
            Clothing clothing = new Clothing();

            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Layer":
                        clothing.Layer = reader.Value;
                        break;

                    case "Label":
                        clothing.Label = reader.Value;
                        break;

                    case "GraphicPath":
                        clothing.GraphicPath = reader.Value;
                        break;

                    case "Color":
                        clothing.Color = GetColor(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Clothing[0] = clothing;
        }

        private static void VisitShell(XmlTextReader reader, int i)
        {
            Clothing clothing = new Clothing();

            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Layer":
                        clothing.Layer = reader.Value;
                        break;

                    case "Label":
                        clothing.Label = reader.Value;
                        break;

                    case "GraphicPath":
                        clothing.GraphicPath = reader.Value;
                        break;

                    case "Color":
                        clothing.Color = GetColor(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Clothing[1] = clothing;
        }

        private static Color GetColor(string colorValue)
        {
            var parts = colorValue.Split(',');
            Color color = new Color();
            color.r = Single.Parse(parts[0]);
            color.g = Single.Parse(parts[1]);
            color.b = Single.Parse(parts[2]);
            color.a = Single.Parse(parts[3]);
            
            return color;
        }

        private static void VisitChildhood(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Index":
                        ColonistManager.Population[i].Backstory[0] = ColonistManager.ChildStories[int.Parse(reader.Value)];
                        break;
                }
            }
        }

        private static void VisitAdulthood(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Index":
                        ResetStoryList(ColonistManager.Population[i], int.Parse(reader.Value));
                        break;
                }
            }
        }

        private static void VisitTraits(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Trait1":
                        ColonistManager.Population[i].Traits[0].TraitName = reader.Value;
                        break;

                    case "Trait2":
                        ColonistManager.Population[i].Traits[1].TraitName = reader.Value;
                        break;
                }
            }
        }

        private static void VisitSkillPool(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Amount":
                        ColonistManager.Population[i].SkillPool = int.Parse(reader.Value);
                        break;
                }
            }
        }

        private static void VisitSkill0(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Construction";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[0] = skill;
        }

        private static void VisitSkill1(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Growing";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[1] = skill;
        }

        private static void VisitSkill2(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Research";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[2] = skill;
        }

        private static void VisitSkill3(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Mining";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[3] = skill;
        }

        private static void VisitSkill4(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Shooting";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[4] = skill;
        }

        private static void VisitSkill5(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Melee";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[5] = skill;
        }

        private static void VisitSkill6(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Social";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[6] = skill;
        }

        private static void VisitSkill7(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Cooking";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[7] = skill;
        }

        private static void VisitSkill8(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Medicine";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[8] = skill;
        }

        private static void VisitSkill9(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Artistic";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[9] = skill;
        }

        private static void VisitSkill10(XmlTextReader reader, int i)
        {
            Colonist_Skill skill = new Colonist_Skill();
            skill.SkillName = "Crafting";
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Value":
                        skill.SkillValue = int.Parse(reader.Value);
                        break;

                    case "Passion":
                        skill.SkillPassion = int.Parse(reader.Value);
                        break;
                }
            }
            ColonistManager.Population[i].Skills[10] = skill;
        }

        private static void VisitWeapon(XmlTextReader reader, int i)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Name":
                        ColonistManager.Population[i].Weapon = reader.Value;
                        break;
                }
            }
        }

        private static void ResetStoryList(Colonist colonist, int index)
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
            ColonistStory.Add(ColonistManager.AdultStories[index]);
            colonist.Backstory = ColonistStory;
        }
    }
}
