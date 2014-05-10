using System;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    internal class LeveledGemFilter : IFilter
    {
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
            get { return "Leveled Gems"; }
        }

        public string Help
        {
            get { return "Leveled Gems"; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            try
            {
                return gem.Properties[1].Values[0].Item1 != "1";
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
