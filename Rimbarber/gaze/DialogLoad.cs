﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    public class DialogLoad : DialogList
    {
        private bool focusedMapNameArea;

        public DialogLoad()
        {
            this.interactButLabel = "LoadGameButton".Translate();
            this.bottomAreaHeight = 85f;
            clearNonEditDialogs = true;
            absorbAllInput = true;
            forcePause = true;
            category = LayerCategory.GameDialog;
        }

        protected override void DoMapEntryInteraction(string MapName)
        {
            Find.Map.info.fileName = MapName;
            LoadColonists.LoadFromFile(Find.Map, Find.Map.info.fileName);
            Find.LayerStack.Remove(this);
            Find.LayerStack.Add(new ColonistCreationMenu(ColonistManager.Population[0]));
        }

        protected override void DoSpecialSaveLoadGUI(Rect inRect)
        {
            GUI.BeginGroup(inRect);
            bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
            float top = inRect.height - 52f;
            GenFont.SetFontSmall();
            GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
            GUI.skin.textField.contentOffset = new Vector2(12f, 0f);
            GUI.skin.settings.doubleClickSelectsWord = true;
            GUI.SetNextControlName("MapNameField");
            Rect position = new Rect(5f, top, 400f, 35f);
            string text = GUI.TextField(position, Find.Map.info.fileName);
            if (GenText.IsValidFilename(text))
            {
                Find.Map.info.fileName = text;
            }
            if (!this.focusedMapNameArea)
            {
                GUI.FocusControl("MapNameField");
                this.focusedMapNameArea = true;
            }
            Rect butRect = new Rect(420f, top, inRect.width - 400f - 20f, 35f);
            if (Widgets.TextButton(butRect, "LoadGameButton".Translate()) || flag)
            {
                LoadColonists.LoadFromFile(Find.Map, Find.Map.info.fileName);
                Find.LayerStack.Remove(this);
                Find.LayerStack.Add(new ColonistCreationMenu(ColonistManager.Population[0]));
            }
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.EndGroup();
        }
    }
}
