using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class BlacksmithsWhetstoneRecipe : MinimumQualityRecipe<Gear>
    {
        public override string Name
        {
            get { return "1 Blacksmith's Whetstone"; }
        }
        
        protected override IEnumerable<Gear> getCandidateItems(IEnumerable<Item> items)
        {
            return items.OfType<Gear>().Where(a => a.GearType == GearType.Axe || a.GearType == GearType.Bow
                || a.GearType == GearType.Claw || a.GearType == GearType.Dagger || a.GearType == GearType.Mace
                || a.GearType == GearType.Quiver || a.GearType == GearType.Sceptre || a.GearType == GearType.Staff
                || a.GearType == GearType.Sword || a.GearType == GearType.Wand || a.GearType == GearType.FishingRod)
                .Where(a => a.IsQuality);
        }

        protected override string getMissingCombinationText(decimal requiredQuality, decimal qualityFound)
        {
            return string.Format("Weapon(s) with quality totaling {0}%", requiredQuality - qualityFound);
        }
    }
}