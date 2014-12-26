using POEApi.Model;
using System;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    class ThreeRedSockets : IFilter
    {
        public bool CanFormCategory
        {
            get { throw new NotImplementedException(); }
        }

        public string Keyword
        {
            get { return "At Least 3 Red Sockets"; }
        }

        public string Help
        {
            get { return "Gear with 3 or more red sockets"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            var gear = item as Gear;

            if (gear == null)
                return false;

            return gear.Sockets.Where(s => s.Attribute.Equals("S", StringComparison.OrdinalIgnoreCase)).Count() >= 3;
        }
    }
}
