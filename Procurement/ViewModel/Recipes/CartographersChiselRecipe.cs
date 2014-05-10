using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel.Recipes
{
    internal class CartographersChiselRecipe : MinimumQualityRecipe<Map>
    {
        public override string Name
        {
            get { return "1 Cartographer's Chisel"; }
        }

        protected override IEnumerable<Map> getCandidateItems(IEnumerable<Item> items)
        {
            return items.OfType<Map>().Where(a => a.IsQuality);
        }

        protected override string getMissingCombinationText(decimal requiredQuality, decimal qualityFound)
        {
            return string.Format("Map(s) with quality totaling {0}%", requiredQuality - qualityFound);
        }
    }
}
