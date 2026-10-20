using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorks.Quickstarts;
using RimWorks.Quickstarts.Verification;
using RimWorld;
using Verse;

namespace AncientMedievalJapanCore.E2E
{
    // Standalone, two-process save migration probe. First game has WorkRoles
    // but NO Grains; next game loads the same actual .rws with Grains enabled.
    // No player save or installed ModsConfig is ever touched.
    [StaticConstructorOnStartup]
    public static class WorkRolesOldSaveProbeBootstrap
    {
        static WorkRolesOldSaveProbeBootstrap()
        {
            new Harmony("sucro.amj.grains.workroles.saveprobe").PatchAll(
                typeof(WorkRolesOldSaveProbeBootstrap).Assembly);
        }
    }

    public static class WorkRolesOldSaveProbe
    {
        public const string SaveName = "AMJ_WROldSave_GrainsMigration";
        public const string CustomLabel = "AMJ_CustomFieldRole_SaveProbe";
        public const string NewType = "AMJC_GrainProcessing";
        public static bool Prepared;
        public static bool Saved;

        const BindingFlags Fields = BindingFlags.Instance | BindingFlags.Public |
                                     BindingFlags.NonPublic;
        const BindingFlags Methods = BindingFlags.Static | BindingFlags.Public |
                                      BindingFlags.NonPublic;

        public static Assembly OriginalAssembly()
        {
            return AppDomain.CurrentDomain.GetAssemblies().First(
                a => a.GetName().Name == "WorkRoles");
        }

        public static object Store()
        {
            var type = OriginalAssembly().GetType("WorkRoles.RoleStore", true);
            return type.GetProperty("Current").GetValue(null, null);
        }

        public static object Role(object store, string defName)
        {
            return store.GetType().GetMethod("RoleByTemplate").Invoke(
                store, new object[] { defName });
        }

        public static int Id(object role) =>
            (int)role.GetType().GetField("id", Fields).GetValue(role);

        public static string[] Entries(object role) =>
            ((IEnumerable)role.GetType().GetField("entries", Fields).GetValue(role))
                .Cast<object>()
                .Select(e => (string)e.GetType().GetMethod("Encode").Invoke(e, null))
                .ToArray();

        public static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        public static void MakeOldSave()
        {
            Require(DefDatabase<WorkTypeDef>.GetNamedSilentFail(NewType) == null,
                "Grains must be ABSENT in old-save phase.");
            object store = Store();
            Require(store != null, "WorkRoles store missing during old-save preparation.");
            if (!(bool)store.GetType().GetField("seeded").GetValue(store))
                OriginalAssembly().GetType("WorkRoles.Seeding", true)
                    .GetMethod("SeedIfNeeded").Invoke(null, null);
            object grower = Role(store, "WS_Grower");
            object farmer = Role(store, "WS_Farmer");
            Require(grower != null && farmer != null, "Original Farmer/Grower not seeded.");
            var commands = OriginalAssembly().GetType("WorkRoles.RoleCommands", true);
            int cut = Array.IndexOf(Entries(farmer), "WorkType:PlantCutting");
            Require(cut >= 0, "Original Farmer has no cut work to customize.");
            commands.GetMethod("RemoveEntry", Methods).Invoke(
                null, new object[] { Id(farmer), cut });
            Require(!Entries(farmer).Contains("WorkType:PlantCutting"),
                "User-edited Farmer cut-work removal must persist.");
            var create = commands.GetMethod("CreateRoleDirect", Methods);
            object custom = create.Invoke(null, new object[] { CustomLabel, false });
            Require(custom != null, "Cannot make custom player role.");
            var core = OriginalAssembly().GetReferencedAssemblies()
                .Select(a => AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(loaded => loaded.GetName().Name == a.Name))
                .First(a => a != null && a.GetType("WorkRoles.Core.JobEntry", false) != null);
            var kind = core.GetType("WorkRoles.Core.JobEntryKind", true);
            var entryType = core.GetType("WorkRoles.Core.JobEntry", true);
            object entry = Activator.CreateInstance(entryType,
                new object[] { Enum.Parse(kind, "WorkType"), "Growing" });
            commands.GetMethod("AddEntryDirect", Methods).Invoke(
                null, new object[] { Id(custom), entry, -1 });
            Pawn pawn = Find.CurrentMap.mapPawns.FreeColonistsSpawned.FirstOrDefault();
            Require(pawn != null, "A real serialized colonist is required.");
            commands.GetMethod("AssignRole", Methods).Invoke(
                null, new object[] { pawn, Id(custom), 0 });
            Require(Entries(custom).SequenceEqual(new[] { "WorkType:Growing" }),
                "Saved custom role must be an ordinary player role.");
            var known = (System.Collections.Generic.List<string>)store.GetType()
                .GetField("knownWorkTypes").GetValue(store);
            Require(!known.Contains(NewType), "The old game must not know Grains work.");
            Prepared = true;
            GameDataSaveLoader.SaveGame(SaveName);
            var saved = typeof(GenFilePaths).GetMethod("FilePathForSavedGame", Methods);
            string path = (string)saved.Invoke(null, new object[] { SaveName });
            Saved = System.IO.File.Exists(path) && new System.IO.FileInfo(path).Length > 1024;
            Log.Message("AMJ_WR_SAVE_CREATED prepared=" + Prepared +
                " saved=" + Saved + " colonist=" + pawn.ThingID);
        }

