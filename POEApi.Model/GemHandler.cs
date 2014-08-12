using System.Collections.Generic;

namespace POEApi.Model
{
    internal class GemHandler
    {
        public static SortedDictionary<string, int> GetGemDistribution(IEnumerable<Gem> gems)
        {
            SortedDictionary<string, int> gemTable = new SortedDictionary<string, int>();
            foreach (Gem gem in gems)
            {
                if (gemTable.ContainsKey(gem.TypeLine))
                    gemTable[gem.TypeLine]++;
                else
                    gemTable[gem.TypeLine] = 1;
            }

            return gemTable;
        }
    }
}
