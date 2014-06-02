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
        private Dictionary<string, IFilter> tokens;
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

            foreach (var item in sortedItems)
            {
                if (Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                {
                    buyouts[Settings.Buyouts[item.UniqueIDHash].Buyout].Add(item);
                    continue;
                }

                var itemBuyoutKey = ApplicationState.Stash[ApplicationState.CurrentLeague].GetTabNameByInventoryId(item.inventoryId);

                if (Settings.TabsBuyouts.ContainsKey(itemBuyoutKey))
                    buyouts[Settings.TabsBuyouts[itemBuyoutKey]].Add(item);
            }

            Dictionary<string, BuyoutFilter> filters = buyouts.Keys.ToDictionary(k => k, k => new BuyoutFilter(k));

            foreach (var set in buyouts)
            {
                builder.AppendLine(string.Format("[spoiler=\"          ~b/o {0}          \"]", set.Key));
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
    }
}
