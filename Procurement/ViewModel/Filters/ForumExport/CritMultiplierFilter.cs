using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class CritMultiplierFilter : ExplicitModBase
    {
        public CritMultiplierFilter()
            : base("Critical Strike Multiplier")
        { }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Critical Strike Multiplier"; }
        }

        public override string Help
        {
            get { return "Items with additional Critical Strike Multiplier"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Crit; }
        }
    }
}
