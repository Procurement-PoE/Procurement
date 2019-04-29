using System.Diagnostics;

namespace POEApi.Model
{
    [DebuggerDisplay("{TypeLine} Charges: {Charges.ToString()}")]
    public class Leaguestone : Item
    {
        public ChargeInfo Charges { get; }

        public Leaguestone(JSONProxy.Item item) : base(item)
        {
            Charges = ProxyMapper.GetCharges(item.Properties);
        }
    }
}