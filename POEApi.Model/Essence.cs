namespace POEApi.Model
{
    public class Essence : StackableItem
    {
        public EssenceType Type { get; }

        public Essence(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetEssenceType(item);
        }
    }
}