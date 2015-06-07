using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    public class ItemQuantityFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.MagicFind; }
        }

        public ItemQuantityFilter()
            : base("Item Quantity", "Item with the Item Quantity stat", "INCREASED QUANTITY")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "увеличение количества находимых предметов" };
                this.keyword = "Увеличение количества предметов";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
