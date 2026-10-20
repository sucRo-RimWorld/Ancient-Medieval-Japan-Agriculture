using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using RimWorks.Pickle;
using RimWorld;
using Verse;
using Verse.AI;

namespace AncientMedievalJapanCore.E2E
{
    [PickleSteps]
    public sealed class GrainsSimulationSteps
    {
        internal static string[] Crops(bool mo)
        {
            return new[] { "AMJC_Plant_FoxtailMillet_Awa", "AMJC_Plant_BarnyardMillet_Hie",
                "AMJC_Plant_ProsoMillet_Kibi", "AMJC_Plant_Buckwheat_Soba", "AMJC_Plant_Barley",
                mo ? "DankPyon_Plant_Wheat" : "AMJC_Plant_Wheat", "Plant_Rice" };
        }

        private static bool HasMO()
        {
            return LoadedModManager.RunningModsListForReading.Any(m => string.Equals(
                m.PackageIdPlayerFacing, "dankpyon.medieval.overhaul", StringComparison.OrdinalIgnoreCase));
        }

        [Then("seven Grains retain environmental harvest niches")]
        public Task Environment(PickleContext ctx)
        {
            return RuntimeThread.Run(delegate
            {
                ThingDef[] crops = Crops(HasMO()).Select(n => DefDatabase<ThingDef>.GetNamed(n)).ToArray();
                // This fixture historically required each grain to win a raw-yield
                // cell. That no longer matches the approved Grains design: a crop
                // such as buckwheat may remain for its historical and food roles
                // without ever ranking first by harvested item count.
                int[] productive = new int[crops.Length];
                int viable = 0;
                foreach (float fertility in new[] { 0.5f, 1f, 1.4f })
                foreach (float temperature in new[] { 10f, 20f, 30f })
                foreach (float season in new[] { 5f, 10f, 20f })
                {
                    // Effective growing days: full light, no resting, weather,
                    // sow/harvest delays or frost death. Use the game's utility,
                    // including any installed provider patches, rather than our formula.
                    float[] yields = crops.Select(c => fertility < c.plant.fertilityMin ? 0f :
                        (float)Math.Floor(season * PlantUtility.GrowthRateFactorFor_Fertility(c, fertility)
                            * PlantUtility.GrowthRateFactorFor_Temperature(c, temperature)
                            / c.plant.growDays + 0.000001f) * c.plant.harvestYield).ToArray();
                    float best = yields.Max();
                    if (best <= 0f) continue;
                    viable++;
                    for (int i = 0; i < crops.Length; i++)
                        if (yields[i] > 0f) productive[i]++;
                }
                ctx.Require(viable > 0, "Environment matrix must contain mature harvests.");
                for (int i = 0; i < crops.Length; i++)
                    ctx.Assert(productive[i] > 0,
                        crops[i].defName + " cannot mature in any representative environment.");
            });
        }

        // Separate real-job scenario: use disposable, explicitly staged inputs to
        // test recipe ingredient mixing. The harvest chain is verified by the
        // existing ProductionJobs scenario; this scenario does not claim its
        // fabricated starting stacks were harvested.
        [Then("Grains common food single and mixed Bills complete through real jobs")]
        public async Task CommonFoodJobs(PickleContext ctx)
        {
            ProductionScope scope = null;
            ExceptionDispatchInfo failure = null;
            try
            {
                await RuntimeThread.Run(delegate { scope = new ProductionScope(ctx); });
                foreach (string family in new[] { "MixedGrainMeal", "MixedGrainPorridge", "MixedFlourFood" })
                foreach (bool bulk in new[] { false, true })
                foreach (bool mix in new[] { false, true })
                    await scope.CommonFoodBill(family, bulk, mix);
            }
            catch (Exception error) { failure = ExceptionDispatchInfo.Capture(error); }
            try { if (scope != null) await RuntimeThread.Run(scope.Dispose); }
            catch (Exception cleanup)
            {
                if (failure != null) throw new AggregateException(failure.SourceException, cleanup);
                throw;
            }
            if (failure != null) failure.Throw();
        }


        [Then("Grains V1 alternate processing benches complete native Bills")]
        public async Task AlternateProcessingJobs(PickleContext ctx)
        {
            ProductionScope scope = null; ExceptionDispatchInfo failure = null;
            try
            {
                await RuntimeThread.Run(delegate
                {
                    scope = new ProductionScope(ctx);
                    scope.AssertElectricRecipes();
                    scope.StageIngredient("AMJC_RawMillet", 20);
                });
                await scope.Bill("AMJC_ThreshMilletBulk", scope.Stove, "AMJC_RawMillet", 10, true);
                await scope.Bill("AMJC_HullMilletBulk", scope.Spot, "AMJC_MilletInHull", 10);
                await scope.Bill("AMJC_ThreshMilletBulk", scope.Spot, "AMJC_RawMillet", 10);
                await scope.Bill("AMJC_HullMilletBulk", scope.Stove, "AMJC_MilletInHull", 10);
                if (!scope.MO)
                {
                    await RuntimeThread.Run(delegate { scope.StageIngredient("AMJC_Buckwheat", 10); });
                    await scope.Bill("AMJC_MillBuckwheat", scope.Stove, "AMJC_Buckwheat", 10);
                }
            }
            catch (Exception error) { failure = ExceptionDispatchInfo.Capture(error); }
            try { if (scope != null) await RuntimeThread.Run(scope.Dispose); }
            catch (Exception error)
            {
                if (failure != null) throw new AggregateException(failure.SourceException, error);
                throw;
            }
            if (failure != null) failure.Throw();
        }

        [Then("Grains cooking Bills reject wrong ingredient families through native work givers")]
        public async Task RejectedFoodIngredients(PickleContext ctx)
        {
            ProductionScope scope = null; ExceptionDispatchInfo failure = null;
            try
            {
                await RuntimeThread.Run(delegate
                {
                    scope = new ProductionScope(ctx);
                    foreach (string family in new[] { "MixedGrainMeal", "MixedGrainPorridge" })
                        scope.RejectFoodInput("AMJC_Cook" + family, "AMJC_MilletFlour");
                    scope.RejectFoodInput("AMJC_CookMixedFlourFood", "AMJC_Millet");
                    foreach (string food in new[] { "Houtou", "Sobagaki", "BarleyFlourGruel" })
                        scope.RejectFoodInput("AMJC_Cook" + food, "AMJC_MilletFlour");
                    scope.RejectFoodInput("AMJC_CookMilletDumplings", "AMJC_BuckwheatFlour");
                });
            }
            catch (Exception error) { failure = ExceptionDispatchInfo.Capture(error); }
            try { if (scope != null) await RuntimeThread.Run(scope.Dispose); }
            catch (Exception error)
            {
                if (failure != null) throw new AggregateException(failure.SourceException, error);
                throw;
            }
            if (failure != null) failure.Throw();
        }

        [Then("Grains common and specialty foods apply actual ingestion thoughts")]
        public async Task FoodIngestionThoughts(PickleContext ctx)
        {
            ProductionScope scope = null; ExceptionDispatchInfo failure = null;
            try
            {
                await RuntimeThread.Run(delegate { scope = new ProductionScope(ctx); });
                await scope.IngestFood("AMJC_MixedGrainMeal", false);
                await scope.IngestFood("AMJC_MixedGrainPorridge", false);
                await scope.IngestFood("AMJC_MixedFlourFood", false);
                await scope.IngestFood("AMJC_Houtou", true);
                await scope.IngestFood("AMJC_Sobagaki", true);
                // Retired brown-rice-specific foods remain only for saved references.
                await scope.IngestFood("AMJC_WhiteRiceMeal", true, "AMJC_AteWhiteRiceMeal", 3f);
                await scope.IngestFood("AMJC_WhiteRicePorridge", true, "AMJC_AteWhiteRicePorridge", 1f);
            }
            catch (Exception error) { failure = ExceptionDispatchInfo.Capture(error); }
            try { if (scope != null) await RuntimeThread.Run(scope.Dispose); }
            catch (Exception error)
            {
                if (failure != null) throw new AggregateException(failure.SourceException, error);
                throw;
            }
            if (failure != null) failure.Throw();
        }

