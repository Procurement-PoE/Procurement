using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class LegionSplinter : Item, ILegion
    {
        public LegionSplinter(JSONProxy.Item item) : base(item)
        {
            Faction = ProxyMapper.GetLegionFaction(item);
        }

        public LegionFaction Faction { get; set; }
    }
}