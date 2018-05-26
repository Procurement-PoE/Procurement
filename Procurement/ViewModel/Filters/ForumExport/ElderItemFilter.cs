using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ElderItemFilter : IFilter
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
                return "Elder Items";
            }
        }

        public string Help
        {
            get
            {
                return "All Elder Items";
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
            return item.Elder;
        }
    }
}