using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class MultipleVeiledModsFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Multiple Veiled Mods"; }
        }

        public string Help
        {
            get { return "Items with more than one veiled mod"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            return item.VeiledMods?.Count > 1;
        }
    }
}
