using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using RimWorld;
using Verse;

namespace ColonistCreationMod
{
    class LoadLanguage
    {
        public static Language language = new Language();

        public static void Load()
        {
            string FolderPath = Environment.CurrentDirectory + @"\Mods\Colonist Creation Mod\Translations\";
            string FilePath = null;

            if (LanguageDatabase.activeLanguage.FriendlyName == "简体中文")
            {
                FilePath = "Chinese.xml";
            }
            Parse(FolderPath + FilePath);
            GC.Collect();
        }

        private static void Parse(string file)
        {
            using (XmlTextReader reader = new XmlTextReader(File.OpenRead(file)))
            {
                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "Translation":
                            VisitTranslation(reader);
                            break;
                    }
                }
            }
        }

        private static void VisitTranslation(XmlTextReader reader)
        {
            Dictionary<string, string> translation = new Dictionary<string, string>();

            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "Easy":
                        translation.Add("Easy", reader.Value);
                        break;

                    case "Normal":
                        translation.Add("Normal", reader.Value);
                        break;

                    case "Hard":
                        translation.Add("Hard", reader.Value);
                        break;

                    case "Insane":
                        translation.Add("Insane", reader.Value);
                        break;

                    case "Yes":
                        translation.Add("Yes", reader.Value);
                        break;

                    case "No":
                        translation.Add("No", reader.Value);
                        break;

                    case "Back":
                        translation.Add("Back", reader.Value);
                        break;

                    case "Accept":
                        translation.Add("Accept", reader.Value);
                        break;

                    case "Export":
                        translation.Add("Export", reader.Value);
                        break;

                    case "Import":
                        translation.Add("Import", reader.Value);
                        break;

                    case "Continue":
                        translation.Add("Continue", reader.Value);
                        break;

                    case "Confirm":
                        translation.Add("Confirm", reader.Value);
                        break;

                    case "Backstory":
                        translation.Add("Backstory", reader.Value);
                        break;

                    case "Childhood":
                        translation.Add("Childhood", reader.Value);
                        break;

                    case "Adulthood":
                        translation.Add("Adulthood", reader.Value);
                        break;

                    case "IncapableOf":
                        translation.Add("Incapable Of", reader.Value);
                        break;

                    case "None":
                        translation.Add("(none)", reader.Value);
                        break;

                    case "Traits":
                        translation.Add("Traits", reader.Value);
                        break;

                    case "Gender":
                        translation.Add("Gender", reader.Value);
                        break;

                    case "Male":
                        translation.Add("Male", reader.Value);
                        break;

                    case "Female":
                        translation.Add("Female", reader.Value);
                        break;

                    case "Age":
                        translation.Add("Age", reader.Value);
                        break;

                    case "Skills":
                        translation.Add("Skills", reader.Value);
                        break;

                    case "SkillPoints":
                        translation.Add("Skill Points", reader.Value);
                        break;

                    case "Construction":
                        translation.Add("Construction", reader.Value);
                        break;

                    case "Growing":
                        translation.Add("Growing", reader.Value);
                        break;

                    case "Research":
                        translation.Add("Research", reader.Value);
                        break;

                    case "Mining":
                        translation.Add("Mining", reader.Value);
                        break;

                    case "Shooting":
                        translation.Add("Shooting", reader.Value);
                        break;

                    case "Melee":
                        translation.Add("Melee", reader.Value);
                        break;

                    case "Social":
                        translation.Add("Social", reader.Value);
                        break;

                    case "Cooking":
                        translation.Add("Cooking", reader.Value);
                        break;

                    case "Medicine":
                        translation.Add("Medicine", reader.Value);
                        break;

                    case "Artistic":
                        translation.Add("Artistic", reader.Value);
                        break;

                    case "Crafting":
                        translation.Add("Crafting", reader.Value);
                        break;

                    case "Passions":
                        translation.Add("Passions", reader.Value);
                        break;

                    case "MajorPassion":
                        translation.Add("Major Passion", reader.Value);
                        break;

                    case "MinorPassion":
                        translation.Add("Minor Passion", reader.Value);
                        break;

                    case "Style":
                        translation.Add("Style", reader.Value);
                        break;

                    case "ChangeClothing":
                        translation.Add("Change Clothing", reader.Value);
                        break;

                    case "ChangeHead":
                        translation.Add("Change Head", reader.Value);
                        break;

                    case "ChangeBody":
                        translation.Add("Change Body", reader.Value);
                        break;

                    case "Weapon":
                        translation.Add("Weapon", reader.Value);
                        break;

                    case "ChooseColor":
                        translation.Add("Choose Color", reader.Value);
                        break;

                    case "Shirt":
                        translation.Add("Shirt", reader.Value);
                        break;

                    case "T-Shirt":
                        translation.Add("T-Shirt", reader.Value);
                        break;

                    case "Button-downShirt":
                        translation.Add("Button-down Shirt", reader.Value);
                        break;

                    case "Coat":
                        translation.Add("Coat", reader.Value);
                        break;

                    case "Jacket":
                        translation.Add("Jacket", reader.Value);
                        break;

                    case "Duster":
                        translation.Add("Duster", reader.Value);
                        break;

                    case "PowerArmor":
                        translation.Add("Power Armor", reader.Value);
                        break;

                    case "Face":
                        translation.Add("Face", reader.Value);
                        break;

                    case "Average":
                        translation.Add("Average", reader.Value);
                        break;

                    case "Narrow":
                        translation.Add("Narrow", reader.Value);
                        break;

                    case "Pointy":
                        translation.Add("Pointy", reader.Value);
                        break;

                    case "Wide":
                        translation.Add("Wide", reader.Value);
                        break;

                    case "Hair":
                        translation.Add("Hair", reader.Value);
                        break;

                    case "Afro":
                        translation.Add("Afro", reader.Value);
                        break;

                    case "Bob":
                        translation.Add("Bob", reader.Value);
                        break;

                    case "Burgundy":
                        translation.Add("Burgundy", reader.Value);
                        break;

                    case "Flowy":
                        translation.Add("Flowy", reader.Value);
                        break;

                    case "Long":
                        translation.Add("Long", reader.Value);
                        break;

                    case "Mohawk":
                        translation.Add("Mohawk", reader.Value);
                        break;

                    case "Mop":
                        translation.Add("Mop", reader.Value);
                        break;

                    case "Pigtails":
                        translation.Add("Pigtails", reader.Value);
                        break;

                    case "Shaved":
                        translation.Add("Shaved", reader.Value);
                        break;

                    case "Spikes":
                        translation.Add("Spikes", reader.Value);
                        break;

                    case "Tuft":
                        translation.Add("Tuft", reader.Value);
                        break;

                    case "Wavy":
                        translation.Add("Wavy", reader.Value);
                        break;

                    case "BodyShape":
                        translation.Add("Body Shape", reader.Value);
                        break;

                    case "Thin":
                        translation.Add("Thin", reader.Value);
                        break;

                    case "Hulk":
                        translation.Add("Hulk", reader.Value);
                        break;

                    case "Fat":
                        translation.Add("Fat", reader.Value);
                        break;

                    case "SkinColor":
                        translation.Add("Skin Color", reader.Value);
                        break;

                    case "PaleWhite":
                        translation.Add("Pale White", reader.Value);
                        break;

                    case "White":
                        translation.Add("White", reader.Value);
                        break;

                    case "Mid":
                        translation.Add("Mid", reader.Value);
                        break;

                    case "LightBlack":
                        translation.Add("Light Black", reader.Value);
                        break;

                    case "DarkBlack":
                        translation.Add("Dark Black", reader.Value);
                        break;

                    case "EnhancedColonistCreation":
                        translation.Add("Enhanced Colonist Creation", reader.Value);
                        break;

                    case "UseTheColonistCreationMenu_Question":
                        translation.Add("Use the Colonist Creation Menu?", reader.Value);
                        break;

                    case "ChooseYourDifficulty":
                        translation.Add("Choose your difficulty", reader.Value);
                        break;

                    case "StartWithHowManyColonists_Question":
                        translation.Add("Start with how many colonists?", reader.Value);
                        break;
                }
            }

            language.Translation = translation;
        }

    }
}
