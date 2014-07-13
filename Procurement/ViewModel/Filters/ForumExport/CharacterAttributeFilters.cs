namespace Procurement.ViewModel.Filters.ForumExport
{
    class StrengthFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public StrengthFilter()
            : base("Increased Strength", "Strength", "Strength")
        { }
    }


    class IntelligenceFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public IntelligenceFilter()
            : base("Increased Intelligence", "Intelligence", "Intelligence")
        { }
    }

    class DexterityFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public DexterityFilter()
            : base("Increased Dexterity", "Increased Dexterity", "Dexterity")
        { }
    }
}
