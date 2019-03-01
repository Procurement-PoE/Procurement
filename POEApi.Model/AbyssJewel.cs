using System.Collections.Generic;

namespace POEApi.Model
{
    public class AbyssJewel : SocketableItem
    {
        public Rarity Rarity { get; set; }

        public AbyssJewel(JSONProxy.Item item) : base(item)
        {
            Rarity = getRarity(item);
        }

        public bool Abyssal { get; } = true;

        protected override Dictionary<string, string> DescriptiveNameComponents
        {
            get
            {
                // TODO: Reduce code duplication between this class's implementation and Gear's (they both have a
                // "Rarity" property that works the same way, but do not inherit it from the same parent class).
                var components = base.DescriptiveNameComponents;
                if (Rarity != Rarity.Normal)
                {
                    if (!Identified)
                    {
                        components["name"] = string.Format("Unidentified {0} {1}", Rarity, TypeLine);
                    }
                    else if (Rarity != Rarity.Magic)
                    {
                        components["name"] = string.Format("\"{0}\", {1} {2}", Name, Rarity, TypeLine);
                    }
                }

                return components;
            }
        }
    }
}