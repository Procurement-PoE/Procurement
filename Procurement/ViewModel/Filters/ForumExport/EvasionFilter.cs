using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class EvasionFilter : OrStatFilter
    {
        public EvasionFilter()
            : base("Evasion", "Items with additional or increased Evasion", "Evasion Rating", "increased Armour and Evasion", "increased Evasion and Energy Shield", "increased Armour, Evasion and Energy Shield", "increased Global Defences")
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
