namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageLightning: OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageLightning()
            : base("Adds Lightning Damage", "Adds Lightning Damage", "Adds \\d+ to \\d+ Lightning Damage", "as Extra Lightning Damage", "as Extra Damage of each Element", "as Extra Damage of a random Element")
        { }
    }
}
