using POEApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void RefreshAll(POEModel currentModel, string currentLeague, string accountName)
        {
            foreach (var tab in Tabs)
            {
                try
                {
                    RefreshTab(currentModel, currentLeague, tab.i, accountName);
                }
                catch (Exception ex)
                {
                    Logger.Log("Error refreshing tab: " + ex.ToString());
                }
            }
        }

        public void RefreshTab(POEModel currentModel, string currentLeague, int tabId, string accountName)
        {
            try
            {
                string inventId = ProxyMapper.STASH + (tabId + 1).ToString();
                items.RemoveAll(i => i.InventoryId == inventId);

                if (Tabs.First(t => t.i == tabId).IsFakeTab)
                {
                    refreshCharacterTab(currentModel, tabId, accountName);
                    return;
                }

                Add(currentModel.GetStash(tabId, currentLeague, accountName, true));
                refreshItemsByTabTab(tabId);
            }
            catch (Exception ex)
            {
                Logger.Log("Error refreshing tab: " + ex.ToString());
            }
        }

        private void refreshCharacterTab(POEModel currentModel, int tabId, string accountName)
        {
            var charTab = Tabs.First(t => t.i == tabId);

            var characterName = charTab.Name;
            var characterItems = currentModel.GetInventory(characterName, true, accountName);
            var characterStashItems = CharacterStashBuilder.GetCharacterStashItems(characterName, characterItems, tabId + 1);

            items.AddRange(characterStashItems);
            refreshItemsByTabTab(tabId);
        }

        public List<Item> GetItemsByTab(int tabId)
        {
            try
            {
                if (itemsByTab == null)
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

                throw new Exception("Error building stash from downloaded tabs, please log a ticket at https://github.com/Stickymaddness/Procurement/ and include all your .bin files");
            }
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
