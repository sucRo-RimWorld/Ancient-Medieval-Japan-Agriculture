using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RimWorks.Pickle;
using RimWorld;
using Verse;
using UnityEngine;

namespace AncientMedievalJapanCore.E2E
{
    // Runs only in a disposable native Grains test target; never alters production
    // Defs, active player language, or the author's saves.
    [PickleSteps]
    public sealed class GrainsUiSteps
    {
        private static readonly string[] RiceDefs = {
            "Plant_Rice", "AMJC_RiceSheaf", "AMJC_RiceInHull", "RawRice"
        };
        private static readonly string[] RiceEnglish = {
            "rice plant", "rice sheaf", "rice in hull", "rice"
        };
        private static readonly string[] RiceJapanese = {
            "稲", "稲束", "籾", "米"
        };
        private static readonly int[] RiceParagraphs = { 2, 2, 2, 4 };
        private static readonly string[] FoodDefs = {
            "AMJC_MixedGrainMeal", "AMJC_MixedGrainPorridge",
            "AMJC_MixedFlourFood"
        };
        private static readonly string[] FoodEnglish = {
            "cooked grains", "porridge", "flour dish"
        };
        private static readonly string[] FoodJapanese = { "飯", "粥", "粉食" };
        private static readonly string[] RenderedGrains = {
            "AMJC_MilletInHull", "AMJC_Millet", "AMJC_BuckwheatInHull",
            "AMJC_Buckwheat", "AMJC_BarleyInHull", "AMJC_Barley",
            "AMJC_Wheat", "AMJC_RiceInHull", "RawRice",
            "AMJC_MilletFlour", "AMJC_BuckwheatFlour",
            "AMJC_BarleyFlour", "AMJC_RiceFlour"
        };
        private static readonly string[] SpecialtyFoods = {
            "AMJC_Houtou", "AMJC_Sobagaki", "AMJC_MilletDumplings",
            "AMJC_BarleyFlourGruel", "AMJC_RiceFlourDumplings"
        };

        private static void AssertText(PickleContext ctx, ThingDef def, string expected,
            int paragraphs, bool japanese)
        {
            ctx.Require(def != null, expected + " loaded ThingDef is missing.");
            ctx.Assert(string.Equals(def.label, expected, StringComparison.Ordinal),
                def.defName + " loaded label differs; expected " + expected
                + " actual " + def.label);
            string content = def.description ?? "";
            ctx.Require(content.Trim().Length > 24, def.defName + " has missing loaded description.");
            ctx.Assert(!content.Contains(@"\n") && !content.Contains(@"\r"),
                def.defName + " has visible escaped newline text rather than paragraphs.");
            var sections = content.Split(new[] { "\n\n" },
                StringSplitOptions.None);
            ctx.Assert(sections.Length == paragraphs &&
                sections.All(s => !string.IsNullOrWhiteSpace(s)),
                def.defName + " expected " + paragraphs + " paragraphs, got " + sections.Length);
            if (japanese)
            {
                ctx.Assert(content.Any(c => c >= '\u3040' && c <= '\u9fff'),
                    def.defName + " does not contain Japanese characters.");
            }
            else
            {
                ctx.Assert(content.IndexOf("AMJGrains", StringComparison.Ordinal) >= 0 ||
                           def.defName == "AMJC_RiceSheaf" || def.defName == "AMJC_RiceInHull",
                    def.defName + " English description is not the approved owner text.");
            }
        }

        private static void CheckMaterial(PickleContext ctx, ThingDef def)
        {
            ctx.Require(def != null && def.graphicData != null,
                def == null ? "Missing graphics ThingDef" : def.defName + " missing graphicData");
            Graphic graphic = def.graphicData.Graphic;
            ctx.Require(graphic != null, def.defName + " failed to load its Graphic.");
            int[] amounts = { 1, 10, 75 };
            foreach (int count in amounts)
            {
                Graphic part = graphic is Graphic_StackCount stacks
                    ? stacks.SubGraphicForStackCount(count, def) : graphic;
                ctx.Require(part != null, def.defName + " missing graphic for count " + count);
                Material mat = part.MatSingle;
                ctx.Assert(mat != null && mat.mainTexture != null &&
                           mat.mainTexture != BaseContent.BadTex,
                    def.defName + " has BadTex/null main material at stack " + count);
            }
        }

