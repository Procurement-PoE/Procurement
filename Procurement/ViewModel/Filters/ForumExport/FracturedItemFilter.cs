using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FracturedItemFilter : IFilter
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
                return "Fractured Items";
            }
        }

        public string Help
        {
            get
            {
                return "All Fractured Items";
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
            return item.Fractured;
        }
    }
}