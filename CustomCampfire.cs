using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace RC.Rimgazer
{
    class CustomCampfire : Building_Campfire
    {
        static int state = -1;
        Region bound = null;

        private static Graphic FireGraphic = GraphicDatabase.Get_Flicker("Things/Special/Fire", ShaderDatabase.MotePostLight, false, Color.blue);

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);

            if (this.bound != null)
                this.bound.valid = false;
        }

        public override void SpawnSetup()
        {
            if (this.def.Equals(ThingDef.Named("Campfire_ex_B")))
                Log.Error("CAMPFIRE B");


            if (state == 8)
            {
                base.SpawnSetup(); 
                return;
            }
            this.DeSpawn();
            state = 8;
            Thing tg = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDef.Named("Campfire_ex_B")), this.Position);
            tg.SetFactionDirect(Faction.OfColony);

            IntVec3 TMP = this.Position;
            TMP.x -= 2;
            TMP.z -= 2;

            Room ttz = Room.MakeNew();
            Region ttx = Region.MakeNewUnfilled(TMP);
            ((CustomCampfire)tg).bound = ttx;
            ttx.Room = ttz;

            int x = TMP.x;
            int z = TMP.z;
            int ssz = 0;

            while (true)
            {
                //Find.RegionGrid.SetRegionAt(new IntVec3(x + (ssz % 5),0,z + (ssz / 5)), ttx);
                ssz++;
                if (ssz > 24)
                    break;
            }
            //ttx.extentsClose.maxX = TMP.x + 4;
            //ttx.extentsClose.maxZ = TMP.z + 4;

            FieldInfo myFieldInfo = 
            typeof(Room).GetField("cachedOpenRoofCount",BindingFlags.NonPublic | BindingFlags.Instance);
            //myFieldInfo.SetValue(ttz, 0);
            tg.SpawnSetup();
            state = -3;
        }

        public override void DrawAt(Vector3 drawLoc)
        {
            base.DrawAt(drawLoc);
            FireGraphic.Draw(drawLoc, IntRot.north, this);
        }

    }
}
