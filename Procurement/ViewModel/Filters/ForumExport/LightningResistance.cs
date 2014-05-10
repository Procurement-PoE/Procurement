namespace Procurement.ViewModel.Filters
{
    public class LightningResistance : StatFilter
    {
        public override  FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public LightningResistance()
            : base("Lightning Resistance", "Lightning Resistance", "to Lightning Resistance")
        { }
    }
}

