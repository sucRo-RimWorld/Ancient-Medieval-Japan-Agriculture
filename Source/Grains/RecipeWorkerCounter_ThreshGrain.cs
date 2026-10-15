using RimWorld;
using Verse;

namespace AMJ.Grains
{
    // Grains threshing produces grain first and optional MO straw second.
    // The default counter rejects multi-product recipes for TargetCount bills;
    // the inherited counting implementation counts the first product.
    public sealed class RecipeWorkerCounter_ThreshGrain : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
            return bill != null && recipe != null && recipe.products != null
                && recipe.products.Count >= 1 && recipe.products[0].thingDef != null;
        }
    }
}
