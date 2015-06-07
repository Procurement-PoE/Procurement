namespace Procurement.ViewModel.Filters.ForumExport
{
    class CurseGemFilter : GemCategoryFilter
    {
        public CurseGemFilter()
            : base("curse")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.filter = "проклятье";
            }
        }


        public override string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Камни проклятий";
                }
                else
                {
                    return "Curse Gems";
                }
            }
        }

        public override string Help
        {
            get { return "Gems curse targets in an area"; }
        }
    }
}