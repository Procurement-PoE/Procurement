using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FatedUniquesFilter : IFilter
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
            get { return "Fated Uniques"; }
        }

        public string Help
        {
            get { return "Unique items upgraded via Prophecy."; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gear gear = item as Gear;
            if (gear == null || gear.Rarity != Rarity.Unique || !gear.Identified)
                return false;

            return Settings.FatedUniques.Any(u => string.Equals(u.FatedItemName, gear.Name,
                System.StringComparison.CurrentCultureIgnoreCase));
        }
    }
}