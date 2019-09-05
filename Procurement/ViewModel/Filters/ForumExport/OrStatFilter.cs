using POEApi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    public abstract class OrStatFilter : StatFilterBase, IFilter
    {
        public abstract FilterGroup Group { get; }

        public OrStatFilter(string keyword, string help, params string[] stats) 
            : base (keyword, help, stats)
        { }

        public bool Applicable(POEApi.Model.Item item)
        {
            Gear gear = item as Gear;
            if (gear == null)
                return false;

            List<Regex> pool = new List<Regex>(stats);
            List<string> all = new List<string>();

            if (gear.Implicitmods != null)
                all.AddRange(gear.Implicitmods.Select(s => s));

            if (gear.Explicitmods != null)
                all.AddRange(gear.Explicitmods.Select(s => s));

            if (gear.Craftedmods != null)
                all.AddRange(gear.Craftedmods.Select(s => s));

            if (gear.Fracturedmods != null)
                all.AddRange(gear.Fracturedmods.Select(s => s));

            if (gear.Enchantmods != null)
                all.AddRange(gear.Enchantmods.Select(s => s));

            foreach (string stat in all)
            {
                Regex result = pool.Find(s => s.IsMatch(stat));

                if (result != null)
                    return true;
            }

            return false;
        }
    }
}
