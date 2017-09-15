using System.Diagnostics;

namespace POEApi.Model
{
    [DebuggerDisplay("{Amount}/{MaxSize}")]
    public class StackInfo
    {
        public int Amount { get; set; }
        public int MaxSize { get; set; }

        internal StackInfo(int amount, int maxSize)
        {
            this.Amount = amount;
            this.MaxSize = maxSize;
        }
    }

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
