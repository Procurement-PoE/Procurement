using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class LegionEmblem : Item, ILegion
    {
        public LegionEmblem(JSONProxy.Item item) : base(item)
        {
            Faction = ProxyMapper.GetLegionFaction(item);
        }

        public LegionFaction Faction { get; set; }
    }
}