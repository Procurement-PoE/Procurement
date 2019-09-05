using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Infrastructure;

namespace POEApi.Model
{
    public class Gear : Item
    {
        public List<Socket> Sockets { get; set; }
        public List<SocketableItem> SocketedItems { get; set; }
        public List<string> Implicitmods { get; set; }
        public List<Requirement> Requirements { get; set; }
        public GearType GearType { get; set; }
        public string BaseType { get; set; }

        public override bool IsGear => true;

        public Gear(JSONProxy.Item item) : base(item)
        {
            Sockets = GetSockets(item);
            Explicitmods = item.ExplicitMods;
            SocketedItems = GetSocketedItems(item);
            Implicitmods = item.ImplicitMods;
            Requirements = ProxyMapper.GetRequirements(item.Requirements);
            ItemType = Model.ItemType.Gear;
            GearType = GearTypeFactory.GetType(this);
            BaseType = GearTypeFactory.GetBaseType(this);
        }

        private List<Socket> GetSockets(JSONProxy.Item item) =>
            item.Sockets == null ? new List<Socket>() : item.Sockets.Select(proxy => new Socket(proxy)).ToList();

        private List<SocketableItem> GetSocketedItems(JSONProxy.Item item) =>
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
                        string quotedName = string.IsNullOrWhiteSpace(Name) ? string.Empty :
                            string.Format("\"{0}\", ", Name);
                        components["name"] = string.Format("{0}{1} {2}", quotedName, Rarity, TypeLine);
                    }
                }

                return components;
            }
        }

        public override string PobData
        {
            get
            {
                try
                {
                    var pobData = new StringBuilder();
                    pobData.AppendLine(Name);
                    pobData.AppendLine(TypeLine);
                    pobData.AppendLine($"Unique ID: {Id}");

                    var itemType = GetSpecialItemType();
                    if (string.IsNullOrEmpty(itemType) == false)
                        pobData.AppendLine(itemType);

                    if (Corrupted)
                        pobData.AppendLine(nameof(Corrupted));

                    if (string.IsNullOrEmpty(Radius) == false)
                        pobData.AppendLine($"Radius: {Radius}");

                    pobData.AppendLine($"Quality: {Quality}");

                    if (Sockets != null && Sockets.Any())
                    {
                        //Links are denoted by "Groups"
                        List<int> groups = Sockets.Select(x => x.Group).Distinct().ToList();
                        string socketData = string.Empty;
                            
                        foreach (var group in groups)
                        {
                            socketData += string.Join("=", Sockets.Where(x => x.Group == group).Select(y => y.ToPobFormat()));
                            
                            //Don't append a space on the last character
                            if (groups.Last() != group)
                            {
                                //Space character denotes group demarcations
                                socketData += " ";
                            }
                        }

                        pobData.AppendLine($"Sockets: {socketData}");
                    }

                    if (Implicitmods != null && Implicitmods.Any())
                    {
                        pobData.AppendLine($"Implicits: {Implicitmods.Count}");
                        Implicitmods.ForEach(x => pobData.AppendLine($"{{crafted}}{x}"));
                    }

                    if (FracturedMods != null && FracturedMods.Any())
                    {
                        FracturedMods.ForEach(x => pobData.AppendLine($"{{fractured}}{x}"));
                    }

                    if (Explicitmods != null && Explicitmods.Any())
                    {
                        Explicitmods.ForEach(x=> pobData.AppendLine(x));
                    }

                    if (CraftedMods != null && CraftedMods.Any())
                    {
                        CraftedMods.ForEach(x => pobData.AppendLine($"{{crafted}}{x}"));
                    }

                    return pobData.ToString();
                }
                catch (Exception e)
                {
                    Logger.Log(e);
                    throw;
                }
            }
        }

        private string GetSpecialItemType()
        {
            if (Elder)
            {
                return "Elder Item";
            }

            if (Shaper)
            {
                return "Shaper Item";
            }

            if (Fractured)
            {
                return "Fractured Item";
            }

            if (Synthesised)
            {
                return "Synthesised Item";
            }

            return null;
        }
    }
}
