using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    class SameBaseTypeRecipe : Recipe
    {
        public SameBaseTypeRecipe()
            : base(60)
        { }

        public override string Name
        {
            get { return "1 Orb of Augmentation - Same base type with normal, magic, rare"; }
        }

        public override string GetResultName(RecipeResult result)
        {
            if (result.MatchedItems.Any(i => i.ItemType == ItemType.Gear
                && (i as Gear).Rarity == Rarity.Unique))
            {
                return "5 Orbs of Chance - Same base type with normal, magic, rare, and unique";
            }

            // All items have 20% quality and are either unidentified or normal rarity.
            if (result.MatchedItems.All(i => i.Quality == 20
                && (!i.Identified || (i is Gear && (i as Gear).Rarity == Rarity.Normal))))
            {
                return "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and unidentified";
            }

            if (result.MatchedItems.All(i => i.Quality == 20))
            {
                return "1 Orb of Alchemy - Same base type with normal, magic, rare, 20% quality";
            }

            if (result.MatchedItems.All(i => !i.Identified || (i is Gear && (i as Gear).Rarity == Rarity.Normal)))
            {
                return "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified";
            }

            return "1 Orb of Augmentation - Same base type with normal, magic, and rare"; // base case
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<POEApi.Model.Item> items)
        {
            List<Gear> allGear = items.OfType<Gear>().ToList();
            Dictionary<string, List<Gear>> baseTypeBuckets = allGear.Where(g => !string.IsNullOrWhiteSpace(g.BaseType))
                                                                    .GroupBy(g => g.BaseType)
                                                                    .ToDictionary(g => g.Key.ToString(), g => g.ToList());

            Func<Gear, bool> emptyConstraint = g => true;
            Func<Gear, bool> qualityConstraint = g => g.Quality == 20;
            Func<Gear, bool> unidentifiedConstraint = g => !g.Identified || g.Rarity == Rarity.Normal;
            Func<Gear, bool> qualityAndUnidentifiedConstraint = g => qualityConstraint(g) && unidentifiedConstraint(g);
            IEnumerable<RecipeResult> allResults = new List<RecipeResult>();

            foreach (var constraint in new List<Func<Gear, bool>>() {
                qualityAndUnidentifiedConstraint, qualityConstraint, unidentifiedConstraint, emptyConstraint })
            {
                allResults = allResults.Concat(getNextResult(baseTypeBuckets, constraint));
            }

            foreach (var result in allResults)
                yield return result;
        }

        private IEnumerable<RecipeResult> getNextResult(Dictionary<string, List<Gear>> buckets, Func<Gear, bool> constraint)
        {
            foreach(var baseTypeBucket in buckets)
            {
                List<Gear> gears = baseTypeBucket.Value; // though, technically, gear is a mass noun

                if (gears == null || gears.Count == 0)
                    continue;

                bool stop = false;
                while (!stop)
                {
                    RecipeResult result = new RecipeResult();
                    result.MatchedItems = new List<Item>();
                    result.Missing = new List<string>();
                    result.PercentMatch = 0;
                    result.Instance = this;

                    Dictionary<Rarity, Gear> set = new Dictionary<Rarity, Gear>();
                    set.Add(Rarity.Normal, gears.FirstOrDefault(g => g.Rarity == Rarity.Normal && constraint(g)));
                    set.Add(Rarity.Magic, gears.FirstOrDefault(g => g.Rarity == Rarity.Magic && constraint(g)));
                    set.Add(Rarity.Rare, gears.FirstOrDefault(g => g.Rarity == Rarity.Rare && constraint(g)));
                    // TODO: Handle case with a unique item.

                    decimal numKeys = set.Keys.Count;
                    foreach (var pair in set)
                    {
                        Rarity rarity = pair.Key;
                        Gear gear = pair.Value;
                        if (gear != null)
                        {
                            result.PercentMatch += (decimal)100.0 / numKeys;
                            result.MatchedItems.Add(gear);
                        }
                        else
                        {
                            result.Missing.Add(string.Format("Item with {0} rarity", rarity.ToString()));
                        }
                    }

                    result.IsMatch = result.PercentMatch > base.ReturnMatchesGreaterThan;
                    if (result.IsMatch) // only remove the items if they are in a "match" -- close enough to show in the UI
                    {
                        foreach (var pair in set)
                            gears.Remove(pair.Value);
                        yield return result;
                    }

                    if (result.Missing.Count > 0 || gears.Count == 0)
                        stop = true;
                }
            }
        }
    }
}
