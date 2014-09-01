using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;
using RimWorld;

namespace ColonistCreationMod
{
    public static class ModdedMapInitParams
    {
        public static bool startedFromEntry;
        public static string mapToLoad;
        public static StorytellerDef chosenStoryteller;
        public static List<Pawn> colonists;

        public static bool StartedDirectInEditor
        {
            get
            {
                return ModdedMapInitParams.mapToLoad == string.Empty && !ModdedMapInitParams.startedFromEntry;
            }
        }

        static ModdedMapInitParams()
        {
            MapInitParams.mapSize = 150;
            ModdedMapInitParams.colonists = new List<Pawn>();
            ModdedMapInitParams.Clear();
        }

        public static void Clear()
        {
            ThingIDCounter.Clear();
            ModdedMapInitParams.startedFromEntry = false;
            ModdedMapInitParams.mapToLoad = string.Empty;
        }

        public static void GenerateColonists()
        {
            do
			{
                ModdedMapInitParams.colonists.Clear();
				for (int i = 0; i < ColonistNum.Amount; i++)
				{
                    ModdedMapInitParams.colonists.Add(PawnGenerator.GeneratePawn("Colonist", Faction.OfColony));
				}
			}
            while (!ModdedMapInitParams.AnyoneCanDoBasicWorks());
            ModdedMapInitParams.chosenStoryteller = DefDatabase<StorytellerDef>.GetNamed("Cassandra");
            if (ModdedMapInitParams.chosenStoryteller == null)
			{
                ModdedMapInitParams.chosenStoryteller = (
					from d in DefDatabase<StorytellerDef>.AllDefs
					orderby d.listOrder
					select d).First<StorytellerDef>();
			}
        }

        public static bool AnyoneCanDoBasicWorks()
        {
            if (ModdedMapInitParams.colonists.Count == 0)
            {
                return false;
            }
            WorkTypeDef[] array = new WorkTypeDef[]
			{
				WorkTypeDefOf.Hauling,
				WorkTypeDefOf.Construction,
				WorkTypeDefOf.Mining,
				WorkTypeDefOf.Mining
			};
            WorkTypeDef[] array2 = array;
            WorkTypeDef wt;
            for (int i = 0; i < array2.Length; i++)
            {
                wt = array2[i];
                if (!ModdedMapInitParams.colonists.Any((Pawn col) => !col.story.WorkTypeIsDisabled(wt)))
                {
                    return false;
                }
            }
            return true;
        }

        public static Pawn RegeneratePawn(Pawn p)
        {
            Pawn pawn = PawnGenerator.GeneratePawn("Colonist", Faction.OfColony);
            ModdedMapInitParams.colonists[ModdedMapInitParams.colonists.IndexOf(p)] = pawn;
            return pawn;
        }
    }
}
