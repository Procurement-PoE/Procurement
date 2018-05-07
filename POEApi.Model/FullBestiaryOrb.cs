using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POEApi.Model
{
    public class FullBestiaryOrb : Item
    {
        public FullBestiaryOrb(JSONProxy.Item item) : base(item)
        {
            this.Rarity = getRarity(item);
            this.ItemType = ItemType.Currency;

            this.Genus = ProxyMapper.GetGenus(item.Properties);
            this.Group = ProxyMapper.GetGroup(item.Properties);
            this.Family = ProxyMapper.GetFamily(item.Properties);

            // TODO: This item's explicit mods are the mods of the contained beast.  These could be various types of
            // mods ("prefix mod", "suffix mod", "monster mod", etc.), but only the name is provided in the JSON for
            // the explicit mod.  Compile a list of each of these types of mods and map them here, so we can style the
            // text of the mods correctly in the item tooltip.  Right now all of the mods have the default blue
            // foreground text and no other styling, which is only correct for prefix/suffix mods.  Note that the
            // in-game detailed tooltip uses "monster mod" for different kinds of mods, including bestiary beast mods
            // (bold white text with red outline) and bloodline mods (magenta text RGB(210, 0, 220)).
        }

        public Rarity Rarity { get; }

        // TODO: Compile the possible values of Genus, Group, and Family, and use enums instead of raw strings.
        public string Genus { get; }
        public string Group { get; }
        public string Family { get; }
    }
}