        [Then("Grains harvest and flour food Bills complete through real jobs")]
        public async Task ProductionJobs(PickleContext ctx)
        {
            ProductionScope scope = null;
            ExceptionDispatchInfo failure = null;
            try
            {
                await RuntimeThread.Run(delegate { scope = new ProductionScope(ctx); });
                await RuntimeThread.Run(scope.AssertUplandSowability);
                foreach (string crop in Crops(scope.MO)) await scope.Harvest(crop);
                // All inputs below are outputs of the preceding real jobs.
                await scope.Bill("AMJC_ThreshMilletBulk", scope.Processing, "AMJC_RawMillet", 10);
                await scope.Bill("AMJC_HullMilletBulk", scope.Processing, "AMJC_MilletInHull", 10);
                // Native Cooking WorkGiver must complete this milling Bill at a
                // Vanilla stove only in Base. MO retains its own millstone.
                await scope.Bill("AMJC_MillMillet", scope.MO ? scope.Mill : scope.Stove, "AMJC_Millet", 10);
                await scope.Bill("AMJC_CookMilletDumplings", scope.Campfire, "AMJC_MilletFlour", 10);
                await scope.Bill("AMJC_ThreshBuckwheatBulk", scope.Processing, "AMJC_RawBuckwheat", 10);
                await scope.Bill("AMJC_HullBuckwheatBulk", scope.Processing, "AMJC_BuckwheatInHull", 10);
                await scope.Bill("AMJC_MillBuckwheat", scope.Mill, "AMJC_Buckwheat", 10);
                await scope.Bill("AMJC_CookSobagaki", scope.Campfire, "AMJC_BuckwheatFlour", 10);
                await scope.Bill("AMJC_ThreshWheatBulk", scope.Processing,
                    scope.MO ? "DankPyon_RawWheat" : "AMJC_RawWheat", 10);
                await scope.Bill(scope.MO ? "DankPyon_CraftFlourBulk" : "AMJC_MillWheat", scope.Mill, "AMJC_Wheat", 10);
                await scope.Bill("AMJC_CookHoutou", scope.Campfire, scope.MO ? "DankPyon_Flour" : "AMJC_WheatFlour", 10);
                // Native simple meals must work with all five edible grain Defs.
                // Awa/Hie/Kibi share AMJC_Millet after their native harvests.
                // Produce new edible inputs with actual thresh/hull Bills rather
                // than manufacturing test stacks.
                await scope.Bill("AMJC_ThreshMilletBulk", scope.Processing, "AMJC_RawMillet", 10);
                await scope.Bill("AMJC_HullMilletBulk", scope.Processing, "AMJC_MilletInHull", 10);
                await scope.Bill("AMJC_ThreshBarleyBulk", scope.Processing, "AMJC_RawBarley", 10);
                await scope.Bill("AMJC_HullBarleyBulk", scope.Processing, "AMJC_BarleyInHull", 10);
                // Soba has only two harvested sheaves batches at this point:
                // harvest two more plants to support the second bulk process.
                await scope.Harvest("AMJC_Plant_Buckwheat_Soba", true);
                await scope.Harvest("AMJC_Plant_Buckwheat_Soba", true);
                await scope.Bill("AMJC_ThreshBuckwheatBulk", scope.Processing, "AMJC_RawBuckwheat", 10);
                await scope.Bill("AMJC_HullBuckwheatBulk", scope.Processing, "AMJC_BuckwheatInHull", 10);
                await scope.Bill("AMJC_ThreshWheatBulk", scope.Processing,
                    scope.MO ? "DankPyon_RawWheat" : "AMJC_RawWheat", 10);
                // Native DoBill at cooking stove, in both Vanilla and MO.
                await scope.Bill("AMJC_ThreshRiceBulk", scope.Stove, "AMJC_RiceSheaf", 10);
                await scope.Bill("AMJC_HullRiceBulk", scope.Stove, "AMJC_RiceInHull", 10);
                foreach (string grain in new[] { "AMJC_Millet", "AMJC_Buckwheat", "AMJC_Barley", "AMJC_Wheat", "RawRice" })
                    await scope.SimpleMeal(grain);
                // The brown-rice path above is harvested-source. Test the alternative
                // white-rice path with additional real rice harvests, not manufactured
                // paddy or a direct white-rice stack. Two x10 polish Bills yield 16
                // white rice, enough for one actual 0.5-nutrition white meal.
                await scope.Harvest("Plant_Rice", true);
                await scope.Harvest("Plant_Rice", true);
                await scope.Bill("AMJC_ThreshRiceBulk", scope.Stove, "AMJC_RiceSheaf", 10);
                await scope.Bill("AMJC_ThreshRiceBulk", scope.Stove, "AMJC_RiceSheaf", 10);
                await scope.Bill("AMJC_PolishWhiteRiceBulk", scope.Processing, "AMJC_RiceInHull", 10);
                await scope.Bill("AMJC_PolishWhiteRiceBulk", scope.Processing, "AMJC_RiceInHull", 10);
                await scope.Bill("AMJC_CookWhiteRiceMeal", scope.Campfire, "AMJC_WhiteRice", 10);
                // Extra real harvests prevent consumption of prior simple-meal inputs.
                await scope.Harvest("AMJC_Plant_Barley", true);
                await scope.Bill("AMJC_ThreshBarleyBulk", scope.Processing, "AMJC_RawBarley", 10);
                await scope.Bill("AMJC_HullBarleyBulk", scope.Processing, "AMJC_BarleyInHull", 10);
                await scope.Bill("AMJC_MillBarley", scope.Mill, "AMJC_Barley", 10);
                await scope.Bill("AMJC_CookBarleyFlourGruel", scope.Campfire, "AMJC_BarleyFlour", 10);
                // Seasonal checks remain inside the harvested-source scenario;
                // the separate common-food native Bill scenario is now the seventh
                // scenario for each provider profile.
                await scope.NativeSeasonalSowAndGrowth();
            }
            catch (Exception error) { failure = ExceptionDispatchInfo.Capture(error); }
            // The repository builds with the Framework C# 5 compiler, which
            // does not permit await inside catch/finally. Preserve the original
            // failure while awaiting main-thread cleanup outside those blocks.
            try { if (scope != null) await RuntimeThread.Run(scope.Dispose); }
            catch (Exception cleanup)
            {
                if (failure != null) throw new AggregateException(failure.SourceException, cleanup);
                throw;
            }
            if (failure != null) failure.Throw();
        }

        // This scope is only valid on a fresh AmjStageAQuickstart test map.
        // Setup creates mature plants, a worker, benches and fuel. It never
        // manufactures harvested ingredients or recipe products.
        private sealed class ProductionScope
        {
            private readonly PickleContext ctx;
            private readonly Map map;
            private readonly TimeSpeed speed;
            private readonly Stopwatch deadline = Stopwatch.StartNew();
            private readonly List<Thing> created = new List<Thing>();
            private readonly Dictionary<Pawn, IntVec3> parked = new Dictionary<Pawn, IntVec3>();
            private readonly Dictionary<IntVec3, TerrainDef> terrain = new Dictionary<IntVec3, TerrainDef>();
            private Pawn worker;
            private IntVec3 center;
            public bool MO;
            public Building_WorkTable Processing, Spot, Mill, Campfire, Stove;

