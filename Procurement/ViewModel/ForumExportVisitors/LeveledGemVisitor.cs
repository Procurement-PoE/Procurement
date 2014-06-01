using System;
using System.Collections.Generic;
using POEApi.Model;
using System.Linq;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class LeveledGemVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;

        public LeveledGemVisitor()
        {
            tokens = new Dictionary<string, IFilter>();
            tokens.Add("{Level1Gems}", new LeveledGemFilter(1));
            tokens.Add("{Level2Gems}", new LeveledGemFilter(2));
            tokens.Add("{Level3Gems}", new LeveledGemFilter(3));
            tokens.Add("{Level4Gems}", new LeveledGemFilter(4));
            tokens.Add("{Level5Gems}", new LeveledGemFilter(5));
            tokens.Add("{Level6Gems}", new LeveledGemFilter(6));
            tokens.Add("{Level7Gems}", new LeveledGemFilter(7));
            tokens.Add("{Level8Gems}", new LeveledGemFilter(8));
            tokens.Add("{Level9Gems}", new LeveledGemFilter(9));
            tokens.Add("{Level10Gems}", new LeveledGemFilter(10));
            tokens.Add("{Level11Gems}", new LeveledGemFilter(11));
            tokens.Add("{Level12Gems}", new LeveledGemFilter(12));
            tokens.Add("{Level13Gems}", new LeveledGemFilter(13));
            tokens.Add("{Level14Gems}", new LeveledGemFilter(14));
            tokens.Add("{Level15Gems}", new LeveledGemFilter(15));
            tokens.Add("{Level16Gems}", new LeveledGemFilter(16));
            tokens.Add("{Level17Gems}", new LeveledGemFilter(17));
            tokens.Add("{Level18Gems}", new LeveledGemFilter(18));
            tokens.Add("{Level19Gems}", new LeveledGemFilter(19));
            tokens.Add("{Level20Gems}", new LeveledGemFilter(20));
            tokens.Add("{Level21Gems}", new LeveledGemFilter(21));
        }

        public override string Visit(IEnumerable<Item> items, string current)
        {
            string updated = current;
            var sorted = items.OrderBy(i => i.H).ThenBy(i => i.IconURL);

            foreach (var token in tokens)
            {
                if (updated.IndexOf(token.Key) < 0)
                    continue;

                updated = updated.Replace(token.Key, runFilter(token.Value, sorted));
            }

            return updated;
        }
    }
}
