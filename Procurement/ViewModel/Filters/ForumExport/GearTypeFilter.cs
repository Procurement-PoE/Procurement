using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{  
    public class GearTypeFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.GearTypes; }
        }

        private GearType gearType;
        public GearTypeFilter(GearType gearType, string keyword)
        {
            this.gearType = gearType;
            this.Keyword = keyword;
        }

        public string Keyword { get; set; }
        public string Help { get { return "Returns All " + gearType.ToString() + " gear"; } }

        public virtual bool Applicable(Item item)
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