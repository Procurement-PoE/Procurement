namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ManaFilter : ExplicitModBase
    {
        public ManaFilter()
            : base("to maximum Mana")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "к максимуму маны";
                //TODO: new mana filter for "повышение максимума маны"
            }
        }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Maximum Mana"; }
        }

        public override string Help
        {
            get { return "Items with +maximum mana"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
