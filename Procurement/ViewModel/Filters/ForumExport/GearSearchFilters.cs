using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    class RingFilter : GearTypeFilter
    {
        public RingFilter()
            : base(GearType.Ring, "Rings")
        { }
    }

    class AmuletFilter : GearTypeFilter
    {
        public AmuletFilter()
            : base(GearType.Amulet, "Amulets")
        { }
        public override bool Applicable(Item item)
        {
            Gear gear = item as Gear;

            if (gear != null)
                return gear.GearType == GearType.Amulet || gear.GearType == GearType.Talisman;

            return false;
        }
    }

    class TalismanFilter : GearTypeFilter
    {
        public TalismanFilter()
            : base (GearType.Talisman, "Talisman")
        { }
    }

    class HelmetFilter : GearTypeFilter
    {
        public HelmetFilter()
            : base(GearType.Helmet, "Helmets")
        { }
    }

    class ChestFilter : GearTypeFilter
    {
        public ChestFilter()
            : base(GearType.Chest, "BodyArmor")
        { }
    }

    class GlovesFilter : GearTypeFilter
    {
        public GlovesFilter()
            : base(GearType.Gloves, "Gloves")
        { }
    }

    class BootFilter : GearTypeFilter
    {
        public BootFilter()
            : base(GearType.Boots, "Boots")
        { }
    }

    class BeltFilter : GearTypeFilter
    {
        public BeltFilter()
            : base(GearType.Belt, "Belts")
        { }
    }

    class AxeFilter : GearTypeFilter
    {
        public AxeFilter()
            : base(GearType.Axe, "Axes")
        { }
    }

    class ClawFilter : GearTypeFilter
    {
        public ClawFilter()
            : base(GearType.Claw, "Claws")
        { }
    }

    class BowFilter : GearTypeFilter
    {
        public BowFilter()
            : base(GearType.Bow, "Bows")
        { }
    }

    class DaggerFilter : GearTypeFilter
    {
        public DaggerFilter()
            : base(GearType.Dagger, "Daggers")
        { }
    }

    class MaceFilter : GearTypeFilter
    {
        public MaceFilter()
            : base(GearType.Mace, "Maces")
        { }
    }

    class QuiverFilter : GearTypeFilter
    {
        public QuiverFilter()
            : base(GearType.Quiver, "Quivers")
        { }
    }

    class SceptreFilter : GearTypeFilter
    {
        public SceptreFilter()
            : base(GearType.Sceptre, "Sceptres")
        { }
    }


    class StaffFilter : GearTypeFilter
    {
        public StaffFilter()
            : base(GearType.Staff, "Staves")
        { }
    }

    class SwordFilter : GearTypeFilter
    {
        public SwordFilter()
            : base(GearType.Sword, "Swords")
        { }
    }

    class ShieldFilter : GearTypeFilter
    {
        public ShieldFilter()
            : base(GearType.Shield, "Shields")
        { }
    }

    class WandFilter : GearTypeFilter
    {
        public WandFilter()
            : base(GearType.Wand, "Wands")
        { }
    }

    class FishingRodFilter : GearTypeFilter
    {
        public FishingRodFilter()
            : base(GearType.FishingRod, "Fishing Rods")
        { }
    }

    class FlaskFilter : GearTypeFilter
    {
        public FlaskFilter()
            : base(GearType.Flask, "Flasks")
        { }
    }

    class DivinationCardFilter : GearTypeFilter
    {
        public DivinationCardFilter()
            : base(GearType.DivinationCard, "Divination Cards")
        {

        }
    }

    class JewelFilter : GearTypeFilter
    {
        public JewelFilter()
            : base(GearType.Jewel, "Jewels")
        {

        }
    }
}
