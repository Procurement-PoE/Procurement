namespace POEApi.Model
{
    public class UnknownItem : Item
    {
        private static JSONProxy.Item getDefaultItem() => new JSONProxy.Item();

        public UnknownItem() : base(getDefaultItem())
        { }
    }
}
