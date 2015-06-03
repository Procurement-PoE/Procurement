using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    public class FireResistance : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public FireResistance()
            : base("Fire Resistance", "Fire Resistance", "to Fire Resistance", "to Fire and Lightning Resistances", "to Fire and Cold Resistances")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к сопротивлению огню", "к сопротивлению огню и молнии", "к сопротивлению огню и холоду" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}

