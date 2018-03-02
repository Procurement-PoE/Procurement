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
            this.Type = gearType;
        }
    }

    public class GearTypeRunnerBase : GearTypeRunner
    {
        protected List<string> generalTypes;
        protected List<string> compatibleTypes;
        protected List<string> incompatibleTypes;

        public GearTypeRunnerBase(GearType gearType, IEnumerable<string> compatibleTypes)
            : base(gearType)
        {
            this.generalTypes = new List<string>();
            this.compatibleTypes = compatibleTypes.ToList();
            this.incompatibleTypes = new List<string>();
        }

        public override bool IsCompatibleType(Gear item)
        {
            if (incompatibleTypes != null && incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return false;
            
            // First, check the general types, to see if there is an easy match.
            foreach (var type in generalTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            // Second, check all known types.
            foreach (var type in compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            return false;
        }

        public override string GetBaseType(Gear item)
        {
            if (incompatibleTypes != null && incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return null;

            foreach (var type in compatibleTypes)
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
            incompatibleTypes = new List<string>() { "Ringmail" };
        }

        public override bool IsCompatibleType(Gear item)
        {
            if (item.TypeLine.Contains("Ring") && !incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return true;

            return false;
        }
    }

    public class AmuletRunner : GearTypeRunnerBase
    {
        public AmuletRunner()
            : base(GearType.Amulet, Settings.GearBaseTypes[GearType.Amulet])
        {
            generalTypes.Add("Amulet");
        }
    }

    public class TalismanRunner : GearTypeRunnerBase
    {
        public TalismanRunner()
            : base(GearType.Talisman, new List<string>() { "Talisman" })
        {
            generalTypes.Add("Talisman");
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
            generalTypes.Add("Breachstone");
        }
    }

    public class LeaguestoneRunner : GearTypeRunnerBase
    {
        public LeaguestoneRunner() : base(GearType.Leaguestone, new List<string>() { "Leaguestone" })
        {
            generalTypes.Add("Leaguestone");
        }
    }

    public class HelmetRunner : GearTypeRunnerBase
    {
        public HelmetRunner()
            : base(GearType.Helmet, Settings.GearBaseTypes[GearType.Helmet])
        {
            generalTypes.AddRange(new List<string>() { "Helmet", "Circlet", "Cap", "Mask", "Chain Coif", "Casque", "Hood", "Ringmail Coif", "Chainmail Coif", "Ring Coif", "Crown", "Burgonet", "Bascinet", "Pelt" });
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
            generalTypes.Add("Belt");
            generalTypes.Add("Sash");
            generalTypes.Add("Stygian Vise");
        }
    }

    public class FlaskRunner : GearTypeRunnerBase
    {
        public FlaskRunner()
            : base(GearType.Flask, Settings.GearBaseTypes[GearType.Flask])
        {
            generalTypes.Add("Flask");
        }
    }

    public class MapRunner : GearTypeRunnerBase
    {
        public MapRunner()
            : base(GearType.Map, Settings.GearBaseTypes[GearType.Map])
        {
            generalTypes.Add("Map");
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
            generalTypes.Add("Jewel");
            incompatibleTypes = new List<string>() { "Jewelled Foil", "Eye Jewel" };
        }
    }

    internal class AbyssJewelRunner : GearTypeRunnerBase
    {
        public AbyssJewelRunner()
            : base(GearType.AbyssJewel, Settings.GearBaseTypes[GearType.AbyssJewel])
        {
            generalTypes.Add("Eye Jewel");
            incompatibleTypes = new List<string>() { "Jewelled Foil" };
        }
    }

    public class GloveRunner : GearTypeRunnerBase
    {
        public GloveRunner()
            : base(GearType.Gloves, Settings.GearBaseTypes[GearType.Gloves])
        {
            generalTypes.Add("Glove");
            generalTypes.Add("Mitts");
            generalTypes.Add("Gauntlets");
        }
    }

    public class BootRunner : GearTypeRunnerBase
    {
        public BootRunner()
            : base(GearType.Boots, Settings.GearBaseTypes[GearType.Boots])
        {
            generalTypes.Add("Greaves");
            generalTypes.Add("Slippers");
            generalTypes.Add("Boots");
            generalTypes.Add("Shoes");
        }
    }

    public class AxeRunner : GearTypeRunnerBase
    {
        public AxeRunner()
            : base(GearType.Axe, Settings.GearBaseTypes[GearType.Axe])
        {
            generalTypes.AddRange(new List<string>() { "Axe", "Chopper", "Splitter", "Labrys", "Tomahawk", "Hatchet", "Poleaxe", "Woodsplitter", "Cleaver" });
        }
    }

    public class ClawRunner : GearTypeRunnerBase
    {
        public ClawRunner()
            : base(GearType.Claw, Settings.GearBaseTypes[GearType.Claw])
        {
            generalTypes.AddRange(new List<string>() { "Fist", "Awl", "Paw", "Blinder", "Ripper", "Stabber", "Claw", "Gouger" });
        }
    }

    public class BowRunner : GearTypeRunnerBase
    {
        public BowRunner()
            : base(GearType.Bow, Settings.GearBaseTypes[GearType.Bow])
        {
            generalTypes.Add("Bow");
        }
    }

    public class DaggerRunner : GearTypeRunnerBase
    {
        public DaggerRunner()
            : base(GearType.Dagger, Settings.GearBaseTypes[GearType.Dagger])
        {
            generalTypes.AddRange(new List<string>() { "Dagger", "Shank", "Knife", "Stiletto", "Skean", "Poignard", "Ambusher", "Boot Blade", "Kris", "Trisula" });
            incompatibleTypes = new List<string>() { "Saint" };
        }
    }

    public class MaceRunner : GearTypeRunnerBase
    {
        public MaceRunner()
            : base(GearType.Mace, Settings.GearBaseTypes[GearType.Mace])
        {
            generalTypes.AddRange(new List<string>() { "Club", "Tenderizer", "Mace", "Hammer", "Maul", "Mallet", "Breaker", "Gavel", "Pernarch", "Steelhead", "Piledriver", "Bladed Mace", "Morning Star" });
        }
    }

    public class QuiverRunner : GearTypeRunnerBase
    {
        public QuiverRunner()
            : base(GearType.Quiver, Settings.GearBaseTypes[GearType.Quiver])
        {
            generalTypes.Add("Quiver");
        }
    }

    public class SceptreRunner : GearTypeRunnerBase
    {
        public SceptreRunner()
            : base(GearType.Sceptre, Settings.GearBaseTypes[GearType.Sceptre])
        {
            generalTypes.Add("Sceptre");
            generalTypes.Add("Fetish");
            generalTypes.Add("Sekhem");
        }
    }

    public class StaffRunner : GearTypeRunnerBase
    {
        public StaffRunner()
            : base(GearType.Staff, Settings.GearBaseTypes[GearType.Staff])
        {
            generalTypes.Add("Staff");
            generalTypes.Add("Gnarled Branch");
            generalTypes.Add("Quarterstaff");
            generalTypes.Add("Lathi");
        }
    }

    public class SwordRunner : GearTypeRunnerBase
    {
        public SwordRunner()
            : base(GearType.Sword, Settings.GearBaseTypes[GearType.Sword])
        {
            generalTypes.AddRange(new List<string>() { "Sword", "sword", "Sabre", "Dusk Blade", "Cutlass", "Baselard", "Gladius", "Variscite Blade", "Vaal Blade", "Midnight Blade", "Corroded Blade",
                   "Highland Blade", "Ezomyte Blade", "Rusted Spike", "Rapier", "Foil", "Pecoraro", "Estoc", "Twilight Blade", "Lithe Blade" });
        }
    }

    public class ShieldRunner : GearTypeRunnerBase
    {
        public ShieldRunner()
            : base(GearType.Shield, Settings.GearBaseTypes[GearType.Shield])
        {
            generalTypes.Add("Shield");
            generalTypes.Add("Spiked Bundle");
            generalTypes.Add("Buckler");
        }
    }

    public class WandRunner : GearTypeRunnerBase
    {
        public WandRunner()
            : base(GearType.Wand, Settings.GearBaseTypes[GearType.Wand])
        {
            generalTypes.Add("Wand");
            generalTypes.Add("Horn");
        }
    }
}