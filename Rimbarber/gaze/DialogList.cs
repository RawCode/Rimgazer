using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    public abstract class DialogList : Layer
    {
        protected const float BoxMargin = 20f;
        protected const float MapEntrySpacing = 8f;
        protected const float MapEntryMargin = 6f;
        protected const float MapNameExtraLeftMargin = 15f;
        protected const float MapDateExtraLeftMargin = 220f;
        protected const float DeleteButtonSpace = 5f;
        protected string interactButLabel = "Error";
        protected float bottomAreaHeight;
        private Vector2 scrollPosition = Vector2.zero;
        private static readonly Color ManualSaveTextColor = new Color(1f, 1f, 0.6f);
        private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

        public DialogList()
        {
            base.SetCentered(600f, 700f);
            this.category = LayerCategory.GameDialog;
            this.closeOnEscapeKey = true;
            this.doCloseButton = true;
            this.doCloseX = true;
            this.absorbAllInput = true;
            this.clearNonEditDialogs = false;
            this.forcePause = true;
        }

        protected override void FillWindow(Rect inRect)
        {
            Vector2 vector = new Vector2(inRect.width - 16f, 48f);
            Vector2 vector2 = new Vector2(100f, vector.y - 12f);
            inRect.height -= 45f;
            List<FileInfo> list = SaveFiles.AllSaveFiles.ToList<FileInfo>();
            float num = vector.y + 8f;
            float height = (float)list.Count * num;
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
            Rect position = new Rect(inRect.AtZero());
            position.height -= this.bottomAreaHeight;
            this.scrollPosition = GUI.BeginScrollView(position, this.scrollPosition, viewRect);
            float num2 = 0f;
            foreach (FileInfo current in list)
            {
                Rect rect = new Rect(0f, num2, vector.x, vector.y);
                Widgets.DrawMenuSection(rect);
                Rect innerRect = rect.GetInnerRect(6f);
                GUI.BeginGroup(innerRect);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(current.Name);
                if (MapFiles.IsAutoSave(fileNameWithoutExtension))
                {
                    GUI.color = DialogList.AutosaveTextColor;
                }
                else
                {
                    GUI.color = DialogList.ManualSaveTextColor;
                }
                Rect position2 = new Rect(15f, 0f, innerRect.width, innerRect.height);
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                GenFont.SetFontSmall();
                GUI.Label(position2, fileNameWithoutExtension);
                GUI.color = Color.white;
                Rect position3 = new Rect(220f, 0f, innerRect.width, innerRect.height);
                GenFont.SetFontTiny();
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                GUI.Label(position3, current.LastWriteTime.ToString());
                GUI.color = Color.white;
                GUI.skin.label.alignment = TextAnchor.UpperLeft;
                GenFont.SetFontSmall();
                float num3 = vector.x - 12f - vector2.x - vector2.y;
                Rect butRect = new Rect(num3, 0f, vector2.x, vector2.y);
                if (Widgets.TextButton(butRect, this.interactButLabel))
                {
                    this.DoMapEntryInteraction(Path.GetFileNameWithoutExtension(current.Name));
                }
                Rect rect2 = new Rect(num3 + vector2.x + 5f, 0f, vector2.y, vector2.y);
                if (Widgets.ImageButton(rect2, ButtonText.DeleteX))
                {
                    FileInfo localFile = current;
                    Find.UIRoot.layers.Add(new Dialog_Confirm("ConfirmDelete".Translate(new object[]
					{
						localFile.Name
					}), delegate
                    {
                        localFile.Delete();
                    }, true));
                }
                TooltipHandler.TipRegion(rect2, "DeleteThisSavegame".Translate());
                GUI.EndGroup();
                num2 += vector.y + 8f;
            }
            GUI.EndScrollView();
            this.DoSpecialSaveLoadGUI(inRect.AtZero());
        }

        protected virtual void DoSpecialSaveLoadGUI(Rect inRect)
        {
        }

        protected abstract void DoMapEntryInteraction(string mapName);

        protected override void OnPreClose()
        {
            Find.LayerStack.Remove(this);
            Find.LayerStack.Add(new ColonistCreationMenu(ColonistManager.Population[0]));
        }
    }
}
