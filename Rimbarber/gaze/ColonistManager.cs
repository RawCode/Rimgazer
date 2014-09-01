using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    public static class ColonistManager
    {
        private static int ChildStoryIndex = 0;
        private static int AdultStoryIndex = 0;
        private static List<Colonist> population = new List<Colonist>();
        private static List<Colonist_Backstory> childstories = new List<Colonist_Backstory>();
        private static List<Colonist_Backstory> adultstories = new List<Colonist_Backstory>();
        private static List<string> ColonistStuff = new List<string>();
        private static List<Clothing> ClothingList = new List<Clothing>();
        private static List<Clothing> ShirtList = new List<Clothing>();
        private static List<Clothing> CoatList = new List<Clothing>();
        private static List<ColHairDef> MaleHairList = new List<ColHairDef>();
        private static List<ColHairDef> FemaleHairList = new List<ColHairDef>();
        private static List<Backstory> allBackstories = new List<Backstory>();
        private static List<string> weaponList = new List<string>();

        #region Properties

        public static List<Colonist> Population
        {
            get { return population; }
            set { population = value; }
        }

        public static List<Colonist_Backstory> ChildStories
        {
            get { return childstories; }
            set { childstories = value; }
        }

        public static List<Colonist_Backstory> AdultStories
        {
            get { return adultstories; }
            set { adultstories = value; }
        }

        public static List<Backstory> AllBackstories
        {
            get { return allBackstories; }
            set { allBackstories = value; }
        }

        public static List<string> WeaponList
        {
            get { return weaponList; }
            set { weaponList = value; }
        }

        #endregion

        public static void RandomColonists()
        {
            Population.Clear();
            ShirtList.Clear();
            CoatList.Clear();
            ClothingList.Clear();
            allBackstories.Clear();

            NameBank nameBank = NameShuffleDatabase.BankOf((NameCategory)1);

            GenClothingList();
            GenHairList();

            //Load in Backstories from BackstoryDatabase
            for (int a = 0; a < BackstoryDatabase.allBackstories.Count - 1; a++)
            {
                allBackstories.Add(BackstoryDatabase.allBackstories.ElementAt(a).Value);
            }
            List<Backstory> SortedBackstories = allBackstories.OrderBy(o => o.title).ToList();
            allBackstories = SortedBackstories;

            //Generate new random colonists
            for (int i = 0; i < ColonistNum.Amount; i++)
            {
                Colonist colonist = new Colonist();

                //Gen random colonist basic info
                int gender = UnityEngine.Random.Range(1, 100);
                if (gender > 50)
                {
                    gender = 1;
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
                    gender = 2;
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
                colonist.Gender = gender;
                colonist.FirstName = nameBank.GetName((Gender)colonist.Gender, (NameSlot)0);
                colonist.NickName = nameBank.GetName((Gender)colonist.Gender, (NameSlot)2);
                colonist.LastName = nameBank.GetName((Gender)0, (NameSlot)1);
                colonist.Age = UnityEngine.Random.Range(16, 70);

                //Gen colonist starting skills
                List<Colonist_Skill> skills = new List<Colonist_Skill>();

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
                
                for (int a = 0; a < 11; a++)
                {
                    Colonist_Skill skill = new Colonist_Skill();
                    if (a == 0)
                    {
                        skill.SkillName = "Construction".Translate();
                        skill.SkillPassion = 1;
                    }
                    else if (a == 1)
                    {
                        skill.SkillName = "Growing".Translate();
                        skill.SkillPassion = 2;
                    }
                    else if (a == 2)
                    {
                        skill.SkillName = "Research".Translate();
                        skill.SkillPassion = 3;
                    }
                    else if (a == 3)
                    {
                        skill.SkillName = "Mining".Translate();
                    }
                    else if (a == 4)
                    {
                        skill.SkillName = "Shooting".Translate();
                    }
                    else if (a == 5)
                    {
                        skill.SkillName = "Melee".Translate();
                    }
                    else if (a == 6)
                    {
                        skill.SkillName = "Social".Translate();
                    }
                    else if (a == 7)
                    {
                        skill.SkillName = "Cooking".Translate();
                    }
                    else if (a == 8)
                    {
                        skill.SkillName = "Medicine".Translate();
                    }
                    else if (a == 9)
                    {
                        skill.SkillName = "Artistic".Translate();
                    }
                    else if (a == 10)
                    {
                        skill.SkillName = "Crafting".Translate();
                    }
                    skill.SkillValue = 0;

                    skills.Add(skill);
                }
                colonist.Skills = skills;

                //Load Backstories
                childstories.Clear();
                adultstories.Clear();
                ChildStoryIndex = 0;
                AdultStoryIndex = 0;

                List<Colonist_Backstory> ColonistStory = new List<Colonist_Backstory>();

                for (int a = 0; a < allBackstories.Count - 1; a++)
                {
                    bool ChildStoryFound = false;
                    bool AdultStoryFound = false;

                    Colonist_Backstory story = new Colonist_Backstory();

                    if (allBackstories[a].slot == BackstorySlot.Childhood)
                    {
                        story.StoryName = allBackstories[a].title;
                        story.StoryAge = 0;
                        story.BaseDescription = allBackstories[a].baseDesc;
                        story.BodyTypeFemale = allBackstories[a].bodyTypeFemale;
                        story.BodyTypeMale = allBackstories[a].bodyTypeMale;
                        story.BodyTypeGlobal = allBackstories[a].bodyTypeGlobal;
                        story.StoryIndex = ChildStoryIndex;

                        List<StorySkillGain> skillGains = new List<StorySkillGain>();
                        for (int b = 0; b < allBackstories[a].skillGainsResolved.Count - 1; b++)
                        {
                            StorySkillGain skillGain = new StorySkillGain();
                            skillGain.SkillName = allBackstories[a].skillGainsResolved.ElementAt(b).Key.skillLabel;
                            skillGain.SkillValue = allBackstories[a].skillGainsResolved.ElementAt(b).Value;
                            skillGains.Add(skillGain);
                        }
                        story.SkillGains = skillGains;

                        List<string> workRestrictions = new List<string>();
                        foreach (WorkTypeDef work in allBackstories[a].DisabledWorkTypes)
                        {
                            workRestrictions.Add(work.defName);
                        }
                        story.WorkRestrictions = workRestrictions;
                        for (int b = 0; b < childstories.Count - 1; b++)
                        {
                            if (childstories[b].StoryName == story.StoryName)
                            {
                                ChildStoryFound = true;
                            }
                        }

                        if (ChildStoryFound == false)
                        {
                            childstories.Add(story);
                            ChildStoryIndex = ChildStoryIndex + 1;
                        }
                    }

                    if (colonist.Gender == 1)
                    {
                        if (allBackstories[a].slot == BackstorySlot.Adulthood)
                        {
                            if (allBackstories[a].bodyTypeMale == colonist.BodyType || allBackstories[a].bodyTypeGlobal == colonist.BodyType)
                            {
                                story.StoryName = allBackstories[a].title;
                                story.StoryAge = 1;
                                story.BaseDescription = allBackstories[a].baseDesc;
                                story.BodyTypeFemale = allBackstories[a].bodyTypeFemale;
                                story.BodyTypeMale = allBackstories[a].bodyTypeMale;
                                story.BodyTypeGlobal = allBackstories[a].bodyTypeGlobal;
                                story.StoryIndex = AdultStoryIndex;

                                List<StorySkillGain> skillGains = new List<StorySkillGain>();
                                for (int b = 0; b < allBackstories[a].skillGainsResolved.Count - 1; b++)
                                {
                                    StorySkillGain skillGain = new StorySkillGain();
                                    skillGain.SkillName = allBackstories[a].skillGainsResolved.ElementAt(b).Key.skillLabel;
                                    skillGain.SkillValue = allBackstories[a].skillGainsResolved.ElementAt(b).Value;
                                    skillGains.Add(skillGain);
                                }
                                story.SkillGains = skillGains;

                                List<string> workRestrictions = new List<string>();
                                foreach (WorkTypeDef work in allBackstories[a].DisabledWorkTypes)
                                {
                                    workRestrictions.Add(work.defName);
                                }
                                story.WorkRestrictions = workRestrictions;

                                for (int b = 0; b < ColonistManager.AdultStories.Count - 1; b++)
                                {
                                    if (ColonistManager.AdultStories[b].StoryName == story.StoryName)
                                    {
                                        AdultStoryFound = true;
                                    }
                                }

                                if (AdultStoryFound == false)
                                {
                                    ColonistManager.AdultStories.Add(story);
                                    AdultStoryIndex = AdultStoryIndex + 1;
                                }
                            }
                        }
                    }
                    else if (colonist.Gender == 2)
                    {
                        if (allBackstories[a].slot == BackstorySlot.Adulthood)
                        {
                            if (allBackstories[a].bodyTypeFemale == colonist.BodyType || allBackstories[a].bodyTypeGlobal == colonist.BodyType)
                            {
                                story.StoryName = allBackstories[a].title;
                                story.StoryAge = 1;
                                story.BaseDescription = allBackstories[a].baseDesc;
                                story.BodyTypeFemale = allBackstories[a].bodyTypeFemale;
                                story.BodyTypeMale = allBackstories[a].bodyTypeMale;
                                story.BodyTypeGlobal = allBackstories[a].bodyTypeGlobal;
                                story.StoryIndex = AdultStoryIndex;

                                List<StorySkillGain> skillGains = new List<StorySkillGain>();
                                for (int b = 0; b < allBackstories[a].skillGainsResolved.Count - 1; b++)
                                {
                                    StorySkillGain skillGain = new StorySkillGain();
                                    skillGain.SkillName = allBackstories[a].skillGainsResolved.ElementAt(b).Key.skillLabel;
                                    skillGain.SkillValue = allBackstories[a].skillGainsResolved.ElementAt(b).Value;
                                    skillGains.Add(skillGain);
                                }
                                story.SkillGains = skillGains;

                                List<string> workRestrictions = new List<string>();
                                foreach (WorkTypeDef work in allBackstories[a].DisabledWorkTypes)
                                {
                                    workRestrictions.Add(work.defName);
                                }
                                story.WorkRestrictions = workRestrictions;

                                for (int b = 0; b < ColonistManager.AdultStories.Count - 1; b++)
                                {
                                    if (ColonistManager.AdultStories[b].StoryName == story.StoryName)
                                    {
                                        AdultStoryFound = true;
                                    }
                                }

                                if (AdultStoryFound == false)
                                {
                                    ColonistManager.AdultStories.Add(story);
                                    AdultStoryIndex = AdultStoryIndex + 1;
                                }
                            }
                        }
                    }
                }

                //Gen random childhood backstory
                ColonistStory.Add(childstories[UnityEngine.Random.Range(0, childstories.Count - 1)]);

                //Gen random adulthood backstory
                ColonistStory.Add(adultstories[UnityEngine.Random.Range(0, adultstories.Count - 1)]);

                colonist.Backstory = ColonistStory;

                //Gen random traits
                int rand_trait = 0;

                List<StoryTrait> StoryTraits = new List<StoryTrait>();
                foreach (TraitDef trait in DefDatabase<TraitDef>.AllDefsListForReading)
                {
                    StoryTrait storyTrait = new StoryTrait();
                    storyTrait.TraitName = trait.label;
                    storyTrait.Effect = trait.effect;
                    StoryTraits.Add(storyTrait);
                }

                StoryTrait storytrait = new StoryTrait();
                storytrait = new StoryTrait();
                rand_trait = UnityEngine.Random.Range(0, StoryTraits.Count - 1);
                storytrait.TraitName = StoryTraits[rand_trait].TraitName;
                storytrait.Effect = StoryTraits[rand_trait].Effect;
                storytrait.TraitDescription = StoryTraits[rand_trait].TraitDescription;
                StoryTraits.Add(storytrait);

                StoryTrait storytrait2 = new StoryTrait();
                int rand_trait2 = 0;
                rand_trait2 = UnityEngine.Random.Range(0, StoryTraits.Count - 1);
                storytrait2.TraitName = StoryTraits[rand_trait2].TraitName;
                storytrait2.Effect = StoryTraits[rand_trait].Effect;
                storytrait2.TraitDescription = StoryTraits[rand_trait2].TraitDescription;
                StoryTraits.Add(storytrait2);

                colonist.Traits = StoryTraits;
                
                //Gen Apparel
                List<Clothing> Outfit = new List<Clothing>();

                Clothing clothing = new Clothing();
                clothing = ShirtList.RandomListElement<Clothing>();
                Outfit.Add(clothing);

                clothing = new Clothing();
                clothing = CoatList.RandomListElement<Clothing>();
                Outfit.Add(clothing);

                colonist.Clothing = Outfit;

                //Gen Body
                colonist.SkinColor = RandomSkinColor();
                colonist.CrownType = ((UnityEngine.Random.value >= 0.5f) ? CrownType.Narrow : CrownType.Average);
                colonist.HeadGraphicPath = GraphicDatabase_Head.GetHeadRandom((Gender)colonist.Gender, colonist.SkinColor, colonist.CrownType).graphicPath;

                //Gen Hair
                colonist.HairColor = RandomHairColor(colonist.SkinColor);

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

                //Set BodyType
                if (colonist.Backstory[1].BodyTypeGlobal.ToString() != "Undefined")
                { 
                    colonist.BodyType = colonist.Backstory[1].BodyTypeGlobal;
                }
                else
                {
                    if (colonist.Gender == 1 && colonist.Backstory[1].BodyTypeMale.ToString() != "Undefined")
                    {
                        colonist.BodyType = colonist.Backstory[1].BodyTypeMale;
                    }
                    else if (colonist.Gender == 1 && colonist.Backstory[1].BodyTypeMale.ToString() == "Undefined")
                    {
                        colonist.BodyType = BodyType.Male;
                    }

                    if (colonist.Gender == 2 && colonist.Backstory[1].BodyTypeFemale.ToString() != "Undefined")
                    {
                        colonist.BodyType = colonist.Backstory[1].BodyTypeFemale;
                    }
                    else if (colonist.Gender == 2 && colonist.Backstory[1].BodyTypeFemale.ToString() == "Undefined")
                    {
                        colonist.BodyType = BodyType.Male;
                    }
                }

                //Gen Weapon
                GenWeaponList();
                colonist.Weapon = WeaponList.RandomListElement<string>();

                //Add colonist to total population
                Population.Add(colonist);
            }
        }

        private static Color RandomSkinColor()
        {
            float value = UnityEngine.Random.value;
            if (value < 0.2f)
            {
                if (UnityEngine.Random.value < 0.5f)
                {
                    return PawnSkinColors.LightBlackSkin;
                }
                return PawnSkinColors.DarkBlackSkin;
            }
            else
            {
                if (value < 0.5f)
                {
                    return PawnSkinColors.MidSkin;
                }
                if (UnityEngine.Random.value < 0.3f)
                {
                    return PawnSkinColors.PaleWhiteSkin;
                }
                return PawnSkinColors.WhiteSkin;
            }
        }

        public static Color RandomHairColor(Color skinColor)
        {
            if (UnityEngine.Random.value < 0.02f)
            {
                return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            }
            if (PawnSkinColors.IsDarkSkin(skinColor) || UnityEngine.Random.value < 0.5f)
            {
                float value = UnityEngine.Random.value;
                if (value < 0.25f)
                {
                    return new Color(0.2f, 0.2f, 0.2f);
                }
                if (value < 0.5f)
                {
                    return new Color(0.31f, 0.28f, 0.26f);
                }
                if (value < 0.75f)
                {
                    return new Color(0.25f, 0.2f, 0.15f);
                }
                return new Color(0.3f, 0.2f, 0.1f);
            }
            else
            {
                float value2 = UnityEngine.Random.value;
                if (value2 < 0.25f)
                {
                    return new Color(0.3529412f, 0.227450982f, 0.1254902f);
                }
                if (value2 < 0.5f)
                {
                    return new Color(0.5176471f, 0.3254902f, 0.184313729f);
                }
                if (value2 < 0.75f)
                {
                    return new Color(0.75686276f, 0.572549045f, 0.333333343f);
                }
                return new Color(0.929411769f, 0.7921569f, 0.6117647f);
            }
        }

        private static HairDef RandomHairDefFor(Colonist c, FactionDef factionType)
        {
            IEnumerable<HairDef> source =
                from hair in DefDatabase<HairDef>.AllDefs
                where hair.hairTags.SharesElementWith(factionType.hairTags)
                select hair;
            return source.RandomElementByWeight((HairDef hair) => HairChoiceLikelihoodFor(hair, c));
        }

        private static float HairChoiceLikelihoodFor(HairDef hair, Colonist c)
        {
            if (c.Gender == 0)
            {
                return 100f;
            }
            if (c.Gender == 1)
            {
                switch (hair.hairGender)
                {
                    case HairGender.Male:
                        return 70f;
                    case HairGender.MaleUsually:
                        return 30f;
                    case HairGender.Any:
                        return 60f;
                    case HairGender.FemaleUsually:
                        return 5f;
                    case HairGender.Female:
                        return 1f;
                }
            }
            if (c.Gender == 2)
            {
                switch (hair.hairGender)
                {
                    case HairGender.Male:
                        return 1f;
                    case HairGender.MaleUsually:
                        return 5f;
                    case HairGender.Any:
                        return 60f;
                    case HairGender.FemaleUsually:
                        return 30f;
                    case HairGender.Female:
                        return 70f;
                }
            }
            
            return 0f;
        }

        private static HairDef RandomColonistHairDefFor(Colonist colonist, FactionDef factionType)
        {
            IEnumerable<HairDef> source =
                from hair in DefDatabase<HairDef>.AllDefs
                where hair.hairTags.SharesElementWith(factionType.hairTags)
                select hair;
            return source.RandomElementByWeight((HairDef hair) => ColonistHairChoiceLikelihoodFor(hair, colonist));
        }

        private static float ColonistHairChoiceLikelihoodFor(HairDef hair, Colonist colonist)
        {
            if (colonist.Gender == 0)
            {
                return 100f;
            }

            if (colonist.Gender == 1)
            {
                switch (hair.hairGender)
                {
                    case HairGender.Male:
                        return 70f;
                    case HairGender.MaleUsually:
                        return 30f;
                    case HairGender.Any:
                        return 60f;
                    case HairGender.FemaleUsually:
                        return 5f;
                    case HairGender.Female:
                        return 1f;
                }
            }

            if (colonist.Gender == 2)
            {
                switch (hair.hairGender)
                {
                    case HairGender.Male:
                        return 1f;
                    case HairGender.MaleUsually:
                        return 5f;
                    case HairGender.Any:
                        return 60f;
                    case HairGender.FemaleUsually:
                        return 30f;
                    case HairGender.Female:
                        return 70f;
                }
            }

            return 0f;
        }

        private static void GenClothingList()
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

        private static void GenWeaponList()
        {
            WeaponList.Clear();
            
            foreach (ThingDef current in DefDatabase<ThingDef>.AllDefs)
            {
                bool WeaponFound = false;
                bool WeaponOkay = true;

                if (current.isGun == true && current.equipmentType == EquipmentType.Primary && current.canBeSpawningInventory)
                {
                    Equipment equipment = (Equipment)ThingMaker.MakeThing(current);

                    for (int i = 0; i < WeaponList.Count - 1; i++)
                    {
                        if (WeaponList[i] == equipment.Label)
                        {
                            WeaponFound = true;
                        }
                    }

                    foreach (string exemption in Genstep_ColonistCreationMod.ExemptWeapons)
                    {
                        if (equipment.Label == exemption)
                        {
                            WeaponOkay = false;
                        }
                    }

                    if (WeaponFound == false)
                    {
                        if (WeaponOkay == true)
                        {
                            WeaponList.Add(equipment.Label);
                        }
                    }
                }
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

        public static void PawnsToColonists()
        {
            for (int p = 0; p < ColonistNum.Amount; p++)
            {
                if (ModdedMapInitParams.colonists[p] != null)
                {
                    Colonist colonist = new Colonist();
                    colonist.Age = ModdedMapInitParams.colonists[p].age;
                    colonist.Gender = (int)ModdedMapInitParams.colonists[p].gender;
                    colonist.FirstName = ModdedMapInitParams.colonists[p].Name.first;
                    colonist.NickName = ModdedMapInitParams.colonists[p].Name.nick;
                    colonist.LastName = ModdedMapInitParams.colonists[p].Name.last;

                    colonist.SkinColor = ModdedMapInitParams.colonists[p].story.skinColor;
                    colonist.CrownType = ModdedMapInitParams.colonists[p].story.crownType;
                    colonist.HeadGraphicPath = ModdedMapInitParams.colonists[p].story.headGraphicPath;
                    colonist.HairColor = ModdedMapInitParams.colonists[p].story.hairColor;

                    ColHairDef hair = new ColHairDef();
                    hair.DefName = ModdedMapInitParams.colonists[p].story.hairDef.defName;
                    hair.GraphicPath = ModdedMapInitParams.colonists[p].story.hairDef.graphicPath;
                    colonist.HairDef = hair;

                    List<StoryTrait> traits = new List<StoryTrait>();
                    StoryTrait trait = new StoryTrait();
                    trait.TraitName = ModdedMapInitParams.colonists[p].story.traits.allTraits[0].def.label;
                    traits.Add(trait);
                    trait = new StoryTrait();
                    trait.TraitName = ModdedMapInitParams.colonists[p].story.traits.allTraits[1].def.label;
                    traits.Add(trait);
                    colonist.Traits = traits;

                    List<Clothing> Outfit = new List<Clothing>();
                    ShirtList.Clear();
                    CoatList.Clear();
                    GenClothingList();

                    Clothing clothing = new Clothing();
                    clothing = ShirtList.RandomListElement<Clothing>();
                    Outfit.Add(clothing);

                    clothing = new Clothing();
                    clothing = CoatList.RandomListElement<Clothing>();
                    Outfit.Add(clothing);

                    colonist.Clothing = Outfit;

                    List<Colonist_Skill> skills = new List<Colonist_Skill>();

                    for (int a = 0; a < ModdedMapInitParams.colonists[p].skills.skills.Count; a++)
                    {
                        Colonist_Skill skill = new Colonist_Skill();
                        skill.SkillName = ModdedMapInitParams.colonists[p].skills.skills[a].def.skillLabel;
                        skill.SkillValue = ModdedMapInitParams.colonists[p].skills.skills[a].level;
                        skill.SkillPassion = (int)ModdedMapInitParams.colonists[p].skills.skills[a].passion;
                        skills.Add(skill);
                    }
                    colonist.Skills = skills;

                    List<Colonist_Backstory> Backstories = new List<Colonist_Backstory>();
                    Colonist_Backstory backstory = new Colonist_Backstory();
                    backstory.StoryName = ModdedMapInitParams.colonists[p].story.childhood.title;
                    backstory.BaseDescription = ModdedMapInitParams.colonists[p].story.childhood.baseDesc;
                    backstory.StoryAge = 0;
                    List<StorySkillGain> skillGains = new List<StorySkillGain>();
                    for (int i = 0; i < ModdedMapInitParams.colonists[p].story.childhood.skillGainsResolved.Count - 1; i++)
                    {
                        StorySkillGain skillGain = new StorySkillGain();
                        skillGain.SkillName = ModdedMapInitParams.colonists[p].story.childhood.skillGainsResolved.ElementAt(i).Key.skillLabel;
                        skillGain.SkillValue = ModdedMapInitParams.colonists[p].story.childhood.skillGainsResolved.ElementAt(i).Value;
                        skillGains.Add(skillGain);
                    }
                    backstory.SkillGains = skillGains;
                    List<string> Restrictions = new List<string>();
                    foreach (WorkTypeDef work in ModdedMapInitParams.colonists[p].story.childhood.DisabledWorkTypes)
                    {
                        Restrictions.Add(work.defName);
                    }
                    backstory.WorkRestrictions = Restrictions;
                    backstory.BodyTypeGlobal = ModdedMapInitParams.colonists[p].story.childhood.bodyTypeGlobal;
                    backstory.BodyTypeMale = ModdedMapInitParams.colonists[p].story.childhood.bodyTypeMale;
                    backstory.BodyTypeFemale = ModdedMapInitParams.colonists[p].story.childhood.bodyTypeFemale;
                    Backstories.Add(backstory);

                    backstory = new Colonist_Backstory();
                    backstory.StoryName = ModdedMapInitParams.colonists[p].story.adulthood.title;
                    backstory.BaseDescription = ModdedMapInitParams.colonists[p].story.adulthood.baseDesc;
                    backstory.StoryAge = 1;
                    skillGains = new List<StorySkillGain>();
                    for (int i = 0; i < ModdedMapInitParams.colonists[p].story.adulthood.skillGainsResolved.Count - 1; i++)
                    {
                        StorySkillGain skillGain = new StorySkillGain();
                        skillGain.SkillName = ModdedMapInitParams.colonists[p].story.adulthood.skillGainsResolved.ElementAt(i).Key.skillLabel;
                        skillGain.SkillValue = ModdedMapInitParams.colonists[p].story.adulthood.skillGainsResolved.ElementAt(i).Value;
                        skillGains.Add(skillGain);
                    }
                    backstory.SkillGains = skillGains;
                    Restrictions = new List<string>();
                    foreach (WorkTypeDef work in ModdedMapInitParams.colonists[p].story.adulthood.DisabledWorkTypes)
                    {
                        Restrictions.Add(work.defName);
                    }
                    backstory.WorkRestrictions = Restrictions;
                    backstory.BodyTypeGlobal = ModdedMapInitParams.colonists[p].story.adulthood.bodyTypeGlobal;
                    backstory.BodyTypeMale = ModdedMapInitParams.colonists[p].story.adulthood.bodyTypeMale;
                    backstory.BodyTypeFemale = ModdedMapInitParams.colonists[p].story.adulthood.bodyTypeFemale;

                    if (backstory.BodyTypeGlobal.ToString() != "Undefined")
                    {
                        colonist.BodyType = backstory.BodyTypeGlobal;
                    }
                    else
                    {
                        if (colonist.Gender == 1 && backstory.BodyTypeMale.ToString() != "Undefined")
                        {
                            colonist.BodyType = backstory.BodyTypeMale;
                        }
                        else if (colonist.Gender == 1 && backstory.BodyTypeMale.ToString() == "Undefined")
                        {
                            colonist.BodyType = BodyType.Male;
                        }

                        if (colonist.Gender == 2 && backstory.BodyTypeFemale.ToString() != "Undefined")
                        {
                            colonist.BodyType = backstory.BodyTypeFemale;
                        }
                        else if (colonist.Gender == 2 && backstory.BodyTypeFemale.ToString() == "Undefined")
                        {
                            colonist.BodyType = BodyType.Male;
                        }
                    }
                    Backstories.Add(backstory);
                    colonist.Backstory = Backstories;

                    GenWeaponList();
                    colonist.Weapon = WeaponList.RandomListElement<string>();

                    colonist.SkillPool = 0;

                    Population.Add(colonist);
                }
            }
        }
    }
}