        [Then("Grains loaded bilingual Info strings and production graphics resolve")]
        public async Task Verify(PickleContext ctx)
        {
            string target = Environment.GetEnvironmentVariable("RIMWORLD_GRAINS_UI_LANGUAGE");
            ctx.Require(target == "English" || target == "Japanese (日本語)",
                "Native UI test must select an explicit test language.");
            bool japanese = target != "English";
            await RuntimeThread.Run(delegate
            {
                string active = Prefs.LangFolderName;
                ctx.Assert(active == target, "Real game did not load requested language: "
                    + target + " actual: " + active);
                for (int i = 0; i < RiceDefs.Length; ++i)
                {
                    var def = DefDatabase<ThingDef>.GetNamed(RiceDefs[i]);
                    AssertText(ctx, def, japanese ? RiceJapanese[i] : RiceEnglish[i],
                        RiceParagraphs[i], japanese);
                }
                for (int i = 0; i < FoodDefs.Length; ++i)
                {
                    var def = DefDatabase<ThingDef>.GetNamed(FoodDefs[i]);
                    string label = japanese ? FoodJapanese[i] : FoodEnglish[i];
                    ctx.Assert(def.label == label, def.defName + " common food label differs: " + def.label);
                    ctx.Require(!string.IsNullOrWhiteSpace(def.description),
                        def.defName + " common food description is empty.");
                }
                foreach (string name in RenderedGrains.Concat(SpecialtyFoods))
                    CheckMaterial(ctx, DefDatabase<ThingDef>.GetNamed(name));
                foreach (string name in new[] {
                    "AMJC_Plant_FoxtailMillet_Awa", "AMJC_Plant_BarnyardMillet_Hie",
                    "AMJC_Plant_ProsoMillet_Kibi", "AMJC_Plant_Buckwheat_Soba",
                    "AMJC_Plant_Barley", "Plant_Rice" })
                {
                    var crop = DefDatabase<ThingDef>.GetNamed(name);
                    ctx.Require(crop.graphicData != null &&
                        crop.graphicData.Graphic != null,
                        name + " has a missing loaded plant graphic.");
                }
            });
            // Capture actual RimWorld InfoCard UI on the virtual display. Capture
            // is evidence for separate human visual QA, not automatic approval.
            string screenshotDir = Environment.GetEnvironmentVariable("RIMWORLD_GRAINS_UI_SHOTS");
            ctx.Require(!string.IsNullOrEmpty(screenshotDir) &&
                        Directory.Exists(screenshotDir),
                "Native screenshot destination not present.");
            for (int i = 0; i < RiceDefs.Length; ++i)
            {
                ThingDef def = null;
                Dialog_InfoCard card = null;
                string filename = Path.Combine(screenshotDir,
                    (japanese ? "ja-" : "en-") + RiceDefs[i] + ".png");
                int index = i;
                await RuntimeThread.Run(delegate
                {
                    def = DefDatabase<ThingDef>.GetNamed(RiceDefs[index]);
                    card = new Dialog_InfoCard(def);
                    Find.WindowStack.Add(card);
                });
                await Task.Delay(350);
                await RuntimeThread.Run(delegate { ScreenCapture.CaptureScreenshot(filename); });
                await Task.Delay(1200);
                await RuntimeThread.Run(delegate { card.Close(false); });
                ctx.Assert(File.Exists(filename) && new FileInfo(filename).Length > 1024,
                    "Native InfoCard screenshot missing or empty: " + filename);
            }
        }
    }
}
