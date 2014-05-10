using System;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class GemCategoryFilter : IFilter
    {
        private string filter;
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

        public string Keyword
        {
            get { return "Category Gems"; }
        }

        public string Help
        {
            get { return "Category Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
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
