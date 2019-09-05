using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class EssenceFilter : IFilter
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
                return "Essence";
            }
        }

        public string Help
        {
            get
            {
                return "All Essences";
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
            var loweredTypeLine = item.TypeLine.ToLowerInvariant();
            return loweredTypeLine.Contains("essence") || loweredTypeLine.Contains("remnant of");
        }
    }
}