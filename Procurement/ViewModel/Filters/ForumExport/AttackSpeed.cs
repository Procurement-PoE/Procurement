namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class AttackSpeed : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public AttackSpeed()
            : base("Increased Attack Speed", "Increased Attack Speed", "increased Attack Speed")
        { }
    }
}
