using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ShaperItemFilter : IFilter
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
                return "Shaper Items";
            }
        }

        public string Help
        {
            get
            {
                return "All Shaper Items";
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
            return item.Shaper;
        }
    }
}