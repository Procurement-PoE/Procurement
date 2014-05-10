using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class Chromatic : Recipe
    {
        public Chromatic()
            : base()
        { }

        public override string Name
        {
            get { return "1 Chromatic - 3 Link RGB Sockets"; }
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<Item> items)
        {
            return items.OfType<Gear>()
                        .Where(g => isMatch(g))
                        .Select(g => getResult(g));
        }

        private bool isMatch(Gear gear)
        {
            if (gear.NumberOfSockets() == 6)
                return false;

            var canditates = gear.Sockets.GroupBy(g => g.Group)
                                         .Where(grp => grp.Count() >= 3);

            foreach (var group in canditates)
            {
                if (group.Select(s => s.Attribute).Distinct().Count() == 3)
                    return true;
            }

            return false;
        }

        private RecipeResult getResult(Gear item)
        {
            RecipeResult result = new RecipeResult();
            result.Instance = this;

            result.PercentMatch = 100;
            result.IsMatch = true;
            result.MatchedItems = new List<Item> { item };
            result.Missing = new List<string>();

            return result;
        }
    }
}
