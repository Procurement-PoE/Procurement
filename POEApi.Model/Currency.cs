namespace POEApi.Model
{
    public class Currency : Item
    {
        public OrbType Type { get; private set; }
        public double ChaosValue { get; private set; }
        public StackInfo StackInfo { get; private set; }

        public Currency(JSONProxy.Item item) : base(item)
        {
            this.Type = ProxyMapper.GetOrbType(item);
            this.ChaosValue = CurrencyHandler.GetChaosValue(this.Type);
            this.StackInfo = ProxyMapper.GetStackInfo(item.Properties);

            this.UniqueIDHash = base.getHash();
        }

        protected override int getConcreteHash()
        {
            var anonomousType = new
            {
                f = this.IconURL,
                f2 = this.Name,
                f3 = this.TypeLine,
                f4 = this.DescrText,
                f5 = this.X,
                f6 = this.Y,
                f7 = this.InventoryId
            };

            return anonomousType.GetHashCode();
        }
    }
}
