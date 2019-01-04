using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class RecipeManager
    {
        private readonly List<Recipe> known;
        public RecipeManager()
        {
            known = new List<Recipe>() 
            {
                new RareSetRecipe(1, 59, true, "1 Chance - i59- Full Rare Set"),
                new RareSetRecipe(1, 59, false, "2 Chance - Unidentified i59- Full Rare Set"),
                new RareSetRecipe(60, 74, true, "1 Chaos - Full Rare Set"),
                new RareSetRecipe(60, 74, false, "2 Chaos - Full Unidentified Rare Set"),
                new RareSetRecipe(75, 100, true, "1 Regal - i75+ Full Rare Set"),
                new RareSetRecipe(75, 100, false, "2 Regal - Unidentified  i75+ Full Rare Set"),
                new Chromatic(), 
                new GCPRecipe(), 
                new ArmourersScrapRecipe(),
                new BlacksmithsWhetstoneRecipe(),
                new GlassblowersBaubleRecipe(),
                new CartographersChiselRecipe(),
                new SameBaseTypeRecipe(),
                new SameNameRecipe("Alchemy Orb - 3 Of The Same Name", 3),
                new SameNameRecipe("Chance Orb - 2 Of The Same Name", 2),
                new VaalOrbRecipe()
            };
        }

        public Dictionary<string, List<RecipeResult>> Run(IDictionary<Tab, List<Item>> items)
        {
            return known.SelectMany(recipe => recipe.Matches(items))
                        .GroupBy(r => r.Name)
                        .Select(group =>
                            new
                            {
                                Name = group.Key,
                                RecipeGroup = group.OrderByDescending(recipe => recipe.PercentMatch)
                            })
                        .ToDictionary(g => g.Name, g => g.RecipeGroup.ToList());
        }
    }
}
