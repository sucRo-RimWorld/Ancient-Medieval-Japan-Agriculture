# AMJ Agriculture Agent Instructions

## Start here

1. Read this file and `main:Docs/Coordination.md`; locate the latest relevant owner/status/evidence, including later corrections. Historical entries are not current approval.
2. Read Project [AGENTS.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/AGENTS.md) and [Docs/SharedRules.md](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/SharedRules.md): apply its stop conditions, then open only the task-relevant canonical procedures.
3. Read the local specification and affected source/tests below. Shared rules are owned by Project; this file owns only local scope and routing. Missing access or conflicting authority blocks the dependent action, not unrelated safe work.

New features cannot enter implementation before the Project [existing-Mod audit gate](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/blob/main/Docs/Research/ExistingModAudit.md#implementation-entry-gate) covers VE and non-VE alternatives and records why independent implementation is needed. Existing approved behavior is not redesigned by this rule audit.

## Dining ownership

Meal assembly/serving runtime, tray/job Defs, translations moved to the independent AMJ Dining repository. Agriculture requires Dining and owns only its food-role mappings and cooking policy. See `Docs/Dining.md`; do not reintroduce the legacy dining DLL here.

## Local task routes and stops

- `Docs/Design.md` owns Agriculture's current MO-required grain/processing design. `Docs/GrainsProfileTesting.md` retains actual-provider runtime evidence, including historical Vanilla/MO matrices. `Docs/GrainsDependencyAudit.md` records the **superseded 2026-10-10 MO dependency-removal audit**, not a current requirement to remove MO. Retain IDs and existing save/provider contracts.
- `Docs/Balance/Crops/ColdTolerance.md` owns custom crop values. CCTO remains optional and retains its own Vanilla/MO support data.
- Art: use the routing below, including the boxed-resource family procedure. Do not reactivate archived diagnostic template contracts as production policy.
- Packaging: `Tests/Release/validate_workshop_payload.py`; release metadata: `Tests/Release/validate_add_changenote.py`. Their success proves the named static contracts, not real-game or Steam completion.

## PNG asset integrity gate

For production PNG writes and image-validation tests, follow `Tests/AGENTS.md`. The `validate-png-assets` gate remains required before Stage A validation; verify actual repository-side bytes before claiming success.

## CCTO framework and crop-data ownership

AMJC owns its custom crops' temperature values, design/balance tables (including archived candidate ranges), Def mappings, optional CCTO compatibility XML, and validation. Maintain these here, not in CCTO. Use `Docs/Balance/Crops/ColdTolerance.md` together with the cultivation design and implemented XML as the local source of truth. CCTO remains an optional framework; its own Vanilla/MO support data remains owned by CCTO.

## Art / image work

For work under `Art/`, follow `Art/AGENTS.md` and its canonical asset-family procedures. For production texture changes outside `Art/`, apply the same Project asset-preservation and fixed-component protections; `Tests/AGENTS.md` governs PNG integrity verification.
