using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using POEApi.Model;

namespace Procurement.ViewModel.Filters
{
    public abstract class StatFilter : IFilter
    {
        private string keyword;
        private string help;
        private List<Regex> stats;

        public abstract FilterGroup Group { get; }

        public StatFilter(string keyword, string help, params string[] stats)
        {
            this.keyword = keyword;
            this.help = help;
            this.stats = stats.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
        }
        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return keyword; }
        }

        public string Help
        {
            get { return help; }
        }

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

            foreach (string stat in all)
            {
                Regex result = pool.Find(s => s.IsMatch(stat));
                pool.Remove(result);
            }

            return pool.Count == 0;
        }
   }
}