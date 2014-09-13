
namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class LifeLeech : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public LifeLeech()
            : base("Life leech", "Items with life leech", "Leeched as life")
        { }
    }
}
