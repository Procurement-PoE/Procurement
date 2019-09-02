namespace Procurement.ViewModel.Filters
{
    public class ProphecyFragmentFilter : TypeLineFilter
    {
        public ProphecyFragmentFilter()
            : base("Volkuur's Key", "Eber's Key", "Yriel's Key", "Inya's Key")
        { }
        public override bool CanFormCategory
        {
            get { return true; }
        }

         public override string Keyword
        {
            get { return "Prophecy Fragments"; }
        }

         public override string Help
        {
            get { return "Prophecy Fragments"; }
        }

         public override FilterGroup Group
        {
            get { return FilterGroup.MapFragments; }
        }
    }
}