using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class IncursionVialsFilter : IFilter
    {
        public bool CanFormCategory
        {
            get
            {
                return false;
            }
        }

        public string Keyword
        {
            get
            {
                return "Incursion Vials";
            }
        }

        public string Help
        {
            get
            {
                return "All Vials used to upgrade items in the Temple of Atzoatl.";
            }
        }

        public FilterGroup Group
        {
            get
            {
                return FilterGroup.Default;
            }
        }

        public bool Applicable(Item item)
        {
            var currency = item as Currency;
            if (currency == null)
                return false;

            return currency.Type == OrbType.IncursionVial;
        }
    }
}