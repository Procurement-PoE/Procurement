namespace Procurement.ViewModel.Filters
{
    public class FireResistance : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public FireResistance()
            : base("Fire Resistance", "Fire Resistance", "to Fire Resistance", "to Fire and Lightning Resistances", "to Fire and Cold Resistances", "to Fire and Chaos Resistances", "to all Elemental Resistances")
        { }
    }
}

