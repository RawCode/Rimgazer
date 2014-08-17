using RC.Rimgazer.Event;
using RC.Rimgazer.Event.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    public class Rimgazer : MapComponent
    {
        public Rimgazer()
        {
            EventRuntime.resolveValidTypes();
            EventRuntime.resolveValidEvents();
            try
            {
                EventRuntime.resolveValidListeners();
                EventBase.sortHandlerList();
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
            }
            EventBase.fireEvent(new GameSavedEvent());


            foreach (Type t in EventBase.resolvedEvents.Keys)
                Log.Warning(t.ToString());

        }
    }
}