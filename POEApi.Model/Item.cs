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
        Jewel,
        Currency,
    }

    public enum Rarity : int
    {
        Normal,
        Magic,
        Rare,
        Unique,
        Relic
    }

    public abstract class Item : ICloneable
    {
        public string Id { get; set; }
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
        public bool Corrupted { get; private set; }
        public List<string> Microtransactions { get; set; }
        public List<String> EnchantMods { get; set; }

        public List<string> CraftedMods { get; set; }

        public int TradeX { get; set; }
        public int TradeY { get; set; }
        public string TradeInventoryId { get; set; }
        public string Character { get; set; }
        public int ItemLevel { get; set; }
        public bool Shaper { get; set; }
        public bool Elder { get; set; }

        protected Item(JSONProxy.Item item)
        {
            this.Id = item.Id;
            this.Verified = item.Verified;
            this.Identified = item.Identified;
            this.W = item.W;
            this.H = item.H;
            this.IconURL = getIconUrl(item.Icon);
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
            this.CraftedMods = item.CraftedMods;
            this.EnchantMods = item.EnchantMods;
            this.ItemLevel = item.Ilvl;
            this.Shaper = item.Shaper;
            this.Elder = item.Elder;

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
            this.Microtransactions = item.CosmeticMods ?? new List<string>();
            this.EnchantMods = item.EnchantMods ?? new List<string>();

            this.TradeX = this.X;
            this.TradeY = this.Y;
            this.TradeInventoryId = this.InventoryId;
            this.Character = string.Empty;
        }
        private string getIconUrl(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                return url;

            return "http://webcdn.pathofexile.com" + url;
        }

        protected Rarity getRarity(JSONProxy.Item item)
        {
            //Looks like isRelic is coming across the wire as an additional field but coincidentally 9 was the correct frame type here.
            if (item.FrameType == 9 || item.IsRelic)
                return Rarity.Relic;

            if (item.FrameType <= 3)
                return (Rarity)item.FrameType;

            return Rarity.Normal;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
