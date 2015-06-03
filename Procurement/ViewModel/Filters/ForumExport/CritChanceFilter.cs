using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class CritChanceFilter : ExplicitModBase
    {
        public CritChanceFilter()
            : base("increased Critical Strike Chance")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "повышение шанса критического удара";
            }
        }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Crit Chance"; }
        }

        public override string Help
        {
            get { return "Items with increased Critical Strike Chance"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Crit; }
        }
    }
}
