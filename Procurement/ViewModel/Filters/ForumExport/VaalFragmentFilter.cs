using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procurement.ViewModel.Filters
{
    public class VaalFragmentFilter : TypeLineFilter
    {
        public VaalFragmentFilter()
            : base("Sacrifice at Dusk", "Sacrifice at Midnight", "Sacrifice at Noon", "Sacrifice at Dawn")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keywords = new string[] { "Жертва на закате", "Жертва в полночь", "Жертва в полдень", "Жертва на рассвете" };
            }
        }
        public override bool CanFormCategory
        {
            get { return true; }
        }

        public override string Keyword
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Фрагменты Ваал";
                }
                else
                {
                    return "Vaal Fragments";
                }
            }
        }

        public override string Help
        {
            get
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "Фрагменты Ваал";
                }
                else
                {
                    return "Vaal Fragments";
                }
            }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.VaalFragments; }
        }
    }
}
