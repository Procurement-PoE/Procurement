using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class IncubatorFilter : IFilter
    {
        public string Keyword { get; } = "Incubators";
        public bool CanFormCategory { get; } = true;
        public string Help { get; } = "All Legion incubators";
        public FilterGroup Group { get; } = FilterGroup.Default;
        public bool Applicable(Item item)
        {
            return item is Incubator;
        }
    }
}