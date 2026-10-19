using System;
using System.Linq;
using System.Threading.Tasks;
using RimWorks.Pickle;
using RimWorld;
using Verse;

namespace AncientMedievalJapanCore.E2E
{
    // Migration smoke contracts. The legacy fixture suite remains independent.
    [PickleSteps]
    public sealed class GrainsProfileSteps
    {
        private static bool Active(string id)
        {
            return LoadedModManager.RunningModsListForReading.Any(
                mod => string.Equals(mod.PackageIdPlayerFacing, id, StringComparison.OrdinalIgnoreCase));
        }

        private static void AssertProviders(PickleContext ctx, bool mo, bool ccto)
        {
            ctx.Assert(Active("dankpyon.medieval.overhaul") == mo, "Real MO presence must match the requested profile.");
            ctx.Assert(Active("sucro.cropcoldtoleranceoverhaul") == ccto, "Real CCTO presence must match the requested profile.");
            ctx.Assert(!Active("sucro.ancientmedievaljapan.core.mofixture"), "MO API fixture must be absent.");
            ctx.Assert(!Active("sucro.ancientmedievaljapan.core.cctofixture"), "CCTO API fixture must be absent.");
            ctx.Assert(!Active("sucro.ancientmedievaljapan.core"), "Production Core must not load alongside its test copy.");
        }

        [Then("the real vanilla Grains providers are active")]
        public void Vanilla(PickleContext ctx) { AssertProviders(ctx, false, false); }
        [Then("the real vanilla CCTO Grains providers are active")]
        public void VanillaCcto(PickleContext ctx) { AssertProviders(ctx, false, true); }
        [Then("the real MO Grains providers are active")]
        public void Mo(PickleContext ctx) { AssertProviders(ctx, true, false); }
        [Then("the real MO CCTO Grains providers are active")]
        public void MoCcto(PickleContext ctx) { AssertProviders(ctx, true, true); }

