namespace POEApi.Model
{
    public class BreachBlessing : StackableItem
    {
        public BreachType Type { get; set; }
        public BreachBlessing(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}