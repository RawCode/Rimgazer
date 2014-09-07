using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    /*
    Root level class (RLC)
    This class required to perform runtime injection at "Gameplay" stage of the game
    Gameplay stage happes as soon as level is loaded from entry or savegame
    Events triggered from Entry stage will happen in gameplay GUI after objects are initialized

    Main uses of this stage:
    1) Temper with map and objects around
    2) Temper with "invisible" objects like storyteller or weather decider

    This class does not require XML def to work
    
    */
    class GazeMapComponent : MapComponent
    {
        static string GitHub = "https://github.com/RawCode/Rimgazer.git";

        //this field used to counter double construction on loading savegame
        static bool kludge = false;

        public GazeMapComponent()
        {
            //counter double construction on loading savegame
            if (kludge) return; kludge ^= true;


            //EventRuntime.resolveValidTypes();
            //EventRuntime.resolveValidEvents();
            //EventRuntime.resolveValidListeners();
            //EventBase.sortHandlerList();

           // if (MapInitData.startedFromEntry)
                //EventBase.fireEvent(new GameStartedEvent());
           // else
                //EventBase.fireEvent(new GameLoadedEvent());
        }

        public override void MapComponentTick()
        {
         //   if (Find.TickManager.tickCount == (DebugSettings.fastEcology ? 1 : 250))
              //  EventBase.fireEvent(new GameFirstTickEvent());
           // EventBase.fireEvent(new GameTickEvent());
        }

        public override void ExposeData()
        {
          //  if (Scribe.mode == LoadSaveMode.LoadingVars)
              //  EventBase.fireEvent(new GameSavedEvent());
        }

       //public override void MapComponentUpdate()
       //{
       // NOT IMPLEMENTED
       //}
    }
    
}
