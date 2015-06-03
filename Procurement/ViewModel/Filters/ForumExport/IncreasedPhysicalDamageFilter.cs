using Procurement.ViewModel.Filters.ForumExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters
{
    public class IncreasedPhysicalDamageFilter : ExplicitModBase
    {
        public IncreasedPhysicalDamageFilter()
            : base("increased Physical Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "увеличение физического урона";
            }
        }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Increased Physical Damage"; }
        }

        public override string Help
        {
            get { return "Items with Increased Physical Damage"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }
    }
}
