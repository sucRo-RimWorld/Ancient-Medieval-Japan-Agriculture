using RimWorld;
using Verse;

namespace AMJ.Grains
{
    // MO threshing yields its principal grain first and straw second.
    // Allow target-count bills; RimWorld's inherited counter counts the primary product.
    public sealed class RecipeWorkerCounter_PrimaryGrain : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
            return bill != null && recipe != null && recipe.products != null
                && recipe.products.Count > 0 && recipe.products[0].thingDef != null;
        }
    }
}
