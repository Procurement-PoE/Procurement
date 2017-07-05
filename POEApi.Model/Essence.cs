using System.Diagnostics;

namespace POEApi.Model
{
    [DebuggerDisplay("{TypeLine}")]
    public class Essence : StackableItem
    {
        public EssenceType Type { get; }

        public Essence(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetEssenceType(item);
        }
    }
}