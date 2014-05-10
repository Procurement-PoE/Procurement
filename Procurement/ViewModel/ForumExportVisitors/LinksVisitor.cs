using System;
using System.Collections.Generic;
using POEApi.Model;
using System.Linq;
using Procurement.ViewModel.Filters;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class LinksVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;
        public LinksVisitor()
        {
            tokens = new Dictionary<string,IFilter>();
            tokens.Add("{6Link}", new Link(6));
            tokens.Add("{5Link}", new FiveLink());
            tokens.Add("{4Link}", new FourLink());
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

        private T getEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }
    }
}
