using System.Collections.Generic;

namespace POEApi.Model
{
    public class Map : Item
    {
        public Rarity Rarity { get; private set; }
        public int MapTier { get; private set; }
        public int MapQuantity { get; private set; }

        internal Map(JSONProxy.Item item) : base(item)
        {
            this.ItemType = Model.ItemType.Gear;
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.Rarity = getRarity(item);
            this.MapTier = int.Parse(Properties.Find(p => p.Name == "Map Tier").Values[0].Item1);

            this.UniqueIDHash = base.getHash();
        }

        protected override int getConcreteHash()
        {
            var anonomousType = new
            {
                f1 = Rarity,
                f2 = MapTier,
                f3 = MapQuantity,
            };

            return anonomousType.GetHashCode();
        }
    }
}
