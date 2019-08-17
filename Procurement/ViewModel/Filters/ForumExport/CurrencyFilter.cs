using POEApi.Model;
using System;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class CurrencyFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Currency"; }
        }

        public string Help
        {
            get { return "All currency items"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            return item is Currency || item is BreachSplinter || item is LegionSplinter || item is LegionEmblem || item is Breachstone || item is Sextant;
        }
    }
}
