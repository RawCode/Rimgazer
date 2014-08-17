using RC.Rimgazer;
using RC.Rimgazer.Event;
using RC.Rimgazer.Event.Game;
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
        public void testa(GameSavedEvent e)
        {
            Log.Warning("FINAL");
        }

        [EventHandler(EventPriorityEnum.DEFAULT)]
        public void testb(GameSavedEvent e)
        {
            Log.Warning("DEFAULT");
        }

        [EventHandler(EventPriorityEnum.LOW)]
        public void testc(GameSavedEvent e)
        {
            Log.Warning("LOW");
        }

        public static SampleOne instance;

        public static void entry()
        {
            instance = new SampleOne();
            Log.Warning("Sample one listener is initialized");
        }
    }
}