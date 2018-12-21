using System;

namespace POEApi.Model
{
    public class Scarab : Item
    {
        public Scarab(JSONProxy.Item item) : base(item)
        {
            ScarabRank = GetScarabRank();
            ScarabEffect = GetScarabEffect();
        }

        public ScarabRank ScarabRank { get; }

        public ScarabEffect ScarabEffect { get; }

        private ScarabEffect GetScarabEffect()
        {
            ScarabEffect result = ScarabEffect.Unknown;

            Enum.TryParse(TypeLine.Split(' ')[1], true, out result);

            return result;
        }

        private ScarabRank GetScarabRank()
        {
            ScarabRank result = ScarabRank.Unknown;

            Enum.TryParse(TypeLine.Split(' ')[0], true, out result);

            return result;
        }
    }

    public enum ScarabRank
    {
        Unknown,

        Rusted,
        Polished,
        Gilded
    }

    public enum ScarabEffect
    {
        Unknown,

        Ambush,
        Bestiary,
        Breach,
        Cartography,
        Divination,
        Elder,
        Harbinger,
        Perandus,
        Reliquary,
        Shaper,
        Sulphite,
        Torment
    }
}