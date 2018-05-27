using POEApi.Model;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class FatedUniquePropheciesFilter : IFilter
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
            get { return "Fated Unique Prophecies"; }
        }

        public string Help
        {
            get { return "Prophecies which can upgrade uniques to Fated Uniques."; }
        }

        public bool Applicable(POEApi.Model.Item item)
        {
            Prophecy prophecy = item as Prophecy;
            if (prophecy == null)
                return false;

            return Settings.FatedUniques.Any(u => string.Equals(u.ProphecyName, prophecy.TypeLine,
                System.StringComparison.CurrentCultureIgnoreCase));
        }
    }
}