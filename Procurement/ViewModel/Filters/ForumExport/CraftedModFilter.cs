using POEApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class CraftedModFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Скрафченные моды";
                }
                else
                {
                    return "Crafted Mods";
                }
            }
        }

        public string Help
        {
            get { return "Items with crafted mods"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            return item.CraftedMods != null && item.CraftedMods.Count() > 0;
        }
    }
}
