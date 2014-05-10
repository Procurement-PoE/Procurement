using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procurement.ViewModel.Filters;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class AuraGemsFilter : IFilter
    {
        public bool Applicable(Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return auraList.Contains(gem.TypeLine);
        }

        private List<string> auraList;

        public AuraGemsFilter()
        {
            auraList = new List<string>();
            auraList.Add("Anger");
            auraList.Add("Determination");
            auraList.Add("Vitality");
            auraList.Add("Grace");
            auraList.Add("Haste");
            auraList.Add("Hatred");
            auraList.Add("Clarity");
            auraList.Add("Discipline");
            auraList.Add("Purity");
            auraList.Add("Wrath");
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Aura Gems"; }
        }

        public string Help
        {
            get { return "Gems that buff the player with an aura"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }
    }
}
