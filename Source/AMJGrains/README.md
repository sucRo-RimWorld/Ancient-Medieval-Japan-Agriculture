# MO thresh TargetCount production assembly

Build only against an authorized RimWorld 1.6 game's existing managed files (never commit or redistribute them):

```sh
dotnet build Source/AMJGrains/AMJGrains.csproj -c Release \
  -p:RimWorldManagedDir=/path/to/RimWorldLinux_Data/Managed \
  -o /tmp/amj-grains-compiled
```

The resulting `AMJGrains.dll` must be verified and included as the tracked `Assemblies/AMJGrains.dll` **before merging** the RecipeDef references. Preserve Base/Vanilla behavior and the existing MO Straw output. Require actual loaded-game TargetCount and `DoBill`, complete ERROR0 profiles and the exact production Workshop payload. Source-only work is not a completed release.
