using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RC.Rimgazer.Impl
{

    public class PawnImpl //: ThingWithComponents, Strippable
    {

    }

    /*
        public int age = 10;
        public Pawn_ApparelTracker apparel;
        public Pawn_CallTracker caller;
        public Pawn_CarryHands carryHands;
        public Pawn_DrawTracker drawer;
        public Pawn_EquipmentTracker equipment;
        public Pawn_FilthTracker filth;
        public Pawn_FoodTracker food;
        public Gender gender = Gender.Male;
        private static readonly Texture2D GetOutOfBedCommandIcon = ContentFinder<Texture2D>.Get("UI/Commands/GetOutOfBed", true);
        public Pawn_HealthTracker healthTracker;
        public Pawn_InventoryTracker inventory;
        public Faction jailerFactionInt;
        public Pawn_JobTracker jobs;
        public PawnKindDef kindDef;
        public Pawn_NativeVerbs natives;
        private static readonly Texture2D OrdersStartCommandIcon = ContentFinder<Texture2D>.Get("UI/Commands/OrdersStart", true);
        public Pawn_Ownership ownership;
        public Pawn_PathFollower pather;
        public Pawn_PlayerController playerController;
        public Pawn_PrisonerTracker prisoner;
        public Pawn_PsychologyTracker psychology;
        public Pawn_RestTracker rest;
        public Pawn_SkillTracker skills;
        public Pawn_StanceTracker stances;
        public Pawn_StoryTracker story;
        public Pawn_TalkTracker talker;
        public Pawn_Thinker thinker;
        public Pawn_WorkSettings workSettings;

        public PawnImpl()
        {
            this.pather = new Pawn_PathFollower(this);
            this.drawer = new Pawn_DrawTracker(this);
            this.stances = new Pawn_StanceTracker(this);
            this.healthTracker = new Pawn_HealthTracker(this);
            this.jobs = new Pawn_JobTracker(this);
            this.natives = new Pawn_NativeVerbs(this);
            this.inventory = new Pawn_InventoryTracker(this);
            this.filth = new Pawn_FilthTracker(this);
        }

        public void AddAndRemoveComponentsAsAppropriate()
        {
            if ((this.JailerFaction != null) && (this.prisoner == null))
            {
                this.prisoner = new Pawn_PrisonerTracker(this);
            }
            if (this.JailerFaction == null)
            {
                this.prisoner = null;
            }
            if ((base.Faction == Faction.OfColony) && (this.playerController == null))
            {
                this.playerController = new Pawn_PlayerController(this);
                this.workSettings.InitialSetupFromSkills();
            }
            if (base.Faction != Faction.OfColony)
            {
                this.playerController = null;
            }
        }

        public bool AnythingToStrip()
        {
            return ((((this.equipment != null) && this.equipment.HasAnything()) || ((this.apparel != null) && this.apparel.HasAnything())) || ((this.inventory != null) && this.inventory.HasAnything()));
        }

        protected override void ApplyDamage(DamageInfo dinfo)
        {
            this.drawer.Notify_DamageApplied(dinfo);
            this.stances.Notify_DamageTaken(dinfo);
            this.jobs.Notify_DamageTaken(dinfo);
            Brain squadBrain = this.GetSquadBrain();
            if (squadBrain != null)
            {
                squadBrain.Notify_PawnTookDamage(this, dinfo);
            }
            this.healthTracker.ApplyDamage(dinfo, true);
        }

        public void ChangePawnFactionTo(Faction newFaction)
        {
            if (newFaction == base.Faction)
            {
                Verse.Log.Warning(string.Concat(new object[] { "Used ChangePawnFactionTo to change ", this, " to same faction ", newFaction }));
            }
            else
            {
                if (this.JailerFaction != null)
                {
                    this.SetJailerFaction(null);
                }
                Find.ListerPawns.DeRegisterPawn(this);
                Find.PawnDestinationManager.RemovePawnFromSystem(this);
                Brain squadBrain = this.GetSquadBrain();
                if (squadBrain != null)
                {
                    squadBrain.Notify_PawnLost(this, PawnLostCondition.ChangedFaction);
                }
                if (newFaction == Faction.OfColony)
                {
                    this.kindDef = DefDatabase<PawnKindDef>.GetNamed("Colonist");
                }
                base.factionInt = newFaction;
                Reachability.ClearReachabilityCache();
                this.AddAndRemoveComponentsAsAppropriate();
                Find.ListerPawns.RegisterPawn(this);
                PawnInventoryGenerator.GiveAppropriateKeysTo(this);
                if (((this.playerController != null) && this.playerController.Drafted) && (base.Faction != Faction.OfColony))
                {
                    this.playerController.Drafted = false;
                }
                Find.GameEnder.CheckGameOver();
            }
        }

        private void ClearMind()
        {
            this.pather.StopDead();
            this.MindState.Reset();
            this.jobs.StopAll();
        }

        private void ClearReservations()
        {
            Find.PawnDestinationManager.RemovePawnFromSystem(this);
            Find.Reservations.ReleaseAllClaimedBy(this);
        }

        public override void DeSpawn()
        {
            base.DeSpawn();
            if ((this.jobs != null) && (this.jobs.curJob != null))
            {
                this.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
            }
        }

        public override void Destroy(DestroyMode mode = 0)
        {
            if (mode == DestroyMode.Kill)
            {
                this.DropAndForbidEverything();
            }
            if (((mode == DestroyMode.Kill) || (mode == DestroyMode.Vanish)) && (base.Faction == Faction.OfColony))
            {
                Find.StoryWatcher.watcherRampUp.Notify_ColonistIncappedOrKilled(this);
                Find.History.AddGameEvent("ColonistDied".Translate() + "\n\n" + this.Label, GameEventType.BadUrgent, false, string.Empty);
            }
            base.Destroy(mode);
            if (this.ownership != null)
            {
                this.ownership.UnclaimAll();
            }
            this.healthTracker.Health = 0;
            this.ClearMind();
            this.ClearReservations();
            Brain squadBrain = this.GetSquadBrain();
            if (squadBrain != null)
            {
                squadBrain.Notify_PawnLost(this, (mode != DestroyMode.Kill) ? PawnLostCondition.Vanished : PawnLostCondition.IncappedOrKilled);
            }
            Find.ListerPawns.DeRegisterPawn(this);
            Find.GameEnder.CheckGameOver();
        }

        public override void DrawAt(Vector3 drawLoc)
        {
            this.drawer.renderer.RenderPawnAt(drawLoc, BodyDrawType.Normal);
            this.stances.StanceTrackerDraw();
            this.pather.PatherDraw();
        }

        public override void DrawGUIOverlay()
        {
            this.drawer.ui.DrawPawnGUIOverlay();
        }

        public override Material DrawMat(IntRot rot)
        {
            Verse.Log.Error("Direct access to pawn DrawMat isn't possible; pawns have multiple body materials overlaid. " + this.ToString());
            return this.drawer.renderer.graphics.MatsBodyBaseAt(base.rotation, BodyDrawType.Normal).First<Material>();
        }

        public void DropAndForbidEverything()
        {
            if ((this.carryHands != null) && (this.carryHands.CarriedThing != null))
            {
                Thing thing;
                this.carryHands.TryDropCarriedThing(base.Position, ThingPlaceMode.Near, out thing);
            }
            if (this.equipment != null)
            {
                this.equipment.DropAllEquipment(base.Position, true);
            }
            if ((this.inventory != null) && (this.inventory.container.TotalStackCount > 0))
            {
                IEnumerator<Thing> enumerator = this.inventory.container.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        enumerator.Current.SetForbidden(true, false);
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
                this.inventory.DropAll(base.Position);
            }
        }

        public void ExitMap()
        {
            Brain squadBrain = this.GetSquadBrain();
            if (squadBrain != null)
            {
                squadBrain.Notify_PawnLost(this, PawnLostCondition.ExitedMap);
            }
            if ((this.carryHands != null) && (this.carryHands.CarriedThing != null))
            {
                this.carryHands.CarriedThing.Destroy(DestroyMode.Vanish);
            }
            this.Destroy(DestroyMode.Vanish);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.LookDef<PawnKindDef>(ref this.kindDef, "kindDef");
            Scribe_References.LookReference<Faction>(ref this.jailerFactionInt, "jailerFaction");
            Scribe_Deep.LookDeep<Pawn_StoryTracker>(ref this.story, "story", this);
            Scribe_Values.LookValue<Gender>(ref this.gender, "sex", Gender.Male, false);
            Scribe_Values.LookValue<int>(ref this.age, "age", 0, false);
            Scribe_Deep.LookDeep<Pawn_ApparelTracker>(ref this.apparel, "apparel", this);
            Scribe_Deep.LookDeep<Pawn_EquipmentTracker>(ref this.equipment, "equipment", this);
            Scribe_Deep.LookDeep<Pawn_Thinker>(ref this.thinker, "mind", this);
            Scribe_Deep.LookDeep<Pawn_PlayerController>(ref this.playerController, "playerController", this);
            Scribe_Deep.LookDeep<Pawn_JobTracker>(ref this.jobs, "jobs", this);
            Scribe_Deep.LookDeep<Pawn_HealthTracker>(ref this.healthTracker, "healthTracker", this);
            Scribe_Deep.LookDeep<Pawn_PathFollower>(ref this.pather, "pather", this);
            Scribe_Deep.LookDeep<Pawn_InventoryTracker>(ref this.inventory, "inventory", this);
            Scribe_Deep.LookDeep<Pawn_FilthTracker>(ref this.filth, "filth", this);
            Scribe_Deep.LookDeep<Pawn_FoodTracker>(ref this.food, "food", this);
            Scribe_Deep.LookDeep<Pawn_RestTracker>(ref this.rest, "rest", this);
            Scribe_Deep.LookDeep<Pawn_CarryHands>(ref this.carryHands, "carryHands", this);
            Scribe_Deep.LookDeep<Pawn_PsychologyTracker>(ref this.psychology, "psychology", this);
            Scribe_Deep.LookDeep<Pawn_PrisonerTracker>(ref this.prisoner, "prisoner", this);
            Scribe_Deep.LookDeep<Pawn_Ownership>(ref this.ownership, "ownership", this);
            Scribe_Deep.LookDeep<Pawn_TalkTracker>(ref this.talker, "talker", this);
            Scribe_Deep.LookDeep<Pawn_SkillTracker>(ref this.skills, "skills", this);
            Scribe_Deep.LookDeep<Pawn_WorkSettings>(ref this.workSettings, "workSettings", this);
        }

        [DebuggerHidden]
        public override IEnumerable<Command> GetCommands()
        {
            return new <GetCommands>c__IteratorD7 { <>f__this = this, $PC = -2 };
        }

        public override string GetInspectString()
        {
            string report;
            StringBuilder builder = new StringBuilder();
            if (DebugDrawSettings.writeBeauty)
            {
                builder.AppendLine("Instant beauty: " + this.psychology.environment.CurrentInstantBeauty().ToString("##0.000"));
                builder.AppendLine("Averaged beauty: " + this.psychology.environment.TimeAveragedLocalBeauty().ToString("##0.000"));
            }
            if (DebugDrawSettings.writeOpenness)
            {
                builder.AppendLine("Instant openness: " + this.psychology.openness.CurrentInstantOpenness().ToString("##0.000"));
            }
            if ((base.Faction != null) && !base.Faction.def.hidden)
            {
                if (this.gender == Gender.Sexless)
                {
                    object[] args = new object[] { this.KindLabel, base.Faction.name };
                    builder.AppendLine("PawnMainDescWithFactionGenderless".Translate(args));
                }
                else
                {
                    object[] objArray2 = new object[] { this.SexLabel, this.KindLabel, base.Faction.name };
                    builder.AppendLine("PawnMainDescWithFactionGendered".Translate(objArray2));
                }
            }
            else if (this.gender == Gender.Sexless)
            {
                object[] objArray3 = new object[] { this.KindLabel };
                builder.AppendLine("PawnMainDescGenderless".Translate(objArray3));
            }
            else
            {
                object[] objArray4 = new object[] { this.SexLabel, this.KindLabel };
                builder.AppendLine("PawnMainDescGendered".Translate(objArray4));
            }
            switch (this.MindState.Sanity)
            {
                case SanityState.DazedWander:
                    builder.AppendLine("SanityDazedWander".Translate());
                    break;

                case SanityState.GiveUpExit:
                    builder.AppendLine("SanityGiveUpExit".Translate());
                    break;

                case SanityState.Psychotic:
                    builder.AppendLine("SanityPsychotic".Translate());
                    break;

                case SanityState.PanicFlee:
                    builder.AppendLine("SanityPanicFlee".Translate());
                    break;
            }
            if ((this.equipment != null) && (this.equipment.primary != null))
            {
                builder.AppendLine("Equipped".Translate() + ": " + ((this.equipment.primary == null) ? "EquippedNothing".Translate() : this.equipment.primary.Label));
            }
            if ((this.carryHands != null) && (this.carryHands.CarriedThing != null))
            {
                builder.Append("Carrying".Translate() + ": ");
                builder.AppendLine(this.carryHands.CarriedThing.Label);
            }
            if (this.healthTracker.Incapacitated)
            {
                report = "Incapacitated".Translate();
            }
            else if (this.jobs.curJob != null)
            {
                try
                {
                    report = this.jobs.curDriver.GetReport();
                }
                catch (Exception exception)
                {
                    return ("Driver exception " + exception);
                }
            }
            else
            {
                report = "No job.";
            }
            builder.AppendLine(report);
            if (base.attachments != null)
            {
                for (int i = 0; i < base.attachments.Count; i++)
                {
                    builder.AppendLine(base.attachments[i].InfoStringAddon);
                }
            }
            if (this.IsPrisonerOfColony)
            {
                builder.AppendLine("InRestraints".Translate());
            }
            return builder.ToString();
        }

        public override TipSignal GetTooltip()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Label + " (" + this.SexLabel);
            if (this.Label != this.KindLabel)
            {
                builder.Append(" " + this.KindLabel);
            }
            builder.AppendLine(")");
            if ((this.equipment != null) && (this.equipment.primary != null))
            {
                builder.AppendLine(this.equipment.primary.Label);
            }
            builder.AppendLine(HealthUtility.GetGeneralConditionLabel(this));
            if (base.attachments != null)
            {
                foreach (AttachableThing thing in base.attachments)
                {
                    builder.AppendLine(thing.InfoStringAddon);
                }
            }
            if (Find.Selector.SingleSelectedThing != null)
            {
                Pawn singleSelectedThing = Find.Selector.SingleSelectedThing as Pawn;
                if (((singleSelectedThing != null) && (singleSelectedThing.equipment != null)) && ((singleSelectedThing.equipment.primary != null) && (singleSelectedThing.Faction != base.Faction)))
                {
                    Verb_LaunchProjectile verb = singleSelectedThing.equipment.primary.verb as Verb_LaunchProjectile;
                    if (verb != null)
                    {
                        builder.AppendLine();
                        object[] args = new object[] { singleSelectedThing.LabelShort };
                        builder.Append("ShotBy".Translate(args) + ":");
                        if (verb.CanHitTarget(new TargetPack(this)))
                        {
                            builder.Append(verb.HitReportFor(this).GetTextReadout());
                        }
                        else
                        {
                            builder.Append("CannotHit".Translate());
                        }
                    }
                }
            }
            char[] trimChars = new char[] { '\n' };
            return new TipSignal(builder.ToString().TrimEnd(trimChars), base.thingIDNumber * 0x252fd, TooltipPriority.Pawn);
        }

        private int MoveTicksProcessed(float originalTicks)
        {
            float num = 1f;
            if (this.story != null)
            {
                num -= this.story.traits.TotalTraitsEffectOnStat(PawnStatDefOf.MoveSpeed);
            }
            if (((this.carryHands != null) && (this.carryHands.CarriedThing != null)) && (this.carryHands.CarriedThing.def.eType == EntityType.Pawn))
            {
                num += 0.75f;
            }
            if (!Find.RoofGrid.Roofed(base.Position))
            {
                num += Find.WeatherManager.CurMoveTicksAddon;
            }
            if (this.JailerFaction != null)
            {
                num *= 3f;
            }
            float moveSpeedMultiplier = 1f;
            if (this.apparel != null)
            {
                moveSpeedMultiplier = this.apparel.MoveSpeedMultiplier;
            }
            moveSpeedMultiplier *= this.healthTracker.GetEfficiency(PawnActivity.Moving);
            if (moveSpeedMultiplier < 0.05f)
            {
                moveSpeedMultiplier = 0.05f;
            }
            float f = ((1f / moveSpeedMultiplier) * originalTicks) * num;
            if (f < 1f)
            {
                f = 1f;
            }
            if (Mathf.RoundToInt(f) > 450)
            {
            }
            return Mathf.RoundToInt(f);
        }

        public void NewlyIncapacitated()
        {
            if (base.Faction == Faction.OfColony)
            {
                Find.StoryWatcher.watcherRampUp.Notify_ColonistIncappedOrKilled(this);
            }
            this.DropAndForbidEverything();
            this.ClearMind();
            this.ClearReservations();
            Brain squadBrain = this.GetSquadBrain();
            if (squadBrain != null)
            {
                squadBrain.Notify_PawnLost(this, PawnLostCondition.IncappedOrKilled);
            }
            this.stances.CancelActionIfPossible();
            if ((this.playerController != null) && this.playerController.Drafted)
            {
                this.playerController.Drafted = false;
            }
        }

        public void Notify_Teleported()
        {
            this.drawer.tweener.ResetToPosition();
            this.pather.Notify_Teleported_Int();
        }

        public void PreSold()
        {
            if (this.ownership != null)
            {
                this.ownership.UnclaimAll();
            }
            this.SetJailerFaction(null);
            this.DropAndForbidEverything();
            this.ClearMind();
            this.ClearReservations();
            Find.ListerPawns.DeRegisterPawn(this);
        }

        public void SetJailerFaction(Faction newJailer)
        {
            if (newJailer == this.JailerFaction)
            {
                Verse.Log.Error(string.Concat(new object[] { "Set ", this, " jailerFaction to ", newJailer, " when it already is." }));
            }
            this.ClearMind();
            this.ClearReservations();
            bool flag = (this.JailerFaction == null) && (newJailer != null);
            this.jailerFactionInt = newJailer;
            Reachability.ClearReachabilityCache();
            if (flag)
            {
                this.DropAndForbidEverything();
                if (base.Faction != null)
                {
                    base.Faction.Notify_MemberCaptured(this, newJailer);
                }
                Brain squadBrain = this.GetSquadBrain();
                if (squadBrain != null)
                {
                    squadBrain.Notify_PawnLost(this, PawnLostCondition.TakenPrisoner);
                }
                if ((this.playerController != null) && this.playerController.Drafted)
                {
                    this.playerController.Drafted = false;
                }
                this.psychology.thoughts.TryGainThought(ThoughtDef.Named("JustImprisoned"));
            }
            this.AddAndRemoveComponentsAsAppropriate();
            if (this.ownership != null)
            {
                this.ownership.Notify_ChangedJailer();
            }
            Find.ListerPawns.UpdateRegistryForPawn(this);
        }

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.drawer.Notify_Spawned();
            this.pather.ResetToCurrentPosition();
            Find.ListerPawns.RegisterPawn(this);
            if (this.workSettings != null)
            {
                this.workSettings.ApplyWorkDisables();
            }
        }

        public void Strip()
        {
            if (this.equipment != null)
            {
                this.equipment.DropAllEquipment(base.Position, false);
            }
            if (this.apparel != null)
            {
                this.apparel.DropAllApparel(base.Position, false);
            }
            if (this.inventory != null)
            {
                this.inventory.DropAll(base.Position);
            }
        }

        public override void Tick()
        {
            if (DebugSettings.noAnimals && !this.RaceProps.humanoid)
            {
                this.Destroy(DestroyMode.Vanish);
            }
            else
            {
                if (!this.stances.FullBodyBusy)
                {
                    this.pather.PatherTick();
                }
                this.drawer.DrawTrackerTick();
                this.healthTracker.HealthTick();
                this.stances.StanceTrackerTick();
                if (this.equipment != null)
                {
                    this.equipment.EquipmentTrackerTick();
                }
                if (this.jobs != null)
                {
                    this.jobs.JobTrackerTick();
                }
                if (this.carryHands != null)
                {
                    this.carryHands.CarryHandsTick();
                }
                if (this.talker != null)
                {
                    this.talker.TalkTrackerTick();
                }
                if (this.psychology != null)
                {
                    this.psychology.PsychologyTick();
                }
                if (this.food != null)
                {
                    this.food.FoodTick();
                }
                if (this.rest != null)
                {
                    this.rest.RestTick();
                }
                if (this.prisoner != null)
                {
                    this.prisoner.PrisonerTrackerTick();
                }
                if (this.caller != null)
                {
                    this.caller.CallTrackerTick();
                }
                if (this.skills != null)
                {
                    this.skills.SkillsTick();
                }
                if (this.thinker != null)
                {
                    this.thinker.MindTick();
                }
                if (this.playerController != null)
                {
                    this.playerController.PlayerControllerTick();
                }
            }
        }

        public override string ToString()
        {
            if (this.story != null)
            {
                return this.Nickname;
            }
            if (this.kindDef != null)
            {
                return (this.KindLabel + "_" + base.ThingID);
            }
            return base.ThingID;
        }

        public Verb BestAttackVerb
        {
            get
            {
                if ((this.equipment != null) && (this.equipment.primary != null))
                {
                    return this.equipment.primary.verb;
                }
                return this.natives.MeleeVerb(MeleeAttackMode.Lethal);
            }
        }

        public Job CurJob
        {
            get
            {
                return this.jobs.curJob;
            }
        }

        public override Mesh DrawMesh
        {
            get
            {
                return this.drawer.renderer.graphics.BodyMeshSet.MeshAt(base.rotation);
            }
        }

        public override Vector3 DrawPos
        {
            get
            {
                return this.drawer.DrawPos;
            }
        }

        public bool Incapacitated
        {
            get
            {
                return this.healthTracker.Incapacitated;
            }
        }

        public bool IsColonist
        {
            get
            {
                return (base.Faction == Faction.OfColony);
            }
        }

        public bool IsPrisonerOfColony
        {
            get
            {
                return (this.JailerFaction == Faction.OfColony);
            }
        }

        public Faction JailerFaction
        {
            get
            {
                return this.jailerFactionInt;
            }
        }

        public string KindLabel
        {
            get
            {
                return this.kindDef.label;
            }
        }

        public override string Label
        {
            get
            {
                if (!this.RaceProps.hasStory)
                {
                    return base.Label;
                }
                if (this.story.adulthood == null)
                {
                    return this.Nickname;
                }
                return (this.Nickname + ", " + this.story.adulthood.titleShort);
            }
        }

        public override string LabelShort
        {
            get
            {
                if (this.RaceProps.humanoid)
                {
                    return this.Nickname;
                }
                return this.Label;
            }
        }

        public Verse.AI.MindState MindState
        {
            get
            {
                return this.thinker.mindState;
            }
        }

        public PawnName Name
        {
            get
            {
                return this.story.name;
            }
        }

        public string Nickname
        {
            get
            {
                return this.Name.nick;
            }
        }

        public PathingParameters PathParams
        {
            get
            {
                return this.kindDef.pathParams;
            }
        }

        public RaceProperties RaceProps
        {
            get
            {
                return base.def.race;
            }
        }

        public string SexLabel
        {
            get
            {
                if (this.gender == Gender.Male)
                {
                    return "Male".Translate();
                }
                if (this.gender == Gender.Female)
                {
                    return "Female".Translate();
                }
                if (this.gender == Gender.Sexless)
                {
                    return string.Empty;
                }
                return "NOSEXLABEL";
            }
        }

        public string StoryArchetypeLabel
        {
            get
            {
                return this.story.adulthood.title;
            }
        }

        public int TicksPerMoveCardinal
        {
            get
            {
                return this.MoveTicksProcessed(this.RaceProps.baseMoveTicks_Cardinal);
            }
        }

        public int TicksPerMoveDiagonal
        {
            get
            {
                return this.MoveTicksProcessed(this.RaceProps.baseMoveTicks_Diagonal);
            }
        }

        public bool UnderPlayerControl
        {
            get
            {
                return (((base.SpawnedInWorld && this.IsColonist) && (this.MindState.Sanity == SanityState.Normal)) && (this.JailerFaction == null));
            }
        }
        }
    }*/
}