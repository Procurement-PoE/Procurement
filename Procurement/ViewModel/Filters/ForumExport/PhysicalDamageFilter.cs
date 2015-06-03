using POEApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class PhysicalDamageFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Damage; }
        }

        public PhysicalDamageFilter()
            : base("Adds Physical Damage", "Adds Physical Damage", "Adds \\d+\\-\\d+ Physical Damage")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "Добавляет \\d+\\-\\d+ физического урона" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}