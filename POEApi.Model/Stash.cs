using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public void RefreshTab(POEModel currentModel, string currentLeague, int tabId)
        {
            string inventId = ProxyMapper.STASH + (tabId + 1).ToString();
            items.RemoveAll(i => i.inventoryId == inventId);
            Add(currentModel.GetStash(tabId, currentLeague, true));
            refreshItemsByTabTab(tabId);
        }

        public List<Item> GetItemsByTab(int tabId)
        {
            if (itemsByTab == null)
                buildItemsByTab();

            ++tabId;
            return itemsByTab[ProxyMapper.STASH + tabId.ToString()];
        }

        public string GetTabNameByInventoryId(string inventoryID)
        {
            if (tabNameByInventoryId == null)
                tabNameByInventoryId = Tabs.ToDictionary(kvp => ProxyMapper.STASH + (kvp.i + 1).ToString(), kvp => kvp.Name);

            return tabNameByInventoryId[inventoryID];
        }

        private void buildItemsByTab()
        {
            var tabs = Tabs.Select(t => ProxyMapper.STASH + (t.i + 1));
            
            itemsByTab = tabs.ToDictionary(kvp => kvp, kvp => items.Where(i => i.inventoryId == kvp).ToList());
        }

        private void refreshItemsByTabTab(int tabId)
        {
            if (itemsByTab == null)
            {
                buildItemsByTab();
                return;
            }

            string inventId = ProxyMapper.STASH + (tabId + 1).ToString();
            itemsByTab[inventId] = items.Where(i => i.inventoryId == inventId).ToList();
        }

        public List<T> Get<T>() where T : Item
        {
            return items.OfType<T>().ToList();
        }
        public List<T> Get<T>(Func<T, bool> match) where T : Item
        {
            return items.OfType<T>()
                        .Where(match)
                        .ToList();
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

        public Dictionary<string, List<Gear>> GetDuplicateRares()
        {
            return Get<Gear>().Where(g => g.Rarity == Rarity.Rare && g.Name != string.Empty)
                              .GroupBy(g => g.Name)
                              .Where(g => g.Count() > 1)
                              .Select(g => g.ToList()).ToDictionary(d => d.First().Name);
        }

        public Dictionary<string, decimal> CalculateFreeSpace()
        {
            Dictionary<string, decimal> freeSpace = new Dictionary<string, decimal>();
            decimal totalSpace = NumberOfTabs * tabSize;
            freeSpace.Add("All", (items.Sum(i => (i.W * i.H)) / totalSpace) * 100);

            foreach (var group in items.GroupBy(item => item.inventoryId))
            {
                decimal sum = group.Sum(i => (i.W * i.H));
                freeSpace.Add(group.Key, (sum / tabSize) * 100);
            }

            return freeSpace;
        }
    }
}
