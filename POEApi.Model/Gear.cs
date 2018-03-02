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

        protected override Dictionary<string, string> DescriptiveNameComponents
        {
            get
            {
                // TODO: Reduce code duplication between this class's implementation and AbyssJewel's (they both
                // have a "Rarity" property that works the same way, but do not inherit it from the same parent class).
                var components = base.DescriptiveNameComponents;
                if (Rarity != Rarity.Normal)
                {
                    if (!Identified)
                    {
                        components["name"] = string.Format("Unidentified {0} {1}", Rarity, TypeLine);
                    }
                    else if (this.Rarity != Rarity.Magic)
                    {
                        components["name"] = string.Format("\"{0}\", {1} {2}", Name, Rarity, TypeLine);
                    }
                }

                return components;
            }
        }
    }
}
