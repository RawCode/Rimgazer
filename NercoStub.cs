using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;
using Verse.AI;

namespace RC.Rimgazer
{
    class NercoStub : Building_Grave
    {
        public override void Notify_ReceivedThing(Thing newItem)
        {
            if (!(newItem is Corpse))
            {
                base.Notify_ReceivedThing(newItem);
                return;
            }

            Corpse shell = (Corpse)newItem;
            Pawn target = shell.innerPawn;
            target.healthTracker.Health = 10;
            target.Position = shell.Position;
            shell.Destroy();
            this.Destroy();

            FieldInfo[] currentstate = typeof(Pawn).GetFields();

            foreach (FieldInfo f in currentstate)
            {
                Log.Warning(f.ToString() + " value is " + f.GetValue(target));
            }



            //target.SpawnSetup();
        }
    }
}
