using POEApi.Infrastructure;
using POEApi.Model;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Timers;
using System.Windows;

namespace Procurement.Utility
{
    class PoeTradeOnlineHelper
    {
        private static PoeTradeOnlineHelper instance;
        private System.Timers.Timer refreshTimer;
        private Uri refreshUri;

        private PoeTradeOnlineHelper()
        {
            refreshTimer = new Timer();
            refreshTimer.Elapsed += (s, e) => { RefreshOnlineStatus(); };
            refreshTimer.Interval = 30 * 60 * 1000;
        }

        public static PoeTradeOnlineHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new PoeTradeOnlineHelper();

                return instance;
            }
        }

        private void RefreshOnlineStatus()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var data = new NameValueCollection();
                    client.UploadValuesAsync(refreshUri, "POST", data);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error refreshing online status in PoeTradeOnlineHelper: " + ex.ToString());
            }
        }

        internal void Start()
        {
            try
            {
                var enabled = Convert.ToBoolean(Settings.UserSettings["PoeTradeRefreshEnabled"]);

                if (!enabled || refreshTimer.Enabled)
                    return;

                var poeTradeUrl = Settings.UserSettings["PoeTradeRefreshUrl"];
                refreshUri = new Uri(poeTradeUrl);

                refreshTimer.Start();
                RefreshOnlineStatus();
            }
            catch (Exception ex)
            {
                Logger.Log("Error starting PoeTradeOnlineHelper: " + ex.ToString());
                MessageBox.Show("Error refreshing poe.trade online status, ensure you have a valid url entered.", "Error Starting PoeTradeOnlineHelper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal void Stop()
        {
            refreshTimer.Stop();
        }
    }
}
