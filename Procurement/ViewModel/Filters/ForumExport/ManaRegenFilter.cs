namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ManaRegenFilter : OrStatFilter
    {
        public override bool CanFormCategory
        {
            get { return false; }
        }
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
        public ManaRegenFilter()
            : base("Mana regen", "Items with increased Mana Regeneration Rate", "increased Mana Regeneration Rate", "Mana per second", "Mana Regenerated per second", "as extra Mana Regeneration")
        { }
    }
}
