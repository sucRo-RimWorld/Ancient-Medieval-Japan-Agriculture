# White-rice illustration candidates — original wooden bowls preserved

**Candidate only, author visual acceptance pending; no production `Textures` changes.**

Two fixed baseline bowls, which remain unchanged in their existing production paths:

- White rice meal: source `Textures/Things/Item/Meal/AMJC_MixedGrainMeal/AMJC_MixedGrainMeal_a.png`, blob `dcd01588db4585504551d1ec88358cca5d33ee45`.
- White rice porridge: source `Textures/Things/Item/Meal/AMJC_MixedGrainPorridge/AMJC_MixedGrainPorridge_a.png`, blob `3c76850ed8e2728c744852de023ca8e8e8704602`.

## Latest rework, 2026-10-20

White rice meal `Art/Candidates/WhiteRice/WhiteRiceMeal_256px_preview.png` has been **reconstructed as individually shaded, plump short-grain cooked rice** inside the existing wooden bowl; **not a recoloring of barley kernels**. It uses an explicit row-wise food-region mask. The exterior bowl, transparent canvas, and visible front rim derive directly from the existing game image, without generating a new bowl. The old full-resolution source is not overwritten.

Source-vs-candidate mechanical check for this new 256×256 RGBA file: 162 independently rendered rice kernels; 16671 food-mask pixels updated; **0 pixels changed outside the compositing mask**, **0 alpha-channel changes**; PNG round-trip decode equals generated RGBA byte-for-byte. Exposed existing brown back-rim pixels are intentionally retained, not categorized as white rice.

White porridge `Art/Candidates/WhiteRice/WhiteRicePorridge_256px_preview.png` stays as a **separate unchanged candidate** derived from the original porridge bowl. Its bowl also remains unreplaced.

**Not final art:** Both are 256px review derivatives, not accepted high-resolution masters. A positive visual judgment and a proper full-resolution source/master path under `Art/Sources` are still required before production promotion, texture wiring, or actual-game visual claims. No Steam upload or RimWorld run.
