using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer.Gaze
{
    class GazePawn : Pawn
    {
        public override void Destroy(DestroyMode mode = DestroyMode.Kill)
        {
            Log.Error("Override invocation");
            if (base.Faction == Faction.OfColony)
            {

                ThingDef target = DefDatabase<ThingDef>.GetNamed("TableButcher");
                target.AllRecipes.Add(DefDatabase<RecipeDef>.GetNamed("ButcherCorpseMechanoid"));
                //typeof(ThingDef).GetField("allRecipesCached", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(target, null);

                base.Destroy(mode);
                return;
            }

            if (base.equipment != null)
            {
                if (base.equipment.Primary != null)
                    base.equipment.DestroyEquipment(base.equipment.Primary);
            }
            if (base.apparel != null)
            {
                //base.apparel.WornApparelListForReading.Clear();
            }
            if (base.inventory != null)
            {
                base.inventory.container.Clear();
            }

            base.Destroy(mode);
        }

    }
}
