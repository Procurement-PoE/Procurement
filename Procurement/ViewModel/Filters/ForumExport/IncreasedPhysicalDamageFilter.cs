using Procurement.ViewModel.Filters.ForumExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters
{
    public class IncreasedPhysicalDamageFilter : OrStatFilter
    {
        public override bool CanFormCategory
        {
            get { return false; }
        }
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }
        public IncreasedPhysicalDamageFilter()
            : base("Increased Physical Damage", "Items with Increased Physical Damage", "increased Physical Damage", "increased Global Physical Damage")
        { }
    }
}
