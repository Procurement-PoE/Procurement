using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public class Gear : Item
    {
        public Rarity Rarity { get; private set; }
        public List<Socket> Sockets { get; set; }
        public List<SocketableItem> SocketedItems { get; set; }
        public List<string> Implicitmods { get; set; }
        public List<Requirement> Requirements { get; set; }
        public GearType GearType { get; set; }
        public string BaseType { get; set; }

        public Gear(JSONProxy.Item item) : base(item)
        {
            this.Rarity = getRarity(item);
            this.Sockets = getSockets(item);
            this.Explicitmods = item.ExplicitMods;
            this.SocketedItems = getSocketedItems(item);
            this.Implicitmods = item.ImplicitMods;
            this.Requirements = ProxyMapper.GetRequirements(item.Requirements);
            this.ItemType = Model.ItemType.Gear;
            this.GearType = GearTypeFactory.GetType(this);
            this.BaseType = GearTypeFactory.GetBaseType(this);
        }

        private List<Socket> getSockets(JSONProxy.Item item) =>
            item.Sockets == null ? new List<Socket>() : item.Sockets.Select(proxy => new Socket(proxy)).ToList();

        private List<SocketableItem> getSocketedItems(JSONProxy.Item item) =>
            item.SocketedItems == null ? new List<SocketableItem>() :
            item.SocketedItems.Select(proxy => (SocketableItem)ItemFactory.Get(proxy)).ToList();

        public bool IsLinked(int links)
        {
            return Sockets.GroupBy(s => s.Group).Any(g => g.Count() == links);
        }

        public int NumberOfSockets()
        {
            return Sockets.Count();
        }

        public override string DescriptiveName
        {
            get
            {
                // TODO: Reduce code duplication between this class's implementation and AbyssJewel's (they both
                // have a "Rarity" property that works the same way, but do not inherit it from the same parent class).
                string qualityString = this.IsQuality ? string.Format(", +{0}% Quality", this.Quality) : string.Empty;
                string itemName = /*this.BaseType ??*/ this.TypeLine;
                if (this.Rarity != Rarity.Normal)
                {
                    if (!this.Identified)
                    {
                        itemName = string.Format("Unidentified {0} {1}", this.Rarity, this.TypeLine);
                    }
                    else if (this.Rarity != Rarity.Magic)
                    {
                        itemName = string.Format("\"{0}\", {1} {2}", this.Name, this.Rarity, this.TypeLine);
                    }
                }
                string iLevelString = this.ItemLevel > 0 ? string.Format(", i{0}", this.ItemLevel) : string.Empty;
                return string.Format("{0}{1}{2}", itemName, qualityString, iLevelString);
            }
        }
    }
}
