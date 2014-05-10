namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageCold : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageCold()
            : base("Adds Cold Damage", "Adds Cold Damage", "Adds \\d+\\-\\d+ Cold Damage")
        { }
    }
}
