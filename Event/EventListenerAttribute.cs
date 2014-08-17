using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EventListenerAttribute : Attribute
    {
        public EventListenerAttribute(string Method,string Field)
        {
        }
    }
}
