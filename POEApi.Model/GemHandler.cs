using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POEApi.Model
{
    internal class GemHandler
    {
        

        static GemHandler()
        {
         
        }

        public static SortedDictionary<string, int> GetGemDistribution(IEnumerable<Gem> gems)
        {
            SortedDictionary<string, int> gemTable = new SortedDictionary<string, int>(Settings.GemTable);
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
