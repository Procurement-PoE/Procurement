using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters
{
    public class ItemQuantityFilter : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.MagicFind; }
        }

        public ItemQuantityFilter()
            : base("Item Quantity", "Items with the Item Quantity stat", "increased Quantity", "increased Item Quantity")
        { }
    }
}
