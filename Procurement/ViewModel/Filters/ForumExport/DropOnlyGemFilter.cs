using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class DropOnlyGemFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        private List<string> dropOnly;

        public DropOnlyGemFilter()
        {
            //From http://en.pathofexilewiki.com/wiki/Drop_Only_Gems
            dropOnly = new List<string>();
            dropOnly.Add("Added Chaos Damage Support");
            dropOnly.Add("Detonate Mines");
            dropOnly.Add("Empower Support");
            dropOnly.Add("Enhance Support");
            dropOnly.Add("Enlighten Support");
            dropOnly.Add("Portal");
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Drop-Only Gems"; }
        }

        public string Help
        {
            get { return "Gems which can only be aquired through drops"; }
        }

        public bool Applicable(Item item)
        {
            Gear gear = item as Gear;
            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return dropOnly.Contains(gem.TypeLine);
        }
    }
}
