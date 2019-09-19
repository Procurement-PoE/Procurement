using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class IdentifiedItemFilter : IFilter
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
                return "Identified Items";
            }
        }

        public string Help
        {
            get
            {
                return "Identified Items";
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
            return item.Identified && item.ItemLevel > 0 && !(item.StackSize > 0) && !(item is Incubator) && !(item is FullBestiaryOrb);
        }
    }
}
