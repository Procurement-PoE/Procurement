using System.Collections.Generic;

namespace POEApi.Model
{
    public class AbyssJewel : SocketableItem
    {
        public AbyssJewel(JSONProxy.Item item) : base(item)
        {

        }

        public bool Abyssal { get; } = true;

        protected override Dictionary<string, string> DescriptiveNameComponents
        {
            get
            {
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