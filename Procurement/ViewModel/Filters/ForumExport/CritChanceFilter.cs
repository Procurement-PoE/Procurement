using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class CritChanceFilter : ExplicitModBase
    {
        public CritChanceFilter()
            : base("Critical Strike Chance")
        { }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Critical Strike Chance"; }
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
