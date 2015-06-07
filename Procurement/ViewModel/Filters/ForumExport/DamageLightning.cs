using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class DamageLightning: StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public DamageLightning()
            : base("Adds Lightning Damage", "Adds Lightning Damage", "Adds \\d+\\-\\d+ Lightning Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "Добавляет \\d+\\-\\d+ к урону от молнии" };
                this.keyword = "Урон от молнии";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
