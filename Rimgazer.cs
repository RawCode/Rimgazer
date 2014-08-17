using RC.Rimgazer.Event;
using RC.Rimgazer.Event.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RC.Rimgazer
{
    public class Rimgazer : MapComponent
    {
        static bool kludge = false;
        public Rimgazer()
        {
            if (kludge) return;
            kludge ^= true;

            EventRuntime.resolveValidTypes();
            EventRuntime.resolveValidEvents();
            EventRuntime.resolveValidListeners();
            EventBase.sortHandlerList();

            if (MapInitData.startedFromEntry)
                EventBase.fireEvent(new GameStartedEvent());
            else
                EventBase.fireEvent(new GameLoadedEvent());
        }

        public override void MapComponentTick()
        {
            if (Find.TickManager.tickCount == (DebugSettings.fastEcology ? 1 : 250))
                EventBase.fireEvent(new GameFirstTickEvent());
            EventBase.fireEvent(new GameTickEvent());
        }

        public override void ExposeData()
        {
            if (Scribe.mode == LoadSaveMode.LoadingVars)
                EventBase.fireEvent(new GameSavedEvent());
        }
    }
}