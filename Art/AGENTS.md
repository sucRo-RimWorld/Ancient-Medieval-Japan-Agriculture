# Art agent instructions — AMJ Grains

Read the repository-root `AGENTS.md`, `main:Docs/Coordination.md`, and Project shared rules first. Keep detailed asset-family procedures in their canonical documents, not in this file.

Resolve rules in this order:
1. Project `Docs/ArtStyle.md` — AMJ-wide visual language and precedence.
2. Owning asset-class specification — subject-specific visual rules only.
3. Project `Docs/GoldenPaths/TextureAssetPipeline.md` — source preservation, generation permission, export and validation.
4. Project `Docs/GoldenPaths/FixedImageTemplates.md` — only for intentionally pixel-exact reused visible parts.

Asset-class routes:
- Grain/crop sources: `Docs/GoldenPaths/GrainImageGeneration.md` (`Scripts/Art/grains_image_generator.py`).
- Boxed resources/masu: `Docs/GoldenPaths/BoxedResourceIconPipeline.md`.
- Prerequisite-Mod retextures: Project `Docs/RetextureImplementationGuidelines.md` and the owner's art specification.

An asset-class document may narrow, but not silently replace, AMJ-wide style. Put a real exception in the owning style specification, not chat history or AGENTS. Follow Project's texture pipeline for generation intent and ImageGen; interpret requested image actions semantically, not via exact-keyword gates. Never regenerate accepted fixed/shared parts solely because new image work is requested.
