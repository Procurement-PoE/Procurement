namespace POEApi.Model
{
    public  class StackableItem : Item
    {
        public StackInfo StackInfo { get; private set; }

        public StackableItem(JSONProxy.Item item) : base(item)
        {
            StackInfo = ProxyMapper.GetStackInfo(item.Properties);
        }
    }
}