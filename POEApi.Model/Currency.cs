using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public class Currency : Item
    {
        public Currency(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetOrbType(item);
            ChaosValue = CurrencyHandler.GetChaosValue(Type);
            StackInfo = ProxyMapper.GetStackInfo(item.Properties);
        }

        public OrbType Type { get; }
        public double ChaosValue { get; private set; }
        public StackInfo StackInfo { get; private set; }

        protected override int getConcreteHash()
        {
            var anonomousType = new
            {
                f = IconURL,
                f2 = Name,
                f3 = TypeLine,
                f4 = DescrText,
                f5 = X,
                f6 = Y,
                f7 = InventoryId
            };

            return anonomousType.GetHashCode();
        }

        /// <summary>
        ///     Method to generate the OrbType enum for essences
        /// </summary>
        /// <returns>List to paste into Orb type</returns>
        public static List<string> EssenceGenerator()
        {
            var tiers = new[] {"Whispering", "Muttering", "Weeping", "Wailing", "Screaming", "Shrieking", "Deafening"};
            var types = new[]
            {
                "Greed", "Contempt", "Hatred", "Woe", "Fear", "Anger", "Torment", "Sorrow", "Rage", "Suffering", "Wrath", "Doubt", "Loathing", "Zeal",
                "Anguish", "Spite", "Scorn", "Envy", "Misery", "Dread"
            };

            var specialEssences = new[] {"Insanity", "Horror", "Delirium", "Hysteria"};

            var essences = new List<string>();
            var upperBound = 0;
            foreach (var tier in tiers)
            {
                upperBound += 4;
                for (var e = 0; e < upperBound; e++)
                {
                    if (types.Length <= e)
                        continue;

                    var type = types[e];
                    essences.Add("OrbType." + tier + type + ",");
                }
            }

            essences.AddRange(specialEssences.Select(type => "OrbType." + type + ","));

            essences.Add("OrbType.Corruption,");

            return essences;
        }
    }
}