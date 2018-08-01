using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class Item : ICloneable
    {
        public string Id { get; set; }
        public bool Verified { get; private set; }
        public bool Identified { get; private set; }
        // Only non-unique equipment, non-corrupted item, or maps that are not already mirrored can be mirrored.
        public bool IsMirrored { get; set; }
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
        public List<string> FlavourText { get; set; }

        public List<string> CraftedMods { get; set; }

        public int TradeX { get; set; }
        public int TradeY { get; set; }
        public string TradeInventoryId { get; set; }
        public string Character { get; set; }
        public int ItemLevel { get; set; }
        public bool Shaper { get; set; }
        public bool Elder { get; set; }
        public int StackSize { get; set; }
        public int MaxStackSize { get; set; }

        protected Item(JSONProxy.Item item)
        {
            this.Id = item.Id;
            this.Verified = item.Verified;
            this.Identified = item.Identified;
            this.IsMirrored = item.Duplicated;
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
            this.FlavourText = item.FlavourText;
            this.ItemLevel = item.Ilvl;
            this.Shaper = item.Shaper;
            this.Elder = item.Elder;
            this.StackSize = item.StackSize;
            this.MaxStackSize = item.MaxStackSize;

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

        // TODO: Allow providing a format string in another function, so how an Item is presented can be customized.
        //       Something similar to (but not as extreme as) the DateTime class' ToString() method.
        // (See: https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
        public virtual string DescriptiveName
        {
            get
            {
                return AssembleDescriptiveName();
            }
        }

        protected virtual Dictionary<string, string> DescriptiveNameComponents
        {
            get
            {
                // TODO: Use a persistent Dictionary that we do not need to recreate for every call.  But this would
                // require reworking the class in multiple places and in the (applicable) getters, so it would not be
                // trivial.  Could make the recreation "lazy", however, by just setting a "dirty" flag in the property
                // setters, and recreating the Dictionary if the data is dirty.
                return new Dictionary<string, string>
                {
                    { "quality", IsQuality ? string.Format("+{0}% Quality", Quality) : null },
                    { "iLevel",  ItemLevel > 0 ? string.Format("i{0}", ItemLevel) : null },
                    { "name", TypeLine },
                };
            }
        }

        protected virtual string AssembleDescriptiveName()
        {
            var parts = DescriptiveNameComponents;
            var orderedParts = new List<string>
            {
                parts["name"], parts["quality"], parts["iLevel"]
            }.Where(i => !string.IsNullOrWhiteSpace(i));
            return string.Join(", ", orderedParts);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private string DebuggerDisplay
        {
            get
            {
                return AssembleDescriptiveName();
            }
        }
    }
}
