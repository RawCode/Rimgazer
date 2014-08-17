using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer.Event.Unit
{
    public class PawnEvent : EventBase
    {
        public Pawn pawn;
        public bool suppressed;
        public PawnEvent(Pawn target)
        {
            this.pawn = target;
        }

        public void Suppress()
        {
            suppressed = true;
        }
    }
}
