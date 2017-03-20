namespace POEApi.Model
{
    public class Leaguestone : Item
    {
        public ChargeInfo Charges { get; }
        public Leaguestone(JSONProxy.Item item) : base(item)
        {
            Charges = ProxyMapper.GetCharges(item.Properties);
        }

        protected override int getConcreteHash()
        {
            return 0;
        }
    }
}