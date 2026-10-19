using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AncientMedievalJapan.Grains
{
    // Read-only extension: no Harmony dependency, Nice Plants Menu dependency,
    // per-tick calculations, or persistent save data.
    public sealed class CropComparisonGameComponent : GameComponent
    {
        public CropComparisonGameComponent(Game game) { }

        public override void GameComponentOnGUI()
        {
            if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap == null)
                return;
            var selected = Find.Selector.SelectedObjects;
            if (selected == null || selected.Count != 1 || !(selected[0] is Zone_Growing zone)
                || zone.Map != Find.CurrentMap || zone.CellCount == 0)
                return;
            if (Widgets.ButtonText(new Rect(UI.screenWidth - 232f, 90f, 216f, 32f),
                "AMJC_CompareCropsButton".Translate()))
                Find.WindowStack.Add(new Dialog_CropComparison(zone));
        }
    }

    // Snapshot current zone fertility and temperature when opened.
    // These are not a future weather or calendar-accurate growing-season forecast.
    public sealed class Dialog_CropComparison : Window
    {
        private sealed class CropSnapshot
        {
            public ThingDef Def;
            public float[] CalendarDaysByCell;
            public float HarvestYield;
            public float MinimumCalendarDays;
            public float UnitsPerDayPerZoneCell;
            public int CellsPossible;
        }

        private readonly List<CropSnapshot> snapshots = new List<CropSnapshot>();
        private readonly int zoneCellCount;
        private readonly float avgFertility;
        private int horizon = 30;
        private Vector2 scroll = Vector2.zero;

        public override Vector2 InitialSize => new Vector2(930f, 650f);

        public Dialog_CropComparison(Zone_Growing zone)
        {
            doCloseX = true;
            closeOnClickedOutside = false;
            absorbInputAroundWindow = true;
            zoneCellCount = zone.CellCount;
            var map = zone.Map;
            var cells = zone.Cells.ToList();
            avgFertility = cells.Average(c => map.fertilityGrid.FertilityAt(c));

            // Vanilla's picker determines sowable choices; not all ThingDefs.
            var growers = new List<IPlantToGrowSettable> { zone };
            foreach (ThingDef plant in PlantUtility.ValidPlantTypesForGrowers(growers))
            {
                if (plant?.plant == null || plant.plant.purpose != PlantPurpose.Food
                    || plant.plant.harvestedThingDef == null
                    || plant.plant.harvestYield <= 0f || plant.plant.growDays <= 0f)
                    continue;
                var snap = new CropSnapshot
                {
                    Def = plant,
                    HarvestYield = plant.plant.harvestYield,
                    CalendarDaysByCell = new float[cells.Count],
                    MinimumCalendarDays = float.MaxValue
                };
                float sumUnitsPerDay = 0f;
                for (int i = 0; i < cells.Count; i++)
                {
                    var c = cells[i];
                    float soil = map.fertilityGrid.FertilityAt(c);
                    if (!plant.plant.completelyIgnoreFertility && soil < plant.plant.fertilityMin)
                    {
                        snap.CalendarDaysByCell[i] = float.PositiveInfinity;
                        continue;
                    }
                    float temp = GenTemperature.GetTemperatureForCell(c, map);
                    float rate = PlantUtility.GrowthRateFactorFor_Fertility(plant, soil)
                               * PlantUtility.GrowthRateFactorFor_Temperature(plant, temp);
                    // 1.6 Vanilla crops rest at night; 13/24 is an approximation
                    // under ideal light, not a prediction of future climate.
                    if (rate <= 0.0001f)
                    {
                        snap.CalendarDaysByCell[i] = float.PositiveInfinity;
                        continue;
                    }
                    float days = plant.plant.growDays / (rate * 13f / 24f);
                    snap.CalendarDaysByCell[i] = days;
                    snap.MinimumCalendarDays = Math.Min(snap.MinimumCalendarDays, days);
                    snap.CellsPossible++;
                    sumUnitsPerDay += plant.plant.harvestYield / days;
                }
                snap.UnitsPerDayPerZoneCell = sumUnitsPerDay / Math.Max(1, zoneCellCount);
                snapshots.Add(snap);
            }
            snapshots.Sort((a, b) => b.UnitsPerDayPerZoneCell.CompareTo(a.UnitsPerDayPerZoneCell));
        }

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(0f, 0f, inRect.width, 32f), "AMJC_CropCompareTitle".Translate());
            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(0f, 34f, inRect.width, 48f),
                "AMJC_CropCompareDisclaimer".Translate());
            Widgets.Label(new Rect(0f, 86f, inRect.width - 350f, 29f),
                "AMJC_CropCompareZoneInfo".Translate(zoneCellCount, avgFertility.ToStringPercent()));
            if (Widgets.ButtonText(new Rect(inRect.width - 318f, 86f, 96f, 28f),
                "15 " + "AMJC_Days".Translate())) horizon = 15;
            if (Widgets.ButtonText(new Rect(inRect.width - 214f, 86f, 96f, 28f),
                "30 " + "AMJC_Days".Translate())) horizon = 30;
            if (Widgets.ButtonText(new Rect(inRect.width - 110f, 86f, 96f, 28f),
                "60 " + "AMJC_Days".Translate())) horizon = 60;

            float rowTop = 122f;
            var body = new Rect(0f, rowTop, inRect.width, inRect.height - rowTop - 10f);
            var view = new Rect(0f, 0f, inRect.width - 21f, 31f * (snapshots.Count + 1));
            Widgets.BeginScrollView(body, ref scroll, view);
            Header(view.width);
            for (int i = 0; i < snapshots.Count; i++)
            {
                var item = snapshots[i];
                float y = (i + 1) * 31f;
                if (i % 2 == 1)
                    Widgets.DrawAltRect(new Rect(0f, y, view.width, 30f));
                int total = 0;
                int minCycles = int.MaxValue;
                int maxCycles = 0;
                foreach (float days in item.CalendarDaysByCell)
                {
                    int cycles = float.IsInfinity(days) ? 0 : Mathf.FloorToInt(horizon / days);
                    total += cycles * Mathf.RoundToInt(item.HarvestYield);
                    minCycles = Math.Min(minCycles, cycles);
                    maxCycles = Math.Max(maxCycles, cycles);
                }
                Cell(item.Def.LabelCap, 0f, y, 205f);
                Cell(item.HarvestYield.ToString("0.#"), 209f, y, 92f);
                Cell(item.CellsPossible == 0 ? "—" : item.MinimumCalendarDays.ToString("0.0"),
                    307f, y, 115f);
                Cell(item.UnitsPerDayPerZoneCell.ToString("0.00"), 429f, y, 107f);
                Cell(total.ToString(), 543f, y, 119f);
                Cell(item.CellsPossible == 0 ? "—" :
                    (minCycles == maxCycles ? maxCycles.ToString() : minCycles + "–" + maxCycles),
                    669f, y, 113f);
                Cell(item.Def.plant.sowMinSkill.ToString(), 793f, y, 65f);
            }
            Widgets.EndScrollView();
        }

        private void Header(float width)
        {
            Widgets.DrawMenuSection(new Rect(0f, 0f, width, 30f));
            Cell("AMJC_CropComparePlant".Translate(), 0f, 0f, 205f);
            Cell("AMJC_CropCompareOnce".Translate(), 209f, 0f, 92f);
            Cell("AMJC_CropCompareMaturity".Translate(), 307f, 0f, 115f);
            Cell("AMJC_CropComparePerDay".Translate(), 429f, 0f, 107f);
            Cell("AMJC_CropCompareTotal".Translate(horizon), 543f, 0f, 119f);
            Cell("AMJC_CropCompareCycles".Translate(), 669f, 0f, 113f);
            Cell("AMJC_CropCompareSkill".Translate(), 793f, 0f, 65f);
        }

        private static void Cell(string text, float x, float y, float width)
        {
            Widgets.Label(new Rect(x + 3f, y + 4f, width - 5f, 26f), text);
        }
    }
}
