namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageFire : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageFire()
            : base("Adds Fire Damage", "Adds Fire Damage", "Adds \\d+ to \\d+ Fire Damage", "as Extra Fire Damage", "as Extra Damage of each Element", "as Extra Damage of a random Element")
        { }
    }
}
