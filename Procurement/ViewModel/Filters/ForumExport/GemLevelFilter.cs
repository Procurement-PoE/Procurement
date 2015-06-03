using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class GemLevelFilter : StatFilter
    {
        internal GemLevelFilter(string keyword)
            : base("Increased " + keyword + " gem level", "Items that increases the level of " + keyword + " gems", "to level of " + keyword + " gems")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к уровню размещённых камней" };
                switch (keyword)
                {
                    case "cold":
                        stats_ru[0] = "к уровню размещённых камней холода";
                        break;
                    case "fire":
                        stats_ru[0] = "к уровню размещённых камней огня";
                        break;
                    case "lightning":
                        stats_ru[0] = "к уровню размещённых камней молнии";
                        break;
                    case "melee":
                        stats_ru[0] = "к уровню размещённых камней ближнего боя";
                        break;
                    case "bow":
                        stats_ru[0] = "к уровню размещённых камней лука";
                        break;
                    default:
                        break;
                }
                this.keyword = "Increased " + keyword + " gem level";
                this.help="Items that increases the level of " + keyword + " gems";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }
    }

    internal class AllGemLevelFilter : StatFilter
    {
        public AllGemLevelFilter()
            : base("Increased all gem level", "Items that increases the level of gems", "to Level of Gems in this item")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к уровню размещённых камней" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }
    }

    internal class ColdGemLevelFilter : GemLevelFilter
    {
        public ColdGemLevelFilter()
            : base("cold")
        {
        }
    }

    internal class FireGemLevelFilter : GemLevelFilter
    {
        public FireGemLevelFilter()
            : base("fire")
        {
        }
    }

    internal class LightningGemLevelFilter : GemLevelFilter
    {
        public LightningGemLevelFilter()
            : base("lightning")
        {
        }
    }

    internal class MeleeGemLevelFilter : GemLevelFilter
    {
        public MeleeGemLevelFilter()
            : base("melee")
        {
        }
    }

    internal class BowGemLevelFilter : GemLevelFilter
    {
        public BowGemLevelFilter()
            : base("bow")
        {
        }
    }

    //TODO: add Aura filter, "аур"
}
