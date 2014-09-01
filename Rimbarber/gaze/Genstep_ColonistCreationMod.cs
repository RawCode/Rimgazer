using System;
using System.Linq;
using System.Collections.Generic;

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistCreationMod
{
    public class Genstep_ColonistCreationMod : Genstep
    {
        public List<bool> baseStats = new List<bool>();
        public List<bool> choice = new List<bool>();
        public List<string> exemptShirts = new List<string>();
        public List<string> exemptCoats = new List<string>();
        public List<string> exemptWeapons = new List<string>();
        public List<StartingItemRecord> startingItems = new List<StartingItemRecord>();
        public static List<StartingItemRecord> StartingItems = new List<StartingItemRecord>();
        public static List<bool> BaseStats = new List<bool>();
        public static List<bool> Choice = new List<bool>();
        public static List<string> ExemptShirts = new List<string>();
        public static List<string> ExemptCoats = new List<string>();
        public static List<string> ExemptWeapons = new List<string>();
        public static bool canInstaDropDuringInit = ModdedMapInitParams.StartedDirectInEditor;
        public static List<List<Thing>> droplist = new List<List<Thing>>();

        public override void Generate()
        {
            BaseStats = baseStats;
            Choice = choice;
            ExemptShirts = exemptShirts;
            ExemptCoats = exemptCoats;
            ExemptWeapons = exemptWeapons;
            StartingItems = startingItems;

            if (LanguageDatabase.activeLanguage.FriendlyName != "English")
            {
                try
                {
                    LoadLanguage.Load();
                }
                catch (Exception e)
                {
                    Log.Message(e.Message);
                }
            }
            
            if (Choice[0] == true)
            {
                Find.LayerStack.Add(new ColonistBypass());
            }
            else
            {
                ColonistManager.Population.Clear();
                if (BaseStats[0] == false)
                {
                    Find.LayerStack.Add(new ColonistDifficulty());
                }
                else
                {
                    Find.LayerStack.Add(new ColonistNum());
                }
            }
        }

        public static void SpawnStuff()
        {
            foreach (Pawn current in ModdedMapInitParams.colonists)
            {
                current.psychology.thoughts.GainThought(ThoughtDef.Named("NewColonyOptimism"));
                current.SetFactionDirect(Faction.OfColony);
                current.AddAndRemoveComponentsAsAppropriate();
                PawnInventoryGenerator.GiveAppropriateKeysTo(current);
            }
            EnsureAllWorkTypesAreAssigned();
            bool canInstaDropDuringInit = ModdedMapInitParams.StartedDirectInEditor;

            List<List<Thing>> list = new List<List<Thing>>();

            foreach (Pawn current2 in ModdedMapInitParams.colonists)
            {
                list.Add(new List<Thing>
				{
					current2
				});
            }

            DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot, list, 110, canInstaDropDuringInit, false);

            List<Thing> list2 = new List<Thing>();
            for (int i = 0; i < 16; i++)
            {
                Thing item = ThingMaker.MakeThing(ThingDef.Named("MealSurvivalPack"));
                list2.Add(item);
            }

            List<List<Thing>> list3 = new List<List<Thing>>();
            list3.Add(list2);

            try
            {
                DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot + new IntVec3(7, 0, 7), list3, 110, canInstaDropDuringInit, true);
            }
            catch (Exception e)
            {
                Log.Message(e.Message);
            }
        }

        private static void EnsureAllWorkTypesAreAssigned()
        {
            foreach (WorkTypeDef w in DefDatabase<WorkTypeDef>.AllDefs)
            {
                Pawn pawn;
                if (!ModdedMapInitParams.colonists.Any((Pawn col) => col.workSettings.WorkIsActive(w)) && (
                    from col in ModdedMapInitParams.colonists
                    where !col.story.WorkTypeIsDisabled(w)
                    select col).TryRandomElement(out pawn))
                {
                    pawn.workSettings.SetPriority(w, 4);
                }
            }
        }

        public static void SpawnStartingColonists()
        {
            foreach (Pawn current in MapInitParams.colonists)
            {
                current.psychology.thoughts.GainThought(ThoughtDef.Named("NewColonyOptimism"));
                current.SetFactionDirect(Faction.OfColony);
                current.AddAndRemoveComponentsAsAppropriate();
                PawnInventoryGenerator.GiveAppropriateKeysTo(current);
            }
            EnsureAllWorkTypesAreAssigned();
            bool startedDirectInEditor = MapInitParams.StartedDirectInEditor;
            List<List<Thing>> list = new List<List<Thing>>();
            foreach (Pawn current2 in MapInitParams.colonists)
            {
                list.Add(new List<Thing>
				{
					current2
				});
            }
            int num = 0;
            foreach (StartingItemRecord current3 in StartingItems)
            {
                Thing thing = ThingMaker.MakeThing(current3.thingDef);
                thing.SetFactionDirect(Faction.OfColony);
                list[num].Add(thing);
                num++;
                if (num >= list.Count)
                {
                    num = 0;
                }
            }
            bool canInstaDropDuringInit = startedDirectInEditor;
            DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot, list, 110, canInstaDropDuringInit, true);
            List<Thing> list2 = new List<Thing>();
            for (int i = 0; i < 16; i++)
            {
                Thing item = ThingMaker.MakeThing(ThingDef.Named("MealSurvivalPack"));
                list2.Add(item);
            }
            List<List<Thing>> list3 = new List<List<Thing>>();
            list3.Add(list2);
            canInstaDropDuringInit = startedDirectInEditor;
            DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot + new IntVec3(7, 0, 7), list3, 110, canInstaDropDuringInit, true);
        }

    }
}
