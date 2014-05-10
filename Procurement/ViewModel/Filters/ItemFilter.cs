using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public class ItemFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return ""; }
        }

        public string Help
        {
            get { return ""; }
        }

        private Item source;
        public ItemFilter(Item source)
        {
            this.source = source;
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            return (item.inventoryId == source.inventoryId &&
                    item.X == source.X &&
                    item.Y == source.Y);
        }
    }
}
