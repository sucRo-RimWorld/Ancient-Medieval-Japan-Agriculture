# White rice image candidates — same original wooden bowls

**Candidate only.** These derived 256×256 PNGs are not final accepted high-resolution sources or shipped production textures. The two existing wooden bowls remain unchanged; each original retains its own distinct design.

- White rice meal: `Art/Candidates/WhiteRice/WhiteRiceMeal_256px_preview.png` — original food texture `Textures/Things/Item/Meal/AMJC_MixedGrainMeal/AMJC_MixedGrainMeal_a.png`, original blob `dcd01588db4585504551d1ec88358cca5d33ee45`.
- White rice porridge: `Art/Candidates/WhiteRice/WhiteRicePorridge_256px_preview.png` — original food texture `Textures/Things/Item/Meal/AMJC_MixedGrainPorridge/AMJC_MixedGrainPorridge_a.png`, original blob `3c76850ed8e2728c744852de023ca8e8e8704602`.

2026-10-20 revision: repaired warm/yellow residual grains in the existing 256px food-area previews without image-model generation. Food-zone-only, deterministic RGB neutralization: meal 1633 further pixels; porridge 107 further pixels. Exact old→new decoded RGBA diff outside the food elliptical masks: **zero** in each; alpha differences: **zero**. Previously recorded original→candidate diff was also restricted to those masks, so the bowls remain pixel-identical to the original production images outside the food masks. Corrected outputs round-trip decode to identical 256×256 RGBA values.

Do not conflate this candidate inspection with accepted source-art transfer; existing high-resolution masters in `Art/Sources` are not modified. Before promotion, obtain author visual acceptance, use the applicable high-res-source export process, and run PNG integrity validation on committed production pixels. No Steam publishing or live-game test authorization.
