using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FossilFilter : IFilter
    {
        public bool CanFormCategory { get; }
        public string Keyword { get; } = "Fossil";
        public string Help { get; } = "All Fossils";
        public FilterGroup Group { get; } = FilterGroup.Default;
        public bool Applicable(Item item)
        {
            return item is Fossil;
        }
    }
}