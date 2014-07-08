using POEApi.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Procurement.ViewModel
{
    public class BuyoutSettingsViewModel : INotifyPropertyChanged
    {
        private const string EMBED_BUYOUTS = "EmbedBuyouts";
        private const string BUYOUT_TAG_ONLY = "BuyoutItemsOnlyVisibleInBuyoutsTag";
        private const string ONLY_DISPLAY_BUYOUTS = "OnlyDisplayBuyouts";

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

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

        public BuyoutSettingsViewModel()
        {
            this.embedBuyouts = getSetting(EMBED_BUYOUTS);
            this.buyoutItemsOnlyVisibleInBuyoutsTag = getSetting(BUYOUT_TAG_ONLY);
            this.onlyDisplayBuyouts = getSetting(ONLY_DISPLAY_BUYOUTS);
        }

        private bool getSetting(string key)
        {
            try
            {
                Convert.ToBoolean(Settings.UserSettings[key]);
            }
            catch (Exception)
            {
                string msg = "Unable to load {0} setting.\n\nIt is either missing from settings.xml or incorrectly configured. The default setting for {0} has been loaded, but it is strongly advised that you fix your settings.xml or replace it with a default one.";
                MessageBox.Show(string.Format(msg, key), "Unable to load setting", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (key == EMBED_BUYOUTS)
                return true;

            return false;
        }
    }
}
