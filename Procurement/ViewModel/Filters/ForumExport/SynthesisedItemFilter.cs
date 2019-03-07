using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class SynthesisedItemFilter : IFilter
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
                return "Synthesised Items";
            }
        }

        public string Help
        {
            get
            {
                return "All Synthesised Items";
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
            return item.Synthesised;
        }
    }
}