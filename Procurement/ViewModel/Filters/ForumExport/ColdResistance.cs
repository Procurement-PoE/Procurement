namespace Procurement.ViewModel.Filters
{
    internal class ColdResistance : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public ColdResistance()
            : base("Cold Resistance", "Cold Resistance", "to Cold Resistance")
        { }
    }
}
