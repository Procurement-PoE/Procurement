using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class OneChaosRecipe : Recipe
    {
        private class MatchedSet
        {
            private static Dictionary<string, PropertyInfo> properties;
            public Gear Amulet { get; set; }
            public Gear Belt { get; set; }
            public Gear Helm { get; set; }
            public Gear RingLeft { get; set; }
            public Gear RingRight { get; set; }
            public Gear Weapon { get; set; }
            public Gear Offhand { get; set; }
            public Gear Boots { get; set; }
            public Gear Armour { get; set; }
            public Gear Gloves { get; set; }

            public MatchedSet()
            {
                properties = properties ?? this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name);
            }

            public decimal Match()
            {
                decimal notNull = properties.Values.Count(p => p.GetValue(this, null) != null);
                return (notNull / properties.Values.Count) * 100;
            }

            public List<Gear> GetAll()
            {
                return properties.Values.Select(p => p.GetValue(this, null))
                                        .Where(v => v != null)
                                        .Distinct()
                                        .Cast<Gear>()
                                        .ToList();
            }

            public List<string> GetMissing()
            {
                return properties.Values.Select(p => new {name = p.Name, value=p.GetValue(this, null) })
                                        .Where(at => at.value == null)
                                        .Select(at => at.name)
                                        .ToList();
            }
        }

        public override string Name
        {
            get { return "1 Chance/Chaos - Full Rare Set"; }
        }

        private List<MatchedSet> sets = new List<MatchedSet>();
        
        public OneChaosRecipe()
            : base(80)
        { }

        public override IEnumerable<RecipeResult> Matches(IEnumerable<Item> items)
        {
            List<Gear> allGear = items.OfType<Gear>().ToList();
            Dictionary<string, List<Gear>> buckets = allGear.Where(g => g.Rarity == Rarity.Rare)
                                                            .GroupBy(g => g.GearType)
                                                            .ToDictionary(g => g.Key.ToString(), g => g.ToList());

            mergeKeys(buckets, "One Handed", g => g.Properties.Any(pr => pr.Name.Contains("One Handed")), GearType.Axe, GearType.Bow, GearType.Claw, GearType.Dagger, GearType.Mace, GearType.Sceptre, GearType.Staff, GearType.Sword, GearType.Wand);
            mergeKeys(buckets, "Two Handed", g => g.Properties.Any(pr => pr.Name.Contains("Two Handed")), GearType.Axe, GearType.Bow, GearType.Claw, GearType.Dagger, GearType.Mace, GearType.Sceptre, GearType.Staff, GearType.Sword, GearType.Wand);
            removeKeys(buckets, GearType.Axe, GearType.Bow, GearType.Claw, GearType.Dagger, GearType.Mace, GearType.Sceptre, GearType.Staff, GearType.Sword, GearType.Wand);

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

            if (buckets["One Handed"].Count > 0 && buckets.ContainsKey(GearType.Shield.ToString()) && buckets[GearType.Shield.ToString()].Count > 0)
            {
                set.Weapon = pullValue(buckets, "One Handed");
                set.Offhand = pullValue(buckets, GearType.Shield.ToString());
            }
            else if (buckets["One Handed"].Count > 1)
            {
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

        private void mergeKeys(Dictionary<string, List<Gear>> buckets, string intoKey, Func<Gear, bool> predicate, params GearType[] toMerge)
        {
            buckets.Add(intoKey, new List<Gear>());
            foreach (GearType keyToMerge in toMerge)
                if (buckets.ContainsKey(keyToMerge.ToString()))
                    buckets[intoKey].AddRange(buckets[keyToMerge.ToString()]);
        }

        private void removeKeys(Dictionary<string, List<Gear>> buckets, params GearType[] keys)
        {
            foreach (GearType keyToMerge in keys)
                if (buckets.ContainsKey(keyToMerge.ToString()))
                    buckets.Remove(keyToMerge.ToString());
        }
    }
}
