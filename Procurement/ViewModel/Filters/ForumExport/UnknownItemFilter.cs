using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class UnknownItemFilter : IFilter
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
                return "Unknown Item";
            }
        }

        public string Help
        {
            get
            {
                return "All Items Procurement Cannot Identify";
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
            return item is UnknownItem;
        }
    }
}
