using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class BreachBlessing : StackableItem, IBreachCurrency
    {
        public BreachType Type { get; set; }

        public BreachBlessing(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}