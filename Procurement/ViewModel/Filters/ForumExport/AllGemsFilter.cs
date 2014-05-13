using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class AllGemsFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "All gems"; }
        }

        public string Help
        {
            get { return "Any and all gems"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public bool Applicable(Item item)
        {
            return item is Gem;
        }
    }
}
