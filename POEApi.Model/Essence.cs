using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public class Essence : StackableItem
    {
        public Essence(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetEssenceType(item);
        }

        public EssenceType Type { get; }

        //This doesn't appear to be used
        protected override int getConcreteHash()
        {
            //Arbitrary number.
            return 15;
        }

        /// <summary>
        ///     Method to generate the OrbType enum for essences
        /// </summary>
        /// <returns>List to paste into Orb type</returns>
        public static List<string> EssenceGenerator()
        {
            var tiers = new[] { "Whispering", "Muttering", "Weeping", "Wailing", "Screaming", "Shrieking", "Deafening" };
            var types = new[]
            {
                "Greed", "Contempt", "Hatred", "Woe", "Fear", "Anger", "Torment", "Sorrow", "Rage", "Suffering", "Wrath", "Doubt", "Loathing", "Zeal",
                "Anguish", "Spite", "Scorn", "Envy", "Misery", "Dread"
            };

            var specialEssences = new[] { "Insanity", "Horror", "Delirium", "Hysteria" };

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

            essences.AddRange(specialEssences.Select(type => "EssenceType." + type + ","));

            essences.Add("EssenceType.Corruption,");

            return essences;
        }
    }
}