        [Then("the Grains primary grain loop resolves")]
        public Task PrimaryLoop(PickleContext ctx)
        {
            return RuntimeThread.Run(delegate
            {
                foreach (string name in new[] { "AMJC_Plant_FoxtailMillet_Awa", "AMJC_Plant_BarnyardMillet_Hie",
                    "AMJC_Plant_ProsoMillet_Kibi", "AMJC_Plant_Buckwheat_Soba", "AMJC_Plant_Barley" })
                {
                    ThingDef crop = DefDatabase<ThingDef>.GetNamed(name);
                    ctx.Require(crop.plant != null && crop.plant.harvestedThingDef != null, name + " must resolve a harvest.");
                    ctx.Assert(crop.plant.sowResearchPrerequisites == null || crop.plant.sowResearchPrerequisites.All(r => r != null),
                        name + " must have no unresolved research.");
                }
                foreach (RecipeDef recipe in DefDatabase<RecipeDef>.AllDefsListForReading.Where(r => r.defName.StartsWith("AMJC_")))
                {
                    ctx.Require(recipe.products != null && recipe.products.Count > 0, recipe.defName + " must resolve products.");
                    ctx.Assert(recipe.products.All(p => p.thingDef != null), recipe.defName + " has an unresolved output.");
                    ctx.Require(recipe.ingredients != null && recipe.ingredients.Count > 0, recipe.defName + " must resolve ingredients.");
                    ctx.Assert(recipe.ingredients.All(i => i.filter.AllowedThingDefs.Any()), recipe.defName + " has an empty ingredient filter.");
                }
                ThingDef barley = DefDatabase<ThingDef>.GetNamed("AMJC_Plant_Barley");
                ctx.Assert(barley.plant.sowResearchPrerequisites == null || barley.plant.sowResearchPrerequisites.Count == 0,
                    "AMJG barley must remain research-free in Base and MO profiles.");
                ThingDef table = DefDatabase<ThingDef>.GetNamed("AMJC_GrainProcessingTable");
                ctx.Assert(table.researchPrerequisites == null || table.researchPrerequisites.Count == 0,
                    "AMJG processing table must remain research-free in Base and MO profiles.");
                ctx.Assert(table.costList.Count == 1 && table.costList[0].thingDef.defName == "Steel" && table.costList[0].count == 30,
                    "AMJG processing table must use 30 Steel in Base and MO profiles.");
                // Threshing/hulling beta stove fallback applies with or without MO.
                ThingDef processingFuelStove = DefDatabase<ThingDef>.GetNamed("FueledStove");
                ThingDef processingElectricStove = DefDatabase<ThingDef>.GetNamed("ElectricStove");
                ThingDef processingSpot = DefDatabase<ThingDef>.GetNamed("AMJC_GrainProcessingSpot");
                foreach (RecipeDef processing in DefDatabase<RecipeDef>.AllDefsListForReading.Where(
                    r => r.defName.StartsWith("AMJC_Thresh") || r.defName.StartsWith("AMJC_Hull")))
                {
                    ctx.Assert(processing.recipeUsers != null && processing.recipeUsers.Count == 4
                        && processing.recipeUsers.Contains(processingSpot) && processing.recipeUsers.Contains(table)
                        && processing.recipeUsers.Contains(processingFuelStove)
                        && processing.recipeUsers.Contains(processingElectricStove),
                        "All 18 grain processing Bills require two AMJ benches and two Vanilla stoves: "
                            + processing.defName);
                }
                if (!Active("dankpyon.medieval.overhaul"))
                {
                    foreach (RecipeDef recipe in DefDatabase<RecipeDef>.AllDefsListForReading.Where(r => r.defName.StartsWith("AMJC_Thresh")))
                        ctx.Assert(recipe.products.Count == 1 && recipe.products.All(p => p.thingDef.defName.StartsWith("AMJC_")),
                            "Base threshing must produce only its AMJ grain: " + recipe.defName);
                }
                // MO produces Straw as a byproduct, but TargetCount tracks grain only.
                bool moThresh = Active("dankpyon.medieval.overhaul");
                foreach (RecipeDef thresh in DefDatabase<RecipeDef>.AllDefsListForReading.Where(
                    r => r.defName.StartsWith("AMJC_Thresh")))
                {
                    ctx.Assert(thresh.products.Count == (moThresh ? 2 : 1)
                        && (!moThresh || thresh.products[1].thingDef.defName == "DankPyon_Straw"),
                        "Thresh outputs must keep primary grain followed by optional MO Straw: " + thresh.defName);
                    if (moThresh)
                    {
                        ctx.Assert(thresh.WorkerCounter.GetType().FullName == "AncientMedievalJapan.Grains.RecipeWorkerCounter_PrimaryGrain",
                            "MO thresh must use the primary-grain counter: " + thresh.defName);
                        // The actual bench-bound Bill verifies CanCountProducts in
                        // AlternateProcessingJobs; an unattached Bill has no Map.
                    }
                }
                ThingDef rice = DefDatabase<ThingDef>.GetNamed("Plant_Rice");
                ThingDef rawRice = DefDatabase<ThingDef>.GetNamed("RawRice");
                ThingDef riceSheaf = DefDatabase<ThingDef>.GetNamed("AMJC_RiceSheaf");
                ThingDef riceHull = DefDatabase<ThingDef>.GetNamed("AMJC_RiceInHull");
                ctx.Require(rice.plant != null && rice.plant.harvestedThingDef == riceSheaf,
                    "Upland rice must harvest AMJC_RiceSheaf; RawRice remains the existing edible item.");
                ctx.Assert(riceSheaf.ingestible.preferability == FoodPreferability.NeverForNutrition
                    && riceHull.ingestible.preferability == FoodPreferability.NeverForNutrition,
                    "Rice intermediates must be inedible.");
                foreach (string part in new[] { "Thresh", "Hull" })
                foreach (bool bulk in new[] { false, true })
                {
                    string name = "AMJC_" + part + "Rice" + (bulk ? "Bulk" : "");
                    RecipeDef rec = DefDatabase<RecipeDef>.GetNamed(name);
                    int n = bulk ? 10 : 1;
                    ThingDef input = part == "Thresh" ? riceSheaf : riceHull;
                    ThingDef output = part == "Thresh" ? riceHull : rawRice;
                    ctx.Assert(rec.ingredients.Count == 1 && rec.ingredients[0].filter.Allows(input)
                        && rec.ingredients[0].GetBaseCount() == n, "Rice processing input differs: " + name);
                    bool withStraw = Active("dankpyon.medieval.overhaul") && part == "Thresh";
                    ThingDef straw = withStraw ? DefDatabase<ThingDef>.GetNamed("DankPyon_Straw") : null;
                    ctx.Assert(rec.products.Count == (withStraw ? 2 : 1)
                        && rec.products.Any(p => p.thingDef == output && p.count == n)
                        && rec.products.All(p => p.count == n
                            && (p.thingDef == output || (withStraw && p.thingDef == straw)))
                        && (!withStraw || rec.products.Any(p => p.thingDef == straw && p.count == n)),
                        "Rice processing output must be the exact grain plus optional MO Straw: " + name);
                }
                ctx.Assert(rice.plant.growDays == 5f && rice.plant.harvestYield == 11f
                    && rice.plant.fertilityMin == 0.7f && rice.plant.fertilitySensitivity == 0.8f,
                    "Rice growth, yield or fertility differs from design.");
                ctx.Assert(rice.plant.minGrowthTemperature == 10f && rice.plant.maxGrowthTemperature == 42f
                    && rice.plant.minOptimalGrowthTemperature == 18f && rice.plant.maxOptimalGrowthTemperature == 32f,
                    "Rice temperatures differ from design.");
                ctx.Assert(rice.plant.sowTags.Count == 1 && rice.plant.sowTags[0] == "Ground",
                    "Rice must sow on ground and not in hydroponics.");
                ctx.Assert(rice.plant.sowMinSkill == 4,
                    "Rice sowing must require Plants level 4; all other crops keep their owner-defined requirements.");
                RecipeDef meal = DefDatabase<RecipeDef>.GetNamed("CookMealSimple");
                ctx.Assert(meal.fixedIngredientFilter != null && meal.fixedIngredientFilter.Allows(rawRice),
                    "RawRice must still be a simple-meal ingredient.");
                var ext = rice.modExtensions == null
                    ? new System.Collections.Generic.List<DefModExtension>()
                    : rice.modExtensions.Where(e => e != null &&
                        e.GetType().FullName == "CropColdToleranceOverhaul.ColdToleranceExtension").ToList();
                bool ccto = Active("sucro.cropcoldtoleranceoverhaul");
                ctx.Assert(ext.Count == (ccto ? 1 : 0), "Rice CCTO extension count differs.");
                if (ccto)
                {
                    var field = ext[0].GetType().GetField("coldDeathTemperature");
                    ctx.Require(field != null && Convert.ToSingle(field.GetValue(ext[0])) == -1f,
                        "Rice must retain the CCTO -1 C cold-death threshold.");
                }
                new StageASteps().AssertSimpleMealAcceptsMillet(ctx);
                AssertLoadedGrainIcons(ctx);
            });
        }


