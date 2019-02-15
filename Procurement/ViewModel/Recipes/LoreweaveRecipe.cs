using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    public class LoreweaveRecipe : Recipe
    {
        private const decimal TOTAL_NEEDED_RINGS = 60m;
        private readonly SetType _setType;

        public LoreweaveRecipe(SetType setType = SetType.Normal) : base(10)
        {
            _setType = setType;
            Name = $"{_setType.ToString()} Loreweave";
        }

        public override string Name { get; }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<Item> items)
        {
            var isShaperWanted = _setType == SetType.Shaper;
            var isElderWanted = _setType == SetType.Elder;

            var uniqueRings = items.OfType<Gear>().Where(x => x.GearType == GearType.Ring
                                                              && x.Rarity == Rarity.Unique
                                                              && x.Shaper == isShaperWanted
                                                              && x.Elder == isElderWanted)
                .ToList();

            var recipeResults = new List<RecipeResult>();

            while (uniqueRings.Any())
            {
                var recipe = new RecipeResult {Instance = this, MatchedItems = new List<Item>()};

                for (var i = uniqueRings.Count - 1; i >= 0; i--)
                {
                    recipe.MatchedItems.Add(uniqueRings[i]);

                    uniqueRings.RemoveAt(i);

                    if (recipe.MatchedItems.Count == TOTAL_NEEDED_RINGS)
                    {
                        break;
                    }
                }

                recipe.PercentMatch = recipe.MatchedItems.Count / TOTAL_NEEDED_RINGS * 100;
                recipe.IsMatch = recipe.PercentMatch >= ReturnMatchesGreaterThan;

                recipeResults.Add(recipe);
            }

            return recipeResults;
        }
    }
}