namespace Procurement.ViewModel.Filters.ForumExport
{
    public class LifeRegenFilter : OrStatFilter
    {
        public override bool CanFormCategory
        {
            get { return false; }
        }
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
        public LifeRegenFilter()
            : base("Life regen", "Items with Life Regenerated per second", "Life Regenerated per second", "Life per second")
        { }
    }
}
