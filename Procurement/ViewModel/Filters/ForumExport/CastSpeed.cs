namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class CastSpeed : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public CastSpeed()
            : base("Increased Cast Speed", "Increased Cast Speed", "increased Cast Speed", "increased Attack and Cast Speed", "increased Attack, Cast and Movement Speed")
        { }
    }
}
