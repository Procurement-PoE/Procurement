using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    class CurseGemFilter : IFilter
    {
        public bool Applicable(Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return curseList.Contains(gem.TypeLine);
        }

        private List<string> curseList;

        public CurseGemFilter()
        {
            curseList = new List<string>();
            curseList.Add("Punishment");
            curseList.Add("Warlord's Mark");
            curseList.Add("Projectile Weakness");
            curseList.Add("Temporal Chains");
            curseList.Add("Conductivity");
            curseList.Add("Critical Weakness");
            curseList.Add("Elemental Weakness");
            curseList.Add("Enfeeble");
            curseList.Add("Flammability");
            curseList.Add("Frostbite");
            curseList.Add("Vulnerability");
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Curse Gems"; }
        }

        public string Help
        {
            get { return "Gems curse targets in an area"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }
    }
}