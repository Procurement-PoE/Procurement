using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FullBestiaryOrbFilter : IFilter
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
                return "Captured Beasts";
            }
        }

        public string Help
        {
            get
            {
                return "All Bestiary Orbs Containing Beasts";
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
            return item is FullBestiaryOrb;
        }
    }
}
