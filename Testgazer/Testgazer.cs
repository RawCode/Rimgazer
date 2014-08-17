using RC.Rimgazer;
using RC.Rimgazer.Event;
using RC.Rimgazer.Event.Game;
using RC.Rimgazer.Event.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RT.Testgazer
{
    [EventListener("instance", "entry")]
    class SampleOne
    {
        [EventHandler(EventPriorityEnum.FINAL)]
        public void testa(GameStartedEvent e)
        {
            Log.Warning("Game started event with FINAL priority");
        }

        [EventHandler(EventPriorityEnum.DEFAULT)]
        public void testb(GameStartedEvent e)
        {
            Log.Warning("Game started event with DEFAULT priority");
        }

        [EventHandler(EventPriorityEnum.LOW)]
        public void testc(GameStartedEvent e)
        {
            Log.Warning("Game started event with LOW priority");
        }

        [EventHandler(EventPriorityEnum.LOW)]
        public void disableFrendlyFire(UnitDamagedEvent e)
        {
            //Log.Warning(e.damageMeta.Instigator.ToString());
            //Log.Warning(e.damageMeta.Source.ToString());

            if (e.damageMeta.Instigator == null)
                return;

            Pawn source = e.damageMeta.Instigator as Pawn;

            if (source == null) 
                return;

            if (source.MindState.Sanity != Verse.AI.SanityState.Normal)
                return;

            if (e.pawn.Faction == source.Faction)
            {
                //Log.Warning("Disabled frendly fire event");
                e.suppressed = true;
            }
        }




        public static SampleOne instance;

        public static void entry()
        {
            instance = new SampleOne();
            Log.Warning("Event listener demo is initialized");
        }
    }
}