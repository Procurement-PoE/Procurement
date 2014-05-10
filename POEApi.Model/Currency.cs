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
            return 0;
        }
    }
}
