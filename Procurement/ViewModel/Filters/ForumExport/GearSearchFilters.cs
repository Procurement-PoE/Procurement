using POEApi.Model;

namespace Procurement.ViewModel.Filters.ForumExport
{
    class RingFilter : GearTypeFilter
    {
        public RingFilter()
            : base(GearType.Ring, "Rings")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Кольца";
        }
    }

    class AmuletFilter : GearTypeFilter
    {
        public AmuletFilter()
            : base(GearType.Amulet, "Amulets")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Амулеты";
        }
    }

    class HelmetFilter : GearTypeFilter
    {
        public HelmetFilter()
            : base(GearType.Helmet, "Helmets")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Шлемы";
        }
    }

    class ChestFilter : GearTypeFilter
    {
        public ChestFilter()
            : base(GearType.Chest, "BodyArmor")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Броня";
        }
    }

    class GlovesFilter : GearTypeFilter
    {
        public GlovesFilter()
            : base(GearType.Gloves, "Gloves")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Перчатки";
        }
    }

    class BootFilter : GearTypeFilter
    {
        public BootFilter()
            : base(GearType.Boots, "Boots")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Ботинки";
        }
    }

    class BeltFilter : GearTypeFilter
    {
        public BeltFilter()
            : base(GearType.Belt, "Belts")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Пояса";
        }
    }

    class AxeFilter : GearTypeFilter
    {
        public AxeFilter()
            : base(GearType.Axe, "Axes")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Топоры";
        }
    }

    class ClawFilter : GearTypeFilter
    {
        public ClawFilter()
            : base(GearType.Claw, "Claws")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Когти";
        }
    }

    class BowFilter : GearTypeFilter
    {
        public BowFilter()
            : base(GearType.Bow, "Bows")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Луки";
        }
    }

    class DaggerFilter : GearTypeFilter
    {
        public DaggerFilter()
            : base(GearType.Dagger, "Daggers")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Кинжалы";
        }
    }

    class MaceFilter : GearTypeFilter
    {
        public MaceFilter()
            : base(GearType.Mace, "Maces")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Булавы";
        }
    }

    class QuiverFilter : GearTypeFilter
    {
        public QuiverFilter()
            : base(GearType.Quiver, "Quivers")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Колчаны";
        }
    }

    class SceptreFilter : GearTypeFilter
    {
        public SceptreFilter()
            : base(GearType.Sceptre, "Sceptres")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Скипетры";
        }
    }


    class StaffFilter : GearTypeFilter
    {
        public StaffFilter()
            : base(GearType.Staff, "Staves")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Посохи";
        }
    }

    class SwordFilter : GearTypeFilter
    {
        public SwordFilter()
            : base(GearType.Sword, "Swords")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Мечи";
        }
    }

    class ShieldFilter : GearTypeFilter
    {
        public ShieldFilter()
            : base(GearType.Shield, "Shields")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Щиты";
        }
    }

    class WandFilter : GearTypeFilter
    {
        public WandFilter()
            : base(GearType.Wand, "Wands")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Жезлы";
        }
    }

    class FlaskFilter : GearTypeFilter
    {
        public FlaskFilter()
            : base(GearType.Flask, "Flasks")
        {
            if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                this.Keyword = "Флаконы";
        }
    }
}
