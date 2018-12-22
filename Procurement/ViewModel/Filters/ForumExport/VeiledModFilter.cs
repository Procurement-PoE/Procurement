using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class VeiledModFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Veiled Mods"; }
        }

        public string Help
        {
            get { return "Items with veiled mods"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            return item.VeiledMods?.Count > 0;
        }
    }
}
