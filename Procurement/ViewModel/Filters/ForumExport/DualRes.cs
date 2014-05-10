using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters
{
    public class DualResistances : ResistanceBase, IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Dual Resists"; }
        }

        public string Help
        {
            get { return "Items with dual resists"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            return resistances.Count(r => r.Applicable(item)) == 2;
        }
    }
}
