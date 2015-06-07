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
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "увеличение урона чар";
                //TODO: "увеличение урона от чар"
            }
        }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Урон от чар";
                }
                else
                {
                    return "Spell Damage";
                }
            }
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
