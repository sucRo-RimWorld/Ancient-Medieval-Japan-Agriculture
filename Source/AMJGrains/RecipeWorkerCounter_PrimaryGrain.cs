using RimWorld;
using Verse;

namespace AncientMedievalJapan.Grains
{
    /// <summary>Counts grain only for MO threshing's grain+Straw recipes.</summary>
    public sealed class RecipeWorkerCounter_PrimaryGrain : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
            if (recipe == null || recipe.products == null || recipe.products.Count == 0
                || recipe.products[0].thingDef == null)
                return false;
            for (int i = 1; i < recipe.products.Count; i++)
                if (recipe.products[i].thingDef == null
                    || recipe.products[i].thingDef.defName != "DankPyon_Straw")
                    return false;
            // Intentionally inherit CountProducts: it already counts products[0]
            // and applies the game's zone, quality, and stockpile restrictions.
            return true;
        }
    }
}
