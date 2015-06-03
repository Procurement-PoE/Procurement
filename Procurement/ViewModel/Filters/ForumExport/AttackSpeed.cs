using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class AttackSpeed : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public AttackSpeed()
            : base("Increased Attack Speed", "Increased Attack Speed", "Increased Attack Speed")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "повышение скорости атаки" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
