using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Verse;
using RimWorld;

namespace ColonistCreationMod
{
    public class Language
    {
        public Dictionary<string, string> Translation = new Dictionary<string, string>();

        public Language()
        {

        }

        public static string FindText(Language language, string text)
        {
            if (LanguageDatabase.activeLanguage.FriendlyName != "English")
            {
                for (int i = 0; i < language.Translation.Count; i++)
                {
                    if (language.Translation.ElementAt(i).Key == text)
                    {
                        text = language.Translation.ElementAt(i).Value;
                        break;
                    }
                }
            }

            return text;
        }
    }
}
