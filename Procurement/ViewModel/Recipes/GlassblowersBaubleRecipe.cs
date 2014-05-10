using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class GlassblowersBaubleRecipe : MinimumQualityRecipe<Gear>
    {
        public override string Name
        {
            get { return "1 Glassblower's Bauble"; }
        }

        protected override IEnumerable<Gear> getCandidateItems(IEnumerable<Item> items)
        {
            return items.OfType<Gear>().Where(a => a.GearType == GearType.Flask).Where(a => a.IsQuality);
        }

        protected override string getMissingCombinationText(decimal requiredQuality, decimal qualityFound)
        {
            return string.Format("Flask(s) with quality totaling {0}%", requiredQuality - qualityFound);
        }
    }
}
