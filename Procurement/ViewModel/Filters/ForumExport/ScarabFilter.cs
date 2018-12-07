using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ScarabFilter : IFilter
    {
        public bool CanFormCategory { get; } = true;
        public string Keyword { get; } = "Scarab";
        public string Help { get; } = "Scarabs";
        public FilterGroup Group { get; }
        public bool Applicable(Item item)
        {
            return item is Scarab;
        }
    }
}
