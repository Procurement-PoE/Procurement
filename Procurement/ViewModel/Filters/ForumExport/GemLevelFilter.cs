using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class GemLevelFilter : StatFilter
    {
        internal GemLevelFilter(string keyword)
            : base("Increased " + keyword + " gem level", "Items that increases the level of " + keyword + " gems", "to level of " + keyword + " gems")
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }
    }

    internal class AllGemLevelFilter : StatFilter
    {
        public AllGemLevelFilter()
            : base("Increased all gem level", "Items that increases the level of gems", "to Level of Gems in this item")
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }
    }

    internal class ColdGemLevelFilter : GemLevelFilter
    {
        public ColdGemLevelFilter()
            : base("cold")
        { }
    }

    internal class FireGemLevelFilter : GemLevelFilter
    {
        public FireGemLevelFilter()
            : base("fire")
        { }
    }

    internal class LightningGemLevelFilter : GemLevelFilter
    {
        public LightningGemLevelFilter()
            : base("lightning")
        { }
    }

    internal class MeleeGemLevelFilter : GemLevelFilter
    {
        public MeleeGemLevelFilter()
            : base("melee")
        { }
    }

    internal class BowGemLevelFilter : GemLevelFilter
    {
        public BowGemLevelFilter()
            : base("bow")
        { }
    }
}
