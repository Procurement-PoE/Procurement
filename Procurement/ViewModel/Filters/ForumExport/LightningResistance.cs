using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.ViewModel.Filters
{
    public class LightningResistance : OrStatFilter
    {
        public override  FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public LightningResistance()
            : base("Lightning Resistance", "Lightning Resistance", "to Lightning Resistance", "to Cold and Lightning Resistances", "to Fire and Lightning Resistances")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к сопротивлению молнии", "к сопротивлению холоду и молнии", "к сопротивлению огню и молнии" };
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}