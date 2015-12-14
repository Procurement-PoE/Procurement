using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class AllGemsFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "All gems"; }
        }

        public string Help
        {
            get { return "Any and all gems"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            return item is Gem;
        }
    }
}
