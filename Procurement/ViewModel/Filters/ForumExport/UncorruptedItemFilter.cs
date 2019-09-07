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
            else if (item is Map || item is Gem)
            {
                return true;
            }
            else if (!(item.MaxStackSize > 0))
            {
                Gear gear = item as Gear;
                if (gear != null)
                {
                    return !gear.TypeLine.StartsWith("Sacrifice at ")
                    && !gear.TypeLine.StartsWith("Mortal ")
                    && !gear.TypeLine.StartsWith("Fragment of the ")
                    && !gear.TypeLine.EndsWith(" Key")
                    && !gear.TypeLine.EndsWith(" Breachstone")
                    && !gear.TypeLine.Contains(" Flask");
                }
            }

            return false;
        }
    }
}
