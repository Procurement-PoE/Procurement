using System.Collections.Generic;
using System.Linq;

namespace Procurement.Tools
{
    internal class EssenceGenerator
    {
        private static readonly string[] _specialEssences = { "Insanity", "Horror", "Delirium", "Hysteria" };
        private static readonly string[] _tiers = { "Whispering", "Muttering", "Weeping", "Wailing", "Screaming", "Shrieking", "Deafening" };
        private static readonly string[] _types =
            {
                "Greed", "Contempt", "Hatred", "Woe", "Fear", "Anger", "Torment", "Sorrow", "Rage", "Suffering", "Wrath", "Doubt", "Loathing", "Zeal",
                "Anguish", "Spite", "Scorn", "Envy", "Misery", "Dread"
            };

        internal static List<string> Generate()
        {
            var essences = new List<string>();
            var upperBound = 0;
            foreach (var tier in _tiers)
            {
                upperBound += 4;
                for (var e = 0; e < upperBound; e++)
                {
                    if (_types.Length <= e)
                        continue;

                    var type = _types[e];
                    essences.Add("OrbType." + tier + type + ",");
                }
            }

            essences.AddRange(_specialEssences.Select(type => "EssenceType." + type + ","));

            essences.Add("EssenceType.Corruption,");

            return essences;
        }
    }
}
