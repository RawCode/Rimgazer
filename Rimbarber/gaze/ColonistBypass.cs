using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    class ColonistBypass : Layer
    {
        public static int Amount = 3;

        private Vector2 WinSize = new Vector2(330f, 140f);
        private static readonly Vector2 ButtonSize = new Vector2(120f, 40f);

        public ColonistBypass()
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
            Rect HeaderPos = new Rect(0f, 0f, WinSize.x, 40f);
            GUI.Label(HeaderPos, Language.FindText(LoadLanguage.language, "Use the Colonist Creation Menu?"));

            //SelectDown
            Rect YesPos = new Rect(0f, 55f, ButtonSize.x, ButtonSize.y);
            if (Widgets.TextButton(YesPos, Language.FindText(LoadLanguage.language, "Yes")))
            {
                ColonistManager.Population.Clear();
                if (Genstep_ColonistCreationMod.BaseStats[0] == false)
                {
                    Find.LayerStack.Add(new ColonistDifficulty());
                }
                else
                {
                    Find.LayerStack.Add(new ColonistNum());
                }
            }

            Rect NoPos = new Rect(WinSize.x - 160f, 55f, ButtonSize.x, ButtonSize.y);
            if (Widgets.TextButton(NoPos, Language.FindText(LoadLanguage.language, "No")))
            {
                base.Close();
                Genstep_ColonistCreationMod.SpawnStartingColonists();
            }
        }
    }
}
