using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class ArmourersScrapRecipe : MinimumQualityRecipe<Gear>
    {
        public override string Name
        {
            get { return "1 Armourer's Scrap"; }
        }
        
        protected override IEnumerable<Gear> getCandidateItems(IEnumerable<Item> items)
        {
            return items.OfType<Gear>().Where(a => a.GearType == GearType.Boots || a.GearType == GearType.Chest
                || a.GearType == GearType.Gloves || a.GearType == GearType.Helmet || a.GearType == GearType.Shield)
                .Where(a => a.IsQuality);
        }

        protected override string getMissingCombinationText(decimal requiredQuality, decimal qualityFound)
        {
            return string.Format("Armor with quality totaling {0}%", requiredQuality - qualityFound);
        }
    }
}