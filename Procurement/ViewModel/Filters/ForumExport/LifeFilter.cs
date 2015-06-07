using POEApi.Model;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class LifeFilter : ExplicitModBase
    {
        public LifeFilter()
            : base("to maximum Life")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.keyword = "к максимуму здоровья";
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
                    return "Максимум к здоровью";
                }
                else
                {
                    return "Maximum Life";
                }
            }
        }

        public override string Help
        {
            get { return "Items with +life"; }
        }

        public override FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }
    }
}
