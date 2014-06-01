using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    internal class CurrencyHandler
    {
        internal static double GetChaosValue(OrbType type)
        {
            if (!Settings.CurrencyRatios.ContainsKey(type))
                return 0;

            CurrencyRatio ratio = Settings.CurrencyRatios[type];

            if (ratio.OrbAmount == 1)
                return ratio.OrbAmount * ratio.ChaosAmount;

            return ratio.ChaosAmount / ratio.OrbAmount;
        }

        public static double GetTotal(OrbType target, IEnumerable<Currency> currency)
        {
            double total = 0;

            foreach (var orb in currency)
                total += orb.StackInfo.Amount * orb.ChaosValue;

            var ratioToChaos = Settings.CurrencyRatios[target];

            total *= (ratioToChaos.OrbAmount / ratioToChaos.ChaosAmount);

            return total;
        }

        public static Dictionary<OrbType, double> GetTotalCurrencyDistribution(OrbType target, IEnumerable<Currency> currency)
        {
            return currency.Where(o => !o.TypeLine.Contains("Shard"))
                           .GroupBy(orb => orb.Type)
                           .Where(group => GetTotal(target, group) > 0)
                           .Select(grp => new { Key = grp.Key, Value = GetTotal(target, grp) })
                           .OrderByDescending(at => at.Value)
                           .ToDictionary(at => at.Key, at => at.Value);
        }

        public static Dictionary<OrbType, double> GetTotalCurrencyCount(IEnumerable<Currency> currency)
        {
            return currency.Where(o => !o.TypeLine.Contains("Shard"))
                           .GroupBy(orb => orb.Type)
                           .Select(grp => new { Key = grp.Key, Value = (double)grp.Sum(c => c.StackInfo.Amount) })
                           .OrderByDescending(at => at.Value)
                           .ToDictionary(at => at.Key, at => at.Value);
        }
    }
}
