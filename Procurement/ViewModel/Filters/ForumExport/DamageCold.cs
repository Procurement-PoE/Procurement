namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageCold : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageCold()
            : base("Adds Cold Damage", "Adds Cold Damage", "Adds \\d+ to \\d+ Cold Damage", "as Extra Cold Damage", "as Extra Damage of each Element", "as Extra Damage of a random Element")
        { }
    }
}
