using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class BuyoutFilter : IFilter
    {
        private string buyoutValue;
        public BuyoutFilter(string buyoutValue)
        {
            this.buyoutValue = buyoutValue;
        }
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return string.Empty; }
        }

        public string Help
        {
            get { return "Returns all items with matching buyout set"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            return Settings.Buyouts.ContainsKey(item.UniqueIDHash) && Settings.Buyouts[item.UniqueIDHash].Buyout.ToLower() == buyoutValue.ToLower();
        }
    }
}
