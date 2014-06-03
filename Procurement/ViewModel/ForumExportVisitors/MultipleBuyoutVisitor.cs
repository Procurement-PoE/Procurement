using POEApi.Model;
using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class MultipleBuyoutVisitor : VisitorBase
    {
        private const string TOKEN = "{Buyouts}";

        protected override bool buyoutItemsOnlyVisibleInBuyoutsTag
        {
            get { return false; }
        }

        protected override bool embedBuyouts
        {
            get { return false; }
        }

        public override string Visit(IEnumerable<Item> items, string current)
        {
            string updated = current;
            var sortedItems = items.OrderBy(i => i.H).ThenBy(i => i.IconURL);

            StringBuilder builder = new StringBuilder();

            Dictionary<string, List<Item>> buyouts = buildBuyoutDictionary();
            Dictionary<string, List<Item>> pricedItems = buildPriceDictionary();

            foreach (var item in sortedItems)
            {
                if (Settings.Buyouts.ContainsKey(item.UniqueIDHash) && !string.IsNullOrEmpty(Settings.Buyouts[item.UniqueIDHash].Buyout))
                {
                    buyouts[Settings.Buyouts[item.UniqueIDHash].Buyout].Add(item);
                    continue;
                }

                if (Settings.Buyouts.ContainsKey(item.UniqueIDHash) && !string.IsNullOrEmpty(Settings.Buyouts[item.UniqueIDHash].Price))
                {
                    pricedItems[Settings.Buyouts[item.UniqueIDHash].Price].Add(item);
                }

                var itemBuyoutKey = ApplicationState.Stash[ApplicationState.CurrentLeague].GetTabNameByInventoryId(item.inventoryId);

                if (Settings.TabsBuyouts.ContainsKey(itemBuyoutKey))
                    buyouts[Settings.TabsBuyouts[itemBuyoutKey]].Add(item);
            }

            Dictionary<string, BuyoutFilter> filters = buyouts.Keys.Union(pricedItems.Keys).Distinct().ToDictionary(k => k, k => new BuyoutFilter(k));

            foreach (var set in buyouts.Where(b => b.Value.Count() != 0))
            {
                builder.AppendLine(string.Format("[spoiler=\"          ~b/o {0}          \"]", set.Key));
                builder.AppendLine(runFilter(filters[set.Key], set.Value));
                builder.AppendLine("[/spoiler]");
            }

            foreach (var set in pricedItems.Where(b => b.Value.Count() != 0))
            {
                builder.AppendLine(string.Format("[spoiler=\"          ~price {0}          \"]", set.Key));
                builder.AppendLine(runFilter(filters[set.Key], set.Value));
                builder.AppendLine("[/spoiler]");
            }

            updated = updated.Replace(TOKEN, builder.ToString());

            return updated;
        }

        private Dictionary<string, List<Item>> buildBuyoutDictionary()
        {
            Dictionary<string, List<Item>> buyouts = new Dictionary<string, List<Item>>();

            var tabBuyouts = Settings.TabsBuyouts.Values;
            var itemBuyouts = Settings.Buyouts.Where(b => b.Value.Buyout != string.Empty).Select(b => b.Value.Buyout);

            foreach (var key in tabBuyouts.Union(itemBuyouts).Distinct())
                buyouts.Add(key, new List<Item>());

            return buyouts;
        }

        private Dictionary<string, List<Item>> buildPriceDictionary()
        {
            Dictionary<string, List<Item>> pricedItems = new Dictionary<string, List<Item>>();

            var itemBuyouts = Settings.Buyouts.Where(b => b.Value.Price != string.Empty).Select(b => b.Value.Price);

            foreach (var key in itemBuyouts.Distinct())
                pricedItems.Add(key, new List<Item>());

            return pricedItems;
        }
    }
}