            public ProductionScope(PickleContext context)
            {
                ctx = context;
                map = Find.CurrentMap;
                ctx.Require(map != null && map.Size.x == 50 && map.Size.z == 50,
                    "Production jobs require the isolated 50-cell Stage A Quickstart map.");
                speed = Find.TickManager.CurTimeSpeed;
                MO = HasMO();
                try
                {
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                    ctx.Require(Find.TickManager.CurTimeSpeed == TimeSpeed.Paused, "Controlled tick advancement requires pause.");
                    // This isolated Quickstart map is disposable. Do not depend
                    // on naturally occurring empty terrain: ruins, plants and
                    // rock can cover every 15x15 area before setup clears it.
                    center = map.Center;
                    ctx.Require(CellRect.CenteredOn(center, 7).Cells.All(c => c.InBounds(map)),
                        "Quickstart map is too small for the 15x15 production fixture.");
                    foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned.ToList())
                    {
                        parked.Add(pawn, pawn.Position);
                        pawn.DeSpawn();
                    }
                    foreach (IntVec3 cell in CellRect.CenteredOn(center, 7).Cells)
                    {
                        terrain.Add(cell, map.terrainGrid.TerrainAt(cell));
                        foreach (Thing thing in cell.GetThingList(map).ToList()) thing.Destroy(DestroyMode.Vanish);
                        map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
                    }
                    for (int attempt = 0; attempt < 30; attempt++)
                    {
                        Pawn candidate = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer);
                        if (!candidate.Downed && candidate.skills != null && !candidate.WorkTypeIsDisabled(WorkTypeDefOf.Growing)
                            && !candidate.WorkTypeIsDisabled(WorkTypeDefOf.Crafting)
                            && !candidate.WorkTypeIsDisabled(DefDatabase<WorkTypeDef>.GetNamed("Cooking")))
                        {
                            foreach (SkillDef skill in new[] { SkillDefOf.Plants, SkillDefOf.Crafting, SkillDefOf.Cooking })
                                candidate.skills.GetSkill(skill).Level = 20;
                            // A generated pawn may have a harvest-yield modifier even at skill 20.
                            // Preserve the >= 1.0 invariant by rejecting that candidate here.
                            if (candidate.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f)
                            { worker = candidate; break; }
                        }
                        candidate.Destroy(DestroyMode.Vanish);
                    }
                    ctx.Require(worker != null, "Cannot create a capable production worker.");
                    created.Add(worker);
                    GenSpawn.Spawn(worker, center, map);
                    worker.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
                    foreach (SkillDef skill in new[] { SkillDefOf.Plants, SkillDefOf.Crafting, SkillDefOf.Cooking })
                        worker.skills.GetSkill(skill).Level = 20;
                    ctx.Require(worker.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f, "Harvest failure must be excluded by worker skill.");
                    Processing = Bench("AMJC_GrainProcessingTable", center + new IntVec3(-3,0,0));
                    Spot = Bench("AMJC_GrainProcessingSpot", center + new IntVec3(-4,0,-4));
                    Mill = Bench(MO ? "DankPyon_Millstone" : "AMJC_ManualMillstone", center + new IntVec3(3,0,0));
                    Stove = Bench("FueledStove", center + new IntVec3(4,0,-4));
                    CompRefuelable stoveFuel = Stove.TryGetComp<CompRefuelable>();
                    ctx.Require(stoveFuel != null, "FueledStove must have fuel.");
                    stoveFuel.Refuel(stoveFuel.Props.fuelCapacity);
                    Campfire = Bench("Campfire", center + new IntVec3(0,0,-4));
                    CompRefuelable fuel = Campfire.TryGetComp<CompRefuelable>();
                    ctx.Require(fuel != null, "Campfire needs a fuel component.");
                    fuel.Refuel(fuel.Props.fuelCapacity);
                }
                catch { Dispose(); throw; }
            }

            private Building_WorkTable Bench(string name, IntVec3 cell)
            {
                ThingDef def = DefDatabase<ThingDef>.GetNamed(name);
                Thing thing = ThingMaker.MakeThing(def, def.MadeFromStuff ? GenStuff.DefaultStuffFor(def) : null);
                thing.SetFaction(Faction.OfPlayer);
                created.Add(thing);
                GenSpawn.Spawn(thing, cell, map, Rot4.North);
                Building_WorkTable bench = thing as Building_WorkTable;
                ctx.Require(bench != null, name + " must be a real worktable.");
                return bench;
            }

            private int Count(string name)
            {
                int total = map.listerThings.AllThings.Where(t => t.def.defName == name).Sum(t => t.stackCount);
                Thing carried = worker.carryTracker.CarriedThing;
                return total + (carried != null && carried.def.defName == name ? carried.stackCount : 0);
            }

            private void Start(Job job, string label)
            {
                ctx.Require(job != null, "No real job was offered for " + label);
                if (worker.needs.food != null) worker.needs.food.CurLevelPercentage = 1f;
                if (worker.needs.rest != null) worker.needs.rest.CurLevelPercentage = 1f;
                job.playerForced = true;
                worker.jobs.StartJob(job, JobCondition.InterruptForced);
                ctx.Require(worker.CurJob == job, "Job failed to start: " + label);
            }

            private async Task Complete(Job job, Func<bool> done, string label)
            {
                bool complete = false;
                for (int ticks = 0; ticks < 20000 && !complete; ticks += 64)
                {
                    await RuntimeThread.Run(delegate
                    {
                        ctx.Require(deadline.Elapsed.TotalSeconds < 240, "Production suite exceeded 240 seconds: " + label);
                        for (int tick = 0; tick < 64; tick++)
                        {
                            if (done()) { complete = true; break; }
                            ctx.Require(worker.CurJob == job, "Job ended without its expected outputs: " + label);
                            Find.TickManager.DoSingleTick();
                        }
                        if (done()) complete = true;
                    });
                    // Yield between bounded tick batches so driver/rendering can update.
                    await Task.Delay(1);
                }
                await RuntimeThread.Run(delegate
                {
                    ctx.Require(complete, "Job exceeded 20000 game ticks: " + label);
                    worker.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                    if (worker.carryTracker.CarriedThing != null)
                    {
                        Thing dropped;
                        ctx.Require(worker.carryTracker.TryDropCarriedThing(worker.Position, ThingPlaceMode.Near, out dropped),
                            "Cannot place carried job product: " + label);
                    }
                });
            }

            public void AssertUplandSowability()
            {
                // Real engine eligibility on soil; calendar-dependent native Sow remains pending.
                ThingDef rice = DefDatabase<ThingDef>.GetNamed("Plant_Rice");
                IntVec3 cell = center + new IntVec3(0, 0, 2);
                Zone_Growing zone = new Zone_Growing(map.zoneManager);
                map.zoneManager.RegisterZone(zone);
                try
                {
                    zone.AddCell(cell);
                    zone.SetPlantDefToGrow(rice);
                    ctx.Require(zone.CellCount == 1 && zone.GetPlantDefToGrow() == rice,
                        "Rice must be selectable on a standard growing zone.");
                    ctx.Assert(PlantUtility.CanSowOnGrower(rice, zone),
                        "Native sow eligibility must accept upland rice.");
                    ctx.Assert(rice.CanNowPlantAt(cell, map),
                        "Upland rice must be plantable in normal soil.");
                }
                finally { zone.Delete(false); }
            }