        // Verify real loaded textures and stack materials on Unity's main thread.
        // One _a PNG is enough for these grain states; no production bytes are pinned.
        private static void AssertLoadedGrainIcons(PickleContext ctx)
        {
            string[][] icons = {
                new[] { "AMJC_MilletInHull", "Things/Item/Resource/AMJC_Millet/MilletInHull" },
                new[] { "AMJC_Millet", "Things/Item/Resource/AMJC_Millet/Millet" },
                new[] { "AMJC_BuckwheatInHull", "Things/Item/Resource/AMJC_Buckwheat/BuckwheatInHull" },
                new[] { "AMJC_Buckwheat", "Things/Item/Resource/AMJC_Buckwheat/Buckwheat" },
                new[] { "AMJC_BarleyInHull", "Things/Item/Resource/AMJC_Barley/BarleyInHull" },
                new[] { "AMJC_Barley", "Things/Item/Resource/AMJC_Barley/Barley" },
                new[] { "AMJC_Wheat", "Things/Item/Resource/AMJC_Wheat/Wheat" },
                new[] { "AMJC_RiceInHull", "Things/Item/Resource/AMJC_Rice/RiceInHull" },
                new[] { "RawRice", "Things/Item/Resource/AMJC_Rice/Rice" }
            };
            foreach (string[] pair in icons)
            {
                string defName = pair[0];
                string path = pair[1];
                ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
                ctx.Require(def != null && def.graphicData != null, defName + " must have loaded graphic data.");
                ctx.Assert(def.graphicData.texPath == path, defName + " must use its grain texture path.");
                ctx.Assert(def.graphicData.graphicClass == typeof(Graphic_StackCount),
                    defName + " must use Graphic_StackCount for _a grain graphics.");
                string filename = path.Substring(path.LastIndexOf('/') + 1) + "_a";
                UnityEngine.Texture2D texture = ContentFinder<UnityEngine.Texture2D>.Get(path + "/" + filename, false);
                ctx.Require(texture != null && texture != BaseContent.BadTex,
                    defName + " must load its _a PNG via Unity.");
                ctx.Assert(texture.width == texture.height && (texture.width == 128 || texture.width == 256),
                    defName + " must load a supported square grain icon.");

                Graphic_StackCount graphic = def.graphicData.Graphic as Graphic_StackCount;
                ctx.Require(graphic != null, defName + " must resolve Graphic_StackCount.");
                foreach (int count in new[] { 1, 2, def.stackLimit })
                {
                    Graphic subGraphic = graphic.SubGraphicForStackCount(count, def);
                    ctx.Require(subGraphic != null, defName + " is missing a subgraphic at count " + count);
                    UnityEngine.Material material = subGraphic.MatSingle;
                    ctx.Require(material != null && material.mainTexture != null
                        && material.mainTexture != BaseContent.BadTex,
                        defName + " needs a valid material at count " + count);
                }
            }
        }

