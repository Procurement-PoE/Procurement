namespace Procurement.ViewModel.Filters.ForumExport
{
    class StrengthFilter : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public StrengthFilter()
            : base("Increased Strength", "Increased Strength", "to Strength", "increased Strength", "to all Attributes", "increased Attributes")
        { }
    }


    class IntelligenceFilter : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public IntelligenceFilter()
            : base("Increased Intelligence", "Increased Intelligence", "to Intelligence", "to Strength and Intelligence", "to Dexterity and Intelligence", "increased Intelligence", "to all Attributes", "increased Attributes")
        { }
    }

    class DexterityFilter : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.CharacterAttributes; }
        }

        public DexterityFilter()
            : base("Increased Dexterity", "Increased Dexterity", "to Dexterity", "to Strength and Dexterity", "increased Dexterity", "to all Attributes", "increased Attributes")
        { }
    }
}
