using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace RimWorld
{
    public static class ModdedPawnInventoryGenerator
    {
        public static void GenerateWeaponFor(Pawn pawn)
        {
            if (pawn.kindDef.weaponTags == null)
            {
                return;
            }
            if (!pawn.RaceDef.humanoid && !pawn.RaceDef.mechanoid)
            {
                return;
            }
            float weaponMoney = pawn.kindDef.moneyForWeapons.RandomInRange;
            IEnumerable<ThingDef> source =
                from weapon in DefDatabase<ThingDef>.AllDefs
                where weapon.equipmentType == EquipmentType.Primary && weapon.basePrice <= weaponMoney && weapon.canBeSpawningInventory && weapon.weaponTags != null && (pawn.Faction == null || pawn.Faction.def.techLevel.CanSpawnWithEquipmentFrom(weapon.techLevel)) && pawn.kindDef.weaponTags.Any((string tag) => weapon.weaponTags.Contains(tag))
                select weapon;
            if (source.Any<ThingDef>())
            {
                ThingDef def = source.RandomElementByWeight((ThingDef w) => w.basePrice);
                Equipment newEq = (Equipment)ThingMaker.MakeThing(def);
                pawn.equipment.AddEquipment(newEq);
            }
        }
        public static void GiveAppropriateKeysTo(Pawn pawn)
        {
            if (pawn.inventory == null)
            {
                return;
            }
            if (pawn.Faction == Faction.OfColony && !pawn.inventory.container.Contains(ThingDefOf.DoorKey))
            {
                pawn.inventory.container.TryAdd(ThingMaker.MakeThing(ThingDefOf.DoorKey));
            }
        }
        public static void GenerateInventoryFor(Pawn p)
        {
            foreach (ThingAmount current in p.kindDef.fixedInventory)
            {
                Thing thing = ThingMaker.MakeThing(current.thingDef);
                thing.stackCount = current.count;
                p.inventory.container.TryAdd(thing);
            }
            if (p.kindDef.inventoryOptions != null)
            {
                foreach (Thing current2 in p.kindDef.inventoryOptions.ResolvedThings())
                {
                    p.inventory.container.TryAdd(current2);
                }
            }
        }
    }
}
