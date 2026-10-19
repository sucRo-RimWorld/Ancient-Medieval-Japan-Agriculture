# White-rice dish image candidates

**Author approval pending.** This branch does not modify production Textures or authoritative high-resolution Art/Sources.

- Original meal bowl: `Textures/Things/Item/Meal/AMJC_MixedGrainMeal/AMJC_MixedGrainMeal_a.png` (blob `dcd01588db4585504551d1ec88358cca5d33ee45`).
- Original porridge bowl: `Textures/Things/Item/Meal/AMJC_MixedGrainPorridge/AMJC_MixedGrainPorridge_a.png` (blob `3c76850ed8e2728c744852de023ca8e8e8704602`).

## 2026-10-20 meal rework

The previously color-neutralized meal still displayed barley-shaped grains, making it invalid as white rice. The new `WhiteRiceMeal_256px_preview.png` is a **new short-grain cooked-rice content layer**, not the old barley pixels recolored. The newly created layer was composed only over the interior food area, on top of the previous unmodified wooden bowl pixels. Preflight 256×256 RGBA candidate inspection: **0 alpha changes; 0 changes to pixels classified as wooden bowl; no pixels outside the defined food area changed.** Newly produced pixels use the source-isolated short-grain rice layer. Visual acceptance remains open, especially for the lower food/bowl boundary at game scale.

The porridge candidate `WhiteRicePorridge_256px_preview.png` has **not** been shape-replaced by this revision; it remains under separate visual review. Do not represent either image as accepted.

The high-resolution original masters remain untouched. Do not merge the draft PR to main or wire production Def textures until author acceptance and high-resolution Art/Sources retention. No RimWorld native run or Steam upload.
