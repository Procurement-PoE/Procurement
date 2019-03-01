using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class Breachstone : Item, IBreachCurrency
    {
        public BreachType Type { get; set; }

        public Breachstone(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}