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
            // First, check the general types, to see if there is an easy match.
            foreach (var type in generalTypes)
                if (item.TypeLine.ToLower().Contains(type.ToLower()))
                    return true;

            // Second, check all known types.
            foreach (var type in compatibleTypes)
                if (item.TypeLine.ToLower().Contains(type.ToLower()))
                    return true;

            return false;
        }

        public override string GetBaseType(Gear item)
        {
            if (incompatibleTypes != null && incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return null;

            foreach (var type in compatibleTypes)
                if (item.TypeLine.ToLower().Contains(type.ToLower()))
                    return type;

            return null;
        }
    }

    public class RingRunner : GearTypeRunnerBase
    {
        public RingRunner()
            : base(GearType.Ring, Settings.GearBaseTypes[GearType.Ring])
        {
            incompatibleTypes = new List<string>() { POEApi.Model.ServerTypeRes.GearTypeRingmail };
        }

        public override bool IsCompatibleType(Gear item)
        {
            if (item.TypeLine.ToLower().Contains(POEApi.Model.ServerTypeRes.GearTypeRing.ToLower()) && !incompatibleTypes.Any(t => item.TypeLine.Contains(t)))
                return true;

            return false;
        }
    }

    public class AmuletRunner : GearTypeRunnerBase
    {
        public AmuletRunner()
            : base(GearType.Amulet, Settings.GearBaseTypes[GearType.Amulet])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeAmulet);
        }
    }

    public class HelmetRunner : GearTypeRunnerBase
    {
        public HelmetRunner()
            : base(GearType.Helmet, Settings.GearBaseTypes[GearType.Helmet])
        {
            generalTypes.AddRange(new List<string>()
            { 
                POEApi.Model.ServerTypeRes.GearTypeHelmet, 
                POEApi.Model.ServerTypeRes.GearTypeCirclet, 
                POEApi.Model.ServerTypeRes.GearTypeCap, 
                POEApi.Model.ServerTypeRes.GearTypeMask, 
                POEApi.Model.ServerTypeRes.GearTypeChainCoif, 
                POEApi.Model.ServerTypeRes.GearTypeCasque, 
                POEApi.Model.ServerTypeRes.GearTypeHood, 
                POEApi.Model.ServerTypeRes.GearTypeRingmailCoif, 
                POEApi.Model.ServerTypeRes.GearTypeChainmailCoif, 
                POEApi.Model.ServerTypeRes.GearTypeRingCoif, 
                POEApi.Model.ServerTypeRes.GearTypeCrown, 
                POEApi.Model.ServerTypeRes.GearTypeBurgonet, 
                POEApi.Model.ServerTypeRes.GearTypeBascinet, 
                POEApi.Model.ServerTypeRes.GearTypePelt
            });
        }
    }

    public class ChestRunner : GearTypeRunnerBase
    {
        public ChestRunner()
            : base(GearType.Chest, Settings.GearBaseTypes[GearType.Chest])
        { }
    }

    public class BeltRunner : GearTypeRunnerBase
    {
        public BeltRunner()
            : base(GearType.Belt, Settings.GearBaseTypes[GearType.Belt])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeBelt);
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeSash);
        }
    }

    public class FlaskRunner : GearTypeRunnerBase
    {
        public FlaskRunner()
            : base(GearType.Flask, Settings.GearBaseTypes[GearType.Flask])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeFlask);
        }
    }

    public class MapRunner : GearTypeRunnerBase
    {
        public MapRunner()
            : base(GearType.Map, Settings.GearBaseTypes[GearType.Map])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.MapText);
        }
    }

    public class GloveRunner : GearTypeRunnerBase
    {
        public GloveRunner()
            : base(GearType.Gloves, Settings.GearBaseTypes[GearType.Gloves])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeGlove);
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeMitts);
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeGauntlets);
        }
    }

    public class BootRunner : GearTypeRunnerBase
    {
        public BootRunner()
            : base(GearType.Boots, Settings.GearBaseTypes[GearType.Boots])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeGreaves);
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeSlippers);
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeBoots);
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeShoes);
        }
    }

    public class AxeRunner : GearTypeRunnerBase
    {
        public AxeRunner()
            : base(GearType.Axe, Settings.GearBaseTypes[GearType.Axe])
        {
            generalTypes.AddRange(new List<string>() {
                POEApi.Model.ServerTypeRes.WeaponTypeAxe,
                POEApi.Model.ServerTypeRes.WeaponTypeChopper, 
                POEApi.Model.ServerTypeRes.WeaponTypeSplitter, 
                POEApi.Model.ServerTypeRes.WeaponTypeLabrys, 
                POEApi.Model.ServerTypeRes.WeaponTypeTomahawk, 
                POEApi.Model.ServerTypeRes.WeaponTypeHatchet, 
                POEApi.Model.ServerTypeRes.WeaponTypePoleaxe, 
                POEApi.Model.ServerTypeRes.WeaponTypeWoodsplitter, 
                POEApi.Model.ServerTypeRes.WeaponTypeCleaver 
            });
        }
    }

    public class ClawRunner : GearTypeRunnerBase
    {
        public ClawRunner()
            : base(GearType.Claw, Settings.GearBaseTypes[GearType.Claw])
        {
            generalTypes.AddRange(new List<string>() {
                POEApi.Model.ServerTypeRes.WeaponTypeFist, 
                POEApi.Model.ServerTypeRes.WeaponTypeAwl, 
                POEApi.Model.ServerTypeRes.WeaponTypePaw, 
                POEApi.Model.ServerTypeRes.WeaponTypeBlinder, 
                POEApi.Model.ServerTypeRes.WeaponTypeRipper, 
                POEApi.Model.ServerTypeRes.WeaponTypeStabber, 
                POEApi.Model.ServerTypeRes.WeaponTypeClaw, 
                POEApi.Model.ServerTypeRes.WeaponTypeGouger 
            });
        }
    }

    public class BowRunner : GearTypeRunnerBase
    {
        public BowRunner()
            : base(GearType.Bow, Settings.GearBaseTypes[GearType.Bow])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeBow);
        }
    }

    public class DaggerRunner : GearTypeRunnerBase
    {
        public DaggerRunner()
            : base(GearType.Dagger, Settings.GearBaseTypes[GearType.Dagger])
        {
            generalTypes.AddRange(new List<string>() {
                POEApi.Model.ServerTypeRes.WeaponTypeDagger, 
                POEApi.Model.ServerTypeRes.WeaponTypeShank, 
                POEApi.Model.ServerTypeRes.WeaponTypeKnife, 
                POEApi.Model.ServerTypeRes.WeaponTypeStiletto, 
                POEApi.Model.ServerTypeRes.WeaponTypeSkean, 
                POEApi.Model.ServerTypeRes.WeaponTypePoignard, 
                POEApi.Model.ServerTypeRes.WeaponTypeAmbusher, 
                POEApi.Model.ServerTypeRes.WeaponTypeBootBlade, 
                POEApi.Model.ServerTypeRes.WeaponTypeKris 
            });
        }
    }

    public class MaceRunner : GearTypeRunnerBase
    {
        public MaceRunner()
            : base(GearType.Mace, Settings.GearBaseTypes[GearType.Mace])
        {
            generalTypes.AddRange(new List<string>() {
                POEApi.Model.ServerTypeRes.WeaponTypeClub, 
                POEApi.Model.ServerTypeRes.WeaponTypeTenderizer, 
                POEApi.Model.ServerTypeRes.WeaponTypeMace, 
                POEApi.Model.ServerTypeRes.WeaponTypeHammer, 
                POEApi.Model.ServerTypeRes.WeaponTypeMaul, 
                POEApi.Model.ServerTypeRes.WeaponTypeMallet, 
                POEApi.Model.ServerTypeRes.WeaponTypeBreaker, 
                POEApi.Model.ServerTypeRes.WeaponTypeGavel, 
                POEApi.Model.ServerTypeRes.WeaponTypePernarch, 
                POEApi.Model.ServerTypeRes.WeaponTypeSteelhead, 
                POEApi.Model.ServerTypeRes.WeaponTypePiledriver, 
                POEApi.Model.ServerTypeRes.WeaponTypeBladedMace 
            });
        }
    }

    public class QuiverRunner : GearTypeRunnerBase
    {
        public QuiverRunner()
            : base(GearType.Quiver, Settings.GearBaseTypes[GearType.Quiver])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.GearTypeQuiver);
        }
    }

    public class SceptreRunner : GearTypeRunnerBase
    {
        public SceptreRunner()
            : base(GearType.Sceptre, Settings.GearBaseTypes[GearType.Sceptre])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeSceptre);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeFetish);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeSekhem);
        }
    }

    public class StaffRunner : GearTypeRunnerBase
    {
        public StaffRunner()
            : base(GearType.Staff, Settings.GearBaseTypes[GearType.Staff])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeStaff);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeGnarledBranch);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeQuarterstaff);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeLathi);
        }
    }

    public class SwordRunner : GearTypeRunnerBase
    {
        public SwordRunner()
            : base(GearType.Sword, Settings.GearBaseTypes[GearType.Sword])
        {
            generalTypes.AddRange(new List<string>() {
                POEApi.Model.ServerTypeRes.WeaponTypeSword, 
                POEApi.Model.ServerTypeRes.WeaponTypeSabre, 
                POEApi.Model.ServerTypeRes.WeaponTypeDuskBlade, 
                POEApi.Model.ServerTypeRes.WeaponTypeCutlass, 
                POEApi.Model.ServerTypeRes.WeaponTypeBaselard, 
                POEApi.Model.ServerTypeRes.WeaponTypeGladius, 
                POEApi.Model.ServerTypeRes.WeaponTypeVarisciteBlade, 
                POEApi.Model.ServerTypeRes.WeaponTypeVaalBlade, 
                POEApi.Model.ServerTypeRes.WeaponTypeMidnightBlade, 
                POEApi.Model.ServerTypeRes.WeaponTypeCorrodedBlade,
                POEApi.Model.ServerTypeRes.WeaponTypeHighlandBlade,
                POEApi.Model.ServerTypeRes.WeaponTypeEzomyteBlade, 
                POEApi.Model.ServerTypeRes.WeaponTypeRustedSpike, 
                POEApi.Model.ServerTypeRes.WeaponTypeRapier, 
                POEApi.Model.ServerTypeRes.WeaponTypeFoil, 
                POEApi.Model.ServerTypeRes.WeaponTypePecoraro, 
                POEApi.Model.ServerTypeRes.WeaponTypeEstoc, 
                POEApi.Model.ServerTypeRes.WeaponTypeTwilightBlade 
            });
        }
    }

    public class ShieldRunner : GearTypeRunnerBase
    {
        public ShieldRunner()
            : base(GearType.Shield, Settings.GearBaseTypes[GearType.Shield])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeShield);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeSpikedBundle);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeBuckler);
        }
    }

    public class WandRunner : GearTypeRunnerBase
    {
        public WandRunner()
            : base(GearType.Wand, Settings.GearBaseTypes[GearType.Wand])
        {
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeWand);
            generalTypes.Add(POEApi.Model.ServerTypeRes.WeaponTypeHorn);
        }
    }
}