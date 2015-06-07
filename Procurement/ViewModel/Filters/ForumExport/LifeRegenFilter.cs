namespace Procurement.ViewModel.Filters.ForumExport
{
    public class LifeRegenFilter : ExplicitModBase
    {
        public LifeRegenFilter()
            : base("Life Regenerated per second")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {

                this.keyword = "регенерации здоровья в секунду";
            }
        }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Регенерация здоровья";
                }
                else
                {
                    return "Life regen";
                }
            }
        }

        public override string Help
        {
            get { return "Items with Life Regenerated per second"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
