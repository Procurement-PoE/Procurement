using System;
using System.Collections.Generic;
using Procurement.ViewModel.Filters;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class SingleBuyoutVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;
        public SingleBuyoutVisitor()
        {
            tokens = Settings.Buyouts.Keys.GroupBy(k => Settings.Buyouts[k])
                                          .ToDictionary(g => string.Concat("{", g.Key.ToLower(), "}"), g => (IFilter)new BuyoutFilter(g.Key.ToLower()));
        }
        public override string Visit(IEnumerable<POEApi.Model.Item> items, string current)
        {
            string updated = current;
            var sorted = items.OrderBy(i => i.H).ThenBy(i => i.IconURL);

            foreach (var token in tokens)
            {
                if (updated.IndexOf(token.Key, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                updated = updated.Replace(token.Key, runFilter(token.Value, sorted));
            }

            return updated;
        }
    }
}
