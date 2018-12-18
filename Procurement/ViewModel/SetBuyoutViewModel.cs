using POEApi.Model;
using System.Collections.Generic;

namespace Procurement.ViewModel
{
    public class SetBuyoutViewModel : ObservableBase
    {
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
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public SetBuyoutViewModel()
        {
            
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
    }
}
