using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    /*
    Root level class (RLC)
    This class required to perform runtime injection at "Entry" stage of the game
    Entry stage happes as soon as game is started or as soon as mod is activated
    Events triggered from Entry stage will happen in pregame GUI

    Main uses of this stage:
    1) Temper with pregame GUI, alter world generation or any other pregame process
    2) Temper with def database, alter describtions of objects without use of XML defs
    3) Perform unspecified actions

    This class do require XML def in order to work properly, this is only essential XML of system so far:
    
    <StageEntrys>
	    <StageEntry>
	    </StageEntry>
    </StageEntrys>

    */
    class StageEntry : Def
    {
        public override void PostLoad()
        {
            Log.Error("POST LOAD");
        }
        public override void ResolveReferences()
        {
            Log.Error("RESOLVE REFERENCES");
        }

        static StageEntry()
        {
            Log.Error("STATIC");
        }

        public StageEntry()
        {
            Log.Error("INSTANCE");
        }
    }
}
