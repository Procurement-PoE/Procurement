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
        private const string LINKITEM = "[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]";

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
            string tabName = ApplicationState.Stash[ApplicationState.CurrentLeague].GetTabNameByInventoryId(item.inventoryId);
            bool isBuyoutItem = Settings.TabsBuyouts.ContainsKey(tabName) || Settings.Buyouts.ContainsKey(item.UniqueIDHash);

            if ((onlyDisplayBuyouts && !isBuyoutItem) || (isBuyoutItem && buyoutItemsOnlyVisibleInBuyoutsTag))
                return string.Empty;
            
            //if (isBuyoutItem)
            //{
            //    if (buyoutItemsOnlyVisibleInBuyoutsTag)
            //        return string.Empty;

            //    if (embedBuyouts)
            //        return string.Format("\n[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]\n~b/o {4}\n", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y, getBuyout<T>(item, tabName));
            //}

            //return string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y);


            if (!(embedBuyouts && isBuyoutItem))
                return string.Format("[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y);
            
            return string.Format("\n[linkItem location=\"{0}\" league=\"{1}\" x=\"{2}\" y=\"{3}\"]{4}", item.inventoryId, ApplicationState.CurrentLeague, item.X, item.Y, appendAdditionalInfo(item, tabName));
        }

        private string appendAdditionalInfo(Item item, string tabName)
        {
            if (!Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                return string.Format("\n~b/o{0}\n", Settings.TabsBuyouts[tabName]);

            var buyoutInfo = Settings.Buyouts[item.UniqueIDHash];
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

        private static string getBuyout<T>(T item, string tabName) where T : Item
        {
            if (Settings.Buyouts.ContainsKey(item.UniqueIDHash))
                return Settings.Buyouts[item.UniqueIDHash].Buyout;

            return Settings.TabsBuyouts[tabName];
        }
    }
}
