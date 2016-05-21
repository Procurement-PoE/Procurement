namespace Procurement.ViewModel.Filters.ForumExport
{
    public class PercentLifeFilter : StatFilter
    {
        public PercentLifeFilter()
            : base("% Increased Life", "Items with % increased life", "% increased maximum life")
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
