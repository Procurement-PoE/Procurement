using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ResonatorFilter : IFilter
    {
        public bool CanFormCategory { get; }
        public string Keyword { get; } = "Resonator";
        public string Help { get; } = "All Resonators";
        public FilterGroup Group { get; } = FilterGroup.Default;
        public bool Applicable(Item item)
        {
            return item is Resonator;
        }
    }
}