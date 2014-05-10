namespace Procurement.ViewModel.Filters
{
    internal class AllElementalResistances : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public AllElementalResistances()
            : base("All elemental Resistances", "All elemental Resistances", "to all Elemental Resistances")
        { }
    }
}

