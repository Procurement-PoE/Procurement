namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ManaRegenFilter : ExplicitModBase
    {
        public ManaRegenFilter()
            : base("increased Mana Regeneration Rate")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "повышение скорости регенерации маны";
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
                    return "Регенерация маны";
                }
                else
                {
                    return "Mana regen";
                }
            }
        }

        public override string Help
        {
            get { return "Items with increased Mana Regeneration Rate"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
