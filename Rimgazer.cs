using RC.Rimgazer.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    public class Rimgazer : MapComponent
    {
        public Rimgazer()
        {
            new EventRuntime().Initialize();
        }
    }
}