using POEApi.Model;
using Procurement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Procurement.ViewModel
{
    public class TradeSettingsViewModel : INotifyPropertyChanged
    {
        private const string EMBED_BUYOUTS = "EmbedBuyouts";
        private const string BUYOUT_TAG_ONLY = "BuyoutItemsOnlyVisibleInBuyoutsTag";
        private const string ONLY_DISPLAY_BUYOUTS = "OnlyDisplayBuyouts";
        private const string POE_TRADE_REFRESH = "PoeTradeRefreshEnabled";

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public bool LoggedIn { get { return !ApplicationState.Model.Offline; } }

        private bool embedBuyouts;
        public bool EmbedBuyouts
        {
            get { return embedBuyouts; }
            set
            {
                embedBuyouts = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(EMBED_BUYOUTS));

                Settings.UserSettings[EMBED_BUYOUTS] = Convert.ToString(value);
                Settings.Save();
            }
        }

        private bool buyoutItemsOnlyVisibleInBuyoutsTag;
        public bool BuyoutItemsOnlyVisibleInBuyoutsTag
        {
            get { return buyoutItemsOnlyVisibleInBuyoutsTag; }
            set
            {
                buyoutItemsOnlyVisibleInBuyoutsTag = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(BUYOUT_TAG_ONLY));

                Settings.UserSettings[BUYOUT_TAG_ONLY] = Convert.ToString(value);
                Settings.Save();
            }
        }

        private bool onlyDisplayBuyouts;
        public bool OnlyDisplayBuyouts
        {
            get { return onlyDisplayBuyouts; }
            set
            {
                onlyDisplayBuyouts = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(ONLY_DISPLAY_BUYOUTS));

                Settings.UserSettings[ONLY_DISPLAY_BUYOUTS] = Convert.ToString(value);
                Settings.Save();
            }
        }

        private bool poeTradeRefreshEnabled;
        public bool PoeTradeRefreshEnabled
        {
            get { return poeTradeRefreshEnabled; }
            set
            {
                poeTradeRefreshEnabled = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(POE_TRADE_REFRESH));

                Settings.UserSettings[POE_TRADE_REFRESH] = Convert.ToString(value);
            }
        }

        private string poeTradeRefreshUrl;
        public string PoeTradeRefreshUrl
        {
            get { return poeTradeRefreshUrl;  }
            set
            {
                poeTradeRefreshUrl = value;
                Settings.UserSettings["PoeTradeRefreshUrl"] = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PoeTradeRefreshUrl"));
            }
        }

        private string threadId;
        public string ThreadId
        {
            get { return threadId; }
            set
            {
                threadId = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ThreadId"));
            }
        }

        private string threadTitle;
        public string ThreadTitle
        {
            get { return threadTitle; }
            set
            {
                threadTitle = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ThreadTitle"));
            }
        }

        public TradeSettingsViewModel()
        {
            this.embedBuyouts = getSetting(EMBED_BUYOUTS);
            this.buyoutItemsOnlyVisibleInBuyoutsTag = getSetting(BUYOUT_TAG_ONLY);
            this.onlyDisplayBuyouts = getSetting(ONLY_DISPLAY_BUYOUTS);
            this.poeTradeRefreshEnabled = getSetting(POE_TRADE_REFRESH);

            saveCommand = new DelegateCommand(saveShopSettings);
            saveRefreshCommand = new DelegateCommand(saveRefreshSettings);

            if (!Settings.ShopSettings.ContainsKey(ApplicationState.CurrentLeague))
                return;

            this.threadId = Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId;
            this.threadTitle = Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle;
            this.poeTradeRefreshUrl = Settings.UserSettings["PoeTradeRefreshUrl"];
        }

        private void saveRefreshSettings(object obj)
        {
            Settings.Save();

            if (poeTradeRefreshEnabled)
                PoeTradeOnlineHelper.Instance.Start();
            else
                PoeTradeOnlineHelper.Instance.Stop();
        }

        private void saveShopSettings(object obj)
        {
            if (!Settings.ShopSettings.ContainsKey(ApplicationState.CurrentLeague))
                Settings.ShopSettings.Add(ApplicationState.CurrentLeague, new ShopSetting());

            Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadId = this.threadId;
            Settings.ShopSettings[ApplicationState.CurrentLeague].ThreadTitle = this.threadTitle;

            if (Settings.SaveShopSettings())
                MessageBox.Show(Procurement.MessagesRes.ShopSettingsSaved, Procurement.MessagesRes.SettingsSaved, MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(Procurement.MessagesRes.UnableToSaveShopSettingsErrorLoggedToDebuginfoLog, Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private DelegateCommand saveCommand;
        public DelegateCommand SaveCommand
        {
            get { return saveCommand; }
            set { saveCommand = value; }
        }

        private DelegateCommand saveRefreshCommand;
        public DelegateCommand SaveRefreshCommand
        {
            get { return saveRefreshCommand; }
            set { saveRefreshCommand = value; }
        }

        private bool getSetting(string key)
        {
            try
            {
                return Convert.ToBoolean(Settings.UserSettings[key]);
            }
            catch (Exception)
            {
                string msg = Procurement.MessagesRes.UnableToLoad0SettingItIsEitherMissingFromSettingsXml;
                MessageBox.Show(string.Format(msg, key), Procurement.MessagesRes.UnableToLoadSetting, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (key == EMBED_BUYOUTS)
                return true;

            return false;
        }
    }
}
