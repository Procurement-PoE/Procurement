using System.Collections.Generic;
using POEApi.Model;
using System.Linq;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class MapVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> known;
        public MapVisitor()
        {
            known = Enumerable.Range(66, 100).ToDictionary(i => string.Concat("{", i.ToString(), "Maps", "}"), i => (IFilter)(new MapLevelFilter(i)));
        }
        public override string Visit(IEnumerable<Item> items, string current)
        {
            var sorted = items.OfType<Map>().OrderBy(i => i.MapLevel).ThenBy(i => i.MapQuantity).ToList();
            string updated = current;
            foreach (var token in known)
            {
                int index = updated.IndexOf(token.Key);
                if (index == -1)
                {
                    updated = updated.Replace(token.Key, string.Empty);
                    continue;
                }

                updated = updated.Replace(token.Key, runFilter(known[token.Key], sorted));
            }

            return updated;
        }
    }
}
