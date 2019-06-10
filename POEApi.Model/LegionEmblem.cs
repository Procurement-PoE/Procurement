using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class LegionEmblem : Item, ILegion
    {
        public LegionEmblem(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetLegionType(item);
        }

        public LegionType Type { get; set; }
    }
}