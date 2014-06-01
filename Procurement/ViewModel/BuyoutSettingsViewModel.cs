using POEApi.Model;
using System;
using System.ComponentModel;

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
            this.embedBuyouts = Convert.ToBoolean(Settings.UserSettings[EMBED_BUYOUTS]);
            this.buyoutItemsOnlyVisibleInBuyoutsTag = Convert.ToBoolean(Settings.UserSettings[BUYOUT_TAG_ONLY]);
            this.onlyDisplayBuyouts = Convert.ToBoolean(Settings.UserSettings[ONLY_DISPLAY_BUYOUTS]);
        }
    }
}
