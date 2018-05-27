using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FatedUniqueTargetsFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Fated Unique Targets"; }
        }

        public string Help
        {
            get { return "Unique items that can be upgraded via Prophecy."; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gear gear = item as Gear;
            if (gear == null || gear.Rarity != Rarity.Unique || !gear.Identified)
                return false;

            return Settings.FatedUniques.Any(u => string.Equals(u.TargetItemName, gear.Name,
                System.StringComparison.CurrentCultureIgnoreCase));
        }
    }
}