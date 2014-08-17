using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace RC.Rimgazer.Event
{
    public class EventHandlerWrapper
    {

        static readonly Type annotation = typeof(EventHandlerAttribute);

        public EventListenerWrapper    DeclaringListener;
        public MethodInfo              TargetMethod;
        public EventPriorityEnum       DesiredPriority;
        public Type                    TargetEvent;

        private EventHandlerWrapper(EventListenerWrapper Owner,MethodInfo Target,Type Event,EventPriorityEnum Priority)
        {
            this.DeclaringListener = Owner;
            this.TargetMethod = Target;
            this.DesiredPriority = Priority;
            this.TargetEvent = Event;
        }

        public void handleEvent(EventBase e)
        {
            if (e.GetType() != TargetEvent)
                return;
            TargetMethod.Invoke(DeclaringListener.AnchorValue, new object[] { e });
        }

        public static EventHandlerWrapper getEventHandlerFor(MethodInfo Target, EventListenerWrapper Owner)
        {
            EventHandlerWrapper store = null;

            Object[] attributes = Target.GetCustomAttributes(annotation, false);
            if (attributes == null || attributes.Length == 0)
            {
                EventException.pushException("Method " + Target + " does not have valid annotation.");
                return null;
            }

            if (Target.ReturnType != typeof(void))
            {
                EventException.pushException("Method " + Target + " must return void.");
                return null;
            }

            if (Target.GetParameters().Length != 1)
            {
                EventException.pushException("Method " + Target + " must have single param.");
                return null;
            }

            Log.Error("THIS");

            Log.Error(Target.GetParameters()[0].ParameterType.ToString());

            if (!Target.GetParameters()[0].ParameterType.IsSubclassOf(typeof(EventBase)))
            {
                EventException.pushException("Method " + Target + " must accept subclass of Event");
                return null;
            }

            Type TargetEvent = Target.GetParameters()[0].ParameterType;

            CustomAttributeData annotationData = CustomAttributeData.GetCustomAttributes(Target)[0];
            if (annotationData == null)
            {
                EventException.pushException("Type " + Target + " have malformed annotation.");
                return null;
            }
            if (annotationData.ConstructorArguments.Count != 1)
            {
                EventException.pushException("Type " + Target + " have malformed annotation.");
                return null;
            }

            EventPriorityEnum basePriority = (EventPriorityEnum)annotationData.ConstructorArguments[0].Value;

            store = new EventHandlerWrapper(Owner, Target, TargetEvent, basePriority);

            return store;
        }

    }
}
