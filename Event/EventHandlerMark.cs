using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    class EventHandlerMark : Attribute
    {
        //ONLY real purporse of this class, to mark types, fields or methods
        //data stored directly inside assembly files
        public EventHandlerMark(EventHandlerFlags a) { }
    }
}
