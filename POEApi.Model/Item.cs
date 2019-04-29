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
        public List<string> VeiledMods { get; set; }
        public List<string> FracturedMods { get; set; }

        public int TradeX { get; set; }
        public int TradeY { get; set; }
        public string TradeInventoryId { get; set; }
        public string Character { get; set; }
        public int ItemLevel { get; set; }
        public bool Shaper { get; set; }
        public bool Elder { get; set; }
        public bool Synthesised { get; set; }
        public bool Fractured { get; set; }
        public int StackSize { get; set; }
        public int MaxStackSize { get; set; }
        public Rarity Rarity { get; set; }

        public string BackgroundUrl { get; private set; }

        public bool HasBackground => string.IsNullOrEmpty(BackgroundUrl) == false;

        public virtual bool IsGear => false;

        protected Item(JSONProxy.Item item)
        {
            Id = item.Id;
            Verified = item.Verified;
            Identified = item.Identified;
            IsMirrored = item.Duplicated;
            W = item.W;
            H = item.H;
            IconURL = getIconUrl(item.Icon);
            League = item.League;
            Name = item.Name;
            TypeLine = item.TypeLine;
            DescrText = item.DescrText;
            X = item.X;
            Y = item.Y;
            InventoryId = item.InventoryId;
            SecDescrText = item.SecDescrText;
            Explicitmods = item.ExplicitMods;
            ItemType = Model.ItemType.UnSet;
            CraftedMods = item.CraftedMods ?? new List<string>();
            VeiledMods = item.VeiledMods ?? new List<string>();
            EnchantMods = item.EnchantMods ?? new List<string>();
            FracturedMods = item.FracturedMods ?? new List<string>();
            FlavourText = item.FlavourText;
            ItemLevel = item.Ilvl;
            Shaper = item.Shaper;
            Elder = item.Elder;
            Synthesised = item.Synthesised;
            Fractured = item.Fractured;
            StackSize = item.StackSize;
            MaxStackSize = item.MaxStackSize;

            if (item.Properties != null)
            {
                Properties = item.Properties.Select(p => new Property(p)).ToList();

                if (Properties.Any(p => p.Name == "Quality"))
                {
                    IsQuality = true;
                    Quality = ProxyMapper.GetQuality(item.Properties);
                }

                if (Properties.Any(p => p.Name == "Radius"))
                {
                    Radius = Properties.First(p => p.Name == "Radius").Values[0].Item1;
                }
            }

            Corrupted = item.Corrupted;
            Microtransactions = item.CosmeticMods ?? new List<string>();
            EnchantMods = item.EnchantMods ?? new List<string>();

            TradeX = X;
            TradeY = Y;
            TradeInventoryId = InventoryId;
            Character = string.Empty;
            Rarity = GetRarity(item);

            if (item.Elder || item.Shaper)
                BackgroundUrl = ItemBackgroundUrlBuilder.GetUrl(this);
        }

        public string Radius { get; set; }

        private string getIconUrl(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                return url;

            return "http://webcdn.pathofexile.com" + url;
        }

        private Rarity GetRarity(JSONProxy.Item item)
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
            return MemberwiseClone();
        }

        private string DebuggerDisplay
        {
            get
            {
                return AssembleDescriptiveName();
            }
        }
        
        public virtual string PobData => String.Empty;
    }
}
