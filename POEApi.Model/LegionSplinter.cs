using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class LegionSplinter : Item, ILegion
    {
        public LegionSplinter(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetLegionType(item);
        }

        public LegionType Type { get; set; }
    }
}