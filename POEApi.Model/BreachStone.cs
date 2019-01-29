using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class BreachStone : StackableItem, IBreachCurrency
    {
        public BreachType Type { get; set; }

        public BreachStone(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}