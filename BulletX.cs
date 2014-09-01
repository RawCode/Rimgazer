using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using VerseBase;

namespace RC.Rimgazer
{
    public class Bulletx : Bullet
    {
        Color test;
        static int zz = 0;
        static Bulletx()
        {
            Log.Warning("static invoked");
        }

        public Bulletx()
        {
            test = colors.RandomElement<Color>();
            Log.Warning("instance invoked");
        }

        static Color[] colors = new Color[]
        {
            Color.red,
            Color.magenta,
            Color.grey
        };
        public override void Draw()
        {
            zz++;
            if (zz % 100 == 0)
                Log.Warning(this.ExactRotation.ToString());

            //Log.Warning("Draw method");
            // base.def.drawMat;
            Material rs = base.def.drawMat;
            rs.color = test;
            Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, base.def.drawMat, 0);
            base.Comps_Draw();
        }

        public override Material DrawMat(IntRot rot)
        {
            Log.Warning("DrawMat method");
            Material tmp = base.DrawMat(rot);
            tmp.color = Color.red;
            return tmp;
        }
    }
}
