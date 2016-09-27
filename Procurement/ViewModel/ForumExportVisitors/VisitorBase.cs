using System.Collections.Generic;
using System.Text;
using POEApi.Model;
using Procurement.ViewModel.Filters;
using System.Linq;
using POEApi.Infrastructure;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal abstract class VisitorBase : IVisitor
    {
        protected virtual bool buyoutItemsOnlyVisibleInBuyoutsTag
        {
            get { return Settings.UserSettings.GetEntry("BuyoutItemsOnlyVisibleInBuyoutsTag").ToLower() == "true"; }
        }

        protected virtual bool embedBuyouts
        {
            get { return Settings.UserSettings.GetEntry("EmbedBuyouts").ToLower() == "true"; }
        }

        protected virtual bool onlyDisplayBuyouts
        {
            get { return Settings.UserSettings.GetEntry("OnlyDisplayBuyouts").ToLower() == "true"; }
        }

        public abstract string Visit(IEnumerable<Item> items, string current);

        protected string runFilter<T>(IEnumerable<Item> items) where T : IFilter, new()
        {
            return runFilter(new T(), items);
        }

        protected string runFilter(IFilter filter, IEnumerable<Item> items)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in items.Where(i => filter.Applicable(i)))
                builder.Append(getLinkItem(item));

            return builder.ToString();
        }

        protected string getLinkItem<T>(T item) where T : Item
        {
            string tabName = ApplicationState.Stash[ApplicationState.CurrentLeague].GetTabNameByInventoryId(item.InventoryId);
            bool isBuyoutItem = Settings.TabsBuyouts.ContainsKey(tabName) || hasItemBuyout<T>(item);

            if ((onlyDisplayBuyouts && !isBuyoutItem) || (isBuyoutItem && buyoutItemsOnlyVisibleInBuyoutsTag))
                return string.Empty;

            if (!(embedBuyouts && isBuyoutItem))
                return getNonBuyoutString(item);

            return getBuyoutString<T>(item, tabName);
        }

        private static bool hasItemBuyout<T>(T item) where T : Item
        {
            if (!Settings.Buyouts.ContainsKey(item.Id))
                return false;

            var buyoutItem = Settings.Buyouts[item.Id];

            return !string.IsNullOrEmpty(buyoutItem.Buyout) || !string.IsNullOrEmpty(buyoutItem.Price) || !string.IsNullOrEmpty(buyoutItem.CurrentOffer);
        }

        private string getBuyoutString<T>(T item, string tabName) where T : Item
        {
            if (item.Character != string.Empty)
                return string.Format("\n[linkItem location=\"{0}\" character=\"{1}\" x=\"{2}\" y=\"{3}\"]{4}", item.TradeInventoryId, item.Character, item.TradeX, item.TradeY, appendAdditionalInfo(item, tabName));

            return string.Format("\n[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]{4}", item.TradeInventoryId, ApplicationState.CurrentLeague, item.TradeX, item.TradeY, appendAdditionalInfo(item, tabName));
        }

        private static string getNonBuyoutString(Item item)
        {
            if (item.Character != string.Empty)
                return string.Format("[linkItem location=\"{0}\" character=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.TradeInventoryId, item.Character, item.TradeX, item.TradeY);

            return string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.TradeInventoryId, ApplicationState.CurrentLeague, item.TradeX, item.TradeY);
        }

        private string appendAdditionalInfo(Item item, string tabName)
        {
            if (!Settings.Buyouts.ContainsKey(item.Id))
                return string.Format("\n~b/o {0}\n", Settings.TabsBuyouts[tabName]);

            var buyoutInfo = Settings.Buyouts[item.Id];
            StringBuilder sb = new StringBuilder();

            if (buyoutInfo.Buyout != string.Empty)
                sb.Append(string.Format("\n~b/o {0}", buyoutInfo.Buyout));

            if (buyoutInfo.CurrentOffer != string.Empty)
                sb.Append(string.Format("\n~c/o {0}", buyoutInfo.CurrentOffer));

            if (buyoutInfo.Price != string.Empty)
                sb.Append(string.Format("\n~price {0}", buyoutInfo.Price));

            sb.Append("\n");

            return sb.ToString();
        }
    }
}