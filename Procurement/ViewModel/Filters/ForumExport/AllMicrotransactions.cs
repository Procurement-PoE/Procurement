using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class AllMicrotransactions : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Microtransactions; }
        }

        public string Keyword
        {
            get { return "All microtransactions"; }
        }

        public string Help
        {
            get { return "Returns all items with Microtransactions"; }
        }

        public bool Applicable(Item item)
        {
            if (item.Microtransactions.Count > 0)
                return true;

            var gear = item as Gear;

            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            return false;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }
    }
}
