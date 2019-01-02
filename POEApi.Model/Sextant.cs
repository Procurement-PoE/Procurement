using System;
using System.CodeDom;

namespace POEApi.Model
{
    public class Sextant : StackableItem
    {
        public Sextant(JSONProxy.Item item) : base(item)
        {
            if (item.TypeLine.StartsWith("Apprentice"))
                Type = SextantType.Apprentice;
            else if (item.TypeLine.StartsWith("Journeyman"))
                Type = SextantType.Journeyman;
            else if (item.TypeLine.StartsWith("Master"))
                Type = SextantType.Master;
            else
                Type = SextantType.Unknown;
        }

        public SextantType Type { get; }
    }
}