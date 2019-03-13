using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POEApi.Model;

namespace Procurement.ViewModel
{
    class ItemHoverViewModelFactory
    {
        internal static ItemHoverViewModel Create(Item item)
        {
            Gear gear = item as Gear;
            Rarity? r = null;

            if (gear != null)
                r = gear.Rarity;

            Map map = item as Map;
            if (map != null)
                r = map.Rarity;

            Leaguestone stone = item as Leaguestone;
            if (stone != null)
                r = stone.Rarity;

            var vessel = item as DivineVessel;
            if(vessel != null)
                r = Rarity.Normal;

            var offering = item as Offering;
            if (offering != null)
                r = Rarity.Normal;

            var abyssJewel = item as AbyssJewel;
            if (abyssJewel != null)
            {
                r = abyssJewel.Rarity;
            }

            var fullBestiaryOrb = item as FullBestiaryOrb;
            if (fullBestiaryOrb != null)
            {
                r = fullBestiaryOrb.Rarity;
            }

            var questItem = item as QuestItem;
            if (questItem != null)
            {
                return new QuestItemHoverViewModel(item);
            }

            if (r != null)
            {
                switch (r)
                {
                    case Rarity.Relic:
                        return new RelicGearItemHoverViewModel(item);
                    case Rarity.Unique:
                        return new UniqueGearItemHoverViewModel(item);
                    case Rarity.Rare:
                        return new RareGearItemHoverViewModel(item); 
                    case Rarity.Magic:
                        return new MagicGearItemHoverViewModel(item);
                    case Rarity.Normal:
                        return new NormalGearItemHoverViewModel(item); 
                }
            }

            if (item is Gem)
                return new GemItemHoverViewModel(item);

            if (item is Currency || item is Sextant || item is Essence || item is Fossil || item is Resonator)
                return new CurrencyItemHoverViewModel(item);

            if (item is Prophecy)
                return new ProphecyItemHoverViewModel(item);

            return new ItemHoverViewModel(item);
        }
    }

    public class SextantItemHoverViewModel : ItemHoverViewModel
    {
        public SextantItemHoverViewModel(Item item) : base(item)
        { }
    }

    public class RelicGearItemHoverViewModel : ItemHoverViewModel
    {
        public RelicGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }


    public class UniqueGearItemHoverViewModel : ItemHoverViewModel
    {
        public UniqueGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class RareGearItemHoverViewModel : ItemHoverViewModel
    {
        public RareGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class MagicGearItemHoverViewModel : ItemHoverViewModel
    {
        public MagicGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class NormalGearItemHoverViewModel : ItemHoverViewModel
    {
        public NormalGearItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class GemItemHoverViewModel : ItemHoverViewModel
    {
        public GemItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class CurrencyItemHoverViewModel : ItemHoverViewModel
    {
        public CurrencyItemHoverViewModel(Item item)
            : base(item)
        { }
    }

    public class ProphecyItemHoverViewModel : ItemHoverViewModel
    {
        public ProphecyItemHoverViewModel(Item item) 
            : base(item)
        { }
    }

    public class QuestItemHoverViewModel : ItemHoverViewModel
    {
        public QuestItemHoverViewModel(Item item) 
            : base(item)
        { }
    }
}
