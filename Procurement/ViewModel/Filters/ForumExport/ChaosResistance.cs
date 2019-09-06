namespace Procurement.ViewModel.Filters
{
    internal class ChaosResistance : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public ChaosResistance()
            : base("Chaos Resistance", "Chaos Resistance", "to Chaos Resistance")
        { }
    }
}

