using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    class ColonistNum : Layer
    {
        public static int Amount = 3;

        private Vector2 WinSize = new Vector2(320f, 200f);
        private static readonly Vector2 ButtonSize = new Vector2(120f, 40f);

        public ColonistNum()
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
            GUI.Label(HeaderPos, Language.FindText(LoadLanguage.language, "Start with how many colonists?"));

            //SelectDown
            Rect DownPos = new Rect(80f, 55f, 40f, 40f);
            if (Widgets.TextButton(DownPos, "<".Translate()))
            {
                if (ColonistDifficulty.Difficulty == "Easy")
                {
                    if (Amount > 3)
                    {
                        //Lower amount and re-display
                        Amount--;
                        ShowAmount(new Rect(135f, 60f, 40f, 40f));
                    }
                }
                else if (ColonistDifficulty.Difficulty == "Normal")
                {
                    if (Amount > 2)
                    {
                        //Lower amount and re-display
                        Amount--;
                        ShowAmount(new Rect(135f, 60f, 40f, 40f));
                    }
                }
                else
                {
                    if (Amount > 1)
                    {
                        //Lower amount and re-display
                        Amount--;
                        ShowAmount(new Rect(135f, 60f, 40f, 40f));
                    }
                }
            }

            //ShowAmount
            ShowAmount(new Rect(135f, 60f, 40f, 40f));

            //SelectUp
            Rect UpPos = new Rect(160f, 55f, 40f, 40f);
            if (Widgets.TextButton(UpPos, ">".Translate()))
            {
                if (ColonistDifficulty.Difficulty == "Easy")
                {
                    if (Amount < 5)
                    {
                        //Raise amount and re-display
                        Amount++;
                        ShowAmount(new Rect(135f, 60f, 40f, 40f));
                    }
                }
                else if (ColonistDifficulty.Difficulty == "Normal")
                {
                    if (Amount < 4)
                    {
                        //Raise amount and re-display
                        Amount++;
                        ShowAmount(new Rect(135f, 60f, 40f, 40f));
                    }
                }
                else
                {
                    if (Amount < 3)
                    {
                        //Raise amount and re-display
                        Amount++;
                        ShowAmount(new Rect(135f, 60f, 40f, 40f));
                    }
                }
            }

            GenFont.SetFontSmall();

            //Return to Menu
            Rect ReturnPos = new Rect(0, WinSize.y - 75, ButtonSize.x, ButtonSize.y);
            if (Widgets.TextButton(ReturnPos, Language.FindText(LoadLanguage.language, "Back")))
            {
                Application.LoadLevel("Entry");
            }

            //Accept
            Rect AcceptPos = new Rect((WinSize.x - ButtonSize.x) - 40f, WinSize.y - 75, ButtonSize.x, ButtonSize.y);
            if (Widgets.TextButton(AcceptPos, Language.FindText(LoadLanguage.language, "Accept")))
            {
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

        private void ShowAmount(Rect AmountPos)
        {
            GenFont.SetFontMedium();
            GUI.Label(AmountPos, Amount.ToString().Translate());
        }
    }
}
