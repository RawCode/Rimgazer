using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class EventHandlerAttribute : Attribute
    {
        public EventHandlerAttribute(EventPriorityEnum priority)
        {
        }
    }
}
