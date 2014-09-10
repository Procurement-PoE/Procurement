using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public List<Requirement> Requirements { get; set; }
        public List<Socket> Sockets { get; set; }
        public ItemType ItemType { get; set; }
        public Rarity ItemRarity { get; private set; }
        public List<Property> Properties { get; set; }
        public bool IsQuality { get; private set; }
        public int Quality { get; private set; }
        public int UniqueIDHash { get; set; }
        public bool Corrupted { get; private set; }
        public List<string> Microtransactions { get; set; }
        public GearType GearType { get; set; }

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
            this.ItemRarity = getRarity(item);
            this.Requirements = ProxyMapper.GetRequirements(item.Requirements);
            this.Sockets = item.Sockets.Select(proxy => new Socket(proxy)).ToList();

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

        private string getIconUrl(string url)
        {
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                return url;

            return "http://webcdn.pathofexile.com" + url;
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

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            if ((this.ItemType == Model.ItemType.Gear || this.ItemType == Model.ItemType.Gem) && this.GearType != null)
            {
                sb.AppendLine("Rarity: " + getRarityString(this.ItemRarity));
                sb.AppendLine(this.Name);
                sb.AppendLine(this.TypeLine);

                //sometimes the "typename is empty" -> amulets
                if (this.Properties != null && this.Properties.Count > 0)
                {
                    sb.AppendLine("--------");
                    //When a flask, use different string, count-1 because the last property seems to be always "chargesleft"
                    if (this.GearType == Model.GearType.Flask)
                    {
                        for (int i = 0; i < this.Properties.Count - 1; i++)
                        {
                            if (this.Properties[i].Name.Equals("Quality"))
                            {
                                sb.Append(this.Properties[i].Name + ": " + this.Properties[i].Values[0].Item1);
                            }
                            else
                            {
                                sb.Append(String.Format(this.Properties[i].Name.Replace("%0", "{0}").Replace("%1", "{1}"), this.Properties[i].Values[0].Item1, this.Properties[i].Values[1].Item1));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this.Properties.Count; i++)
                        {
                            if (this.Properties[i].Values.Count > 0)
                            {
                                sb.Append(this.Properties[i].Name + ": " + this.Properties[i].Values[0].Item1);
                                if (this.Properties[i].Values[0].Item2 > 0)
                                    sb.AppendLine(" (augmented)");
                                else
                                    sb.AppendLine();
                            }
                            else
                                sb.AppendLine(this.Properties[i].Name);
                        }
                    }
                }

                if (this.Requirements != null && this.Requirements.Count > 0)
                {
                    sb.AppendLine("--------");
                    sb.AppendLine("Requirements:");
                    for (int i = 0; i < this.Requirements.Count; i++)
                    {
                        sb.AppendLine(this.Requirements[i].Name + ": " + this.Requirements[i].Value);
                    }
                }

                if (this.Sockets != null && this.Sockets.Count > 0)
                {
                    sb.AppendLine("Sockets:");
                    sb.AppendLine("--------");
                    sb.AppendLine(Item.getSocketString(this.Sockets));
                }
                //sb.AppendLine("Itemlevel: " );
                //sb.AppendLine("--------");
                if (this.Explicitmods != null && this.Explicitmods.Count > 0)
                {
                    sb.AppendLine("--------");
                    for (int i = 0; i < this.Explicitmods.Count; i++)
                    {
                        sb.AppendLine(this.Explicitmods[i]);
                    }
                }
            }
            else
                sb.AppendLine(this.TypeLine);
            return sb.ToString();
        }
        public String getRarityString(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Normal: return "Normal";
                case Rarity.Magic: return "Magic";
                case Rarity.Rare: return "Rare";
                case Rarity.Unique: return "Unique";
                default: return "Normal";
            }
        }
        public static String getSocketString(List<Socket> sockets)
        {
            StringBuilder sb = new StringBuilder();
            var groupsockets = sockets.GroupBy(u => u.Group).Select(grp => grp.ToList()).ToList();
            foreach (var group in groupsockets)
            {
                foreach (var socket in group)
                {
                    sb.Append(Socket.encodeSocketChar(socket.Attribute) + "-");
                }
                sb.Length--;
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
