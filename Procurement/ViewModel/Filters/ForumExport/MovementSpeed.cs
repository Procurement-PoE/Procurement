using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class MovementSpeed : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public MovementSpeed()
            : base("Movement speed", "Items with movement speed", "movement speed")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "повышение скорости передвижения" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
