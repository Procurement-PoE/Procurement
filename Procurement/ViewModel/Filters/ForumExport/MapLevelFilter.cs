using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class MapLevelFilter : IFilter
    {
        private int level;
        public MapLevelFilter(int level)
        {
            this.level = level;
        }
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Map Level"; }
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

            return map.MapTier == level;
        }
    }
}
