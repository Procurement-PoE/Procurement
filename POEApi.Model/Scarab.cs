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
            if (TypeLine.Contains("Ambush"))
            {
                return ScarabEffect.Ambush;
            }

            if (TypeLine.Contains("Bestiary"))
            {
                return ScarabEffect.Bestiary;
            }

            if (TypeLine.Contains("Breach"))
            {
                return ScarabEffect.Breach;
            }

            if (TypeLine.Contains("Cartography"))
            {
                return ScarabEffect.Cartography;
            }

            if (TypeLine.Contains("Divination"))
            {
                return ScarabEffect.Divination;
            }

            if (TypeLine.Contains("Elder"))
            {
                return ScarabEffect.Elder;
            }

            if (TypeLine.Contains("Harbinger"))
            {
                return ScarabEffect.Harbinger;
            }

            if (TypeLine.Contains("Perandus"))
            {
                return ScarabEffect.Perandus;
            }

            if (TypeLine.Contains("Reliquary"))
            {
                return ScarabEffect.Reliquary;
            }

            if (TypeLine.Contains("Shaper"))
            {
                return ScarabEffect.Shaper;
            }

            if (TypeLine.Contains("Sulphite"))
            {
                return ScarabEffect.Sulphite;
            }

            if (TypeLine.Contains("Torment"))
            {
                return ScarabEffect.Torment;
            }

            return ScarabEffect.Unknown;
        }

        private ScarabRank GetScarabRank()
        {
            if (TypeLine.Contains("Gilded"))
            {
                return ScarabRank.Gilded;
            }

            if (TypeLine.Contains("Polished"))
            {
                return ScarabRank.Polished;
            }

            if (TypeLine.Contains("Rusted"))
            {
                return ScarabRank.Rusted;
            }

            return ScarabRank.Unknown;
        }
    }

    public enum ScarabRank
    {
        Rusted,
        Polished,
        Gilded,

        Unknown
    }

    public enum ScarabEffect
    {
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
        Torment,

        Unknown
    }
}