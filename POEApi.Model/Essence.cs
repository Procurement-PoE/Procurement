using System.Diagnostics;

namespace POEApi.Model
{
    [DebuggerDisplay("{TypeLine}")]
    public class Essence : Item
    {
        public EssenceType Type { get; }

        public Essence(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetEssenceType(item);
        }
    }
}