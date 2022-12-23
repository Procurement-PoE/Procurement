using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class UnidentifiedItemFilter : IFilter
    {
        public bool CanFormCategory
        {
            get
            {
                return false;
            }
        }

        public string Keyword
        {
            get
            {
                return "Unidentified Items";
            }
        }

        public string Help
        {
            get
            {
                return "Unidentified Items";
            }
        }

        public FilterGroup Group
        {
            get
            {
                return FilterGroup.Default;
            }
        }

        public bool Applicable(Item item)
        {
            // Item types that can't be unidentified always have identified value as true so a simple check is sufficient
            return !item.Identified;
        }
    }
}
