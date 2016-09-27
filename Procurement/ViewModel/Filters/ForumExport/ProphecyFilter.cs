using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ProphecyFilter : IFilter
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
                return "Prophecy";
            }
        }

        public string Help
        {
            get
            {
                return "All Prophecies";
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
            return item is Prophecy;
        }
    }
}