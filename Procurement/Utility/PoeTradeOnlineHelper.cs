using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Net;
using System.Collections.Specialized;
using POEApi.Model;

namespace Procurement.Utility
{
    class PoeTradeOnlineHelper
    {
        private static PoeTradeOnlineHelper instance;

        private System.Timers.Timer refreshTimer;
        private HashSet<ShopSetting> shopSettings;
        private Dictionary<WebClient, ShopSetting> mapping;

        private PoeTradeOnlineHelper()
        {
            refreshTimer = new Timer();
            refreshTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            refreshTimer.Interval = 30 * 60 * 1000; //Refresh every half an hour to be sure
            refreshTimer.Enabled = true;

            shopSettings = new HashSet<ShopSetting>();
            mapping = new Dictionary<WebClient, ShopSetting>();
        }

        public void RegisterForOnlineRefresh(List<ShopSetting> shopSettings)
        {
            foreach (ShopSetting shopSetting in shopSettings)
            {
                RegisterForOnlineRefresh(shopSetting);
            }
        }

        public void RegisterForOnlineRefresh(ShopSetting shopSetting)
        {
            shopSettings.Add(shopSetting);
            //Refresh just the new one
            RefreshOnlineStatus(shopSetting);
        }

        public void UnregisterForOnlineRefresh(ShopSetting shopSetting)
        {
            shopSettings.Remove(shopSetting);
        }

        public static PoeTradeOnlineHelper Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new PoeTradeOnlineHelper();
                }
                return instance;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Refreshing online status...");
            foreach (ShopSetting shopSetting in shopSettings)
            {
                RefreshOnlineStatus(shopSetting);
            }
        }

        private void RefreshOnlineStatus(ShopSetting shopSetting)
        {
            if (shopSetting.PoeTradeURL != null && shopSetting.PoeTradeURL.Length > 0)
            {
                Uri refreshUri = new Uri(shopSetting.PoeTradeURL);
                using (var client = new WebClient())
                {
                    var data = new NameValueCollection();
                    client.UploadValuesAsync(new Uri(shopSetting.PoeTradeURL), "POST", data);
                    client.UploadValuesCompleted += client_UploadValuesCompleted;
                    mapping[client] = shopSetting;
                }
            }
        }

        private void client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            ShopSetting shopSetting = mapping[(WebClient)sender];
            mapping.Remove((WebClient)sender);

            Console.WriteLine("Successfully refreshed online status for " + shopSetting.PoeTradeURL);
        }

    }

}
