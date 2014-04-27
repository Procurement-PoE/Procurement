using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class RecipeManager
    {
        private List<Recipe> known;
        public RecipeManager()
        {
            known = new List<Recipe>() 
            { 
                new OneChaosRecipe(), 
                new Chromatic(), 
                new GCPRecipe(), 
                new ArmourersScrapRecipe(),
                new BlacksmithsWhetstoneRecipe(),
                new GlassblowersBaubleRecipe(),
                new CartographersChiselRecipe(),
                new SameBaseTypeRecipe(),
                new SameNameRecipe("Chance Orb - 2 Of The Same Name", 2, true),
                new SameNameRecipe("Alchemy Orb - 3 Of The Same Name", 3, false)
            };
        }

        public Dictionary<string, List<RecipeResult>> Run(IEnumerable<Item> items)
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
