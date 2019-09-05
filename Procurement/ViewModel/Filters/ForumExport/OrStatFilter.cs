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

            if (gear.CraftedMods != null)
                all.AddRange(gear.CraftedMods.Select(s => s));

            if (gear.FracturedMods != null)
                all.AddRange(gear.FracturedMods.Select(s => s));

            if (gear.EnchantMods != null)
                all.AddRange(gear.EnchantMods.Select(s => s));

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
