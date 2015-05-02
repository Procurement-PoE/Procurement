using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using POEApi.Infrastructure;

namespace POEApi.Model
{
    //Do you want to be refactored settings.cs? Because this is how you end up getting refactored.
    public static class Settings
    {
        private const string SAVE_LOCATION = "Settings.xml";
        private const string DATA_LOCATION = "Data.xml";
        internal const string BUYOUT_LOCATION = "Buyouts.xml";

        public static Dictionary<OrbType, CurrencyRatio> CurrencyRatios { get; private set; }
        public static Dictionary<string, string> UserSettings { get; private set; }
        public static Dictionary<string, string> ProxySettings { get; private set; }
        public static Dictionary<string, List<string>> Lists { get; private set; }
        public static Dictionary<int, ItemTradeInfo> Buyouts { get; private set; }
        public static Dictionary<string, string> TabsBuyouts { get; private set; }
        public static Dictionary<string, ShopSetting> ShopSettings { get; private set; }
        public static List<string> PopularGems { get; private set; }
        private static XElement settingsFile;
        private static XElement buyoutFile;

        public static Dictionary<GearType, List<string>> GearBaseTypes { get; private set; }

        static Settings()
        {
            settingsFile = XElement.Load(SAVE_LOCATION);
            CurrencyRatios = settingsFile.Elements("Ratios").Descendants().ToDictionary(orb => orb.Attribute("type").GetEnum<OrbType>(), orb => new CurrencyRatio(orb.Attribute("type").GetEnum<OrbType>(), getOrbAmount(orb), getChaosAmount(orb)));

            UserSettings = getStandardNameValue("UserSettings");
            ProxySettings = getStandardNameValue("ProxySettings");

            Lists = new Dictionary<string, List<string>>();

            if (settingsFile.Element("Lists") != null)
                Lists = settingsFile.Element("Lists").Elements("List").ToDictionary(list => list.Attribute("name").Value, list => list.Elements("Item").Select(e => e.Attribute("value").Value).ToList());

            loadBuyouts();

            PopularGems = new List<string>();
            if (settingsFile.Element("PopularGems") != null)
                PopularGems = settingsFile.Element("PopularGems").Elements("Gem").Select(e => e.Attribute("name").Value).ToList();

            loadGearTypeData();
            loadShopSettings();
        }

        private static void loadBuyouts()
        {
            try
            {
                buyoutFile = XElement.Load(BUYOUT_LOCATION);
                Buyouts = new Dictionary<int, ItemTradeInfo>();

                if (buyoutFile.Element("ItemBuyouts") != null)
                    Buyouts = loadItemBuyouts();

                TabsBuyouts = new Dictionary<string, string>();
                if (buyoutFile.Element("TabBuyouts") != null)
                    TabsBuyouts = buyoutFile.Element("TabBuyouts").Elements("Item").ToDictionary(list => list.Attribute("id").Value, list => list.Attribute("value").Value);
            }
            catch (Exception ex)
            {
                Logger.Log("Error loading Buyouts: " + ex.ToString());
                throw ex;
            }
        }

        private static Dictionary<int, ItemTradeInfo> loadItemBuyouts()
        {            
            var items = buyoutFile.Element("ItemBuyouts").Elements("Item");
            var legacyBuyouts = items.Where(i => i.Attribute("value") != null).Any();

            if (legacyBuyouts)
                return items.ToDictionary(list => (int)list.Attribute("id"), list => new ItemTradeInfo(list.Attribute("value").Value, string.Empty, string.Empty, string.Empty, false));

            return items.ToDictionary(list => (int)list.Attribute("id"), list => new ItemTradeInfo(tryGetValue(list, "BuyoutValue"), tryGetValue(list, "PriceValue"), tryGetValue(list, "CurrentOfferValue"), tryGetValue(list, "Notes"), bool.Parse(tryGetValue(list, "IsManuallySelected"))));
        }

        private static void loadShopSettings()
        {
            ShopSettings = settingsFile.Elements("ShopSettings").Descendants().ToDictionary(shop => shop.Attribute("League").Value, shop => createShopSetting(shop));
        }

        private static ShopSetting createShopSetting(XElement shop)
        {
            return new ShopSetting { ThreadId = shop.Attribute("ThreadId").Value, ThreadTitle = shop.Attribute("ThreadTitle").Value };
        }

        private static string tryGetValue(XElement list, string key)
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

