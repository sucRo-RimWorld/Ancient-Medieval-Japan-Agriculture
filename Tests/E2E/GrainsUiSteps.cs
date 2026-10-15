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



        // Distinct from fontNames reassignment: create a new temporary Unity
        // Font object from the native installed Japanese face, then attach the
        // object to the real Verse GUIStyles and fonts[] in this process only.
        private static string ApplyExplicitTestFontToGuiStyles(string requested)
        {
            if (Prefs.LangFolderName != "Japanese (日本語)" ||
                requested != "Noto Sans JP")
                throw new InvalidOperationException("Diagnostic font object: invalid locale/family");
            Type fontType = LoadedType("UnityEngine.Font");
            if (fontType == null)
                throw new InvalidOperationException("No loaded Unity.Font API");
            var creator = fontType.GetMethod("CreateDynamicFontFromOSFont",
                new[] { typeof(string), typeof(int) });
            if (creator == null)
                throw new InvalidOperationException("Unity.Font.CreateDynamicFontFromOSFont unavailable");
            object obj = creator.Invoke(null, new object[] { requested, 20 });
            UnityEngine.Object newFont = obj as UnityEngine.Object;
            if (newFont == null)
                throw new InvalidOperationException("Native Unity could not create Japanese font object");
            newFont.name = "AMJ_Test_NotoSansJP";

            var fields = typeof(Verse.Text).GetFields(
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            int assigned = 0;
            var report = new StringBuilder();
            report.AppendLine("font_object_requested=" + requested);
            report.AppendLine("font_object_actual=" + newFont.name);
            report.AppendLine("font_object_fontNames=" +
                string.Join(",", fontType.GetProperty("fontNames").GetValue(obj, null) as string[]
                          ?? new string[0]));
            foreach (var field in fields)
            {
                if (field.Name != "fonts" && field.Name != "fontStyles" &&
                    field.Name != "textFieldStyles" && field.Name != "textAreaStyles" &&
                    field.Name != "textAreaReadOnlyStyles")
                    continue;
                var value = field.GetValue(null) as Array;
                if (value == null || value.Length != 3)
                    throw new InvalidOperationException("Original Verse field not three fonts: " +
                        field.Name);
                for (int i = 0; i < value.Length; ++i)
                {
                    if (field.Name == "fonts")
                    {
                        value.SetValue(obj, i);
                    }
                    else
                    {
                        object style = value.GetValue(i);
                        var property = style.GetType().GetProperty("font");
                        if (property == null || !property.CanWrite)
                            throw new InvalidOperationException("Verse GUIStyle font not writable");
                        property.SetValue(style, obj, null);
                        if (!System.Object.ReferenceEquals(property.GetValue(style, null), obj))
                            throw new InvalidOperationException("Verse font assignment was ignored");
                    }
                    ++assigned;
                }
                report.AppendLine("assigned_verse_field=" + field.Name + " count=3");
            }
            report.AppendLine("assigned_verse_font_slots=" + assigned);
            if (assigned != 15)
                throw new InvalidOperationException("Expected exactly 15 original Verse font slots");
            return report.ToString();
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
                    unity.name != "Arial_medium" && unity.name != "Calibri_tiny" &&
                    unity.name != "AMJ_Test_NotoSansJP"))
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


        // Nonshipping diagnostic only: discover whether the real Unity legacy
        // font atlas can materialize an individual Japanese glyph. Do not infer
        // glyph availability from dynamic Font.HasCharacter(), which can always
        // report true even when the atlas lacks the character.
        private static string CaptureGlyphAtlasEvidence()
        {
            var evidence = new StringBuilder();
            evidence.AppendLine("probe=real-rimworld-legacy-unity-glyph-atlas-v1");
            evidence.AppendLine("language=" + Prefs.LangFolderName);
            Type fontType = LoadedType("UnityEngine.Font");
            Type infoType = LoadedType("UnityEngine.CharacterInfo");
            Type styleType = LoadedType("UnityEngine.FontStyle");
            if (fontType == null || infoType == null || styleType == null)
                throw new InvalidOperationException("Unity legacy font atlas API types unavailable");
            object normal = Enum.Parse(styleType, "Normal");
            Type[] charArgs = { typeof(char), infoType.MakeByRefType(),
                                typeof(int), styleType };
            var getInfo = fontType.GetMethod("GetCharacterInfo", charArgs);
            var request = fontType.GetMethod("RequestCharactersInTexture",
                new[] { typeof(string), typeof(int), styleType });
            if (getInfo == null || request == null)
                throw new InvalidOperationException("Unity glyph atlas API signatures unavailable");

            var fontList = (typeof(Resources).GetMethod("FindObjectsOfTypeAll",
                new[] { typeof(Type) })).Invoke(null, new object[] { fontType }) as Array;
            if (fontList == null)
                throw new InvalidOperationException("Unity loaded font inventory unavailable");
            int probed = 0;
            foreach (object value in fontList)
            {
                UnityEngine.Object unityFont = value as UnityEngine.Object;
                if (unityFont == null || (unityFont.name != "Calibri_tiny" &&
                    unityFont.name != "Arial_small" && unityFont.name != "Arial_medium"))
                    continue;
                ++probed;
                evidence.AppendLine("font_begin=" + unityFont.name);
                var dyn = fontType.GetProperty("dynamic");
                evidence.AppendLine("dynamic=" +
                    (dyn == null ? "UNAVAILABLE" : dyn.GetValue(value, null).ToString()));
                var names = fontType.GetProperty("fontNames");
                var families = names == null ? null :
                    names.GetValue(value, null) as string[];
                evidence.AppendLine("families=" +
                    (families == null ? "UNAVAILABLE" : string.Join(",", families)));
                try
                {
                    var material = fontType.GetProperty("material")?.GetValue(value, null)
                        as Material;
                    var atlas = material == null ? null : material.mainTexture;
                    evidence.AppendLine("atlas=" + (atlas == null ? "<null>" :
                        atlas.width + "x" + atlas.height + " type=" +
                        atlas.GetType().Name));
                    evidence.AppendLine("shader=" + (material == null ||
                        material.shader == null ? "<null>" : material.shader.name));
                }
                catch (Exception ex)
                {
                    evidence.AppendLine("atlas_metadata_error=" + ex.GetType().Name +
                        ":" + ex.Message);
                }

                foreach (int size in new[] { 16, 22 })
                {
                    foreach (string phase in new[] { "before", "after" })
                    {
                        if (phase == "after")
                        {
                            // A/B within a disposable runtime only; records if the
                            // glyph atlas was actually populated after a request.
                            try
                            {
                                request.Invoke(value, new object[] { "A稲あ中", size, normal });
                                evidence.AppendLine("request=ok font=" + unityFont.name +
                                    " size=" + size);
                            }
                            catch (Exception ex)
                            {
                                evidence.AppendLine("request=error font=" + unityFont.name +
                                    " size=" + size + " type=" + ex.GetType().Name +
                                    " details=" + ex.Message);
                            }
                        }
                        foreach (char c in new[] { 'A', '稲', 'あ', '中' })
                        {
                            try
                            {
                                var args = new object[] { c,
                                    Activator.CreateInstance(infoType), size, normal };
                                bool available = (bool)getInfo.Invoke(value, args);
                                var adv = infoType.GetField("advance");
                                string metric = available && adv != null ?
                                    Convert.ToString(adv.GetValue(args[1])) : "NA";
                                evidence.AppendLine("glyph=" + unityFont.name +
                                    " size=" + size + " phase=" + phase +
                                    " U+" + ((int)c).ToString("X4") +
                                    " available=" + available + " advance=" + metric);
                            }
                            catch (Exception ex)
                            {
                                evidence.AppendLine("glyph_error=" + unityFont.name +
                                    " U+" + ((int)c).ToString("X4") + " size=" +
                                    size + " phase=" + phase + " type=" +
                                    ex.GetType().Name + " details=" + ex.Message);
                            }
                        }
                    }
                }
                evidence.AppendLine("font_end=" + unityFont.name);
            }
            evidence.AppendLine("glyph_atlas_probe_count=" + probed);
            if (probed != 3)
                throw new InvalidOperationException("Expected three active legacy fonts, got " + probed);
            return evidence.ToString();
        }


        // Read-only shape diagnosis for generated Unity IMGUI text. We do not
        // install fonts, alter production GUIStyle, or interpret glyph count as
        // pixel/visual approval. TextGenerator is loaded by reflection because
        // Unity splits text APIs into optional managed modules by game version.
        private static string CaptureTextGeometryEvidence()
        {
            var b = new StringBuilder();
            b.AppendLine("probe=real-unity-textgenerator-legacy-imgui-v1");
            b.AppendLine("language=" + Prefs.LangFolderName);
            Type generator = LoadedType("UnityEngine.TextGenerator");
            Type settingsType = LoadedType("UnityEngine.TextGenerationSettings");
            Type fontType = LoadedType("UnityEngine.Font");
            b.AppendLine("generator_type=" + (generator == null ? "MISSING" : generator.AssemblyQualifiedName));
            b.AppendLine("settings_type=" + (settingsType == null ? "MISSING" : settingsType.AssemblyQualifiedName));
            if (generator == null || settingsType == null || fontType == null)
                return b.AppendLine("probe_status=API_UNAVAILABLE").ToString();
            var populate = generator.GetMethod("Populate", new[] { typeof(string), settingsType });
            var ctor = generator.GetConstructor(new[] { typeof(int) }) ??
                       generator.GetConstructor(Type.EmptyTypes);
            if (populate == null || ctor == null)
                return b.AppendLine("probe_status=SIGNATURE_UNAVAILABLE").ToString();
            var objects = typeof(Resources).GetMethod("FindObjectsOfTypeAll",
                new[] { typeof(Type) }).Invoke(null, new object[] { fontType }) as Array;
            if (objects == null)
                return b.AppendLine("probe_status=FONT_INVENTORY_UNAVAILABLE").ToString();
            int found = 0;
            foreach (object font in objects)
            {
                var unity = font as UnityEngine.Object;
                if (unity == null || (unity.name != "Arial_small" &&
                    unity.name != "Arial_medium" && unity.name != "Calibri_tiny"))
                    continue;
                ++found;
                b.AppendLine("font=" + unity.name);
                foreach (string sample in new[] { "A", "稲", "あ", "中", "A稲あ中" })
                {
                    string sampleId = string.Join("_", sample.Select(
                        c => ((int)c).ToString("X4")).ToArray());
                    try
                    {
                        object settings = Activator.CreateInstance(settingsType);
                        Action<string, object> field = (name, val) =>
                        {
                            FieldInfo fi = settingsType.GetField(name);
                            if (fi != null) fi.SetValue(settings, val);
                        };
                        field("font", font);
                        field("fontSize", 20);
                        field("color", Color.white);
                        field("fontStyle", Enum.Parse(LoadedType("UnityEngine.FontStyle"), "Normal"));
                        field("textAnchor", Enum.Parse(LoadedType("UnityEngine.TextAnchor"), "UpperLeft"));
                        field("horizontalOverflow", Enum.Parse(LoadedType("UnityEngine.HorizontalWrapMode"), "Overflow"));
                        field("verticalOverflow", Enum.Parse(LoadedType("UnityEngine.VerticalWrapMode"), "Overflow"));
                        field("scaleFactor", 1f);
                        field("lineSpacing", 1f);
                        field("generationExtents", new Vector2(700f, 350f));
                        field("pivot", new Vector2(0f, 1f));
                        field("richText", false);
                        field("resizeTextForBestFit", false);
                        object instance = ctor.GetParameters().Length == 1 ?
                            ctor.Invoke(new object[] { 256 }) : ctor.Invoke(new object[0]);
                        bool ok = (bool)populate.Invoke(instance, new[] { (object)sample, settings });
                        var vertices = generator.GetProperty("vertexCount");
                        var characters = generator.GetProperty("characterCountVisible");
                        var verts = generator.GetProperty("verts");
                        object vertexCount = vertices == null ? null : vertices.GetValue(instance, null);
                        object characterCount = characters == null ? null : characters.GetValue(instance, null);
                        object list = verts == null ? null : verts.GetValue(instance, null);
                        var countProp = list == null ? null : list.GetType().GetProperty("Count");
                        object actualCount = countProp == null ? null : countProp.GetValue(list, null);
                        b.AppendLine("generated=" + unity.name + " sample=" + sampleId +
                            " populate=" + ok + " vertex_count=" +
                            (vertexCount ?? "UNAVAILABLE") + " char_count=" +
                            (characterCount ?? "UNAVAILABLE") + " verts_count=" +
                            (actualCount ?? "UNAVAILABLE"));
                        // Capture the first glyph quad's actual coordinates, UVs
                        // and color alpha, not just its vertex count. This can
                        // discriminate degenerate/missing atlas geometry from
                        // nonzero triangles without exporting any texture data.
                        if (sample.Length == 1 && list is System.Collections.IEnumerable)
                        {
                            int vertexIndex = 0;
                            foreach (object vertex in (System.Collections.IEnumerable)list)
                            {
                                if (vertexIndex >= 4) break;
                                var vertexType = vertex.GetType();
                                Func<string, string> attr = name =>
                                {
                                    var member = vertexType.GetField(name);
                                    object val = member == null ? null : member.GetValue(vertex);
                                    return val == null ? "UNAVAILABLE" : val.ToString();
                                };
                                b.AppendLine("quad=" + unity.name + " sample=" + sampleId +
                                    " v=" + (vertexIndex++) + " position=" + attr("position") +
                                    " uv0=" + attr("uv0") + " color=" + attr("color"));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        b.AppendLine("generation_error=" + unity.name + " sample=" + sampleId +
                            " type=" + ex.GetType().Name + " message=" +
                            (ex.InnerException == null ? ex.Message : ex.InnerException.Message));
                    }
                }
            }
            b.AppendLine("text_generator_font_count=" + found);
            bool testFontInjected = Environment.GetEnvironmentVariable("RIMWORLD_GRAINS_UI_DIAGNOSTIC_FONT_OBJECT") == "Noto Sans JP";
            b.AppendLine("probe_status=" + (found == (testFontInjected ? 4 : 3) ? "COLLECTED" : "WRONG_FONT_COUNT"));
            return b.ToString();
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
                    string fontObject = Environment.GetEnvironmentVariable(
                        "RIMWORLD_GRAINS_UI_DIAGNOSTIC_FONT_OBJECT");
                    if (!string.IsNullOrEmpty(fontObject))
                    {
                        if (!string.IsNullOrEmpty(forceFamily))
                            throw new InvalidOperationException(
                                "Font object and fontNames A/B cannot run together");
                        string recordPath = Environment.GetEnvironmentVariable(
                            "RIMWORLD_GRAINS_UI_FONT_OBJECT_REPORT");
                        ctx.Require(!string.IsNullOrWhiteSpace(recordPath),
                            "Diagnostic active font assignment requires exact report path");
                        await RuntimeThread.Run(delegate
                        {
                            File.WriteAllText(recordPath,
                                ApplyExplicitTestFontToGuiStyles(fontObject), Encoding.UTF8);
                        });
                        ctx.Assert(File.Exists(recordPath) &&
                            new FileInfo(recordPath).Length > 150,
                            "New Japanese GUI font object was not verified");
                        await Task.Delay(650);
                    }
                    else if (!string.IsNullOrEmpty(forceFamily))
                    {
                        await RuntimeThread.Run(delegate
                        {
                            ApplyDiagnosticFontNames(forceFamily);
                        });
                        await Task.Delay(500);
                    }
                    string geometryPath = Environment.GetEnvironmentVariable(
                        "RIMWORLD_GRAINS_UI_TEXT_GEOMETRY_REPORT");
                    if (!string.IsNullOrEmpty(geometryPath))
                    {
                        await RuntimeThread.Run(delegate
                        {
                            File.WriteAllText(geometryPath,
                                CaptureTextGeometryEvidence(), Encoding.UTF8);
                        });
                        ctx.Assert(File.Exists(geometryPath) &&
                            new FileInfo(geometryPath).Length > 200,
                            "Unity text geometry report missing or empty.");
                    }
                    string glyphsPath = Environment.GetEnvironmentVariable(
                        "RIMWORLD_GRAINS_UI_GLYPH_REPORT");
                    if (!string.IsNullOrEmpty(glyphsPath))
                    {
                        await RuntimeThread.Run(delegate
                        {
                            File.WriteAllText(glyphsPath,
                                CaptureGlyphAtlasEvidence(), Encoding.UTF8);
                        });
                        ctx.Assert(File.Exists(glyphsPath) &&
                            new FileInfo(glyphsPath).Length > 500,
                            "Native Unity glyph atlas report missing or empty.");
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
