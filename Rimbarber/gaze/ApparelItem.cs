using System;

using Verse;
using RimWorld;

namespace ColonistCreationMod
{
    public class ApparelItem
    {
        public Apparel apparel;
        public bool selected;
        public ThingDef thingdef;

        public ApparelItem(Apparel apparel, bool selected, ThingDef thingdef)
        {
            this.apparel = apparel;
            this.selected = selected;
            this.thingdef = thingdef;
        }
    }
}
