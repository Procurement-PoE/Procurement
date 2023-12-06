using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    public class StatFilterBase
    {
        protected string keyword;
        protected string help;
        protected List<Regex> stats;

        public StatFilterBase(string keyword, string help, params string[] stats)
        {
            this.keyword = keyword;
            this.help = help;
            this.stats = stats.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).ToList();
        }
        public virtual bool CanFormCategory
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
    }
}
