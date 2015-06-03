using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class ManaLeech : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public ManaLeech()
            : base("Mana leech", "Items with mana leech", "Leeched as mana")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "похищается в виде маны" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
