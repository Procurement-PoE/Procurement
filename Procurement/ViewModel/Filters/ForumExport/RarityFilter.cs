using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public class RarityFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Rarity; }
        }

        private Rarity rarity;
        public RarityFilter(Rarity rarity)
        {
            this.rarity = rarity;
        }

        public string Keyword { get { return rarity.ToString() + " rarity"; } }
        public string Help { get { return "Returns All " + rarity.ToString() + " rarity items"; } }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear != null)
                return gear.Rarity == rarity;

            return false;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }
    }
}