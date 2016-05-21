using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class PercentEnergyShieldFilter : StatFilter
    {
        public PercentEnergyShieldFilter()
            : base("% Increased Energy Shield", "Items with % increased energy shield", "% increased maximum energy shield")
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
