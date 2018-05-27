using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FatedUniqueBaseTypesFilter : IFilter
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
            get { return "Fated Unique Base Types"; }
        }

        public string Help
        {
            get { return "Unidentified unique items that share a base type with a Fated Unique."; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gear gear = item as Gear;
            if (gear == null || gear.Rarity != Rarity.Unique || gear.Identified)
                return false;

            // TODO: Unique items each have their own, well, unique icon URL.  It is possible to build a map from icon
            //       URL to unique item, so we can know which unique item an unidentified unique is, and therefore
            //       whether it is a fated unique target.  However, alternate art items complicate this.
            return Settings.FatedUniques.Any(u => string.Equals(u.BaseTypeName, gear.BaseType,
                System.StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
