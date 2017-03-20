namespace POEApi.Model
{
    public class Leaguestone : Item
    {
        public ChargeInfo Charges { get; }
        public Rarity Rarity { get; }

        public Leaguestone(JSONProxy.Item item) : base(item)
        {
            Charges = ProxyMapper.GetCharges(item.Properties);
            Rarity = getRarity(item);
        }

        protected override int getConcreteHash()
        {
            var anonomousType = new
            {
                f1 = Rarity,
                f2 = Charges,
            };

            return anonomousType.GetHashCode();
        }
    }
}