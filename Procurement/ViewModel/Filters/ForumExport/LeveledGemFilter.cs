using System;
using System.Linq;
using POEApi.Model;
using System.Collections.Generic;

namespace Procurement.ViewModel.Filters
{
    internal class LeveledGemFilter : IFilter
    {
        private int level;

        public LeveledGemFilter(int level)
        {
            this.level = level;
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Gems; }
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return (this.level == 0) ? "Leveled Gems" : "Level " + level.ToString() + " Gems"; }
        }

        public string Help
        {
            get { return (this.level == 0) ? "Leveled Gems" : "Level " + level.ToString() + " Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return gem.Level == this.level;
        }
    }
}
