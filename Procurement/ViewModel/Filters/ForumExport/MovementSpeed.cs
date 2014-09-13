namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class MovementSpeed : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public MovementSpeed()
            : base("Movement speed", "Items with movement speed", "movement speed")
        { }
    }
}
