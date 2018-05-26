namespace POEApi.Model
{
    public class UnknownItem : Item
    {
        private static JSONProxy.Item getDefaultItem() => new JSONProxy.Item();

        public UnknownItem() : base(getDefaultItem())
        { }

        public UnknownItem(JSONProxy.Item item) : base(item)
        { }

        public UnknownItem(JSONProxy.Item item, string errorInformation) : this(item)
        {
            ItemSource = item;
            ErrorInformation = errorInformation;
        }

        public JSONProxy.Item ItemSource { get; }
        public string ErrorInformation { get; }
    }
}