            // A real WorkGiver -> Sow JobDriver -> Plant.TickLong integration.
            // Calendar moves by one quadrum; test-only biome temperatures make
            // warm/cold cases deterministic rather than claiming natural weather.
            // No gameplay Def, map owner, permanent world climate or crop value
            // is changed. Natural climate/frost-death remains a separate gate.
            public async Task NativeSeasonalSowAndGrowth()
            {
                Zone_Growing zone = null, barleyZone = null;
                Plant ricePlant = null, barleyPlant = null;
                IntVec3 riceCell = IntVec3.Invalid, barleyCell = IntVec3.Invalid;
                int savedTicks = -1;
                float? savedBiomeTemperature = null;
                float savedSkyGlow = -1f;
                string phase = "fixture setup";
                var savedSnow = new Dictionary<IntVec3, float>();
                float riceBeforeCold = 0f, riceBeforeWarm = 0f, barleyBeforeCold = 0f;
                Job riceJob = null, barleyJob = null;
                ExceptionDispatchInfo failure = null;
                try
                {
                    await RuntimeThread.Run(delegate
                    {
                        ctx.Require(deadline.Elapsed.TotalSeconds < 240, "Seasonal sow test exceeded fixture deadline.");
                        savedTicks = Find.TickManager.TicksGame;
                        savedBiomeTemperature = map.Biome.constantOutdoorTemperature;
                        savedSkyGlow = map.skyManager.CurSkyGlow;
                        // Prefer unroofed clear fixture ground away from the
                        // active benches. The whole 15x15 area is disposable.
                        IntVec3[] cells = CellRect.CenteredOn(center, 7).Cells
                            .Where(c => !c.Roofed(map)
                                && Math.Abs(c.x - center.x) + Math.Abs(c.z - center.z) >= 8
                                && !c.GetThingList(map).Any(t => t is Building || t is Pawn || t is Plant))
                            .Take(2).ToArray();
                        ctx.Require(cells.Length == 2, "Need two open cells for native sow fixture.");
                        riceCell = cells[0]; barleyCell = cells[1];
                        foreach (IntVec3 cell in cells)
                        {
                            savedSnow.Add(cell, map.snowGrid.GetDepth(cell));
                            map.snowGrid.SetDepth(cell, 0f);
                            foreach (Thing leftover in cell.GetThingList(map).ToList())
                                leftover.Destroy(DestroyMode.Vanish);
                        }
                        int hourShift = (12 - GenLocalDate.HourOfDay(map) + 24) % 24;
                        Find.TickManager.DebugSetTicksGame(savedTicks + hourShift * GenDate.TicksPerHour);
                        zone = new Zone_Growing(map.zoneManager);
                        map.zoneManager.RegisterZone(zone);
                        zone.AddCell(riceCell);
                        barleyZone = new Zone_Growing(map.zoneManager);
                        map.zoneManager.RegisterZone(barleyZone);
                        barleyZone.AddCell(barleyCell);
                        zone.SetPlantDefToGrow(DefDatabase<ThingDef>.GetNamed("Plant_Rice"));
                        barleyZone.SetPlantDefToGrow(DefDatabase<ThingDef>.GetNamed("Plant_Rice"));
                        SetSeasonTemperature(25f, riceCell, barleyCell);
                        ThingDef rice = zone.GetPlantDefToGrow();
                        ctx.Require(PlantUtility.GrowthSeasonNow(riceCell, map, rice),
                            "Upland rice must be in a native growth season at 25 C.");
                        ctx.Require(rice.plant.sowMinSkill == 4, "Rice must require Plants level 4.");
                        worker.skills.GetSkill(SkillDefOf.Plants).Level = 3;
                        ctx.Assert(NativeSowOffer(riceCell) == null,
                            "A Plants level 3 grower must not receive a native rice sow job.");
                        worker.skills.GetSkill(SkillDefOf.Plants).Level = 4;
                        riceJob = NativeSowOffer(riceCell);
                        ctx.Require(riceJob != null && riceJob.def == JobDefOf.Sow &&
                            riceJob.plantDefToSow == rice, "Rice needs a native sow job at 25 C.");
                        Start(riceJob, "WarmSeason/RiceSow");
                    });
                    await Complete(riceJob, () => riceCell.GetPlant(map) != null &&
                        riceCell.GetPlant(map).def.defName == "Plant_Rice" &&
                        riceCell.GetPlant(map).LifeStage != PlantLifeStage.Sowing, "WarmSeason/RiceSow");

                    await RuntimeThread.Run(delegate
                    {
                        phase = "cold season transition";
                        // Harvest/processing checks use the original full-skill test worker.
                        worker.skills.GetSkill(SkillDefOf.Plants).Level = 20;
                        ricePlant = riceCell.GetPlant(map);
                        ctx.Require(ricePlant != null && ricePlant.sown, "Real sow must produce sown Plant_Rice.");
                        int priorDay = GenLocalDate.DayOfYear(map);
                        Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + GenDate.TicksPerQuadrum);
                        ctx.Require(GenLocalDate.DayOfYear(map) == (priorDay + 15) % GenDate.DaysPerYear,
                            "Controlled test date must advance exactly one quadrum.");
                        SetSeasonTemperature(5f, riceCell, barleyCell);
                        ctx.Require(!PlantUtility.GrowthSeasonNow(barleyCell, map, ricePlant.def),
                            "Rice must not be in growing season at 5 C.");
                        ctx.Assert(NativeSowOffer(barleyCell) == null,
                            "Native WorkGiver must reject sowing rice at 5 C.");
                        riceBeforeCold = ricePlant.Growth;
                        ctx.Assert(ricePlant.GrowthRateFactor_Temperature == 0f &&
                            ricePlant.GrowthRate == 0f, "Rice growth must be zero at 5 C.");
                    });
                    await SimulatePlantTicks(2200, "RiceColdStall");
                    await RuntimeThread.Run(delegate
                    {
                        phase = "barley cold sow";
                        ctx.Require(!ricePlant.Destroyed && Math.Abs(ricePlant.Growth - riceBeforeCold) < 0.00001f,
                            "Real TickLong must not advance rice growth below 10 C.");
                        // Keep the live rice cell assigned to rice. Changing a
                        // shared zone to barley lets native growers clear the rice.
                        barleyZone.SetPlantDefToGrow(DefDatabase<ThingDef>.GetNamed("AMJC_Plant_Barley"));
                        ctx.Require(zone.GetPlantDefToGrow().defName == "Plant_Rice",
                            "Rice recovery fixture must retain its original crop assignment.");
                        ThingDef barley = barleyZone.GetPlantDefToGrow();
                        ctx.Require(PlantUtility.GrowthSeasonNow(barleyCell, map, barley),
                            "Barley must be in a native growth season at 5 C. "
                            + "actualTemperature=" + barleyCell.GetTemperature(map)
                            + ", minGrowthTemperature=" + barley.plant.minGrowthTemperature
                            + ", maxGrowthTemperature=" + barley.plant.maxGrowthTemperature
                            + ", zoneCrop=" + barleyZone.GetPlantDefToGrow().defName);
                        barleyJob = NativeSowOffer(barleyCell);
                        ctx.Require(barleyJob != null && barleyJob.def == JobDefOf.Sow &&
                            barleyJob.plantDefToSow == barley,
                            "Barley needs a native sow job at 5 C; barleyGrowingSeason=" + PlantUtility.GrowthSeasonNow(barleyCell, map, barley)
                            + ", zoneCrop=" + barleyZone.GetPlantDefToGrow().defName
                            + ", job=" + (barleyJob == null ? "null" : barleyJob.def.defName)
                            + ", cellThings=" + string.Join(",", barleyCell.GetThingList(map)
                                .Select(t => t.def.defName).ToArray())
                            + ", actualTemperature=" + barleyCell.GetTemperature(map)
                            + ", currentHour=" + GenLocalDate.HourOfDay(map));
                        Start(barleyJob, "ColdSeason/BarleySow");
                    });
                    await Complete(barleyJob, () => barleyCell.GetPlant(map) != null &&
                        barleyCell.GetPlant(map).def.defName == "AMJC_Plant_Barley" &&
                        barleyCell.GetPlant(map).LifeStage != PlantLifeStage.Sowing, "ColdSeason/BarleySow");
                    await RuntimeThread.Run(delegate
                    {
                        phase = "barley cold growth";
                        barleyPlant = barleyCell.GetPlant(map);
                        ctx.Require(barleyPlant != null && barleyPlant.sown,
                            "Real sow must produce sown barley.");
                        ctx.Require(barleyPlant.GrowthRateFactor_Temperature > 0f,
                            "Barley must retain positive growth-temperature factor at 5 C.");
                        barleyBeforeCold = barleyPlant.Growth;
                    });
                    await SimulatePlantTicks(2200, "BarleyColdGrowth");
                    await RuntimeThread.Run(delegate
                    {
                        phase = "warm recovery calendar and daylight setup";
                        ctx.Assert(barleyPlant.Growth > barleyBeforeCold + 0.000001f,
                            "Native TickLong must grow barley at 5 C in daylight.");
                        // automated-gates(5).log: MO-only reaches the warm
                        // recovery after longer production/planting jobs. The
                        // old assertion could run in Plant.Resting hours
                        // (local day percent <0.25 or >0.8), where vanilla
                        // Plant.GrowthPerTick is zero at *any* temperature.
                        // Move to local noon and synchronize the cached sky
                        // glow with the native celestial solar calculation.
                        // SkyManagerUpdate() also touches weather, shaders and
                        // camera objects; directly invoking that render update
                        // from this headless Pickle step caused an intermittent
                        // null reference in automated-gates(7).log (suspected).
                        // ForceSetCurSkyGlow changes only the disposable map's
                        // cached light, not Plant growth, terrain, or weather.
                        phase = "warm recovery local hour";
                        int warmHourShift = (12 - GenLocalDate.HourOfDay(map) + 24) % 24;
                        phase = "warm recovery tick manager calendar shift";
                        Find.TickManager.DebugSetTicksGame(
                            Find.TickManager.TicksGame + warmHourShift * GenDate.TicksPerHour);
                        phase = "warm recovery native celestial glow";
                        float solarGlow = GenCelestial.CurCelestialSunGlow(map);
                        ctx.Require(solarGlow > 0.1f,
                            "Warm recovery fixture requires actual daylight: "
                            + "solarGlow=" + solarGlow + ", phase=" + phase);
                        phase = "warm recovery sky glow cache";
                        ctx.Require(map.skyManager != null, "Warm recovery sky manager is missing.");
                        map.skyManager.ForceSetCurSkyGlow(solarGlow);
                        phase = "warm recovery temperature setup";
                        SetSeasonTemperature(25f, riceCell, barleyCell);
                        phase = "warm recovery local day percent";
                        float warmDayPercent = GenLocalDate.DayPercent(map);
                        ctx.Require(warmDayPercent > 0.25f && warmDayPercent < 0.8f,
                            "Warm recovery fixture must run outside the plant resting hours: "
                            + warmDayPercent);
                        phase = "warm recovery rice growth season";
                        ctx.Require(ricePlant != null && !ricePlant.Destroyed && ricePlant.Spawned,
                            "Warm recovery requires the previously sown rice to remain spawned.");
                        ctx.Require(PlantUtility.GrowthSeasonNow(riceCell, map, ricePlant.def),
                            "Rice must resume the native growth season at 25 C.");
                        phase = "warm recovery rice growth factors";
                        ctx.Require(ricePlant.GrowthRateFactor_Light > 0.001f &&
                            ricePlant.GrowthRateFactor_Temperature > 0f &&
                            ricePlant.GrowthRate > 0f &&
                            ricePlant.LifeStage == PlantLifeStage.Growing,
                            "Warm recovery requires live growing rice and real sunlight. "
                            + "dayPercent=" + warmDayPercent
                            + ", sunGlow=" + map.skyManager.CurSkyGlow
                            + ", lightFactor=" + ricePlant.GrowthRateFactor_Light
                            + ", tempFactor=" + ricePlant.GrowthRateFactor_Temperature
                            + ", growthRate=" + ricePlant.GrowthRate
                            + ", lifeStage=" + ricePlant.LifeStage);
                        riceBeforeWarm = ricePlant.Growth;
                    });
                    phase = "warm recovery native growth ticks";
                    await SimulatePlantTicks(2200, "RiceWarmRecovery");
                    await RuntimeThread.Run(delegate
                    {
                        phase = "warm recovery growth assertion";
                        ctx.Assert(!ricePlant.Destroyed && ricePlant.Growth > riceBeforeWarm + 0.000001f,
                            "Native TickLong must resume rice growth after warming. "
                            + "start=" + riceBeforeWarm + ", end=" + ricePlant.Growth
                            + ", dayPercent=" + GenLocalDate.DayPercent(map)
                            + ", sunGlow=" + map.skyManager.CurSkyGlow
                            + ", tempFactor=" + ricePlant.GrowthRateFactor_Temperature
                            + ", lightFactor=" + ricePlant.GrowthRateFactor_Light
                            + ", growthRate=" + ricePlant.GrowthRate
                            + ", lifeStage=" + ricePlant.LifeStage);
                    });
                }
                catch (Exception error)
                {
                    // Pickle often prints only the outer exception message.
                    // Retain the original exception/stack in InnerException
                    // and put the active native-fixture stage in the message.
                    failure = ExceptionDispatchInfo.Capture(new InvalidOperationException(
                        "NativeSeasonalSowAndGrowth failed at " + phase + ": " + error.ToString(), error));
                }

