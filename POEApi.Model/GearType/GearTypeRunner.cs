using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public abstract class GearTypeRunner
    {
        public abstract bool IsCompatibleType(Gear item);
        public abstract string GetBaseType(Gear item);
        public GearType Type { get; set; }

        public GearTypeRunner(GearType gearType)
        {
            Type = gearType;
        }
    }

    public class GearTypeRunnerBase : GearTypeRunner
    {
        protected List<string> _generalTypes;
        protected List<string> _compatibleTypes;
        protected List<string> _incompatibleTypes;

        public GearTypeRunnerBase(GearType gearType, IEnumerable<string> compatibleTypes)
            : base(gearType)
        {
            _generalTypes = new List<string>();
            _compatibleTypes = compatibleTypes.OrderByDescending(s => s.Length).ToList();
            _incompatibleTypes = new List<string>();
        }

        public override bool IsCompatibleType(Gear item)
        {
            if (_incompatibleTypes != null && _incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return false;
            
            // First, check the general types, to see if there is an easy match.
            foreach (var type in _generalTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            // Second, check all known types.
            foreach (var type in _compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            return false;
        }

        public override string GetBaseType(Gear item)
        {
            if (_incompatibleTypes != null && _incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return null;

            foreach (var type in _compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return type;

            return null;
        }
    }

    public class RingRunner : GearTypeRunnerBase
    {
        public RingRunner()
            : base(GearType.Ring, Settings.GearBaseTypes[GearType.Ring])
        {
            _incompatibleTypes = new List<string>() { "Ringmail" };
        }

        public override bool IsCompatibleType(Gear item)
        {
            if (item.TypeLine.Contains("Ring") && !_incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return true;

            return false;
        }
    }

    public class AmuletRunner : GearTypeRunnerBase
    {
        public AmuletRunner()
            : base(GearType.Amulet, Settings.GearBaseTypes[GearType.Amulet])
        {
            _generalTypes.Add("Amulet");
        }
    }

    public class TalismanRunner : GearTypeRunnerBase
    {
        public TalismanRunner()
            : base(GearType.Talisman, new List<string>() { "Talisman" })
        {
            _generalTypes.Add("Talisman");
        }
    }

    public class BreachstoneRunner : GearTypeRunnerBase
    {
        public BreachstoneRunner()
            : base(GearType.Breachstone, new List<string>()
            {
                "Chayula's Breachstone",
                "Xoph's Breachstone",
                "Esh's Breachstone",
                "Tul's Breachstone",
                "Uul-Netol's Breachstone"
            })
        {
            _generalTypes.Add("Breachstone");
        }
    }

    public class LeaguestoneRunner : GearTypeRunnerBase
    {
        public LeaguestoneRunner() : base(GearType.Leaguestone, new List<string>() { "Leaguestone" })
        {
            _generalTypes.Add("Leaguestone");
        }
    }

    public class HelmetRunner : GearTypeRunnerBase
    {
        public HelmetRunner()
            : base(GearType.Helmet, Settings.GearBaseTypes[GearType.Helmet])
        {
            _generalTypes.AddRange(new List<string>() { "Helmet", "Circlet", "Cap", "Mask", "Chain Coif", "Casque", "Hood", "Ringmail Coif", "Chainmail Coif", "Ring Coif", "Crown", "Burgonet", "Bascinet", "Pelt" });
            _incompatibleTypes.Add("Her Mask");
        }
    }

    public class ChestRunner : GearTypeRunnerBase
    {
        public ChestRunner()
            : base(GearType.Chest, Settings.GearBaseTypes[GearType.Chest])
        {
            
        }
    }

    public class BeltRunner : GearTypeRunnerBase
    {
        public BeltRunner()
            : base(GearType.Belt, Settings.GearBaseTypes[GearType.Belt])
        {
            _generalTypes.Add("Belt");
            _generalTypes.Add("Sash");
            _generalTypes.Add("Stygian Vise");
        }
    }

    public class FlaskRunner : GearTypeRunnerBase
    {
        public FlaskRunner()
            : base(GearType.Flask, Settings.GearBaseTypes[GearType.Flask])
        {
            _generalTypes.Add("Flask");
        }
    }

    public class MapRunner : GearTypeRunnerBase
    {
        public MapRunner()
            : base(GearType.Map, Settings.GearBaseTypes[GearType.Map])
        {
            _generalTypes.Add("Map");
        }
    }

    public class DivinationCardRunner : GearTypeRunnerBase
    {
        public DivinationCardRunner()
            : base(GearType.DivinationCard, Settings.GearBaseTypes[GearType.DivinationCard])
        {
        }
    }

    public class JewelRunner : GearTypeRunnerBase
    {
        public JewelRunner()
            : base(GearType.Jewel, Settings.GearBaseTypes[GearType.Jewel])
        {
            _generalTypes.Add("Jewel");
            _incompatibleTypes = new List<string>() { "Jewelled Foil", "Eye Jewel" };
        }
    }

    internal class AbyssJewelRunner : GearTypeRunnerBase
    {
        public AbyssJewelRunner()
            : base(GearType.AbyssJewel, Settings.GearBaseTypes[GearType.AbyssJewel])
        {
            _generalTypes.Add("Eye Jewel");
            _incompatibleTypes = new List<string>() { "Jewelled Foil" };
        }
    }

    public class GloveRunner : GearTypeRunnerBase
    {
        public GloveRunner()
            : base(GearType.Gloves, Settings.GearBaseTypes[GearType.Gloves])
        {
            _generalTypes.Add("Glove");
            _generalTypes.Add(" Mitts");
            _generalTypes.Add("Gauntlets");
        }
    }

    public class BootRunner : GearTypeRunnerBase
    {
        public BootRunner()
            : base(GearType.Boots, Settings.GearBaseTypes[GearType.Boots])
        {
            _generalTypes.Add("Greaves");
            _generalTypes.Add("Slippers");
            _generalTypes.Add("Boots");
            _generalTypes.Add("Shoes");
        }
    }

    public class AxeRunner : GearTypeRunnerBase
    {
        public AxeRunner()
            : base(GearType.Axe, Settings.GearBaseTypes[GearType.Axe])
        {
            _generalTypes.AddRange(new List<string>() { "Axe", "Chopper", "Splitter", "Labrys", "Tomahawk", "Hatchet", "Poleaxe", "Woodsplitter", "Cleaver" });
        }
    }

    public class ClawRunner : GearTypeRunnerBase
    {
        public ClawRunner()
            : base(GearType.Claw, Settings.GearBaseTypes[GearType.Claw])
        {
            _generalTypes.AddRange(new List<string>() { "Fist", "Awl", "Paw", "Blinder", "Ripper", "Stabber", "Claw", "Gouger" });
        }
    }

    public class BowRunner : GearTypeRunnerBase
    {
        public BowRunner()
            : base(GearType.Bow, Settings.GearBaseTypes[GearType.Bow])
        {
            _generalTypes.Add("Bow");
        }
    }

    public class DaggerRunner : GearTypeRunnerBase
    {
        public DaggerRunner()
            : base(GearType.Dagger, Settings.GearBaseTypes[GearType.Dagger])
        {
            _generalTypes.AddRange(new List<string>() { "Dagger", "Shank", "Knife", "Stiletto", "Skean", "Poignard", "Ambusher", "Boot Blade", "Kris", "Trisula" });
            _incompatibleTypes = new List<string>() { "Saint" };
        }
    }

    public class MaceRunner : GearTypeRunnerBase
    {
        public MaceRunner()
            : base(GearType.Mace, Settings.GearBaseTypes[GearType.Mace])
        {
            _generalTypes.AddRange(new List<string>() { "Club", "Tenderizer", "Mace", "Hammer", "Maul", "Mallet", "Breaker", "Gavel", "Pernarch", "Steelhead", "Piledriver", "Bladed Mace", "Morning Star" });
        }
    }

    public class QuiverRunner : GearTypeRunnerBase
    {
        public QuiverRunner()
            : base(GearType.Quiver, Settings.GearBaseTypes[GearType.Quiver])
        {
            _generalTypes.Add("Quiver");
        }
    }

    public class SceptreRunner : GearTypeRunnerBase
    {
        public SceptreRunner()
            : base(GearType.Sceptre, Settings.GearBaseTypes[GearType.Sceptre])
        {
            _generalTypes.Add("Sceptre");
            _generalTypes.Add("Fetish");
            _generalTypes.Add("Sekhem");
        }
    }

    public class StaffRunner : GearTypeRunnerBase
    {
        public StaffRunner()
            : base(GearType.Staff, Settings.GearBaseTypes[GearType.Staff])
        {
            _generalTypes.Add("Staff");
            _generalTypes.Add("Gnarled Branch");
            _generalTypes.Add("Quarterstaff");
            _generalTypes.Add("Lathi");
        }
    }

    public class SwordRunner : GearTypeRunnerBase
    {
        public SwordRunner()
            : base(GearType.Sword, Settings.GearBaseTypes[GearType.Sword])
        {
            _generalTypes.AddRange(new List<string>() { "Sword", "sword", "Sabre", "Dusk Blade", "Cutlass", "Baselard", "Gladius", "Variscite Blade", "Vaal Blade", "Midnight Blade", "Corroded Blade",
                   "Highland Blade", "Ezomyte Blade", "Rusted Spike", "Rapier", "Foil", "Pecoraro", "Estoc", "Twilight Blade", "Lithe Blade" });
            _incompatibleTypes.Add("The Sword King's Salute");
        }
    }

    public class ShieldRunner : GearTypeRunnerBase
    {
        public ShieldRunner()
            : base(GearType.Shield, Settings.GearBaseTypes[GearType.Shield])
        {
            _generalTypes.Add("Shield");
            _generalTypes.Add("Spiked Bundle");
            _generalTypes.Add("Buckler");
        }
    }

    public class WandRunner : GearTypeRunnerBase
    {
        public WandRunner()
            : base(GearType.Wand, Settings.GearBaseTypes[GearType.Wand])
        {
            _generalTypes.Add("Wand");
            _generalTypes.Add("Horn");
        }
    }

    public class FishingRodRunner : GearTypeRunnerBase
    {
        public FishingRodRunner()
            : base(GearType.FishingRod, Settings.GearBaseTypes[GearType.FishingRod])
        {
        }
    }
}