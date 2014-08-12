using System;
using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public enum ItemType : int
    {
        UnSet,
        Gear,
        Gem,
        Currency,
    }

    public enum Rarity : int
    {
        Normal,
        Magic,
        Rare,
        Unique
    }

    public abstract class Item : ICloneable
    {
        public bool Verified { get; private set; }
        public bool Identified { get; private set; }
        public int W { get; private set; }
        public int H { get; private set; }
        public string IconURL { get; private set; }
        public string League { get; private set; }
        public string Name { get; private set; }
        public string TypeLine { get; private set; }
        public string DescrText { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string InventoryId { get; set; }
        public string SecDescrText { get; private set; }
        public List<string> Explicitmods { get; set; }
        public ItemType ItemType { get; set; }
        public List<Property> Properties { get; set; }
        public bool IsQuality { get; private set; }
        public int Quality { get; private set; }
        public int UniqueIDHash { get; set; }
        public bool Corrupted { get; private set; }
        public List<string> Microtransactions { get; set; }

        public int TradeX { get; set; }
        public int TradeY { get; set; }
        public string TradeInventoryId { get; set; }
        public string Character { get; set; }

        protected Item(JSONProxy.Item item)
        {
            this.Verified = item.Verified;
            this.Identified = item.Identified;
            this.W = item.W;
            this.H = item.H;
            this.IconURL = item.Icon;
            this.League = item.League;
            this.Name = item.Name;
            this.TypeLine = item.TypeLine;
            this.DescrText = item.DescrText;
            this.X = item.X;
            this.Y = item.Y;
            this.InventoryId = item.InventoryId;
            this.SecDescrText = item.SecDescrText;
            this.Explicitmods = item.ExplicitMods;
            this.ItemType = Model.ItemType.UnSet;

            if (item.Properties != null)
            {
                this.Properties = item.Properties.Select(p => new Property(p)).ToList();

                if (this.Properties.Any(p => p.Name == "Quality"))
                {
                    this.IsQuality = true;
                    this.Quality = ProxyMapper.GetQuality(item.Properties);
                }
            }

            this.Corrupted = item.Corrupted;
            this.Microtransactions = item.CosmeticMods == null ? new List<string>() : item.CosmeticMods;

            this.TradeX = this.X;
            this.TradeY = this.Y;
            this.TradeInventoryId = this.InventoryId;
            this.Character = string.Empty;
        }

        protected abstract int getConcreteHash();

        protected int getHash()
        {
            var anonomousType = new
            {
                f = this.IconURL,
                f1 = this.League,
                f2 = this.Name,
                f3 = this.TypeLine,
                f4 = this.DescrText,
                f5 = this.Explicitmods != null ? string.Join(string.Empty, this.Explicitmods.ToArray()) : string.Empty,
                f6 = this.Properties != null ? string.Join(string.Empty, this.Properties.Select(p => string.Concat(p.DisplayMode, p.Name, string.Join(string.Empty, p.Values.Select(t => string.Concat(t.Item1, t.Item2)).ToArray()))).ToArray()) : string.Empty,
                f7 = getConcreteHash()
            };

            return anonomousType.GetHashCode();
        }

        protected Rarity getRarity(JSONProxy.Item item)
        {
            if (item.frameType <= 3)
                return (Rarity)item.frameType;

            return Rarity.Normal;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
