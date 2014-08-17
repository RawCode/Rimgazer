using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace RC.Rimgazer.Event
{
    public class EventListenerWrapper
    {
        static public Dictionary<Type, EventListenerWrapper> resolvedEventListeners = 
                  new Dictionary<Type, EventListenerWrapper>();
        static readonly Type annotation = typeof(EventListenerAttribute);

        public FieldInfo                    AnchorField;
        public Object                       AnchorValue;
        public Type                         DeclaringType;
        public MethodInfo                   IniterMethod;
        public List<EventHandlerWrapper>    DescendantHandlers = new List<EventHandlerWrapper>();
        public bool                         Disabled = false;

        private EventListenerWrapper(FieldInfo Anchor, MethodInfo Initer,Type Owner)
        {
            this.AnchorField    = Anchor;
            this.IniterMethod   = Initer;
            this.DeclaringType  = Owner;
            this.AnchorValue = AnchorField.GetValue((object)null);
        }

        public void resolveAllEventHandlers(Type Target)
        {
            Log.Error("NAME OF TYPE AT PLAY IS " + Target);
            foreach (MethodInfo method in Target.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Log.Error("NAME OF METHOD AT PLAY IS " + method);
                EventHandlerWrapper tmpz = EventHandlerWrapper.getEventHandlerFor(method, this);
                if (tmpz == null)
                {
                    Log.Warning(EventException.popException());
                    continue;
                }
                this.DescendantHandlers.Add(tmpz);
                EventBase.registerHandler(tmpz);
            }
        }

        public void refreshAnchor()
        {
            this.AnchorValue = AnchorField.GetValue(null);
        }

        public static EventListenerWrapper getOrCreateWrapperFor(Type Target)
        {
            EventListenerWrapper store = null;
            if (resolvedEventListeners.TryGetValue(Target,out store))
                return store;

            CustomAttributeData annotationData = CustomAttributeData.GetCustomAttributes(Target)[0];
            if (annotationData == null)
            {
                EventException.pushException("Type " + Target + " have malformed annotation.");
                return null;
            }
            if (annotationData.ConstructorArguments.Count != 2)
            {
                EventException.pushException("Type " + Target + " have malformed annotation.");
                return null;
            }

            string anchorDef = (string)annotationData.ConstructorArguments[0].Value;
            string initerDef = (string)annotationData.ConstructorArguments[1].Value;

            if (anchorDef == null || anchorDef.Equals(""))
            {
                EventException.pushException("Type " + Target + " have invalid anchor field def.");
                return null;
            }
            if (initerDef == null || initerDef.Equals(""))
            {
                EventException.pushException("Type " + Target + " have invalid initializer def.");
                return null;
            }

            FieldInfo anchorField = Target.GetField(anchorDef,BindingFlags.Static | BindingFlags.Public);
            if (anchorField == null)
            {
                EventException.pushException("Type " + Target + " have no staticpublic field " + anchorDef);
                return null;
            }

            MethodInfo initerMethod = Target.GetMethod(initerDef, BindingFlags.Static | BindingFlags.Public);
            if (initerMethod == null)
            {
                EventException.pushException("Type " + Target + " have no staticpublic method " + initerDef);
                return null;
            }

            if (initerMethod.GetParameters().Length != 0 || initerMethod.ReturnType != typeof(void))
            {
                EventException.pushException("Method " + initerMethod + " must be void()");
                return null;
            }
            try
            {
                initerMethod.Invoke(null, null);
            }
            catch (Exception e)
            {
                EventException.pushException(e.ToString());
                return null;
            }
            store = new EventListenerWrapper(anchorField, initerMethod,Target);
            resolvedEventListeners.Add(Target, store);
            store.resolveAllEventHandlers(Target);

            return store;
        }
    }
}
