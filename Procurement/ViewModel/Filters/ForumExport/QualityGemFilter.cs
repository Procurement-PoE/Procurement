using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class QualityGemFilter : IFilter
    {
        private readonly int quality;

        public QualityGemFilter()
        {
            this.quality = 0;
        }

        public QualityGemFilter(int quality)
        {
            this.quality = quality;
        }
        
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
            get { return "Quality Gems"; }
        }

        public string Help
        {
            get { return "Quality Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            if (quality == 0)
                return gem.IsQuality;

            return gem.Quality == quality;
        }
    }
}
