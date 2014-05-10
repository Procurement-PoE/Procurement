using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class GCPRecipe : MinimumQualityRecipe<Gem>
    {
        public override string Name
        {
            get { return "1 GCP"; }
        }

        protected override IEnumerable<Gem> getCandidateItems(IEnumerable<Item> items)
        {
            return items.OfType<Gem>().Where(g => g.IsQuality);
        }

        protected override string getMissingCombinationText(decimal requiredQuality, decimal qualityFound)
        {
            return string.Format("Gem(s) with quality totaling {0}%", requiredQuality - qualityFound);
        }
    }
}
