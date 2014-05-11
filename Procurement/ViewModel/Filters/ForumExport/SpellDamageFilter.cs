using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class SpellDamageFilter : ExplicitModBase
    {
        public SpellDamageFilter()
            : base("increased Spell Damage")
        { }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Spell Damage"; }
        }

        public override string Help
        {
            get { return "Items with increased Spell Damage"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
