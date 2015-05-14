using POEApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace POEApi.Model
{
    public class Stash
    {
        private List<Item> items;
        private const int tabSize = 144;
        public int NumberOfTabs { get; set; }
        public List<Tab> Tabs { get; set; }
        private Dictionary<string, List<Item>> itemsByTab;
        private Dictionary<string, string> tabNameByInventoryId;
        private Dictionary<int, string> tabNameByTabId;

        internal Stash(JSONProxy.Stash proxy)
        {
            if (proxy.Items == null)
            {
                items = new List<Item>();
                NumberOfTabs = 0;
                return;
            }

            items = proxy.Items.Select(item => ItemFactory.Get(item)).ToList();
            this.NumberOfTabs = proxy.NumTabs;
            this.Tabs = ProxyMapper.GetTabs(proxy.Tabs);

            tabNameByTabId = Tabs.Where(t => t.IsFakeTab == false).ToDictionary(t => t.i, t => t.Name);
        }

        public void ClearItems()
        {
            items.Clear();
        }

        public void Add(Stash stash)
        {
            items.AddRange(stash.items);
        }

        public void AddCharacterTab(Tab tab, List<Item> characterItems)
        {
            items.AddRange(characterItems);
            Tabs.Add(tab);
        }

        public void RefreshTab(POEModel currentModel, string currentLeague, int tabId, string accname)
        {
            try
            {
                string inventId = ProxyMapper.STASH + (tabId + 1).ToString();
                items.RemoveAll(i => i.InventoryId == inventId);

                if (Tabs.First(t => t.i == tabId).IsFakeTab)
                {
                    refreshCharacterTab(currentModel, tabId);
                    return;
                }

                Add(currentModel.GetStash(tabId, currentLeague, true));
                refreshItemsByTabTab(tabId);
            }
            catch (Exception ex)
            {
                Logger.Log("Error refreshing tab: " + ex.ToString());
            }
        }

        private void refreshCharacterTab(POEModel currentModel, int tabId)
        {
            var charTab = Tabs.First(t => t.i == tabId);

            var characterName = charTab.Name;
            var characterItems = currentModel.GetInventory(characterName, true);
            var characterStashItems = CharacterStashBuilder.GetCharacterStashItems(characterName, characterItems, tabId + 1);

            items.AddRange(characterStashItems);
            refreshItemsByTabTab(tabId);
        }

        public List<Item> GetItemsByTab(int tabId)
        {
            try
            {
                buildItemsByTab();

                ++tabId;
                return itemsByTab[ProxyMapper.STASH + tabId.ToString()];
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine();
                sb.AppendLine("Exception attempting to get items by tab for tabId : " + tabId);
                sb.AppendLine("Current TabIds:");

                foreach (var key in itemsByTab.Keys)
                    sb.AppendLine(key);

                sb.AppendLine("End of TabIds.");
                sb.AppendLine("Exception Details: " + ex.ToString());

                Logger.Log(sb.ToString());

                return new List<Item>();
            }
        }

        public string GetTabNameByTabId(int tabID)
        {
            return tabNameByTabId[tabID];
        }

        public string GetTabNameByInventoryId(string inventoryID)
        {
            if (tabNameByInventoryId == null)
                tabNameByInventoryId = Tabs.ToDictionary(kvp => ProxyMapper.STASH + (kvp.i + 1).ToString(), kvp => kvp.Name);

            return tabNameByInventoryId[inventoryID];
        }

        private void buildItemsByTab()
        {
            try
            {
                var tabs = Tabs.Select(t => ProxyMapper.STASH + (t.i + 1));

                if (!Tabs.Exists(tab_item => tab_item.Name=="Manually Selected Tab"))
                {
                    //add new virtual manual selection tab
                    Tab ManuallySelectedTab = new Tab();
                    ManuallySelectedTab.Hidden = true;
                    ManuallySelectedTab.i = 999;
                    ManuallySelectedTab.IsFakeTab = true;
                    ManuallySelectedTab.Name = "Manually Selected Tab";
                    Tabs.Add(ManuallySelectedTab);
                }

                XElement buyoutFile = XElement.Load(POEApi.Model.Settings.BUYOUT_LOCATION);
                Dictionary<int, ItemTradeInfo> Buyouts = new Dictionary<int, ItemTradeInfo>();

                if (buyoutFile.Element("ItemBuyouts") != null) Buyouts = loadItemBuyouts(buyoutFile);
                itemsByTab = tabs.ToDictionary(kvp => kvp, kvp => items.Where(i => i.InventoryId == kvp).ToList());

                //check if buyout item is manually selected
                foreach (KeyValuePair<int,ItemTradeInfo> curr_buyout_item in Buyouts)
                {
                    foreach (List<Item> curr_tab_item in itemsByTab.Values)
                    {
                        Item equal_hashes = null;
                        equal_hashes = curr_tab_item.Find(curr_item => curr_item.UniqueIDHash==curr_buyout_item.Key);
                        if (equal_hashes != null)
                        {
                            //some item is selected manually
                            equal_hashes.IsSelectedManually = curr_buyout_item.Value.IsManualSelected;

                            bool already_exist_in_manual_tab=items.Exists(curr_item => curr_item.UniqueIDHash==equal_hashes.UniqueIDHash && curr_item.InventoryId=="Stash1000");
                            if (equal_hashes.IsSelectedManually && !already_exist_in_manual_tab)
                            {
                                //add copy of item to virtual manual selection tab (TabID is 999+1)
                                //TradeInvertoryID is still real
                                Item selected_copy = equal_hashes.Clone() as Item;
                                selected_copy.InventoryId = "Stash1000";
                                items.Add(selected_copy);
                            }
                        }
                    }
                }

                //reinit items tabs
                itemsByTab = tabs.ToDictionary(kvp => kvp, kvp => items.Where(i => i.InventoryId == kvp).ToList());
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Error building items by tab. Tab data:");

                foreach (var tab in Tabs)
                    sb.AppendLine(string.Format("i = {0}, hidden = {1}, fake = {2}", tab.i, tab.Hidden, tab.IsFakeTab));

                sb.AppendLine("End of tab data");
                Logger.Log(sb.ToString());

                throw new Exception("Error building stash from downloaded tabs, please log a ticket at https://github.com/medvedttn/Procurement/issues and include all your .bin files");
            }
        }

        private Dictionary<int, ItemTradeInfo> loadItemBuyouts(XElement buyoutFile)
        {
            var items = buyoutFile.Element("ItemBuyouts").Elements("Item");
            var legacyBuyouts = items.Where(i => i.Attribute("value") != null).Any();

            if (legacyBuyouts)
                return items.ToDictionary(list => (int)list.Attribute("id"), list => new ItemTradeInfo(list.Attribute("value").Value, string.Empty, string.Empty, string.Empty, false));

            return items.ToDictionary(list => (int)list.Attribute("id"), list => new ItemTradeInfo(tryGetValue(list, "BuyoutValue"), tryGetValue(list, "PriceValue"), tryGetValue(list, "CurrentOfferValue"), tryGetValue(list, "Notes"), bool.Parse(tryGetValue(list, "IsManuallySelected"))));
        }

        private string tryGetValue(XElement list, string key)
        {
            if (list.Attribute(key) == null)
            {
                if (key == "IsManuallySelected")
                {
                    return "False";
                }
                else
                {
                    return string.Empty;
                }
            }

            return list.Attribute(key).Value;
        }

        private void refreshItemsByTabTab(int tabId)
        {
            if (itemsByTab == null)
            {
                buildItemsByTab();
                return;
            }

            string inventId = ProxyMapper.STASH + (tabId + 1).ToString();
            itemsByTab[inventId] = items.Where(i => i.InventoryId == inventId).ToList();
        }

        public List<T> Get<T>() where T : Item
        {
            return items.OfType<T>().ToList();
        }

        public double GetTotal(OrbType target)
        {
            return CurrencyHandler.GetTotal(target, Get<Currency>());
        }

        public Dictionary<OrbType, double> GetTotalCurrencyDistribution(OrbType target)
        {
            return CurrencyHandler.GetTotalCurrencyDistribution(target, Get<Currency>());
        }

        public Dictionary<OrbType, double> GetTotalCurrencyCount()
        {
            return CurrencyHandler.GetTotalCurrencyCount(Get<Currency>());
        }

        public SortedDictionary<string, int> GetTotalGemDistribution()
        {
            return GemHandler.GetGemDistribution(Get<Gem>());
        }
    }
}
