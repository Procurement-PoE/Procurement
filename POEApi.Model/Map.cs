using System.Collections.Generic;

namespace POEApi.Model
{
    public class Map : Item
    {
        public Rarity Rarity { get; private set; }
        public int MapTier { get; private set; }
        public int MapQuantity { get; private set; }

        public Map(JSONProxy.Item item) : base(item)
        {
            this.ItemType = Model.ItemType.Gear;
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.Rarity = getRarity(item);
            this.MapTier = int.Parse(Properties.Find(p => p.Name == "Map Tier").Values[0].Item1);
        }
    }
}
