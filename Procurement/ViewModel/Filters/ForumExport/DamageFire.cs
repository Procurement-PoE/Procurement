namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageFire : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageFire()
            : base("Adds Fire Damage", "Adds Fire Damage", "Adds \\d+\\-\\d+ Fire Damage")
        { }
    }
}
