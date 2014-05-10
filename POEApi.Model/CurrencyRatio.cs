namespace POEApi.Model
{
    public class CurrencyRatio
    {
        public OrbType OrbType { get; set; }
        public double OrbAmount { get; set; }
        public double ChaosAmount { get; set; }

        public CurrencyRatio(OrbType orbType, double OrbAmount, double ChaosAmount)
        {
            this.OrbType = orbType;
            this.OrbAmount = OrbAmount;
            this.ChaosAmount = ChaosAmount;
        }
    }
}
