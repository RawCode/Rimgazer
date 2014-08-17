using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;
using System.Reflection;

namespace RC.Rimgazer.Event
{
    [PersistentComment("You can download source code from github:")]
    [PersistentComment("https://github.com/RawCode/Rimgazer.git")]
    public class EventRuntime
    {

        public static List<Type> resolvedListeners  = new List<Type>();
        public static List<Type> resolvedEvents     = new List<Type>();
        public static readonly Type TYPE_EVENT      = typeof(Event);
        public static readonly Type TYPE_LISTENER   = typeof(EventListenerAttribute);

        /**

        */

        public static void resolveValidTypes()
        {
            foreach ( Mod ResolvedMod in LoadedModManager.LoadedMods)
            {
                foreach( Assembly ResolvedAssembly in ResolvedMod.assemblies.loadedAssemblies)
                {
                    foreach (Type ResolvedType in ResolvedAssembly.GetTypes())
                    {
                        if (ResolvedType.IsSubclassOf(TYPE_EVENT))
                        {
                            resolvedEvents.Add(ResolvedType);
                            continue;
                        }


                    }
                }
            }
        }

        public void Initialize()
        {
            Mod[] lm = LoadedModManager.LoadedMods.ToArray();

            foreach (Mod m in lm)
            {
                foreach (Assembly a in m.assemblies.loadedAssemblies)
                {
                    foreach (Type t in a.GetTypes())
                    {
                        //try
                        //{
                            EventListenerWrapper tz = EventListenerWrapper.getOrCreateWrapperFor(t);
                            if (tz != null)
                                Log.Warning(t + " is registered as listener");
                            else
                            {
                                Log.Warning(EventException.popException());
                            }
                       // }
                        //catch(Exception e)
                        //{
                         //   Log.Warning(e.ToString());
                       // }


                        //if (t.IsSubclassOf(typeof(Event)))
                        //    Log.Error(t.ToString() + " EVENT DEFINITION FOUND");

                        //Log.Warning(t.ToString());
                       // if (t.GetCustomAttributes(typeof(EventListenerAttribute),false).Length != 0)
                       //     Log.Warning(t.ToString() + " registered as listener");
                    }
                }
            }
        }
    }
}
