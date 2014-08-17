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
    [PersistentComment("You can download source code from github: https://github.com/RawCode/Rimgazer.git")]
    public class EventRuntime
    {
        public static List<Type> resolvedListeners  = new List<Type>();
        public static List<Type> resolvedEvents     = new List<Type>();
        public static readonly Type TYPE_EVENT      = typeof(EventBase);
        public static readonly Type TYPE_LISTENER   = typeof(EventListenerAttribute);

        /**
        This method will resolve types that can be used by system:

        Subclasses of Event class;
        Types with [EventListenerAttribute] annotation;

        Everything else is mostly ignored.
        */

        public static void resolveValidEvents()
        {
            foreach (Type Target in resolvedEvents)
            {
                EventBase.registerEvent(Target);
            }
        }

        public static void resolveValidListeners()
        {
            foreach (Type Target in resolvedListeners)
            {
                if (EventListenerWrapper.getOrCreateWrapperFor(Target) == null)
                    Log.Error(EventException.popException());
            }
        }

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
                            //Event types not allowed to handle events.
                            //Servere risk of infinite loops.
                            resolvedEvents.Add(ResolvedType);
                            continue;
                        }
                        if (ResolvedType.GetCustomAttributes(TYPE_LISTENER, false).Length != 0)
                            resolvedListeners.Add(ResolvedType);
                    }
                }
            }

            Log.Warning("Found " + resolvedEvents.Count + " events");
            Log.Warning("Found " + resolvedListeners.Count + " listeners");
        }

    }
}