        [Then("the Grains wheat flour and minimum food chains resolve")]
        public Task FlourChain(PickleContext ctx)
        {
            return RuntimeThread.Run(delegate
            {
                bool mo = Active("dankpyon.medieval.overhaul");
                foreach (string fallback in new[] { "AMJC_Plant_Wheat", "AMJC_RawWheat", "AMJC_WheatFlour", "AMJC_ManualMillstone" })
                    ctx.Assert((DefDatabase<ThingDef>.GetNamedSilentFail(fallback) != null) == !mo,
                        "Fallback presence must match MO absence: " + fallback);
                ctx.Assert((DefDatabase<RecipeDef>.GetNamedSilentFail("AMJC_MillWheat") != null) == !mo,
                    "Only Base must expose the fallback wheat milling recipe.");
                ThingDef wheat = DefDatabase<ThingDef>.GetNamed(mo ? "DankPyon_Plant_Wheat" : "AMJC_Plant_Wheat");
                string sheaf = mo ? "DankPyon_RawWheat" : "AMJC_RawWheat";
                ctx.Assert(wheat.plant.harvestedThingDef.defName == sheaf && wheat.plant.growDays == 12f
                    && wheat.plant.harvestYield == 28f && wheat.plant.fertilityMin == 0.7f
                    && wheat.plant.fertilitySensitivity == 0.9f, "Wheat provider must retain the six-grain balance.");
                if (!mo)
                {
                    ctx.Assert(wheat.plant.sowResearchPrerequisites == null || wheat.plant.sowResearchPrerequisites.Count == 0,
                        "Base wheat must be research-free.");
                    bool ccto = Active("sucro.cropcoldtoleranceoverhaul");
                    var extensions = wheat.modExtensions == null ? new System.Collections.Generic.List<DefModExtension>()
                        : wheat.modExtensions.Where(e => e != null && e.GetType().FullName == "CropColdToleranceOverhaul.ColdToleranceExtension").ToList();
                    ctx.Assert(extensions.Count == (ccto ? 1 : 0), "Fallback CCTO extension must match the provider.");
                    if (ccto)
                        ctx.Assert(Convert.ToSingle(extensions[0].GetType().GetField("coldDeathTemperature").GetValue(extensions[0])) == -6f,
                            "Fallback wheat must retain the -6 C cold-death value.");
                }
                ThingDef inputSheaf = DefDatabase<ThingDef>.GetNamed(sheaf);
                ctx.Assert(inputSheaf.ingestible.preferability == FoodPreferability.NeverForNutrition,
                    "Wheat sheaves must not bypass threshing.");
                foreach (string name in new[] { "AMJC_ThreshWheat", "AMJC_ThreshWheatBulk" })
                {
                    RecipeDef rec = DefDatabase<RecipeDef>.GetNamed(name);
                    int count = name.EndsWith("Bulk") ? 10 : 1;
                    ctx.Assert(rec.ingredients.Count == 1 && rec.ingredients[0].filter.Allows(inputSheaf)
                        && rec.ingredients[0].GetBaseCount() == count, "Wheat thresh input differs: " + name);
                    ctx.Assert(rec.products.Count == (mo ? 2 : 1)
                        && rec.products.Single(p => p.thingDef.defName == "AMJC_Wheat").count == count,
                        "Wheat thresh convergence differs: " + name);
                }
                if (mo)
                {
                    // All three upstream MO grinding recipes include Hay, but
                    // AMJ's conditional XML patch must remove that byproduct.
                    foreach (string sourceName in new[] { "DankPyon_CraftFlour_Manual",
                        "DankPyon_CraftFlour", "DankPyon_CraftFlourBulk" })
                    {
                        RecipeDef loaded = DefDatabase<RecipeDef>.GetNamed(sourceName);
                        ctx.Assert(loaded.products != null && loaded.products.Count == 1
                            && loaded.products[0].thingDef.defName == "DankPyon_Flour",
                            "Grains must remove upstream MO milling Hay: " + sourceName);
                    }
                }
                ThingDef mill = DefDatabase<ThingDef>.GetNamed(mo ? "DankPyon_Millstone" : "AMJC_ManualMillstone");
                ctx.Assert(mill.researchPrerequisites == null || mill.researchPrerequisites.Count == 0,
                    "The standard Grains mill must be research-free.");
                string[] grains = { "AMJC_Buckwheat", "AMJC_Millet", "AMJC_Wheat", "AMJC_Barley" };
                string[] powders = { "AMJC_BuckwheatFlour", "AMJC_MilletFlour", mo ? "DankPyon_Flour" : "AMJC_WheatFlour", "AMJC_BarleyFlour" };
                string[] recipes = { "AMJC_MillBuckwheat", "AMJC_MillMillet", mo ? "DankPyon_CraftFlour" : "AMJC_MillWheat", "AMJC_MillBarley" };
                for (int index = 0; index < grains.Length; index++)
                {
                    RecipeDef rec = DefDatabase<RecipeDef>.GetNamed(recipes[index]);
                    ThingDef grain = DefDatabase<ThingDef>.GetNamed(grains[index]);
                    ThingDef powder = DefDatabase<ThingDef>.GetNamed(powders[index]);
                    ctx.Assert(rec.ingredients.Any(i => i.filter.Allows(grain)) && rec.recipeUsers.Contains(mill),
                        "Grain must connect to the standard mill: " + grains[index]);
                    // The V1 stove fallback applies to all AMJ non-MO milling Recipes only.
                    // MO's native wheat recipe is kept unchanged and is checked separately above.
                    if (rec.defName.StartsWith("AMJC_Mill"))
                    {
                        ThingDef fueled = DefDatabase<ThingDef>.GetNamed("FueledStove");
                        ThingDef electric = DefDatabase<ThingDef>.GetNamed("ElectricStove");
                        ctx.Assert(mo
                            ? rec.recipeUsers.Count == 1 && rec.recipeUsers[0] == mill
                            : rec.recipeUsers.Count == 3 && rec.recipeUsers.Contains(fueled)
                                && rec.recipeUsers.Contains(electric),
                            "V1 beta: only Base must offer milling at both Vanilla stoves: " + recipes[index]);
                    }
                    ctx.Assert(rec.products.Count == 1 && rec.products[0].thingDef == powder,
                        "Milling must yield only the designated powder: " + recipes[index]);
                    ctx.Assert(Math.Abs(rec.ingredients[0].GetBaseCount() * grain.GetStatValueAbstract(StatDefOf.Nutrition)
                        - rec.products[0].count * powder.GetStatValueAbstract(StatDefOf.Nutrition)) < 0.001f,
                        "Milling must conserve nutrition: " + recipes[index]);
                    ctx.Assert(powder.ingestible.preferability == FoodPreferability.NeverForNutrition,
                        "Powders must be used by cooking rather than eaten directly.");
                }
                string[] foods = { "Sobagaki", "MilletDumplings", "Houtou", "BarleyFlourGruel" };
                for (int index = 0; index < foods.Length; index++)
                {
                    RecipeDef rec = DefDatabase<RecipeDef>.GetNamed("AMJC_Cook" + foods[index]);
                    ThingDef food = DefDatabase<ThingDef>.GetNamed("AMJC_" + foods[index]);
                    ThingDef powder = DefDatabase<ThingDef>.GetNamed(powders[index]);
                    ctx.Assert(rec.ingredients.Count == 1 && rec.ingredients[0].filter.Allows(powder)
                        && rec.ingredients[0].GetBaseCount() == 0.5f, "Powder meal must require 0.5 nutrition.");
                    ctx.Assert(rec.products.Count == 1 && rec.products[0].thingDef == food && rec.products[0].count == 1,
                        "Cooking must yield one designed flour meal.");
                    ctx.Assert(rec.recipeUsers.Any(u => u.defName == "Campfire") && rec.researchPrerequisite == null
                        && (rec.researchPrerequisites == null || rec.researchPrerequisites.Count == 0),
                        "Minimum powder food must be research-free and available at a campfire.");
                    RecipeDef bulk = DefDatabase<RecipeDef>.GetNamed("AMJC_Cook" + foods[index] + "Bulk");
                    ctx.Assert(bulk.ingredients.Count == 1 && bulk.ingredients[0].filter.Allows(powder)
                        && bulk.fixedIngredientFilter.Allows(powder)
                        && Math.Abs(bulk.ingredients[0].GetBaseCount() - 2f) < 0.001f,
                        "Bulk powder meal must consume 2 Nutrition from the same flour.");
                    ctx.Assert(bulk.products.Count == 1 && bulk.products[0].thingDef == food
                        && bulk.products[0].count == 4 && Math.Abs(bulk.workAmount - 1200f) < 0.001f,
                        "Four-meal production must output 4 servings for 1200 work.");
                    ctx.Assert(bulk.recipeUsers.Any(u => u.defName == "Campfire")
                        && bulk.recipeUsers.Any(u => u.defName == "FueledStove")
                        && bulk.recipeUsers.Any(u => u.defName == "ElectricStove")
                        && bulk.researchPrerequisite == null
                        && (bulk.researchPrerequisites == null || bulk.researchPrerequisites.Count == 0),
                        "Bulk powder food must be research-free and available at standard cook stations.");
                    ctx.Assert(Math.Abs(food.GetStatValueAbstract(StatDefOf.Nutrition) - 0.9f) < 0.001f
                        && food.ingestible.tasteThought.defName == "AMJC_AteFlourFood", "Food nutrition/thought differs.");
                    var rottable = food.comps.OfType<CompProperties_Rottable>().ToList();
                    ctx.Assert(rottable.Count == 1 && rottable[0].daysToRotStart == 2.5f,
                        "Powder meals must contain exactly one 2.5-day rottable comp.");
                }
                // Shared fallback foods must accept mixed cereal stacks but
                // must never silently cross grain and flour input families.
                string[] rawGrains = { "AMJC_Millet", "AMJC_Buckwheat", "AMJC_Barley",
                    "AMJC_Wheat", "RawRice" };
                string[] mixedPowders = { "AMJC_MilletFlour", "AMJC_BuckwheatFlour",
                    "AMJC_BarleyFlour",
                    mo ? "DankPyon_Flour" : "AMJC_WheatFlour" };
                string[] commonFoods = { "MixedGrainMeal", "MixedGrainPorridge", "MixedFlourFood" };
                float[] commonWork = { 240f, 200f, 300f };
                float[] commonRotDays = { 3f, 1.5f, 2f };
                for (int index = 0; index < commonFoods.Length; index++)
                {
                    ThingDef food = DefDatabase<ThingDef>.GetNamed("AMJC_" + commonFoods[index]);
                    ThingDef[] accepted = (index == 2 ? mixedPowders : rawGrains)
                        .Select(s => DefDatabase<ThingDef>.GetNamed(s)).ToArray();
                    ThingDef[] forbidden = (index == 2 ? rawGrains : mixedPowders)
                        .Select(s => DefDatabase<ThingDef>.GetNamed(s)).ToArray();
                    for (int batchIndex = 0; batchIndex < 2; batchIndex++)
                    {
                        int servings = batchIndex == 0 ? 1 : 4;
                        RecipeDef recipe = DefDatabase<RecipeDef>.GetNamed("AMJC_Cook"
                            + commonFoods[index] + (servings == 4 ? "Bulk" : ""));
                        ctx.Assert(recipe.allowMixingIngredients && recipe.ingredients.Count == 1
                            && Math.Abs(recipe.ingredients[0].GetBaseCount() - 0.5f * servings) < 0.001f
                            && Math.Abs(recipe.workAmount - commonWork[index] * servings) < 0.001f,
                            "Generic food recipe must allow mixed inputs at fixed nutrition and work.");
                        ctx.Assert(recipe.products.Count == 1 && recipe.products[0].thingDef == food
                            && recipe.products[0].count == servings,
                            "Generic food Bill output quantity or product differs.");
                        foreach (ThingDef ing in accepted)
                            ctx.Assert(recipe.ingredients[0].filter.Allows(ing)
                                && recipe.fixedIngredientFilter.Allows(ing),
                                "Generic food excludes an eligible grain or flour: " + ing.defName);
                        foreach (ThingDef ing in forbidden)
                            ctx.Assert(!recipe.ingredients[0].filter.Allows(ing)
                                && !recipe.fixedIngredientFilter.Allows(ing),
                                "Generic food erroneously crosses flour/grain boundary: " + ing.defName);
                        ctx.Assert(recipe.recipeUsers.Any(t => t.defName == "Campfire")
                            && recipe.recipeUsers.Any(t => t.defName == "FueledStove")
                            && recipe.recipeUsers.Any(t => t.defName == "ElectricStove")
                            && recipe.researchPrerequisite == null
                            && (recipe.researchPrerequisites == null || recipe.researchPrerequisites.Count == 0),
                            "Common food must be research-free and available at ordinary cook stations.");
                    }
                    ctx.Assert(Math.Abs(food.GetStatValueAbstract(StatDefOf.Nutrition) - 0.9f) < 0.001f,
                        "Generic cooked food must match one simple meal nutrition.");
                    var commonRottable = food.comps.OfType<CompProperties_Rottable>().ToList();
                    ctx.Assert(commonRottable.Count == 1
                        && Math.Abs(commonRottable[0].daysToRotStart - commonRotDays[index]) < 0.001f,
                        "Shared food rotting interval or inherited comps drifted.");
                }
                ctx.Assert(DefDatabase<ThoughtDef>.GetNamed("AMJC_AteFlourFood").stages[0].baseMoodEffect == 2f,
                    "The extra flour processing reward must remain +2 mood.");
            });
        }

        [Then("Grains cold tolerance extensions are present")]
        public void ColdPresent(PickleContext ctx) { new StageASteps().AssertLoadedCropCctoCompatibility(ctx); }

        [Then("Grains cold tolerance extensions are absent")]
        public void ColdAbsent(PickleContext ctx)
        {
            foreach (ThingDef crop in DefDatabase<ThingDef>.AllDefsListForReading.Where(d => d.defName.StartsWith("AMJC_") && d.plant != null))
                ctx.Assert(crop.modExtensions == null || !crop.modExtensions.Any(e => e != null &&
                    e.GetType().FullName == "CropColdToleranceOverhaul.ColdToleranceExtension"),
                    crop.defName + " must not load a CCTO extension without CCTO.");
        }
    }
}
