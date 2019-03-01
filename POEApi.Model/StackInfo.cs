using System;
using System.Diagnostics;

namespace POEApi.Model
{
    //Todo: Stackable items now have it in the base of the message - rather than the properties. 
    //This is left here for the items that used to have their stack info in the properties - they just
    //duplicate the information in the json
    [DebuggerDisplay("{Amount}/{MaxSize}")]
    [Obsolete("Please use StackSize and MaxStackSize on the item instead.")]
    public class StackInfo
    {
        public int Amount { get; set; }
        public int MaxSize { get; set; }

        internal StackInfo(int amount, int maxSize)
        {
            Amount = amount;
            MaxSize = maxSize;
        }
    }

    public class ChargeInfo
    {
        public int Charges { get; set; }
        public int MaxCharges { get; set; }

        internal ChargeInfo(int charges, int maxCharges)
        {
            Charges = charges;
            MaxCharges = maxCharges;
        }

        public override string ToString()
        {
            return $"{Charges}/{MaxCharges}";
        }
    }
}
