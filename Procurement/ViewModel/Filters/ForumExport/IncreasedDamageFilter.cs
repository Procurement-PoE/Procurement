﻿namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class IncreasedDamageFilter : StatFilter
    {
        internal IncreasedDamageFilter(string keyword, string help, params string[] stats)
            : base(keyword, help, stats)
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }
    }

    internal class IncreasedDamageFilterCold : IncreasedDamageFilter
    {
        public IncreasedDamageFilterCold()
            : base("Increased Cold Damage", "Increased Cold Damage", "Increased Cold Damage")
        { }
    }

    internal class IncreasedDamageFilterFire : IncreasedDamageFilter
    {
        public IncreasedDamageFilterFire()
            : base("Increased Fire Damage", "Increased Fire Damage", "Increased Fire Damage")
        { }
    }

    internal class IncreasedDamageFilterLightning : IncreasedDamageFilter
    {
        public IncreasedDamageFilterLightning()
            : base("Increased Lightning Damage", "Increased Lightning Damage", "Increased Lightning Damage")
        { }
    }

    internal class IncreasedDamageFilterElemental : IncreasedDamageFilter
    {
        public IncreasedDamageFilterElemental()
            : base("Increased Elemental Damage", "Increased Elemental Damage", "\\d+% increased Elemental Damage$")
        { }
    }

    internal class IncreasedDamageFilterElementalWithAttackSkills : IncreasedDamageFilter
    {
        public IncreasedDamageFilterElementalWithAttackSkills()
            : base("Increased Elemental Damage With Attack Skills", "Increased Elemental Damage With Attack Skills",
            "\\d+% increased Elemental Damage with Attack Skills")
        { }
    }
}
