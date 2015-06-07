using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    internal class ItemRarityFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.MagicFind; }
        }

        public ItemRarityFilter()
            : base("Item Rarity", "Item with the Item Rarity stat", "INCREASED RARITY")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "повышение редкости находимых предметов" };
                this.keyword = "Увеличение редкости предметов";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
