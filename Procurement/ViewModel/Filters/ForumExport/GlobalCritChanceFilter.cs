using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class GlobalCritChanceFilter : ExplicitModBase
    {
        public GlobalCritChanceFilter()
            : base("Global Critical Strike Chance")
        { }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get { return "Global Critical Strike Chance"; }
        }

        public override string Help
        {
            get { return "Items with increased Global Critical Strike Chance"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Crit; }
        }
    }
}