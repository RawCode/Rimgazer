using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    class SaveColonists
    {
        private static Stream saveStream;
        private static XmlWriter writer;
        private static string saveDataPath = null;
        public static LoadSaveMode mode;
        public static XmlNode curParent = null;

        public static void InitWriting(string groupName)
        {
            mode = LoadSaveMode.Saving;
            saveStream = new FileStream(FilePathForSavechars(groupName), FileMode.Create, FileAccess.Write, FileShare.None);
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.IndentChars = "\t";
            writer = XmlWriter.Create(saveStream, xmlWriterSettings);
            writer.WriteStartDocument();
        }

        public static string CharSaveGamesFolderPath
        {
            get
            {
                string text = Path.Combine(CharSaveDataFolderPath, "CharSaves");
                DirectoryInfo directoryInfo = new DirectoryInfo(text);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                return text;
            }
        }

        public static string CharSaveDataFolderPath
        {
            get
            {
                if (saveDataPath == null)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
                    if (Application.isEditor)
                    {
                        saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
                    }
                    else
                    {
                        saveDataPath = Application.persistentDataPath;
                    }
                    DirectoryInfo directoryInfo2 = new DirectoryInfo(saveDataPath);
                    if (!directoryInfo2.Exists)
                    {
                        directoryInfo2.Create();
                    }
                }
                return saveDataPath;
            }
        }

        public static string FilePathForSavechars(string groupName)
        {
            return Path.Combine(CharSaveGamesFolderPath, groupName + ".col");
        }

        public static void EnterNode(string elementName)
        {
            if (mode == LoadSaveMode.Saving)
            {
                writer.WriteStartElement(elementName);
            }
        }

        public static void SaveToFile(Map map, string groupName)
        {
            try
            {
                InitWriting(groupName);
                int i = 1;
                EnterNode("Colonists");
                EnterNode("ColonistAmount");
                writer.WriteAttributeString("amount", ColonistNum.Amount.ToString());
                ExitNode();
                foreach (Colonist colonist in ColonistManager.Population)
                {
                    WriteColonist(colonist, i);
                    i++;
                }
                ExitNode();
            }
            finally
            {
                FinalizeWriting();
                mode = LoadSaveMode.Inactive;
            }
            GC.Collect();
        }

        public static void FinalizeWriting()
        {
            writer.WriteEndDocument();
            writer.Close();
            saveStream.Close();
        }

        public static void ExitNode()
        {
            if (mode == LoadSaveMode.Saving)
            {
                writer.WriteEndElement();
            }
        }

        public static void WriteColonist(Colonist colonist, int i)
        {
            EnterNode("Colonist");
            writer.WriteAttributeString("num", i.ToString());

                EnterNode("BasicInfo");
                writer.WriteAttributeString("Name", colonist.FirstName + " " + colonist.NickName + " " + colonist.LastName);
                ExitNode();

                EnterNode("BasicInfo");
                writer.WriteAttributeString("Age", colonist.Age.ToString());
                ExitNode();

                EnterNode("BasicInfo");
                writer.WriteAttributeString("Gender", colonist.Gender.ToString());
                ExitNode();

                EnterNode("BasicInfo");
                writer.WriteAttributeString("Trait1", colonist.Traits[0].TraitName);
                ExitNode();

                EnterNode("BasicInfo");
                writer.WriteAttributeString("Trait2", colonist.Traits[1].TraitName);
                ExitNode();

                EnterNode("Graphics");
                writer.WriteAttributeString("BodyType", colonist.BodyType.ToString());
                ExitNode();

                EnterNode("Graphics");
                writer.WriteAttributeString("SkinColor", colonist.SkinColor.r.ToString() + "," + colonist.SkinColor.g.ToString() + "," + colonist.SkinColor.b.ToString() + "," + colonist.SkinColor.a.ToString());
                ExitNode();

                EnterNode("Graphics");
                writer.WriteAttributeString("CrownType", colonist.CrownType.ToString());
                ExitNode();

                EnterNode("Graphics");
                writer.WriteAttributeString("HeadGraphicPath", colonist.HeadGraphicPath);
                ExitNode();

                EnterNode("Graphics");
                writer.WriteAttributeString("HairColor", colonist.HairColor.r.ToString() + "," + colonist.HairColor.g.ToString() + "," + colonist.HairColor.b.ToString() + "," + colonist.HairColor.a.ToString());
                ExitNode();

                EnterNode("HairDef");
                writer.WriteAttributeString("DefName", colonist.HairDef.DefName);
                writer.WriteAttributeString("GraphicPath", colonist.HairDef.GraphicPath);
                writer.WriteAttributeString("HairGender", colonist.HairDef.HairGender.ToString());
                writer.WriteAttributeString("Label", colonist.HairDef.Label);
                ExitNode();

                int b = 1;
                foreach (Clothing clothing in colonist.Clothing)
                {
                    if (b == 1)
                    {
                        EnterNode("OnSkin");
                    }
                    else
                    {
                        EnterNode("Shell");
                    }
                    writer.WriteAttributeString("Layer", clothing.Layer);
                    writer.WriteAttributeString("Label", clothing.Label);
                    writer.WriteAttributeString("GraphicPath", clothing.GraphicPath);
                    writer.WriteAttributeString("Color", clothing.Color.r.ToString() + "," + clothing.Color.g.ToString() + "," + clothing.Color.b.ToString() + "," + clothing.Color.a.ToString());

                    ExitNode();
                    b++;
                }

                EnterNode("Backstories");

                    EnterNode("Childhood");
                    writer.WriteAttributeString("Index", colonist.Backstory[0].StoryIndex.ToString());
                    ExitNode();

                    EnterNode("Adulthood");
                    writer.WriteAttributeString("Index", colonist.Backstory[1].StoryIndex.ToString());
                    ExitNode();

                ExitNode();

                EnterNode("Traits");

                    writer.WriteAttributeString("Trait1", colonist.Traits[0].TraitName);
                    writer.WriteAttributeString("Trait2", colonist.Traits[1].TraitName);

                ExitNode();

                EnterNode("SkillPool");
                writer.WriteAttributeString("Amount", colonist.SkillPool.ToString());
                ExitNode();

                EnterNode("Skills");
                for (int c = 0; c < colonist.Skills.Count; c++)
                {
                    EnterNode("Skill" + c.ToString());
                    writer.WriteAttributeString("Value", colonist.Skills[c].SkillValue.ToString());
                    writer.WriteAttributeString("Passion", colonist.Skills[c].SkillPassion.ToString());
                    ExitNode();
                }
                ExitNode();

                EnterNode("Weapon");
                writer.WriteAttributeString("Name", colonist.Weapon);
                ExitNode();

                EnterNode("End");
                ExitNode();
                
            ExitNode();
        }
    }
}
