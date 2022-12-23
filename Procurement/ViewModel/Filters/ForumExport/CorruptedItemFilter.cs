using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class CorruptedItemFilter : IFilter
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
                return "Corrupted Items";
            }
        }

        public string Help
        {
            get
            {
                return "Corrupted Items";
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
            return item.Corrupted;
        }
    }
}
