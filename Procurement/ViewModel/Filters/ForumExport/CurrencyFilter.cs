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
            get { return "All currency orbs"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            var currency = item as Currency;

            if (currency == null)
                return false;

            return true;
        }
    }
}
