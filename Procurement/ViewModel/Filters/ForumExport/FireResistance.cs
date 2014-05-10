namespace Procurement.ViewModel.Filters
{
    public class FireResistance : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public FireResistance()
            : base("Fire Resistance", "Fire Resistance", "to Fire Resistance")
        { }
    }
}

