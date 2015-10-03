using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class MapTierFilter : IFilter
    {
        private int tier;
        public MapTierFilter(int tier)
        {
            this.tier = tier;
        }
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Map Tier"; }
        }

        public string Help
        {
            get { return "Returns Map of a particular level"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Level; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Map map = item as Map;
            if (map == null)
                return false;

            return map.MapTier == tier;
        }
    }
}
