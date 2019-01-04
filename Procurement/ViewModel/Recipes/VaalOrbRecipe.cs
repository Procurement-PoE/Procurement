using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class VaalOrbRecipe : Recipe
    {
        public override string Name => "1 Vaal Orb";

        public override IEnumerable<RecipeResult> Matches(IEnumerable<Item> items)
        {
            List<RecipeResult> recipeSets = new List<RecipeResult>();

            var candidateGems = items.OfType<Gem>().Where(gem => gem.Corrupted).Cast<Item>().ToList();

            var fragments = items.Where(x => x.TypeLine.StartsWith("Sacrifice at", StringComparison.CurrentCultureIgnoreCase)).ToList();
            while(candidateGems.Count > 0)
            {
                var recipeResult = new RecipeResult()
                {
                    Instance = this,
                    MatchedItems = new List<Item>(),
                    Missing = new List<string>(),
                    IsMatch = true
                };

                for (int i = candidateGems.Count - 1; i >= 0; i--)
                {
                    var candidateGem = candidateGems[i];
                    if (recipeResult.MatchedItems.Count <= 6)
                    {
                        recipeResult.MatchedItems.Add(candidateGem);
                        candidateGems.Remove(candidateGem);
                    }
                    else
                    {
                        break;
                    }
                }

                var numberOfMissingGems = 7 - recipeResult.MatchedItems.Count;
                if (numberOfMissingGems >= 1)
                {
                    recipeResult.Missing.Add($"{numberOfMissingGems} Vaal Skill gems");
                }

                if (fragments.Any())
                {
                    recipeResult.MatchedItems.Add(fragments.First());
                    fragments.RemoveAt(0);
                }
                else
                {
                    recipeResult.Missing.Add("Sacrifice Fragment");
                }

                recipeResult.PercentMatch = (decimal) ((recipeResult.MatchedItems.Count / 8d) * 100) ;
                recipeSets.Add(recipeResult);
            }

            return recipeSets;
        }
    }
}
