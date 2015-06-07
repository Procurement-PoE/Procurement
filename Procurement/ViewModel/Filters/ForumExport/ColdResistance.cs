using System.Linq;
using System.Text.RegularExpressions;


namespace Procurement.ViewModel.Filters
{
    internal class ColdResistance : OrStatFilter
    {
        public override FilterGroup Group
        {
            get { return FilterGroup.Resistances; }
        }

        public ColdResistance()
            : base("Cold Resistance", "Cold Resistance", "to Cold Resistance", "to Fire and Cold Resistances", "to Cold and Lightning Resistances")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                string[] stats_ru = { "к сопротивлению холоду", "к сопротивлению огню и холоду", "к сопротивлению холоду и молнии" };
                this.keyword = "Сопротивление к холоду";
                this.stats = stats_ru.Select(stat => new Regex(stat, RegexOptions.Singleline | RegexOptions.IgnoreCase)).ToList();
            }
        }
    }
}
