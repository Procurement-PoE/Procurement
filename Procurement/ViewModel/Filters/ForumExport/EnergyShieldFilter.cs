namespace Procurement.ViewModel.Filters.ForumExport
{
    public class EnergyShieldFilter : ExplicitModBase
    {
        public EnergyShieldFilter()
            : base("Energy Shield")
        { }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Energy Shield"; }
        }

        public override string Help
        {
            get { return "Items with Energy Shield"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
