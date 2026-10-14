"""Verify native production worker selection preserves real harvest-yield precondition."""
from pathlib import Path
import unittest

SOURCE = Path(__file__).resolve().parent / "E2E" / "GrainsSimulationSteps.cs"


class HarvestWorkerSelectionTests(unittest.TestCase):
    def test_selected_pawn_meets_same_strict_harvest_yield_gate(self):
        source = SOURCE.read_text(encoding="utf-8")
        start = source.index("for (int attempt = 0; attempt < 30; attempt++)")
        end = source.index("ctx.Require(worker != null", start)
        selection = source[start:end]
        level = selection.index("candidate.skills.GetSkill(skill).Level = 20")
        stat = selection.index("candidate.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f")
        self.assertLess(level, stat)
        self.assertIn("{ worker = candidate; break; }", selection)
        self.assertIn("candidate.Destroy(DestroyMode.Vanish)", selection)
        self.assertIn(
            'ctx.Require(worker.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f, "Harvest failure must be excluded by worker skill.");',
            source)


if __name__ == "__main__":
    unittest.main()
