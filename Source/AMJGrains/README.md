# MO thresh TargetCount production assembly

Build only against an authorized RimWorld 1.6 game's existing managed files (never commit or redistribute them):

```sh
dotnet build Source/AMJGrains/AMJGrains.csproj -c Release \
  -p:RimWorldManagedDir=/path/to/RimWorldLinux_Data/Managed \
  -o /tmp/amj-grains-compiled
```

The resulting `AMJGrains.dll` must be verified and included as the tracked `Assemblies/AMJGrains.dll` **before merging** the RecipeDef references. Preserve Base/Vanilla behavior and the existing MO Straw output. Require actual loaded-game TargetCount and `DoBill`, complete ERROR0 profiles and the exact production Workshop payload. Source-only work is not a completed release.

## Draft crop comparison UI (not yet shipped)

`CropComparisonUI.cs` adds a read-only, bilingual (EN/JA) food-crop throughput window when a single `Zone_Growing` is selected. It compares the actual zone fertility cells and *current* temperature at constant assumed ideal light, with 15/30/60 **calendar** day snapshots and an explicit warning that weather, player work, disease and time-varying DBH bonuses are not forecast. It does not replace Nice Plants Menu or patch its binary. Only food crops are compared: non-food cotton/hops/smokeleaf count is not comparable to food quantity.

**Build gate:** This PR adds only C# source, `csproj` inclusion, two keyed translations and owning design. The existing tracked `Assemblies/AMJGrains.dll` is **unchanged**. Consequently, the feature is not available in game until the exact 1.6 managed build is produced, inspected, deployed as that assembly and acceptance-tested. Do not merge this PR as if it shipped a working UI; do not claim runtime PASS from static checks. Compiling against an actual authorized RimWorld 1.6 managed directory and checking the new loaded `GameComponent` must precede the production merge. If runtime integration is not possible, defer the feature rather than ship source-only claims.