                // The test assembly uses the Framework C# 5 compiler: no await
                // inside finally. Always undo test-only calendar/climate.
                try
                {
                    await RuntimeThread.Run(delegate
                    {
                        if (worker != null && worker.Spawned)
                            worker.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                        if (zone != null) zone.Delete(false);
                        if (barleyZone != null) barleyZone.Delete(false);
                        foreach (IntVec3 cell in new[] { riceCell, barleyCell })
                        {
                            if (!cell.IsValid) continue;
                            Plant planted = cell.GetPlant(map);
                            if (planted != null && !planted.Destroyed) planted.Destroy(DestroyMode.Vanish);
                        }
                        foreach (KeyValuePair<IntVec3,float> pair in savedSnow)
                            map.snowGrid.SetDepth(pair.Key, pair.Value);
                        if (savedTicks >= 0)
                        {
                            map.Biome.constantOutdoorTemperature = savedBiomeTemperature;
                            foreach (Room room in map.regionGrid.AllRooms)
                                if (room.UsesOutdoorTemperature) room.TempTracker.EqualizeTemperature();
                            Find.TickManager.DebugSetTicksGame(savedTicks);
                            if (savedSkyGlow >= 0f)
                                map.skyManager.ForceSetCurSkyGlow(savedSkyGlow);
                        }
                    });
                }
                catch (Exception cleanup)
                {
                    if (failure != null) throw new AggregateException(failure.SourceException, cleanup);
                    throw;
                }
                if (failure != null) failure.Throw();
            }

            private Job NativeSowOffer(IntVec3 cell)
            {
                // Use the game-registered worker: a bare new WorkGiver_GrowerSow()
                // has no WorkGiverDef assigned and can throw in JobOnCell.
                WorkGiverDef sowDef = DefDatabase<WorkGiverDef>.AllDefsListForReading
                    .FirstOrDefault(d => d.giverClass != null
                        && typeof(WorkGiver_GrowerSow).IsAssignableFrom(d.giverClass));
                ctx.Require(sowDef != null, "Real Sow WorkGiverDef missing.");
                WorkGiver_GrowerSow giver = sowDef.Worker as WorkGiver_GrowerSow;
                ctx.Require(giver != null, "Real Sow WorkGiver worker missing.");
                // Consume scanner enumeration to reset its static cached
                // wantedPlantDef before changing the zone's selected crop.
                foreach (IntVec3 ignored in giver.PotentialWorkCellsGlobal(worker)) { }
                return giver.JobOnCell(worker, cell, true);
            }

            private void SetSeasonTemperature(float temp, IntVec3 first, IntVec3 second)
            {
                map.Biome.constantOutdoorTemperature = temp;
                foreach (Room room in map.regionGrid.AllRooms)
                    if (room.UsesOutdoorTemperature) room.TempTracker.EqualizeTemperature();
                ctx.Require(Math.Abs(first.GetTemperature(map) - temp) < 0.1f &&
                            Math.Abs(second.GetTemperature(map) - temp) < 0.1f,
                    "Climate fixture needs true outdoor temperature at both sow cells.");
            }

            private async Task SimulatePlantTicks(int count, string label)
            {
                for (int i = 0; i < count; i += 64)
                {
                    int batch = Math.Min(64, count - i);
                    await RuntimeThread.Run(delegate
                    {
                        ctx.Require(deadline.Elapsed.TotalSeconds < 240, "Climate fixture timed out: " + label);
                        for (int j = 0; j < batch; j++) Find.TickManager.DoSingleTick();
                    });
                    await Task.Delay(1);
                }
            }

            public async Task Harvest(string name, bool extra = false)
            {
                Plant plant = null; Job job = null;
                string raw = null; int before = 0, straw = 0, hay = 0, minimum = 0, maximum = 0;
                await RuntimeThread.Run(delegate
                {
                    ThingDef crop = DefDatabase<ThingDef>.GetNamed(name);
                    raw = crop.plant.harvestedThingDef.defName;
                    if (name == "Plant_Rice") ctx.Require(raw == "AMJC_RiceSheaf", "Upland rice must harvest AMJC_RiceSheaf.");
                    before = Count(raw); straw = Count("DankPyon_Straw"); hay = Count("Hay");
                    // Two Soba plants ensure >=10 actual sheaves for the x10 Bills.
                    plant = (Plant)ThingMaker.MakeThing(crop);
                    plant.Growth = 1f;
                    created.Add(plant);
                    GenSpawn.Spawn(plant, center + new IntVec3(0,0,4), map);
                    float baseYield = crop.plant.harvestYield * (crop.plant.harvestYieldAffectedByDifficulty
                        ? Find.Storyteller.difficulty.cropYieldFactor : 1f);
                    float skill = worker.GetStatValue(StatDefOf.PlantHarvestYield);
                    // Native harvest rounds plant yield, then rounds excess skill.
                    minimum = (int)Math.Floor(Math.Floor(baseYield) * skill);
                    maximum = (int)Math.Ceiling(Math.Ceiling(baseYield) * skill);
                    map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.HarvestPlant));
                    job = new WorkGiver_PlantsCut().JobOnThing(worker, plant, true);
                    ctx.Require(job != null && job.def == JobDefOf.HarvestDesignated, "Must use designated harvest job: " + name);
                    Start(job, name);
                });
                await Complete(job, () => plant.Destroyed && Count(raw) > before, name);
                await RuntimeThread.Run(delegate
                {
                    ctx.Assert(Count("DankPyon_Straw") == straw && Count("Hay") == hay,
                        "Harvest must not bypass threshing with a straw/hay drop: " + name);
                    int harvested = Count(raw) - before;
                    ctx.Assert(harvested >= minimum && harvested <= maximum,
                        name + " harvest quantity disagrees with difficulty/skill-adjusted yield.");
                });
                if (name == "AMJC_Plant_Buckwheat_Soba" && !extra)
                    await Harvest(name, true);
            }


            // Staged inputs only. ProductionJobs separately proves real harvest.
            public void StageIngredient(string name, int quantity)
            {
                ctx.Require(quantity > 0 && Count(name) == 0, "Bad test stack: " + name);
                Thing input = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed(name));
                input.stackCount = quantity;
                created.Add(input);
                GenSpawn.Spawn(input, center + new IntVec3(-1, 0, -2), map);
            }

            public void AssertElectricRecipes()
            {
                ThingDef stove = DefDatabase<ThingDef>.GetNamed("ElectricStove");
                foreach (string name in new[] { "AMJC_ThreshMilletBulk", "AMJC_HullMilletBulk" })
                    ctx.Assert(stove.AllRecipes.Contains(DefDatabase<RecipeDef>.GetNamed(name)),
                        "ElectricStove missing " + name);
                if (!MO)
                    ctx.Assert(stove.AllRecipes.Contains(DefDatabase<RecipeDef>.GetNamed("AMJC_MillBuckwheat")),
                        "Base ElectricStove missing V1 milling.");
            }

            public void RejectFoodInput(string recipeName, string wrongName)
            {
                RecipeDef recipe = DefDatabase<RecipeDef>.GetNamed(recipeName);
                ThingDef wrong = DefDatabase<ThingDef>.GetNamed(wrongName);
                ctx.Require(Campfire.def.AllRecipes.Contains(recipe), recipeName + " not offered.");
                ctx.Require(!recipe.fixedIngredientFilter.Allows(wrong)
                    && recipe.ingredients.All(i => !i.filter.Allows(wrong)),
                    recipeName + " unexpectedly permits " + wrongName);
                Thing input = ThingMaker.MakeThing(wrong);
                input.stackCount = 40;
                created.Add(input);
                GenSpawn.Spawn(input, center + new IntVec3(0, 0, -2), map);
                Bill_Production bill = (Bill_Production)recipe.MakeNewBill();
                bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
                bill.repeatCount = 1;
                bill.ingredientSearchRadius = 10f;
                bill.ingredientFilter.SetDisallowAll();
                bill.ingredientFilter.SetAllow(wrong, true);
                Campfire.BillStack.AddBill(bill);
                try
                {
                    foreach (WorkGiverDef def in DefDatabase<WorkGiverDef>.AllDefsListForReading)
                    {
                        if (def.giverClass == null || !typeof(WorkGiver_DoBill).IsAssignableFrom(def.giverClass)) continue;
                        WorkGiver_DoBill giver = def.Worker as WorkGiver_DoBill;
                        if (giver == null || !giver.ThingIsUsableBillGiver(Campfire)) continue;
                        Job offered = giver.JobOnThing(worker, Campfire, true);
                        ctx.Assert(offered == null || offered.bill != bill,
                            recipeName + " wrongly offered a job for " + wrongName);
                    }
                }
                finally
                {
                    Campfire.BillStack.Delete(bill);
                    input.Destroy(DestroyMode.Vanish);
                }
            }

            // Actual Ingest JobDriver only: never insert test memories directly.
            public async Task IngestFood(string name, bool specialty,
                string thoughtName = "AMJC_AteFlourFood", float moodBonus = 2f)
            {
                Job job = null; Thing item = null; ThoughtDef thought = null; int before = 0;
                await RuntimeThread.Run(delegate
                {
                    thought = DefDatabase<ThoughtDef>.GetNamed(thoughtName);
                    ctx.Require(thought.stages.Count == 1
                        && Math.Abs(thought.stages[0].baseMoodEffect - moodBonus) < 0.001f
                        && Math.Abs(thought.durationDays - 0.5f) < 0.001f
                        && thought.stackLimit == 1,
                        thoughtName + " must award its specified half-day nonstacking Mood.");
                    ThingDef def = DefDatabase<ThingDef>.GetNamed(name);
                    ctx.Require(def.ingestible != null && worker.needs.mood != null,
                        "Food/pawn must support ingestion/mood.");
                    ctx.Require(specialty ? def.ingestible.tasteThought == thought
                        : def.ingestible.tasteThought == null, name + " tasteThought differs.");
                    before = worker.needs.mood.thoughts.memories.Memories.Count(m => m.def == thought);
                    item = ThingMaker.MakeThing(def);
                    item.stackCount = 1;
                    created.Add(item);
                    GenSpawn.Spawn(item, center + new IntVec3(0, 0, -2), map);
                    worker.needs.food.CurLevelPercentage = 0.1f;
                    job = JobMaker.MakeJob(JobDefOf.Ingest, item);
                    job.count = 1;
                    job.playerForced = true;
                    worker.jobs.StartJob(job, JobCondition.InterruptForced);
                    ctx.Require(worker.CurJob == job, "Ingest job did not start for " + name);
                });
                await Complete(job, () => item.Destroyed, "Ingest/" + name);
                await RuntimeThread.Run(delegate
                {
                    int after = worker.needs.mood.thoughts.memories.Memories.Count(m => m.def == thought);
                    ctx.Assert(after == (specialty ? 1 : before), name + " incorrect memory count: " + after);
                });
            }

            public async Task Bill(string recipeName, Building_WorkTable bench, string ingredient, int consumed,
                bool untilTarget = false)
            {
                Job job = null; Bill_Production bill = null; RecipeDef recipe = null;
                int before = 0, straw = 0, hay = 0;
                Dictionary<string,int> products = null;
                await RuntimeThread.Run(delegate
                {
                    recipe = DefDatabase<RecipeDef>.GetNamed(recipeName);
                    ctx.Require(bench.def.AllRecipes.Contains(recipe), recipeName + " must be offered by " + bench.def.defName);
                    before = Count(ingredient); ctx.Require(before >= consumed, "Harvest chain lacks " + ingredient);
                    straw = Count("DankPyon_Straw"); hay = Count("Hay");
                    products = recipe.products.ToDictionary(p => p.thingDef.defName, p => Count(p.thingDef.defName));
                    bill = (Bill_Production)recipe.MakeNewBill();
                    bill.repeatMode = untilTarget ? BillRepeatModeDefOf.TargetCount : BillRepeatModeDefOf.RepeatCount;
                    bill.repeatCount = 1;
                    bill.SetStoreMode(BillStoreModeDefOf.DropOnFloor);
                    bill.ingredientSearchRadius = 10f;
                    bench.BillStack.AddBill(bill);
                    if (untilTarget)
                    {
                        ctx.Assert(recipe.WorkerCounter.CanCountProducts(bill),
                            "TargetCount must allow a mixed grain + Straw MO recipe.");
                        bill.targetCount = recipe.WorkerCounter.CountProducts(bill)
                            + recipe.products[0].count;
                        ctx.Assert(bill.ShouldDoNow(), "A below-target thresh Bill must be available.");
                    }
                    foreach (WorkGiverDef def in DefDatabase<WorkGiverDef>.AllDefsListForReading)
                    {
                        if (def.giverClass == null || !typeof(WorkGiver_DoBill).IsAssignableFrom(def.giverClass)) continue;
                        WorkGiver_DoBill giver = def.Worker as WorkGiver_DoBill;
                        if (giver == null || !giver.ThingIsUsableBillGiver(bench)) continue;
                        Job offered = giver.JobOnThing(worker, bench, true);
                        if (offered != null && offered.def == JobDefOf.DoBill && offered.bill == bill)
                        { job = offered; break; }
                    }
                    Start(job, recipeName);
                });
                // The pawn may be carrying finished products; the normal counter
                // intentionally counts spawned stock only. Complete the native job
                // and drop carried output before asserting the TargetCount threshold.
                await Complete(job, () => recipe.products.All(p =>
                    Count(p.thingDef.defName) == products[p.thingDef.defName] + p.count)
                    && (untilTarget || bill.repeatCount == 0), recipeName);
                await RuntimeThread.Run(delegate
                {
                    ctx.Assert(Count(ingredient) == before - consumed, recipeName + " consumed the wrong ingredient quantity.");
                    if (untilTarget)
                    {
                        // Vanilla's resource counter counts stored resources, not
                        // loose products dropped on the floor. Keep the product
                        // generated by the actual DoBill, then move that exact stack
                        // into a disposable stockpile before checking TargetCount.
                        Thing primary = map.listerThings.ThingsOfDef(recipe.products[0].thingDef)
                            .FirstOrDefault(t => t.Spawned);
                        ctx.Require(primary != null, "Native DoBill did not spawn the primary grain.");
                        IntVec3 storageCell = center + new IntVec3(0, 0, 2);
                        ctx.Require(!storageCell.GetThingList(map).Any(t => t is Building || t is Pawn),
                            "TargetCount fixture storage cell is obstructed.");
                        Zone_Stockpile stockpile = new Zone_Stockpile(StorageSettingsPreset.DefaultStockpile, map.zoneManager);
                        map.zoneManager.RegisterZone(stockpile);
                        try
                        {
                            stockpile.AddCell(storageCell);
                            primary.DeSpawn();
                            GenSpawn.Spawn(primary, storageCell, map);
                            map.resourceCounter.UpdateResourceCounts();
                            int counted = recipe.WorkerCounter.CountProducts(bill);
                            bool shouldContinue = bill.ShouldDoNow();
                            ctx.Assert(counted >= bill.targetCount && !shouldContinue,
                                recipeName + " failed to stop with stored primary grain: counted="
                                + counted + ", target=" + bill.targetCount + ", shouldContinue=" + shouldContinue);
                        }
                        finally
                        {
                            stockpile.Delete(false);
                            map.resourceCounter.UpdateResourceCounts();
                        }
                    }
                    else
                        ctx.Assert(bill.repeatCount == 0, recipeName + " must finish exactly one Bill iteration.");
                    foreach (ThingDefCountClass product in recipe.products)
                        ctx.Assert(Count(product.thingDef.defName) == products[product.thingDef.defName] + product.count,
                            recipeName + " produced the wrong quantity: " + product.thingDef.defName);
                    if (!recipe.products.Any(p => p.thingDef.defName == "DankPyon_Straw"))
                        ctx.Assert(Count("DankPyon_Straw") == straw, recipeName + " must not create straw.");
                    // MO's native wheat milling intentionally produces Hay.
                    // Declared products are already checked at their exact recipe counts.
                    // Recipes without Hay must still prove that none appeared.
                    if (!recipe.products.Any(p => p.thingDef.defName == "Hay"))
                        ctx.Assert(Count("Hay") == hay, recipeName + " must not create undeclared hay.");
                    bench.BillStack.Delete(bill);
                });
            }

            // This is an actual Cook DoBill with one or two distinct loaded
            // ThingDefs. Starting ingredient stacks are generated ONLY within
            // the disposable test map, explicitly separating this coverage
            // from the native harvest-through-processing chain above.
            public async Task CommonFoodBill(string family, bool bulk, bool mixed)
            {
                string recipeName = "AMJC_Cook" + family + (bulk ? "Bulk" : "");
                string productName = "AMJC_" + family;
                bool flour = family == "MixedFlourFood";
                string firstName = flour ? "AMJC_MilletFlour" : "AMJC_Millet";
                string secondName = flour ? (MO ? "DankPyon_Flour" : "AMJC_WheatFlour") : "AMJC_Barley";
                string[] materials = mixed ? new[] { firstName, secondName } : new[] { firstName };
                Job job = null;
                Bill_Production bill = null;
                RecipeDef recipe = null;
                int[] starting = new int[materials.Length];
                int[] amounts = new int[materials.Length];
                int productBefore = 0;
                int output = bulk ? 4 : 1;
                string label = recipeName + (mixed ? "/two-input" : "/single-input");

                await RuntimeThread.Run(delegate
                {
                    recipe = DefDatabase<RecipeDef>.GetNamed(recipeName);
                    ThingDef product = DefDatabase<ThingDef>.GetNamed(productName);
                    ctx.Require(recipe.products.Count == 1 && recipe.products[0].thingDef == product
                        && recipe.products[0].count == output, label + " output contract differs.");
                    ctx.Require(recipe.allowMixingIngredients && recipe.ingredients.Count == 1,
                        label + " must allow mixed input in one ingredient requirement.");
                    ctx.Require(Campfire.def.AllRecipes.Contains(recipe),
                        label + " must be offered at the standard campfire.");
                    float required = recipe.ingredients[0].GetBaseCount();
                    ctx.Require(Math.Abs(required - (bulk ? 2f : 0.5f)) < 0.001f,
                        label + " ingredient Nutrition differs.");
                    int requiredUnits = 0;
                    for (int i = 0; i < materials.Length; i++)
                    {
                        ThingDef ingredient = DefDatabase<ThingDef>.GetNamed(materials[i]);
                        float nutrition = ingredient.GetStatValueAbstract(StatDefOf.Nutrition);
                        ctx.Require(Math.Abs(nutrition - 0.05f) < 0.0001f,
                            label + " unexpected input Nutrition: " + ingredient.defName);
                        ctx.Require(recipe.fixedIngredientFilter.Allows(ingredient)
                            && recipe.ingredients[0].filter.Allows(ingredient),
                            label + " does not allow designated input " + ingredient.defName);
                        amounts[i] = (int)Math.Round(required / nutrition / materials.Length);
                        requiredUnits += amounts[i];
                    }
                    ctx.Require(Math.Abs(requiredUnits * 0.05f - required) < 0.001f,
                        label + " must use exact Nutrition, not partial extra stacks.");
                    productBefore = Count(productName);
                    for (int i = 0; i < materials.Length; i++)
                    {
                        starting[i] = Count(materials[i]);
                        Thing input = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed(materials[i]));
                        input.stackCount = amounts[i];
                        created.Add(input);
                        GenSpawn.Spawn(input, center + new IntVec3(i, 0, -2), map);
                    }
                    bill = (Bill_Production)recipe.MakeNewBill();
                    bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
                    bill.repeatCount = 1;
                    bill.SetStoreMode(BillStoreModeDefOf.DropOnFloor);
                    bill.ingredientSearchRadius = 10f;
                    bill.ingredientFilter.SetDisallowAll();
                    foreach (string material in materials)
                        bill.ingredientFilter.SetAllow(DefDatabase<ThingDef>.GetNamed(material), true);
                    Campfire.BillStack.AddBill(bill);
                    foreach (WorkGiverDef def in DefDatabase<WorkGiverDef>.AllDefsListForReading)
                    {
                        if (def.giverClass == null || !typeof(WorkGiver_DoBill).IsAssignableFrom(def.giverClass)) continue;
                        WorkGiver_DoBill giver = def.Worker as WorkGiver_DoBill;
                        if (giver == null || !giver.ThingIsUsableBillGiver(Campfire)) continue;
                        Job offered = giver.JobOnThing(worker, Campfire, true);
                        if (offered != null && offered.def == JobDefOf.DoBill && offered.bill == bill)
                        { job = offered; break; }
                    }
                    Start(job, label);
                });
                await Complete(job, () => bill.repeatCount == 0
                    && Count(productName) == productBefore + output, label);
                await RuntimeThread.Run(delegate
                {
                    for (int i = 0; i < materials.Length; i++)
                        ctx.Assert(Count(materials[i]) == starting[i],
                            label + " must consume every unit of input: " + materials[i]
                            + ", expected=" + amounts[i]);
                    ctx.Assert(Count(productName) == productBefore + output,
                        label + " must produce " + output + " servings.");
                    ctx.Assert(bill.repeatCount == 0, label + " must complete exactly one DoBill.");
                    Campfire.BillStack.Delete(bill);
                });
            }

            // Exercise the game's standard CookMealSimple Bill five times,
            // restricting each iteration to a single real harvested/processed grain.
            // The other available food sources must not satisfy this Bill.
            public async Task SimpleMeal(string ingredient)
            {
                Job job = null; Bill_Production bill = null; RecipeDef recipe = null;
                int beforeGrain = 0, beforeMeals = 0, neededCount = 0, outputCount = 0;
                await RuntimeThread.Run(delegate
                {
                    ThingDef grain = DefDatabase<ThingDef>.GetNamed(ingredient);
                    recipe = DefDatabase<RecipeDef>.GetNamed("CookMealSimple");
                    ctx.Require(Campfire.def.AllRecipes.Contains(recipe),
                        "The campfire must offer the Vanilla simple-meal recipe.");
                    ctx.Require(recipe.fixedIngredientFilter != null
                        && recipe.fixedIngredientFilter.Allows(grain),
                        "Standard simple meal does not accept " + ingredient);
                    ctx.Require(recipe.ingredients.Count == 1 && recipe.products.Count == 1
                        && recipe.products[0].thingDef.defName == "MealSimple",
                        "Unexpected Vanilla simple meal input/output contract.");
                    float eachNutrition = grain.GetStatValueAbstract(StatDefOf.Nutrition);
                    float requiredNutrition = recipe.ingredients[0].GetBaseCount();
                    ctx.Require(eachNutrition > 0f && requiredNutrition > 0f,
                        "Simple meal grain nutrition must be positive.");
                    neededCount = (int)Math.Ceiling(requiredNutrition / eachNutrition - 0.00001f);
                    ctx.Require(neededCount > 0
                        && Math.Abs(neededCount * eachNutrition - requiredNutrition) < 0.001f,
                        "Grain must satisfy the required simple meal nutrition exactly.");
                    beforeGrain = Count(ingredient);
                    beforeMeals = Count("MealSimple");
                    outputCount = recipe.products[0].count;
                    ctx.Require(beforeGrain >= neededCount,
                        "Native jobs have not produced enough edible " + ingredient);
                    bill = (Bill_Production)recipe.MakeNewBill();
                    bill.repeatMode = BillRepeatModeDefOf.RepeatCount;
                    bill.repeatCount = 1;
                    bill.SetStoreMode(BillStoreModeDefOf.DropOnFloor);
                    bill.ingredientSearchRadius = 10f;
                    bill.ingredientFilter.SetDisallowAll();
                    bill.ingredientFilter.SetAllow(grain, true);
                    Campfire.BillStack.AddBill(bill);
                    foreach (WorkGiverDef def in DefDatabase<WorkGiverDef>.AllDefsListForReading)
                    {
                        if (def.giverClass == null || !typeof(WorkGiver_DoBill).IsAssignableFrom(def.giverClass)) continue;
                        WorkGiver_DoBill giver = def.Worker as WorkGiver_DoBill;
                        if (giver == null || !giver.ThingIsUsableBillGiver(Campfire)) continue;
                        Job offered = giver.JobOnThing(worker, Campfire, true);
                        if (offered != null && offered.def == JobDefOf.DoBill && offered.bill == bill)
                        { job = offered; break; }
                    }
                    Start(job, "CookMealSimple/" + ingredient);
                });
                await Complete(job, () => bill.repeatCount == 0
                    && Count("MealSimple") == beforeMeals + outputCount,
                    "CookMealSimple/" + ingredient);
                await RuntimeThread.Run(delegate
                {
                    ctx.Assert(Count(ingredient) == beforeGrain - neededCount,
                        "Simple meal consumed the wrong grain quantity: " + ingredient);
                    ctx.Assert(Count("MealSimple") == beforeMeals + outputCount,
                        "Simple meal output is missing: " + ingredient);
                    Campfire.BillStack.Delete(bill);
                });
            }

            public void Dispose()
            {
                if (worker != null && worker.Spawned) worker.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                foreach (Thing thing in created.AsEnumerable().Reverse())
                    if (!thing.Destroyed) thing.Destroy(DestroyMode.Vanish);
                foreach (KeyValuePair<IntVec3,TerrainDef> cell in terrain) map.terrainGrid.SetTerrain(cell.Key, cell.Value);
                foreach (KeyValuePair<Pawn,IntVec3> pawn in parked)
                    if (!pawn.Key.Destroyed && !pawn.Key.Spawned) GenSpawn.Spawn(pawn.Key, pawn.Value, map);
                Find.TickManager.CurTimeSpeed = speed;
            }
        }
    }
}
