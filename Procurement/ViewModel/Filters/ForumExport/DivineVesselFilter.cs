using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class DivineVesselFilter : IFilter
    {
        public bool CanFormCategory { get; } = true;
        public string Keyword { get; } = "Divine Vessel";
        public string Help { get; } = "Divine Vessels";
        public FilterGroup Group { get; }
        public bool Applicable(Item item)
        {
            return item is DivineVessel;
        }
    }
}