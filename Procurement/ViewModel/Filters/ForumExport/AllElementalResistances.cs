using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    internal class AllElementalResistances : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public AllElementalResistances()
            : base("All elemental Resistances", "All elemental Resistances", "to all Elemental Resistances")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к сопротивлению всем стихиям" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}

