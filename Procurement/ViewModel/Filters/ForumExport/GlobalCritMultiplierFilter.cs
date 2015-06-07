using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class GlobalCritMultiplierFilter : ExplicitModBase
    {
        public GlobalCritMultiplierFilter()
            : base("increased Global Critical Strike Multiplier")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "увеличение глобального множителя критического удара";
            }
        }

        public override bool CanFormCategory
        {
            get { return false; }
        }

        public override string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Глобальный множитель критического удара";
                }
                else
                {
                    return "Global Crit Multiplier";
                }
            }
        }

        public override string Help
        {
            get { return "Items with increased Global Critical Strike Multiplier"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Crit; }
        }
    }
}
