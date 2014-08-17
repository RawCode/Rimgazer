using RC.Rimgazer.Event.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer.Event
{
    public abstract class Event 
    {
        static public Dictionary<Type, List<EventHandlerWrapper>> resolvedEvents =
                  new Dictionary<Type, List<EventHandlerWrapper>>();

        static public List<EventHandlerWrapper> roamingHandlers = new List<EventHandlerWrapper>();

        static void registerEventTypeOnBus(Type Target)1
        {
            if (resolvedEvents.ContainsKey(Target))
            {
                EventException.pushException("Event " + Target + " is already registered.");
                return;
            }
            if (!Target.IsSubclassOf(typeof(Event)))
            {
                EventException.pushException("Type " + Target + " is not valid Event.");
                return;
            }

            resolvedEvents.Add(Target, new List<EventHandlerWrapper>());
        }

        static void fireEvent(Event Target)
        {
            if (!resolvedEvents.ContainsKey(Target.GetType()))
            {
                EventException.pushException("Event " + Target + " not registered");
                return;
            }

            List<EventHandlerWrapper> Querry;
            resolvedEvents.TryGetValue(Target.GetType(),out Querry);

            foreach (EventHandlerWrapper EHW in Querry)
            {
                try
                {
                    EHW.handleEvent(Target);
                }
                catch (Exception e) { Log.Error(e.ToString()); }
            }
        }


        //suppress - cancel any futher processing
        //cancel - let other mods to process but cancel output completely
    }
}
