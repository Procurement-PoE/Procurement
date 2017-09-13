using System.Diagnostics;

namespace POEApi.Model
{
    [DebuggerDisplay("Type: {Type} Stack: {StackInfo.Amount}/{StackInfo.MaxSize}")]
    public class Currency : StackableItem
    {
        public Currency(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetOrbType(item);
            ChaosValue = CurrencyHandler.GetChaosValue(Type);
        }

        public OrbType Type { get; }
        public double ChaosValue { get; private set; }
       
    }
}