using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using UnityEngine;

namespace ColonistCreationMod
{
    public class ItemDescription
    {
        public string Name;
        public float DamageAbsorbtion;
        public string Layer;
        public Texture texture;
        public ResearchProjectDef research;

        public ItemDescription(string Name, string Layer, float DamageAbsorbtion, Texture texture, ResearchProjectDef research)
        {
            this.Name = Name;
            this.Layer = Layer;
            this.DamageAbsorbtion = DamageAbsorbtion;
            this.texture = texture;
            this.research = research;
        }
    }
}
