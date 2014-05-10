using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public class GearTypeFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        private GearType gearType;
        public GearTypeFilter(GearType gearType)
        {
            this.gearType = gearType;
        }

        public string Keyword { get { return gearType.ToString() + " gear"; } }
        public string Help { get { return "Returns All " + gearType.ToString() + " gear"; } }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear != null)
                return gear.GearType == gearType;

            return false;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }


    }
}