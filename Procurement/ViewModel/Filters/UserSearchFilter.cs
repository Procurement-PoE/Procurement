using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters
{
    public class UserSearchFilter : IFilter
    {
        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        private string filter;
        public UserSearchFilter(string filter)
        {
            this.filter = filter;
        }
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "User search"; }
        }

        public string Help
        {
            get { return "Matches user search on name/typeline, geartype, mods and texts"; }
        }

        public bool Applicable(Item item)
        {
            if (string.IsNullOrEmpty(filter))
                return false;

            if (item.TypeLine.ToLowerInvariant().Contains(filter.ToLowerInvariant()) || item.Name.ToLowerInvariant().Contains(filter.ToLowerInvariant()) || containsMatchedCosmeticMod(item) || isMatchedGear(item))
                return true;

            if (item.Explicitmods != null)
                foreach (var mod in item.Explicitmods)
                    if (mod.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                        return true;

            if (item.Implicitmods != null)
                foreach (var mod in item.Implicitmods)
                    if (mod.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                        return true;

            if (item.FracturedMods != null)
                foreach (var mod in item.FracturedMods)
                    if (mod.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                        return true;

            if (item.CraftedMods != null)
                foreach (var mod in item.CraftedMods)
                    if (mod.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                        return true;

            if (item.EnchantMods != null)
                foreach (var mod in item.EnchantMods)
                    if (mod.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                        return true;

            if (item.FlavourText != null)
                foreach (var text in item.FlavourText)
                    if (text.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                        return true;

            if (item.DescrText != null)
                if (item.DescrText.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                    return true;

            if (item.SecDescrText != null)
                if (item.SecDescrText.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                    return true;

            if (item.ProphecyText != null)
                if (item.ProphecyText.ToLowerInvariant().Contains(filter.ToLowerInvariant()))
                    return true;

            var gear = item as Gear;

            if (gear != null && gear.SocketedItems.Any(x => Applicable(x)))
                return true;

            return false;
        }

        private bool containsMatchedCosmeticMod(Item item)
        {
            return item.Microtransactions.Any(x => x.ToLowerInvariant().Contains(filter.ToLowerInvariant()));
        }

        private bool isMatchedGear(Item item)
        {
            Gear gear = item as Gear;

            if (gear == null)
                return false;

            return gear.GearType.ToString().ToLowerInvariant().Contains(filter.ToLowerInvariant());
        }
    }
}
