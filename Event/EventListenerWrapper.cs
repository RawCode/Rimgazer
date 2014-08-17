using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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


        public void resolveAllEventHandlers()
        {
            foreach (MethodInfo method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                EventHandlerWrapper tmpz = EventHandlerWrapper.getEventHandlerFor(method, this);
                if (tmpz == null)
                    continue;
                this.DescendantHandlers.Add(tmpz);
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
            if (Target.IsSubclassOf(typeof(Event)))
            {
                EventException.pushException("Type " + Target +" cant be Event and Listener at same time.");
                return null;
            }
            Object[] attributes = Target.GetCustomAttributes(annotation, false);
            if (attributes == null || attributes.Length == 0)
            {
                EventException.pushException("Type " + Target +" does not have valid annotation.");
                return null;
            }

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

            return store;
        }
    }
}
