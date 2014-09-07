using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    public class EventHandlerTag : Attribute
    {
        public EventHandlerTag() { }
        //public EventHandlerTag(EventHandlerFlag a) { }
        //this constructor is used for BindToField and BindToMethod handlers currently
        //public EventHandlerTag(EventHandlerFlag a, string b) { }

    }
}
