using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters.ForumExport
{
    internal class AccurayFilter : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Attacks; }
        }

        public AccurayFilter()
            : base("Increased Accuracy", "Increased Accuracy", "Accuracy")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к точности" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
