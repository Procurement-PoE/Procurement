namespace Procurement.ViewModel.Filters
{
    internal class ColdResistance : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public ColdResistance()
            : base("Cold Resistance", "Cold Resistance", "to Cold Resistance", "to Fire and Cold Resistances", "to Cold and Lightning Resistances", "to Cold and Chaos Resistances", "to all Elemental Resistances")
        { }
    }
}
