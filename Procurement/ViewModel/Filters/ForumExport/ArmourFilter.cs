using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class ArmourFilter : OrStatFilter
    {
        public ArmourFilter()
            : base("Armour", "Items with additional or increased Armour", "to Armour", "increased Armour", "increased Global Armour", "increased Global Defences")
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
