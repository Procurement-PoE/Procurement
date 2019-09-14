using System;
using POEApi.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Procurement.View.ViewModel;

namespace Procurement.ViewModel
{
    public class SetBuyoutViewModel : ObservableBase
    {
        private readonly Item item;

        //Todo: Replace this with ProxyMapper.orbMap, duplicate info.
        private static List<string> orbTypes = new List<string>()
        {
            "Chaos Orb",
            "Vaal Orb",
            "Exalted Orb",
            "Divine Orb",
            "Orb of Fusing",
            "Orb of Alchemy",
            "Orb of Alteration",
            "Gemcutter's Prism",
            "Orb of Chance",
            "Cartographer's Chisel",
            "Orb of Scouring",
            "Orb of Regret",
            "Regal Orb",
            "Jeweller's Orb",
            "Chromatic Orb",
            "Blessed Orb"
        };
        public List<string> OrbTypes
        {
            get { return orbTypes; }
        }
        private PricingInfo buyoutInfo;
        public PricingInfo BuyoutInfo
        {
            get { return buyoutInfo; }
            set { buyoutInfo = value; }
        }
        private PricingInfo offerInfo;
        public PricingInfo OfferInfo
        {
            get { return offerInfo; }
            set { offerInfo = value; }
        }
        private PricingInfo priceInfo;
        public PricingInfo PriceInfo
        {
            get { return priceInfo; }
            set { priceInfo = value; }
        }

        private string notes;
        private bool isDataInClipboard;

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public SetBuyoutViewModel(Item item)
        {
            this.item = item;

            buyoutInfo = new PricingInfo();
            offerInfo = new PricingInfo();
            priceInfo = new PricingInfo();
            Notes = string.Empty;
        }

        public void SetBuyoutInfo(ItemTradeInfo info)
        {
            buyoutInfo.Update(info.Buyout);
            offerInfo.Update(info.CurrentOffer);
            priceInfo.Update(info.Price);
            Notes = info.Notes;
        }

        public bool IsDataInClipboard
        {
            get { return isDataInClipboard; }
            set
            {
                isDataInClipboard = value;
                OnPropertyChanged();
            }
        }

        public ICommand PobDataGenerationCommand => new RelayCommand(GetPobData, CanGetPobData);

        private void GetPobData(object o)
        {
            try
            {
                Clipboard.SetText(item.PobData);

                IsDataInClipboard = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to generate POB item data; please check log.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanGetPobData(object o)
        {
            return item.GetType() == typeof(Gear) && item.ItemLevel > 0;
        }
    }
}
