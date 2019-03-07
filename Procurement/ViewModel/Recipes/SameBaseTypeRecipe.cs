using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    public class SameBaseTypeRecipe : Recipe
    {
        private bool _strictConditionChecking;
        public bool StrictConditionChecking
        {
            get
            {
                return _strictConditionChecking;
            }
            protected set
            {
                _strictConditionChecking = value;
            }
        }

        public SameBaseTypeRecipe(decimal minimumMatchPercentage = 60, bool strictRecipeChecking = true)
            : base(minimumMatchPercentage)
        {
            StrictConditionChecking = strictRecipeChecking;
        }

        public override string Name
        {
            get { return "1 Orb of Augmentation - Same base type with normal, magic, rare"; }
        }

        public override string GetResultName(RecipeResult result)
        {
            bool useShortRecipeDescriptions = GetShouldUseShortRecipeDescriptions();
            if (result.MatchedItems.Any(i => i.ItemType == ItemType.Gear
                && (i as Gear).Rarity == Rarity.Unique))
            {
                if (useShortRecipeDescriptions)
                    return "5 Orbs of Chance - SBT, NMRU";
                else
                    return "5 Orbs of Chance - Same base type with normal, magic, rare, and unique";
            }

            // All items have 20% quality and are either unidentified or normal rarity.
            if (result.MatchedItems.All(i => i.Quality == 20
                && (!i.Identified || (i is Gear && (i as Gear).Rarity == Rarity.Normal))))
            {
                if (useShortRecipeDescriptions)
                    return "2 Orbs of Alchemy - SBT, NMR, 20% (U)";
                else
                    return "2 Orbs of Alchemy - Same base type with normal, magic, rare, 20% quality and unidentified";
            }

            if (result.MatchedItems.All(i => i.Quality == 20))
            {
                if (useShortRecipeDescriptions)
                    return "1 Orb of Alchemy - SBT, NMR, 20%";
                else
                    return "1 Orb of Alchemy - Same base type with normal, magic, rare, 20% quality";
            }

            if (result.MatchedItems.All(i => !i.Identified || (i is Gear && (i as Gear).Rarity == Rarity.Normal)))
            {
                if (useShortRecipeDescriptions)
                    return "2 Orbs of Augmentation - SBT, NMR, (U)";
                else
                    return "2 Orbs of Augmentation - Same base type with normal, magic, rare, unidentified";
            }

            // base case
            if (useShortRecipeDescriptions)
                return "1 Orb of Augmentation - SBT, NMR";
            else
                return "1 Orb of Augmentation - Same base type with normal, magic, and rare";
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<POEApi.Model.Item> items)
        {
            List<GearType> invalidGearType = new List<GearType> { GearType.Flask, GearType.QuestItem,
                GearType.DivinationCard, GearType.Talisman, GearType.Breachstone, GearType.Leaguestone };

            List<Gear> allGear = items.OfType<Gear>()
                                      .Where(g => !invalidGearType.Contains(g.GearType))
                                      .ToList();
            Dictionary<string, List<Gear>> baseTypeBuckets = allGear.Where(g => !string.IsNullOrWhiteSpace(g.BaseType))
                                                                    .GroupBy(g => g.BaseType)
                                                                    .ToDictionary(g => g.Key.ToString(), g => g.ToList());

            Func<Gear, bool> emptyConstraint = g => true;
            Func<Gear, bool> qualityConstraint = g => g.Quality == 20;
            Func<Gear, bool> identifiedConstraint = g => g.Identified || g.Rarity == Rarity.Normal;
            Func<Gear, bool> unidentifiedConstraint = g => !g.Identified || g.Rarity == Rarity.Normal;
            Func<Gear, bool> qualityAndUnidentifiedConstraint = g => qualityConstraint(g) && unidentifiedConstraint(g);

            Func<Gear, bool> qualityAndIdentifiedConstraint = g => qualityConstraint(g) && identifiedConstraint(g);
            Func<Gear, bool> notQualityAndUnidentifiedConstraint = g => !qualityConstraint(g) && unidentifiedConstraint(g);
            Func<Gear, bool> neitherConstraint = g => !qualityConstraint(g) && identifiedConstraint(g);

            List<Func<Gear, bool>> variantConstraints;
            if (StrictConditionChecking)
            {
                // Only include items which do not meet more specific requirements.  For example, do not include items
                // with 20% quality in recipe variants which do not require 20% quality.
                variantConstraints = new List<Func<Gear, bool>>()
                {
                    qualityAndUnidentifiedConstraint, qualityAndIdentifiedConstraint,
                    notQualityAndUnidentifiedConstraint, neitherConstraint
                };
            }
            else
            {
                variantConstraints = new List<Func<Gear, bool>>()
                {
                    qualityAndUnidentifiedConstraint, qualityConstraint, unidentifiedConstraint, emptyConstraint
                };
            }

            // Since we check for both complete and partial matches for a variant before moving on to the next, less
            // valuable variant, we might not find complete but less valuable matches, when there is a partial but more
            // valuable match that uses those items.  We could introduce another option to first check each variant for
            // complete matches, and then check each variant for partial matches, to favor quantity of matches over
            // quality of matches.

            IEnumerable<RecipeResult> allResults = new List<RecipeResult>();
            foreach (var constraint in variantConstraints)
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
