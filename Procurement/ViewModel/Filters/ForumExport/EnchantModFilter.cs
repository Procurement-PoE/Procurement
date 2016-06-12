using POEApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class EnchantModFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Enchanted"; }
        }

        public string Help
        {
            get { return "Items with enchantments"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            return item.EnchantMods != null && item.EnchantMods.Count() > 0;
        }
    }
}
