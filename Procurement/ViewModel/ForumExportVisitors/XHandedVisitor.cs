using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class XHandedVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;
        public XHandedVisitor()
        {
            tokens = new Dictionary<string, IFilter>();
            tokens.Add("{OneHanders}", new OneHandedFilter());
            tokens.Add("{TwoHanders}", new TwoHandedFilter());
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
