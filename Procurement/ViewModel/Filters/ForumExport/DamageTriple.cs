using System.Collections.Generic;
using System.Linq;
using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageTriple : IFilter
    {
        private List<OrStatFilter> resistances;

        public FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageTriple()
        {
            resistances = new List<OrStatFilter>();
            resistances.Add(new DamageCold());
            resistances.Add(new DamageFire());
            resistances.Add(new DamageLightning());
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return "Triple Elemental Damage"; }
        }

        public string Help
        {
            get { return "Returns items with Triple Elemental Damage"; }
        }

        public bool Applicable(Item item)
        {
            return resistances.Count(r => r.Applicable(item)) >= 3;
        }

        public bool IsChecked { get; set; }
    }
}
