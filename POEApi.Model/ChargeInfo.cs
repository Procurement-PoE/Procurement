namespace POEApi.Model
{
    public class ChargeInfo
    {
        public int Charges { get; set; }
        public int MaxCharges { get; set; }

        internal ChargeInfo(int charges, int maxCharges)
        {
            this.Charges = charges;
            this.MaxCharges = maxCharges;
        }

        public override string ToString()
        {
            return $"{Charges}/{MaxCharges}";
        }
    }
}
