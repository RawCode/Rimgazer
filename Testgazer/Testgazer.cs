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
        [EventHandler(EventPriorityEnum.HIGH)]
        public void test(GameSavedEvent e)
        {
            Log.Warning("GameSavedEvent is thrown");
        }

        public static SampleOne instance;

        public static void entry()
        {
            instance = new SampleOne();
            Log.Warning("Sample one listener is initialized");
        }
    }
}