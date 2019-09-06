namespace Procurement.ViewModel.Filters
{
    public class VaalUberFragmentFilter : TypeLineFilter
    {
        public VaalUberFragmentFilter()
            : base("Mortal Grief", "Mortal Rage", "Mortal Hope", "Mortal Ignorance")
        { }
        public override bool CanFormCategory
        {
            get { return true; }
        }

        public override string Keyword
        {
            get { return "Uber Vaal Fragments"; }
        }

        public override string Help
        {
            get { return "Uber Vaal Fragments"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.VaalFragments; }
        }
    }
}
