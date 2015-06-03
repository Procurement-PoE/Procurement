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
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                dropOnly.Add("Усилитель");
                dropOnly.Add("Портал");
                dropOnly.Add("Сокращение");
                dropOnly.Add("Замедленные снаряды");
                dropOnly.Add("Улучшитель");
            }
            else
            {
                dropOnly.Add("Empower");
                dropOnly.Add("Portal");
                dropOnly.Add("Reduced Duration");
                dropOnly.Add("Slower Projectiles");
                dropOnly.Add("Enhance");
            }
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Drop Only Gems"; }
        }

        public string Help
        {
            get { return "Gems only which can only be aquired through drops"; }
        }

        public bool Applicable(Item item)
        {
            Gem gem = item as Gem;
            if (gem == null)
                return false;

            return dropOnly.Contains(gem.TypeLine);
        }
    }
}
