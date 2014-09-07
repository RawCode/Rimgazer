using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    /*

    Root level class (RLC)
    At static level this class trigger "Entry" event and initialize system.
    At instance level this class resolve def database injections.

    Entry stage happes as soon as game is started or as soon as Rimgazer mod is activated
    Entry event happens in pregame GUI

    Def database injections are moddable, every injection will trigger event
    Initialize event triggered before modifications
    Finalize after modifications

    Rimgazer injections NOT hardcoded and stored in XML defs

    Main uses of this stage:
    1) Temper with pregame GUI, alter world generation or any other pregame process
    2) Temper with def database at level not possible with simple XML defs
    3) Perform unspecified actions

    This class do require XML def in order to work properly, system work on static level, at least
    one instance is required, more possible but not required

    Type that perform dedefines must have valid fieldset
    
    <Redefines>
	    <Redefine>
	    </Redefine>
    </Redefines>

    */
    class Redefine : Def
    {
        static int runcount = 0;
        public override void PostLoad()
        {
            Log.Error("POST LOAD" + runcount);
        }
        public override void ResolveReferences()
        {
            Log.Error("RESOLVE REFERENCES" + runcount);
        }

        static Redefine()
        {
            Log.Error("STATIC" + runcount);
        }

        public Redefine()
        {
            Log.Error("INSTANCE" + runcount);
        }
    }
}
