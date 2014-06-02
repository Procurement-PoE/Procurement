namespace POEApi.Model
{
    public class ItemTradeInfo
    {
        public string Buyout { get; set; }
        public string Price { get; set; }
        public string CurrentOffer { get; set; }
        public bool IsEmpty { get; set; }

        public ItemTradeInfo()
        {
            this.IsEmpty = true;
        }

        public ItemTradeInfo(string buyout, string price, string currentOffer)
        {
            this.Buyout = buyout;
            this.Price = price;
            this.CurrentOffer = currentOffer;
            this.IsEmpty = false;
        }
    }
}
