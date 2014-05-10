using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class CorruptedGemFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Corrupted Gems"; }
        }

        public string Help
        {
            get { return "Corrupted Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return gem.Corrupted;
        }
    }
}
