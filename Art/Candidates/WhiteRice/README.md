# White-rice illustration candidates — source-preserving 256px previews

These two images are NOT approved sources or production Textures. They are visual proofs derived mechanically from the existing accepted game textures, since the high-resolution original PNGs are not available for programmatic transfer in this session.

Original 256px baselines, byte-for-byte unchanged:
- Meal: `Textures/Things/Item/Meal/AMJC_MixedGrainMeal/AMJC_MixedGrainMeal_a.png` (Git blob `dcd01588db4585504551d1ec88358cca5d33ee45`)
- Porridge: `Textures/Things/Item/Meal/AMJC_MixedGrainPorridge/AMJC_MixedGrainPorridge_a.png` (Git blob `3c76850ed8e2728c744852de023ca8e8e8704602`)

The exterior wooden vessels are inherited from the two separate actual originals. Only selected pixels inside the food areas are neutralized from yellow/beige to white; dimensions 256x256 and transparency unchanged. There is NO new pottery, no bowl redraw, and no change to existing originals. A pixel diff confirms zero changes outside their respective inner food-area masks and zero alpha differences.

Candidate-only files in this folder: `Art/Candidates/WhiteRice/WhiteRiceMeal_256px_preview.png`, `Art/Candidates/WhiteRice/WhiteRicePorridge_256px_preview.png`.

**Do not promote these previews to `Art/Sources` or `Textures` as final without author acceptance.** Production should derive from accepted high-resolution masters, and must pass owner PNG integrity tests. The 256px proof-of-concept is not an accepted replacement for that process.