        private static void loadGearTypeData()
        {
            XElement dataDoc = XElement.Load(DATA_LOCATION);
            GearBaseTypes = new Dictionary<GearType, List<string>>();

            if (dataDoc.Element("GearBaseTypes") == null)
                return;

            GearBaseTypes = dataDoc.Element("GearBaseTypes").Elements("GearBaseType")
                                                            .ToDictionary(g => (GearType)Enum.Parse(typeof(GearType), g.Attribute("name").Value), g => g.Elements("Item")
                                                            .Select(e => e.Attribute("name").Value)
                                                            .ToList());
        }

        private static double getChaosAmount(XElement orb)
        {
            return double.Parse(orb.Attribute("ChaosAmount").Value, CultureInfo.InvariantCulture);
        }

        private static double getOrbAmount(XElement orb)
        {
            return double.Parse(orb.Attribute("OrbAmount").Value, CultureInfo.InvariantCulture);
        }

        private static Dictionary<string, string> getStandardNameValue(string root)
        {
            return settingsFile.Elements(root).Descendants().ToDictionary(setting => setting.Attribute("name").Value, setting => setting.Attribute("value").Value);
        }

        public static void Save()
        {
            foreach (string key in UserSettings.Keys)
            {
                XElement update = settingsFile.Elements("UserSettings").Descendants().First(x => x.Attribute("name").Value == key);
                if (UserSettings[key] != null)
                    update.Attribute("value").SetValue(UserSettings[key]);
            }

            foreach (OrbType key in CurrencyRatios.Keys)
            {
                XElement update = settingsFile.Elements("Ratios").Descendants().First(x => x.Attribute("type").Value == key.ToString());
                update.Attribute("OrbAmount").SetValue(CurrencyRatios[key].OrbAmount.ToString(CultureInfo.InvariantCulture));
                update.Attribute("ChaosAmount").SetValue(CurrencyRatios[key].ChaosAmount.ToString(CultureInfo.InvariantCulture));
            }
            
            updateLists();

            try
            {
                settingsFile.Save(SAVE_LOCATION);
            }
            catch (Exception ex)
            {
                Logger.Log("Couldn't save settings: " + ex.ToString());
            }
        }

        public static void SaveBuyouts()
        {
            buyoutFile.Element("ItemBuyouts").RemoveNodes();

            foreach (int key in Buyouts.Keys)
            {
                XElement buyout = new XElement("Item", new XAttribute("id", key), new XAttribute("BuyoutValue", Buyouts[key].Buyout), new XAttribute("PriceValue", Buyouts[key].Price), new XAttribute("CurrentOfferValue", Buyouts[key].CurrentOffer), new XAttribute("Notes", Buyouts[key].Notes), new XAttribute("IsManuallySelected", Buyouts[key].IsManualSelected));
                buyoutFile.Element("ItemBuyouts").Add(buyout);
            }

            buyoutFile.Save(BUYOUT_LOCATION);
        }

        public static void SaveTabBuyouts()
        {
            buyoutFile.Element("TabBuyouts").RemoveNodes();

            foreach (var item in TabsBuyouts)
            {
                XElement tabBuyout = new XElement("Item", new XAttribute("id", item.Key), new XAttribute("value", item.Value));
                buyoutFile.Element("TabBuyouts").Add(tabBuyout);
            }

            buyoutFile.Save(BUYOUT_LOCATION);
        }

        public static void SaveLists()
        {
            updateLists();
            settingsFile.Save(SAVE_LOCATION);
        }

        private static void updateLists()
        {
            var listKeys = Settings.Lists.Keys.Where(k => k == "IgnoreTabsInRecipes" || k == "MyCharacters" || k == "MyLeagues");

            foreach (var listKey in listKeys)
            {
                XElement original = settingsFile.Element("Lists").Descendants().FirstOrDefault(x => x.Attribute("name") != null && string.Equals(x.Attribute("name").Value, listKey));

                if (original == null)
                    original = new XElement("List", new XAttribute("name", listKey));

                original.RemoveNodes();

                foreach (var listValue in Settings.Lists[listKey])
                    original.Add(new XElement("Item", new XAttribute("value", listValue)));
            }
        }

        public static bool SaveShopSettings()
        {
            try
            {
                if (!settingsFile.Elements("ShopSettings").Any())
                    settingsFile.Add(new XElement("ShopSettings"));

                settingsFile.Element("ShopSettings").RemoveNodes();

                foreach (var shop in ShopSettings)
                {
                    XElement buyout = new XElement("Shop", new XAttribute("League", shop.Key), new XAttribute("ThreadId", shop.Value.ThreadId), new XAttribute("ThreadTitle", shop.Value.ThreadTitle));
                    settingsFile.Element("ShopSettings").Add(buyout);
                }

                settingsFile.Save(SAVE_LOCATION);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to save shop settings: " + ex.ToString());
                return false;
            }
        }
    }
}