using RimWorld;
using Verse;

namespace AMJ.Grains
{
    // The first product is the primary grain. Optional MO straw is a byproduct.
    // Delegate actual counting, bill filters and storage validation to Vanilla.
    public sealed class RecipeWorkerCounter_PrimaryProduct : RecipeWorkerCounter
    {
        public override bool CanCountProducts(Bill_Production bill)
        {
            return recipe != null &&
                   recipe.specialProducts == null &&
                   recipe.products != null &&
                   recipe.products.Count > 0 &&
                   recipe.products[0].thingDef != null;
        }

        public override string ProductsDescription(Bill_Production bill)
        {
            return recipe.products[0].thingDef.label;
        }
    }
}
