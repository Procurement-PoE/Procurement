using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class UncorruptedItemFilter : IFilter
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
                return "Uncorrupted Items";
            }
        }

        public string Help
        {
            get
            {
                return "Uncorrupted Items";
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
            Gear gear = item as Gear;

            return !item.Corrupted
            && (item is Gem || (item.ItemLevel > 0 && !(item.StackSize > 0) && (!gear?.GearType.Equals(GearType.Flask) ?? true)))
            && !(item is Incubator) && !(item is FullBestiaryOrb);
        }
    }
}
