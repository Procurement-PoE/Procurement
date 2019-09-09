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
            if (item.Corrupted)
            {
                return false;
            }
            else if (item is Map || item is Gem || item is AbyssJewel)
            {
                return true;
            }
            else
            {
                Gear gear = item as Gear;
                if (gear != null)
                {
                    return !gear.GearType.Equals(GearType.Flask)
                    && !gear.GearType.Equals(GearType.Unknown)
                    && !gear.GearType.Equals(GearType.DivinationCard)
                    && !gear.GearType.Equals(GearType.Breachstone);
                }
            }

            return false;
        }
    }
}
