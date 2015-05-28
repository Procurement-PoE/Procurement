using System.Collections.Generic;

namespace POEApi.Model
{
    public class Map : Item
    {
        public Rarity Rarity { get; private set; }
        public int MapLevel { get; private set; }
        public int MapQuantity { get; private set; }

        internal Map(JSONProxy.Item item) : base(item)
        {
            this.ItemType = Model.ItemType.Gear;
            this.Properties = ProxyMapper.GetProperties(item.Properties);
            this.Rarity = getRarity(item);
            this.MapLevel = int.Parse(Properties.Find(p => p.Name == POEApi.Model.ServerTypeRes.MapLevelText).Values[0].Item1);

            this.UniqueIDHash = base.getHash();
        }

        protected override int getConcreteHash()
        {
            var anonomousType = new
            {
                f1 = Rarity,
                f2 = MapLevel,
                f3 = MapQuantity,
            };

            return anonomousType.GetHashCode();
        }
    }
}
