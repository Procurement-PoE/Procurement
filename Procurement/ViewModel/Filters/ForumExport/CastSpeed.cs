using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class CastSpeed : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public CastSpeed()
            : base("Increased Cast Speed", "Increased Cast Speed", "Increased Cast Speed")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "повышение скорости сотворения чар" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
