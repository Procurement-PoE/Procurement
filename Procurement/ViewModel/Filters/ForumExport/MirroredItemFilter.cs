using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class MirroredItemFilter : IFilter
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
                return "Mirrored";
            }
        }

        public string Help
        {
            get
            {
                return "All Mirrored Items";
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
            return item.IsMirrored;
        }
    }
}
