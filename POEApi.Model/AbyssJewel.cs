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
    }
}