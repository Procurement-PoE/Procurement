namespace POEApi.Model
{
    public class UnknownItem : Item
    {
        private static JSONProxy.Item getDefaultItem() => new JSONProxy.Item();

        public UnknownItem() : base(getDefaultItem())
        { }

        public UnknownItem(JSONProxy.Item item) : base(item)
        {
            ItemSource = item;
        }

        public UnknownItem(JSONProxy.Item item, string errorInformation) : this(item)
        {
            ErrorInformation = errorInformation;
        }

        protected override string AssembleDescriptiveName()
        {
            return string.Format("{0} {1}", "[Unknown Item]", base.AssembleDescriptiveName()).TrimEnd();
        }

        public JSONProxy.Item ItemSource { get; }
        public string ErrorInformation { get; }
    }
}
