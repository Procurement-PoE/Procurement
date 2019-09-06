namespace Procurement.ViewModel.Filters
{
    public class LightningResistance : OrStatFilter
    {
        public override  FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public LightningResistance()
            : base("Lightning Resistance", "Lightning Resistance", "to Lightning Resistance", "to Cold and Lightning Resistances", "to Fire and Lightning Resistances", "to Lightning and Chaos Resistances", "to all Elemental Resistances")
        { }
    }
}