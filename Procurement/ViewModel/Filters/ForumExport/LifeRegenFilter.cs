namespace Procurement.ViewModel.Filters.ForumExport
{
    public class LifeRegenFilter : ExplicitModBase
    {
        public LifeRegenFilter()
            : base("Life Regenerated per second")
        { }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Life regen"; }
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
