namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class ManaLeech : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public ManaLeech()
            : base("Mana leech", "Items with mana leech", "Leeched as mana")
        { }
    }
}
