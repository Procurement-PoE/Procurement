namespace Procurement.ViewModel.Filters.ForumExport
{
    public class EnergyShieldFilter : ExplicitModBase
    {
        public EnergyShieldFilter()
            : base("Energy Shield")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "энергетического щита";
            }
        }

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
