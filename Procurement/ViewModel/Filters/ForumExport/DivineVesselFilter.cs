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

    public class OfferingFilter : IFilter
    {
        public bool CanFormCategory { get; } = true;
        public string Keyword { get; } = "Offering to the Goddess";
        public string Help { get; } = "Offering to the Goddesses";
        public FilterGroup Group { get; }
        public bool Applicable(Item item)
        {
            return item is Offering;
        }
    }
    
}