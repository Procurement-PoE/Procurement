using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class LeagestoneFilter : IFilter
    {
        public bool CanFormCategory => false;

        public string Keyword => "Leaguestone";

        public string Help => "All Leaguestones";

        public FilterGroup Group => FilterGroup.Default;

        public bool Applicable(Item item)
        {
            return item.TypeLine.ToLower().Contains("leaguestone");
        }
    }
}