using POEApi.Model;
using System;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    class FiveBlueSockets : IFilter
    {
        public bool CanFormCategory
        {
            get { throw new NotImplementedException(); }
        }

        public string Keyword
        {
            get { return "At Least 5 Blue Sockets"; }
        }

        public string Help
        {
            get { return "Gear with 5 or more blue sockets"; }
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

            return gear.Sockets.Where(s => s.Attribute.Equals("I", StringComparison.OrdinalIgnoreCase)).Count() >= 5;
        }
    }
}
