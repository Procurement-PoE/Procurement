using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class IncreasedDamageFilter : StatFilter
    {
        internal IncreasedDamageFilter(string keyword, string help, params string[] stats)
            : base(keyword, help, stats)
        { }

        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }
    }

    internal class IncreasedDamageFilterCold : IncreasedDamageFilter
    {
        public IncreasedDamageFilterCold()
            : base("Increased Cold Damage", "Increased Cold Damage", "Increased Cold Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "увеличение урона от холода" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }

    internal class IncreasedDamageFilterFire : IncreasedDamageFilter
    {
        public IncreasedDamageFilterFire()
            : base("Increased Fire Damage", "Increased Fire Damage", "Increased Fire Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "увеличение урона от огня" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }

    internal class IncreasedDamageFilterLightning : IncreasedDamageFilter
    {
        public IncreasedDamageFilterLightning()
            : base("Increased Lightning Damage", "Increased Lightning Damage", "Increased Lightning Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "увеличение урона от молнии" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
