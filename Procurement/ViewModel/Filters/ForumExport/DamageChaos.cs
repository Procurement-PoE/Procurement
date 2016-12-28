namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageChaos : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageChaos()
            : base("Adds Chaos Damage", "Adds Chaos Damage", "Adds \\d+ to \\d+ Chaos Damage")
        { }
    }
}
