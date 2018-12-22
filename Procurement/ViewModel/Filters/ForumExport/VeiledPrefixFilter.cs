using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class VeiledPrefixFilter : IFilter
    {
        public bool CanFormCategory
        {
            get { return false; }
        }

        public string Keyword
        {
            get { return "Veiled Prefixes"; }
        }

        public string Help
        {
            get { return "Items with veiled prefixes"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            if (item.VeiledMods == null)
                return false;

            return item.VeiledMods.Count > 0 && item.VeiledMods.Any(x => x.StartsWith("Prefix"));
        }
    }
}
