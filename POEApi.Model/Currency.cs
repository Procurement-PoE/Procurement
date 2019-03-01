using System.Diagnostics;

namespace POEApi.Model
{
    [DebuggerDisplay("Type: {Type} Stack: {StackInfo.Amount}/{StackInfo.MaxSize}")]
    public class Currency : Item
    {
        public Currency(JSONProxy.Item item) : base(item)
        {
            ItemType = ItemType.Currency;
            Type = ProxyMapper.GetOrbType(item);
            ChaosValue = CurrencyHandler.GetChaosValue(Type);
        }

        public OrbType Type { get; }
        public double ChaosValue { get; private set; }
       
    }
}