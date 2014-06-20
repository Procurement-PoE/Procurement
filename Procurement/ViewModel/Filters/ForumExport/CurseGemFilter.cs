namespace Procurement.ViewModel.Filters.ForumExport
{
    class CurseGemFilter : GemCategoryFilter
    {
        public CurseGemFilter()
            : base("curse")
        { }


        public override string Keyword
        {
            get { return "Curse Gems"; }
        }

        public override string Help
        {
            get { return "Gems curse targets in an area"; }
        }
    }
}