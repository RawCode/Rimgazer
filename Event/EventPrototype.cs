using RC.Rimgazer.Event.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer.Event
{
    /*
    public class EventRoot 
    {

       // static private EventHandler







      ///  static public Dictionary<Type, List<EventHandlerWrapper>> resolvedEvents =
      //            new Dictionary<Type, List<EventHandlerWrapper>>();
      //  static public int topSize = 0;

        public static void registerEvent(Type Target)
        {
            if (resolvedEvents.ContainsKey(Target))
            {
                EventException.pushException("Event " + Target + " is already registered");
                return;
            }
      //      resolvedEvents.Add(Target, new List<EventHandlerWrapper>());
            //Log.Warning(Target + " registered as event");
        }

        public static void sortHandlerList()
        {
         //   int     levels                      = Enum.GetNames(typeof(EventPriorityEnum)).Length; //5
         //   int[]   offsetByLevel               = new int[levels];                                 //5
         //   EventHandlerWrapper[] sortingPool   = new EventHandlerWrapper[levels * topSize];       //5*3

            //Log.Warning("SortHandlerList DATA");
            //Log.Warning(levels.ToString());
            //Log.Warning(topSize.ToString());

          //  foreach (List<EventHandlerWrapper> list in resolvedEvents.Values)
            {
                if (list.Count <= 1) continue;//no need to sort array of 1 element actually

                //here we have count of 3 by default;


                //reset offsets each iteration, this faster then reseting sortingpool
                for (int i = 0; i < levels; i++)
                {
                    offsetByLevel[i] = 0;
                }

                foreach (EventHandlerWrapper wrapper in list)
                {
                    int E2I = (int)wrapper.DesiredPriority; //0-4 range
                    int targetindex = (E2I * topSize) + offsetByLevel[E2I];
                    offsetByLevel[E2I]++;
                    sortingPool[targetindex] = wrapper;
                }

                //after this invocation 3 event levels will have offsetByLevel equals to 1
                //2 other levels will have offsetByLevel equals to 0

                //we clear list in order to inject all objects in new order
                list.Clear();

                //since execution order is fullcustom we use unlimited loop

                int iterator = 0;
                int shift = 0;

                while(true)
                {
                    if (iterator >= offsetByLevel[shift] + (shift * topSize))
                    {
                        //by default this triggered when we hit event level with no handlers
                        //each next level have offset of 3(by default)
                        // in such case we shift both values and continue
                        //do not shift if shift already 4
                        if (shift == levels - 1)
                            break;
                        shift++;
                        iterator = shift * topSize;
                        continue;
                    }
                    // if everything fine we add values to new array
                    if (sortingPool[iterator] == null)
                    {
                        Log.Warning("ERROR POINTER TO ZERO OBJECTS FOME ISSUE RISED");
                        Log.Warning("ITERATOR ID TO BAD SLOT IS " + iterator);
                        break;
                    }
                    list.Add(sortingPool[iterator]);
                    iterator++;
                }

                //Log.Error("LIST OF SOMETHING");
                //foreach (EventHandlerWrapper ex in sortingPool)
                //    if (ex != null)
                      //  Log.Warning(ex.ToString());
            }

            /*
                    if (iterator == offsetstore[step] + priorityDimension * topSize)
                    {
                        if (step == priorityDimension - 1)
                            break;
                        step++;
                        iterator = step * topSize;
                        continue;
                    }
                    list.Add(sortingPool[iterator]);
                    iterator++;
            }*/
      //  }
/*
        public static void registerHandler(EventHandlerWrapper Target)
        {
            //Log.Warning("Event handler probably will be registered " + Target);
            if (!resolvedEvents.ContainsKey(Target.TargetEvent))
            {
                EventException.pushException("Handler " + Target + " linked to invalid type");
                return;
            }
            List<EventHandlerWrapper> handlers = null;
            resolvedEvents.TryGetValue(Target.TargetEvent, out handlers);
            handlers.Add(Target);

            //Log.Error("HANDLERS HASHCODE IS " + handlers.GetHashCode().ToString());

            //Log.Warning("Event handler is registered " + Target);

            //Log.Warning("EVENT HANDLER IS REGISTERED TO");
            //Log.Warning(Target.TargetEvent.ToString());
            if (handlers.Count > topSize) topSize = handlers.Count;

        }

        public static void fireEvent(EventRoot Target)
        {
            if (!resolvedEvents.ContainsKey(Target.GetType()))
            {
                EventException.pushException("Event " + Target + " not registered");
                return;
            }

            List<EventHandlerWrapper> Querry;

            //Log.Warning("TYPE AND HASHKEY FOR FIRE EVENT IS");
            //Log.Warning(Target.GetType().ToString());

            resolvedEvents.TryGetValue(Target.GetType(),out Querry);

            //Log.Error("QUERRY HASHCODE IS " + Querry.GetHashCode().ToString());

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
    }*/
}