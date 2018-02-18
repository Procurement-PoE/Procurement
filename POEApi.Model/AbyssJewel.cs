namespace POEApi.Model
{
    public class AbyssJewel : SocketableItem
    {
        public Rarity Rarity { get; set; }

        public AbyssJewel(JSONProxy.Item item) : base(item)
        {
            Rarity = getRarity(item);
        }

        public bool Abyssal { get; } = true;

        public override string DescriptiveName
        {
            get
            {
                // TODO: Reduce code duplication between this class's implementation and AbyssJewel's (they both
                // have a "Rarity" property that works the same way, but do not inherit it from the same parent class).
                string qualityString = this.IsQuality ? string.Format("+{0}% Quality, ", this.Quality) : string.Empty;
                string itemName = this.TypeLine;
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
                return string.Format("{0}, {1}i{2}", itemName, qualityString, this.ItemLevel);
            }
        }
    }
}