using POEApi.Model;
using Procurement.ViewModel.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class QualityGemVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;

        public QualityGemVisitor()
        {
            tokens = new Dictionary<string, IFilter>();

            tokens.Add("{QualityGems}", new QualityGemFilter(0));

            for (int i = 1; i < 24; i++)
                tokens.Add("{Quality" + i + "Gems}", new QualityGemFilter(i));
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
