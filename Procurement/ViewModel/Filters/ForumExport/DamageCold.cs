using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageCold : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageCold()
            : base("Adds Cold Damage", "Adds Cold Damage", "Adds \\d+\\-\\d+ Cold Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "Добавляет \\d+\\-\\d+ к урону от холода" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
