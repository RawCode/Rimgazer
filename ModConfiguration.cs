using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    public class ModConfiguration
    {
        //this is tech demo
        //API based version upcoming *soon(C)*

        public string CustomGreeting = "configurable greeting";

        static public void ResolveConfigurations()
        {
            //configuration bound to mod itself, there is no support for reading configuration of other mods yet
            //at later stage configuration will be implemented as single hashmap with modname:property:value
            //resolving configuration will be performed by API, reading and writting configuration also
            //will be handled by API
        }

        public void ResolveConfiguration()
        {
            //RimWorld load mod's code in way, that support filtering and reinstrumentation and hotplugging
            //this comes with price - there is no "filesystem" path of assembly
            //from perspective of system assembly exists only in memory
            Log.Error(System.Reflection.Assembly.GetExecutingAssembly().FullName);
        }

    }
}
