using System;
using System.Collections.Generic;
using POEApi.Model;
using System.Linq;
using Procurement.ViewModel.Filters;
using System.Collections;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class GemVisitor : VisitorBase
    {
        private Dictionary<string, IFilter> tokens;
        public GemVisitor()
        {
            tokens = new Dictionary<string, IFilter>();
            tokens.AddRange(from cat1 in Enum.GetNames(typeof(GemCategory))
                            from cat2 in Enum.GetNames(typeof(GemCategory))
                            select new KeyValuePair<string, IFilter>(string.Concat("{", cat1, cat2, "Gems}"), new AndFilter(new GemCategoryFilter(cat1), new GemCategoryFilter(cat2))));

            tokens.AddRange(from cat1 in Enum.GetNames(typeof(GemCategory))
                            select new KeyValuePair<string, IFilter>(string.Concat("{", cat1, "Gems}"), new GemCategoryFilter(cat1)));
            
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

    public static class ext
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dic, IEnumerable<KeyValuePair<TKey, TValue>> range)
        {
            IDictionary<TKey, TValue> ret = (IDictionary<TKey, TValue>)dic;
            foreach (var item in range)
                ret.Add(item);
        }
    }
}
