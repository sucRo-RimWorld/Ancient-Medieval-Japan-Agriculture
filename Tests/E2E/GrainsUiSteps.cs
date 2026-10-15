using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;
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
            "稲", "稲束", "籾", "米(生)"
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


        // Diagnostic evidence from the actual Unity UI process, not from the host's
        // fontconfig inventory. This runs only in the disposable test target.
        private static Type LoadedType(string fullname)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(fullname, false);
                if (type != null)
                    return type;
            }
            return null;
        }

        private static string DescribeFont(object font, Type fontType)
        {
            if (font == null || fontType == null)
                return "<null>";
            var b = new StringBuilder();
            var unityObject = font as UnityEngine.Object;
            b.Append(unityObject == null ? fontType.FullName : unityObject.name);
            try
            {
                var names = fontType.GetProperty("fontNames");
                var values = names == null ? null : names.GetValue(font, null) as string[];
                if (values != null)
                    b.Append(" families=").Append(string.Join(",", values));
            }
            catch (Exception ex) { b.Append(" families_error=").Append(ex.GetType().Name); }

            MethodInfo hasCharacter = fontType.GetMethod("HasCharacter", new[] { typeof(char) });
            if (hasCharacter == null)
                return b.Append(" HasCharacter=unavailable").ToString();
            foreach (char c in new[] { 'A', '稲', '籾', '米', '飯', 'あ', '漢', '中', '国' })
            {
                try
                {
                    b.Append(" U+").Append(((int)c).ToString("X4")).Append('=')
                     .Append(hasCharacter.Invoke(font, new object[] { c }));
                }
                catch (Exception ex)
                {
                    b.Append(" U+").Append(((int)c).ToString("X4")).Append("_error=")
                     .Append(ex.GetType().Name);
                }
            }
            return b.ToString();
        }


        // Disposable rendering A/B only. The author game, production Grains, and
        // all non-Japanese UI profiles retain their original fontNames.
        private static int ApplyDiagnosticFontNames(string family)
        {
            if (string.IsNullOrWhiteSpace(family))
                return 0;
            if (Prefs.LangFolderName != "Japanese (日本語)" || family != "Noto Sans JP")
                throw new InvalidOperationException("Unapproved native font experiment locale/family");
            Type fontType = LoadedType("UnityEngine.Font");
            if (fontType == null)
                throw new InvalidOperationException("Unity Font type is unavailable");
            var method = typeof(Resources).GetMethod("FindObjectsOfTypeAll",
                new[] { typeof(Type) });
            var names = fontType.GetProperty("fontNames");
            if (method == null || names == null || !names.CanWrite)
                throw new InvalidOperationException("Unity dynamic fontNames setter is unavailable");
            var objects = method.Invoke(null, new object[] { fontType }) as Array;
            if (objects == null)
                throw new InvalidOperationException("Loaded Unity Font inventory unavailable");
            int changed = 0;
            foreach (object value in objects)
            {
                var unity = value as UnityEngine.Object;
                if (unity == null || (unity.name != "Arial_small" &&
                    unity.name != "Arial_medium" && unity.name != "Calibri_tiny"))
                    continue;
                var previous = names.GetValue(value, null) as string[];
                var updated = new List<string> { family };
                if (previous != null)
                    foreach (string item in previous)
                        if (!string.IsNullOrEmpty(item) && !updated.Contains(item))
                            updated.Add(item);
                names.SetValue(value, updated.ToArray(), null);
                var observed = names.GetValue(value, null) as string[];
                if (observed == null || observed.Length == 0 || observed[0] != family)
                    throw new InvalidOperationException("Unity ignored selected diagnostic fallback");
                changed++;
            }
            if (changed != 3)
                throw new InvalidOperationException("Expected three original Verse UI fonts, got " + changed);
            return changed;
        }

        private static string CaptureRuntimeFontEvidence()
        {
            var b = new StringBuilder();
            b.AppendLine("probe=real-rimworld-native-Unity-font-after-InfoCard-render");
            b.AppendLine("language=" + Prefs.LangFolderName);
            b.AppendLine("diagnostic_font_override=" +
                (Environment.GetEnvironmentVariable("RIMWORLD_GRAINS_UI_DIAGNOSTIC_FONT_FAMILY") ?? "NONE"));
            Type fontType = LoadedType("UnityEngine.Font");
            b.AppendLine("unity_font_type=" + (fontType == null ? "UNAVAILABLE" : fontType.AssemblyQualifiedName));
            if (fontType != null)
            {
                try
                {
                    var method = typeof(Resources).GetMethod("FindObjectsOfTypeAll",
                        new[] { typeof(Type) });
                    var fonts = method == null ? null :
                        method.Invoke(null, new object[] { fontType }) as Array;
                    b.AppendLine("loaded_unity_font_count=" +
                        (fonts == null ? "UNAVAILABLE" : fonts.Length.ToString()));
                    if (fonts != null)
                    {
                        int count = 0;
                        foreach (object font in fonts)
                        {
                            if (count++ >= 96) { b.AppendLine("fonts_truncated=96"); break; }
                            b.AppendLine("loaded_font=" + DescribeFont(font, fontType));
                        }
                    }
                }
                catch (Exception ex)
                {
                    b.AppendLine("font_inventory_error=" + ex.ToString());
                }
            }

            // The actual info window uses Verse.Text / Unity IMGUI, not fontconfig
            // directly. Identify the selected skin and style fonts separately from
            // the (possibly unused) loaded Unity.Font inventory above.
            try
            {
                Type gui = LoadedType("UnityEngine.GUI");
                var skinProperty = gui == null ? null : gui.GetProperty("skin",
                    BindingFlags.Public | BindingFlags.Static);
                object skin = skinProperty == null ? null : skinProperty.GetValue(null, null);
                b.AppendLine("unity_gui_skin=" + (skin == null ? "UNAVAILABLE" : skin.ToString()));
                if (skin != null)
                {
                    foreach (string styleName in new[] { "label", "box", "button" })
                    {
                        var prop = skin.GetType().GetProperty(styleName);
                        object style = prop == null ? null : prop.GetValue(skin, null);
                        object font = style == null ? null :
                            style.GetType().GetProperty("font")?.GetValue(style, null);
                        b.AppendLine("unity_gui_" + styleName + "_font=" +
                            DescribeFont(font, fontType));
                    }
                    object defaultFont = skin.GetType().GetProperty("font")?.GetValue(skin, null);
                    b.AppendLine("unity_gui_skin_font=" + DescribeFont(defaultFont, fontType));
                }
            }
            catch (Exception ex) { b.AppendLine("gui_style_error=" + ex.ToString()); }

            foreach (var member in typeof(Verse.Text).GetFields(
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (member.Name.IndexOf("font", StringComparison.OrdinalIgnoreCase) < 0 &&
                    member.Name.IndexOf("style", StringComparison.OrdinalIgnoreCase) < 0)
                    continue;
                try
                {
                    object value = member.GetValue(null);
                    b.AppendLine("verse_text_field=" + member.Name + " type=" +
                        member.FieldType.FullName + " value=" +
                        (value == null ? "<null>" : value.ToString()));
                    var array = value as Array;
                    if (array != null)
                    {
                        int index = 0;
                        foreach (object style in array)
                        {
                            if (index >= 20) break;
                            object font = style == null ? null :
                                style.GetType().GetProperty("font")?.GetValue(style, null);
                            b.AppendLine("verse_text_style_" + (index++) + "_font=" +
                                DescribeFont(font, fontType));
                        }
                    }
                }
                catch (Exception ex)
                {
                    b.AppendLine("verse_text_field_error=" + member.Name + ":" +
                        ex.GetType().Name);
                }
            }
            return b.ToString();
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
                if (index == 0)
                {
                    string forceFamily = Environment.GetEnvironmentVariable(
                        "RIMWORLD_GRAINS_UI_DIAGNOSTIC_FONT_FAMILY");
                    if (!string.IsNullOrEmpty(forceFamily))
                    {
                        await RuntimeThread.Run(delegate
                        {
                            ApplyDiagnosticFontNames(forceFamily);
                        });
                        // Let the original IMGUI InfoCard render additional frames
                        // before taking screenshots; no owner/game font is persisted.
                        await Task.Delay(500);
                    }
                    string fontsPath = Environment.GetEnvironmentVariable(
                        "RIMWORLD_GRAINS_UI_FONT_REPORT");
                    if (!string.IsNullOrEmpty(fontsPath))
                    {
                        await RuntimeThread.Run(delegate
                        {
                            File.WriteAllText(fontsPath,
                                CaptureRuntimeFontEvidence(), Encoding.UTF8);
                        });
                        ctx.Assert(File.Exists(fontsPath) &&
                            new FileInfo(fontsPath).Length > 256,
                            "Native Unity font inventory missing or empty.");
                    }
                }
                await RuntimeThread.Run(delegate
                {
                    // Unity puts ScreenCapture in an optional managed module;
                    // resolve it from the installed game's actual assemblies.
                    var screenType = Type.GetType(
                        "UnityEngine.ScreenCapture, UnityEngine.ScreenCaptureModule", true);
                    var capture = screenType.GetMethod(
                        "CaptureScreenshot", new[] { typeof(string) });
                    ctx.Require(capture != null,
                        "Native Unity screenshot API is unavailable.");
                    capture.Invoke(null, new object[] { filename });
                });
                await Task.Delay(1200);
                await RuntimeThread.Run(delegate { card.Close(false); });
                ctx.Assert(File.Exists(filename) && new FileInfo(filename).Length > 1024,
                    "Native InfoCard screenshot missing or empty: " + filename);
            }
        }
    }
}
