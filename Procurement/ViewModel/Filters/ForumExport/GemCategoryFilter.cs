using System;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class GemCategoryFilter : IFilter
    {
        internal string filter;
        public GemCategoryFilter(string filter)
        {
            this.filter = filter;
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public bool CanFormCategory
        {
            get { return false; }
        }

        public virtual string Keyword
        {
            get { return "Category Gems"; }
        }

        public virtual string Help
        {
            get { return "Category Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gear gear = item as Gear;
            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;
            
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            try
            {
                return gem.Properties[0].Name.ToLower().Contains(filter.ToLower());
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
