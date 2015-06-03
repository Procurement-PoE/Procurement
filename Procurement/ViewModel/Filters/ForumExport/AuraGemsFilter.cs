namespace Procurement.ViewModel.Filters
{
    internal class AuraGemsFilter : GemCategoryFilter
    {
        public AuraGemsFilter()
            : base("aura")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.filter = "аура";
            }
        }

        public override string Keyword
        {
            get { return "Aura Gems"; }
        }

        public override string Help
        {
            get { return "Gems that buff the player with an aura"; }
        }
    }
}
