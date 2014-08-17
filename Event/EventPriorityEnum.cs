using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC.Rimgazer.Event
{
    public enum EventPriorityEnum
    {
        ENTRY          //One per event
        ,LOW
        ,DEFAULT        //execution order for same level handler is undefined
        ,HIGH
        ,FINAL          //One per event
    }
}
