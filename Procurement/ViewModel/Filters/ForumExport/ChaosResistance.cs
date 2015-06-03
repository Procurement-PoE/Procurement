using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    internal class ChaosResistance : StatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public ChaosResistance()
            : base("Chaos Resistance", "Chaos Resistance", "to Chaos Resistance")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к сопротивлению хаосу" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}

