using System.Collections.Generic;

namespace POEApi.Model
{
    public class Map : Item
    {
        public int MapTier { get; private set; }
        public int MapQuantity { get; private set; }

        public Map(JSONProxy.Item item) : base(item)
        {
            ItemType = Model.ItemType.Gear;
            Properties = ProxyMapper.GetProperties(item.Properties);
            MapTier = int.Parse(Properties.Find(p => p.Name == "Map Tier").Values[0].Item1);
        }
    }
}