        public static bool VerifyLoaded()
        {
            object store = Store();
            Require(store != null && (bool)store.GetType().GetField("seeded").GetValue(store),
                "Loaded WorkRoles store was not seeded.");
            Require(DefDatabase<WorkTypeDef>.GetNamedSilentFail(NewType) != null,
                "Grains new WorkType did not load.");
            var known = (System.Collections.Generic.List<string>)store.GetType()
                .GetField("knownWorkTypes").GetValue(store);
            Require(known.Contains(NewType), "WorkRoles failed new-type migration after reload.");
            object farmer = Role(store, "WS_Farmer"), grower = Role(store, "WS_Grower");
            Require(farmer != null && grower != null, "Original stock roles lost after reload.");
            var farmerEntries = Entries(farmer);
            var growerEntries = Entries(grower);
            Require(farmerEntries.Contains("WorkType:" + NewType)
                && growerEntries.Contains("WorkType:" + NewType),
                "New Grains work was not added to original roles after reload.");
            Require(!farmerEntries.Contains("WorkType:PlantCutting"),
                "User-edited Farmer entry was overwritten by migration.");
            Require(Array.IndexOf(growerEntries, "WorkType:Growing")
                < Array.IndexOf(growerEntries, "WorkType:" + NewType),
                "Grower priority order changed on migration.");
            var roles = (IEnumerable)store.GetType().GetField("roles").GetValue(store);
            object custom = roles.Cast<object>().SingleOrDefault(role =>
                (string)role.GetType().GetField("label", Fields).GetValue(role) == CustomLabel);
            Require(custom != null && Entries(custom).SequenceEqual(new[] { "WorkType:Growing" }),
                "The customized role must remain unchanged after Scribe reload.");
            int id = Id(custom);
            var pawnSets = (IDictionary)store.GetType().GetField("pawnSets").GetValue(store);
            Require(pawnSets.Count > 0, "No pawn assignment survived Scribe reload.");
            bool assigned = false;
            foreach (DictionaryEntry item in pawnSets)
            {
                if (!(item.Key is Pawn pawn) || pawn.Destroyed) continue;
                var list = (IEnumerable)item.Value.GetType().GetField("assignments").GetValue(item.Value);
                if (list.Cast<object>().Any(a =>
                    (int)a.GetType().GetField("roleId").GetValue(a) == id))
                    assigned = true;
            }
            Require(assigned, "Custom role lost its real colonist assignment.");
            return true;
        }
    }

    public sealed class AMJWorkRolesOldSaveQuickstart : AbstractQuickstart
    {
        public override TaggedString description => "Pre-Grains WorkRoles saved game migration probe";
        public override int mapSize => 50;
        public override string seed => "AMJ-WR-Save-Migration";
        public override void PostLoaded() { WorkRolesOldSaveProbe.MakeOldSave(); }
        public override QuickstartVerification Verify()
        {
            var checks = new QuickstartVerification();
            checks.Assert("old save written with customized WorkRoles role",
                () => WorkRolesOldSaveProbe.Prepared && WorkRolesOldSaveProbe.Saved);
            return checks;
        }
    }

    // Real entry UI must initialize before normal load-game scene transition.
    [HarmonyPatch(typeof(Root_Entry), "Update")]
    public static class WorkRolesOldSaveEntry
    {
        private static int frames;
        private static bool queued;
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (queued || Environment.GetEnvironmentVariable("AMJ_WR_SAVE_RELOAD") != "1"
                || ++frames < 180) return;
            queued = true;
            Log.Message("AMJ_WR_RELOAD_BEGIN original_game_save=true");
            LongEventHandler.QueueLongEvent(() =>
            {
                Current.Game = new Game {
                    InitData = new GameInitData { gameToLoad = WorkRolesOldSaveProbe.SaveName }
                };
            }, "Play", "LoadingLongEvent", true, null);
        }
    }

    [HarmonyPatch(typeof(Game), "LoadGame")]
    public static class WorkRolesOldSaveLoadMarker
    {
        public static bool Loaded;
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Environment.GetEnvironmentVariable("AMJ_WR_SAVE_RELOAD") == "1")
                Loaded = true;
        }
    }

    [HarmonyPatch(typeof(Root_Play), "Update")]
    public static class WorkRolesOldSaveVerify
    {
        private static int frames;
        private static bool finished;
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (finished || !WorkRolesOldSaveLoadMarker.Loaded ||
                Environment.GetEnvironmentVariable("AMJ_WR_SAVE_RELOAD") != "1" ||
                ++frames < 180) return;
            finished = true;
            try
            {
                bool passed = WorkRolesOldSaveProbe.VerifyLoaded();
                Log.Message("AMJ_WR_RELOAD_RESULT passed=" + passed +
                    " actualScribeGameReload=true customRolePreserved=true");
            }
            catch (Exception error)
            {
                Log.Error("AMJ_WR_RELOAD_RESULT passed=False " + error);
            }
            UnityEngine.Application.Quit();
        }
    }
}
