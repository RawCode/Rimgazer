using RimWorld;
using RimWorld.SquadAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RC.Rimgazer.Override
{
    class PawnOverride : Pawn
    {
        public PawnOverride()
            : base()
        {


        }

        protected override void ApplyDamage(DamageInfo dinfo)
        {
            Log.Warning("NO DAMAGE PFFF");
            if (true) return;
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
        public override void DeSpawn()
        {
            base.DeSpawn();
            if ((this.jobs != null) && (this.jobs.curJob != null))
            {
                this.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
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



    }
}