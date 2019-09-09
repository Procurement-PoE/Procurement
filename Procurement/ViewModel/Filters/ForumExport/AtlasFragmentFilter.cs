namespace Procurement.ViewModel.Filters
{
    public class AtlasFragmentFilter : TypeLineFilter
    {
        public AtlasFragmentFilter()
            : base("Fragment of the Hydra", "Fragment of the Phoenix", "Fragment of the Minotaur", "Fragment of the Chimera")
        { }
        public override bool CanFormCategory
        {
            get { return true; }
        }

        public override string Keyword
        {
            get { return "Atlas Fragments"; }
        }

        public override string Help
        {
            get { return "Atlas Fragments"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.MapFragments; }
        }
    }
}