namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class AttackSpeed : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public AttackSpeed()
            : base("Increased Attack Speed", "Increased Attack Speed", "increased Attack Speed", "increased Attack and Cast Speed", "increased Attack, Cast and Movement Speed")
        { }
    }
}
