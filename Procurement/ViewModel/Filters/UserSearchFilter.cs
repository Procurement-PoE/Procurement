using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters
{
    public class UserSearchFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        private string filter;
        public UserSearchFilter(string filter)
        {
            this.filter = filter;
        }
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "User search"; }
        }

        public string Help
        {
            get { return "Matches user search on name/typeline and geartype"; }
        }

        public bool Applicable(Item item)
        {
            if (string.IsNullOrEmpty(filter))
                return false;

            if (item.TypeLine.ToLower().Contains(filter.ToLower()) || item.Name.ToLower().Contains(filter.ToLower()) || containsMatchedCosmeticMod(item) || isMatchedGear(item))
                return true;

            var gear = item as Gear;

            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            return false;
        }

        private bool containsMatchedCosmeticMod(Item item)
        {
            return item.Microtransactions.Any(x => x.ToLower().Contains(filter.ToLower()));
        }

        private bool isMatchedGear(Item item)
        {
            Gear gear = item as Gear;

            if (gear == null)
                return false;

            return gear.GearType.ToString().ToLower().Contains(filter.ToLower());
        }
    }
}
