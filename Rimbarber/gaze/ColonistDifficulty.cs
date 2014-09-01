using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    class ColonistDifficulty : Layer
    {
        private Vector2 WinSize = new Vector2(320f, 288f);
        private static readonly Vector2 ButtonSize = new Vector2(120f, 40f);
        public static string Difficulty = "";

        public ColonistDifficulty()
        {
            base.SetCentered(WinSize);
            clearNonEditDialogs = true;
            category = LayerCategory.GameDialog;
            absorbAllInput = true;
            forcePause = true;
        }

        protected override void FillWindow(Rect inRect)
        {
            GenFont.SetFontMedium();

            //Header
            Rect HeaderPos = new Rect(40f, 0f, WinSize.x, 40f);
            GUI.Label(HeaderPos, Language.FindText(LoadLanguage.language, "Choose your difficulty") + ":");

            //Easy
            Rect EasyPos = new Rect(0f, 60f, 280f, 40f);
            if (Widgets.TextButton(EasyPos, Language.FindText(LoadLanguage.language, "Easy")))
            {
                Difficulty = "Easy";
                ColonistNum.Amount = 3;
                Find.LayerStack.Add(new ColonistNum());
            }

            //Normal
            Rect NormalPos = new Rect(0f, 110f, 280f, 40f);
            if (Widgets.TextButton(NormalPos, Language.FindText(LoadLanguage.language, "Normal")))
            {
                Difficulty = "Normal";
                ColonistNum.Amount = 3;
                Find.LayerStack.Add(new ColonistNum());
            }

            //Hard
            Rect HardPos = new Rect(0f, 160f, 280f, 40f);
            if (Widgets.TextButton(HardPos, Language.FindText(LoadLanguage.language, "Hard")))
            {
                Difficulty = "Hard";
                ColonistNum.Amount = 3;
                Find.LayerStack.Add(new ColonistNum());
            }

            //Insane
            Rect InsanePos = new Rect(0f, 210f, 280f, 40f);
            if (Widgets.TextButton(InsanePos, Language.FindText(LoadLanguage.language, "Insane")))
            {
                Difficulty = "Insane";
                ColonistNum.Amount = 1;
                ModdedMapInitParams.GenerateColonists();
                if (Genstep_ColonistCreationMod.BaseStats[0] == false)
                {
                    ColonistManager.RandomColonists();
                }
                else
                {
                    ColonistManager.PawnsToColonists();
                }
                Find.LayerStack.Add(new ColonistCreationMenu(ColonistManager.Population[0]));
            }
        }
    }
}
