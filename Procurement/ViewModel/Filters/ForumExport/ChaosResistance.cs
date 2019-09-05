namespace Procurement.ViewModel.Filters
{
    internal class ChaosResistance : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public ChaosResistance()
            : base("Chaos Resistance", "Chaos Resistance", "to Chaos Resistance", "to Fire and Chaos Resistances", "to Cold and Chaos Resistances", "to Lightning and Chaos Resistances")
        { }
    }
}

