using System;
using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    public class RareSetRecipe : Recipe
    {
        private readonly int minimumItemLevel;
        private readonly int maximumItemLevel;
        private readonly bool itemsIdentified;
        private readonly string name;

        public override string Name
        {
            get { return name; }
        }

        private List<MatchedSet> sets = new List<MatchedSet>();
        
        public RareSetRecipe(int minimumItemLevel, int maximumItemLevel, bool itemsIdentified, string name)
            : base(80)
        {
            this.minimumItemLevel = minimumItemLevel;
            this.maximumItemLevel = maximumItemLevel;
            this.itemsIdentified = itemsIdentified;
            this.name = name;
        }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<Item> items)
        {
            List<Gear> allGear = items.OfType<Gear>().ToList();
            Dictionary<string, List<Gear>> buckets =
                allGear.Where(g => g.Rarity == Rarity.Rare &&
                                   g.ItemLevel <= maximumItemLevel &&
                                   g.ItemLevel >= minimumItemLevel &&
                                   g.Identified == itemsIdentified)
                       .GroupBy(g => g.GearType)
                       .ToDictionary(g => g.Key.ToString(), g => g.ToList());

            GearType[] oneHandedOnlyGearTypes = { GearType.Claw, GearType.Dagger, GearType.Sceptre, GearType.Wand,
                GearType.Shield };
            GearType[] twoHandedOnlyGearTypes = { GearType.Bow, GearType.Staff };
            GearType[] mixedGearTypes = { GearType.Axe, GearType.Mace, GearType.Sword };

            moveSelectedBucketsContents(buckets, "Two Handed", twoHandedOnlyGearTypes);
            resortSelectedBuckets(buckets, "Two Handed", g => g.Properties.Any(
                pr => pr.Name.Contains("Two Handed")), mixedGearTypes);
            moveSelectedBucketsContents(buckets, "One Handed", oneHandedOnlyGearTypes);
            moveSelectedBucketsContents(buckets, "One Handed", mixedGearTypes);

            foreach (KeyValuePair<string, List<Gear>> bucket in buckets)
            {
                bucket.Value.Sort((x, y) => x.ItemLevel.CompareTo(y.ItemLevel));
            }

            RecipeResult result = getNextResult(buckets);
            while (result.IsMatch)
            {
                yield return result;
                result = getNextResult(buckets);
            }
        }

        private RecipeResult getNextResult(Dictionary<string, List<Gear>> buckets)
        {
            RecipeResult result = new RecipeResult();
            result.Instance = this;

            MatchedSet set = new MatchedSet();

            set.Amulet = pullValue(buckets, GearType.Amulet.ToString());
            set.Armour = pullValue(buckets, GearType.Chest.ToString());
            set.Belt = pullValue(buckets, GearType.Belt.ToString());
            set.Boots = pullValue(buckets, GearType.Boots.ToString());
            set.Gloves = pullValue(buckets, GearType.Gloves.ToString());
            set.Helm = pullValue(buckets, GearType.Helmet.ToString());
            set.RingLeft = pullValue(buckets, GearType.Ring.ToString());
            set.RingRight = pullValue(buckets, GearType.Ring.ToString());

            // Use two one-handed items or one two-handed item, based on which has the lowest item level.
            int oneHandedItemLevel = buckets.ContainsKey("One Handed") && buckets["One Handed"].Count > 1 ?
                buckets["One Handed"][0].ItemLevel : int.MaxValue;
            int twoHandedItemLevel = buckets.ContainsKey("Two Handed") && buckets["Two Handed"].Count > 0 ?
                buckets["Two Handed"][0].ItemLevel : int.MaxValue;

            if (oneHandedItemLevel <= twoHandedItemLevel)
            {
                // Includes the case where buckets["Two Handed"] is empty and buckets["One Handed"] has one item.
                set.Weapon = pullValue(buckets, "One Handed");
                set.Offhand = pullValue(buckets, "One Handed");
            }
            else
            {
                set.Weapon = pullValue(buckets, "Two Handed");
                set.Offhand = set.Weapon;
            }

            result.PercentMatch = set.Match();
            result.IsMatch = result.PercentMatch > base.ReturnMatchesGreaterThan;
            result.MatchedItems = set.GetAll().Cast<Item>().ToList();
            result.Missing = set.GetMissing();

            return result;
        }

        private Gear pullValue(Dictionary<string, List<Gear>> buckets, string fromBucket)
        {
            if (!buckets.ContainsKey(fromBucket))
                return null;

            List<Gear> bucket = buckets[fromBucket];
            if (bucket.Count == 0)
                return null;

            Gear ret = bucket[0];
            bucket.RemoveAt(0);

            return ret;

        }

        private void moveSelectedBucketsContents(Dictionary<string, List<Gear>> buckets, string intoKey,
            params GearType[] toMerge)
        {
            resortSelectedBuckets(buckets, intoKey, g => true, toMerge);
        }

        private void resortSelectedBuckets(Dictionary<string, List<Gear>> buckets, string intoKey, Func<Gear, bool>
            predicate, params GearType[] toMerge)
        {
            if (!buckets.ContainsKey(intoKey))
            {
                buckets.Add(intoKey, new List<Gear>());
            }

            foreach (GearType keyToMerge in toMerge)
            {
                if (buckets.ContainsKey(keyToMerge.ToString()))
                {
                    var currentSourceBucket = buckets[keyToMerge.ToString()];
                    for (int i = 0; i < currentSourceBucket.Count; i++)
                    {
                        if (predicate(currentSourceBucket[i]))
                        {
                            buckets[intoKey].Add(currentSourceBucket[i]);
                            currentSourceBucket.RemoveAt(i);
                            i--;
                        }
                    }

                    if (currentSourceBucket.Count == 0)
                    {
                        buckets.Remove(keyToMerge.ToString());
                    }
                }
            }
        }
    }
}
