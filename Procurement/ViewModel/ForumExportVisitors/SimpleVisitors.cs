using Procurement.ViewModel.Filters;
using Procurement.ViewModel.Filters.ForumExport;

namespace Procurement.ViewModel.ForumExportVisitors
{
    internal class ProphecyVisitor : SimpleVisitor
    {
        public ProphecyVisitor() : base("{Prophecies}", new ProphecyFilter())
        {
        }
    }

    internal class AbyssJewelVisitor : SimpleVisitor
    {
        public AbyssJewelVisitor() : base("{AbyssJewels}", new AbyssJewelFilter())
        {
        }
    }

    internal class FossilVisitor : SimpleVisitor
    {
        public FossilVisitor() : base("{Fossil}", new FossilFilter())
        {
        }
    }

    internal class ResonatorVisitor : SimpleVisitor
    {
        public ResonatorVisitor() : base("{Resonator}", new ResonatorFilter())
        {
        }
    }

    internal class ShaperItemVisitor : SimpleVisitor
    {
        public ShaperItemVisitor() : base("{ShaperItems}", new ShaperItemFilter())
        {
        }
    }

    internal class ElderItemVisitor : SimpleVisitor
    {
        public ElderItemVisitor() : base("{ElderItems}", new ElderItemFilter())
        {
        }
    }

    internal class CurrencyVisitor : SimpleVisitor
    {
        public CurrencyVisitor() : base("{Currency}", new CurrencyFilter())
        {
        }
    }

    internal class CorruptedGemVisitor : SimpleVisitor
    {
        public CorruptedGemVisitor() : base("{CorruptedGems}", new CorruptedGemFilter())
        {
        }
    }

    internal class AllGemVisitor : SimpleVisitor
    {
        public AllGemVisitor() : base("{AllGems", new AllGemsFilter())
        {
        }
    }

    internal class DivineVesselVisitor : SimpleVisitor
    {
        public DivineVesselVisitor() : base("{DivineVessel}", new DivineVesselFilter())
        {
        }
    }

    internal class DropOnlyGemVisitor : SimpleVisitor
    {
        public DropOnlyGemVisitor() : base("{DropOnlyGems}", new DropOnlyGemFilter())
        {
        }
    }

    internal class DualResVisitor : SimpleVisitor
    {
        public DualResVisitor() : base("{DualRes}", new DualResistances())
        {
        }
    }

    internal class EssenceVisitor : SimpleVisitor
    {
        public EssenceVisitor() : base("{Essence}", new EssenceFilter())
        {
        }
    }

    internal class LeagueStoneVisitor : SimpleVisitor
    {
        public LeagueStoneVisitor() : base("{Leaguestone}", new LeagestoneFilter())
        {
        }
    }

    internal class FatedUniqueBaseTypeVisitor : SimpleVisitor
    {
        public FatedUniqueBaseTypeVisitor() : base("{FatedUniqueBaseType}", new FatedUniqueBaseTypesFilter())
        {
        }
    }

    internal class FatedUniqueProphecyVisitor : SimpleVisitor
    {
        public FatedUniqueProphecyVisitor() : base("{FatedUniqueProphecy}", new FatedUniquePropheciesFilter())
        {
        }
    }

    internal class FatedUniqueTargetVisitor : SimpleVisitor
    {
        public FatedUniqueTargetVisitor() : base("{FatedUniqueTarget}", new FatedUniqueTargetsFilter())
        {
        }
    }

    internal class FatedUniqueVisitor : SimpleVisitor
    {
        public FatedUniqueVisitor() : base("{FatedUnique}", new FatedUniquesFilter())
        {
        }
    }

    internal class FracturedItemVisitor : SimpleVisitor
    {
        public FracturedItemVisitor() : base("{FracturedItems}", new FracturedItemFilter())
        {
        }
    }

    // TODO: Add similar visitors that look for orbs that have exactly one or two Bestiary beast mods, once we can
    // identify which mods those are.
    internal class FullBestiaryOrbVisitor : SimpleVisitor
    {
        public FullBestiaryOrbVisitor() : base("{FullBestiaryOrbs}", new FullBestiaryOrbFilter())
        {
        }
    }

    internal class IncursionVialVisitor : SimpleVisitor
    {
        public IncursionVialVisitor() : base("{IncursionVial}", new IncursionVialsFilter())
        {
        }
    }

    internal class MirroredItemVisitor : SimpleVisitor
    {
        public MirroredItemVisitor() : base("{Mirrored}", new MirroredItemFilter())
        {
        }
    }

    internal class OfferingVisitor : SimpleVisitor
    {
        public OfferingVisitor() : base("{Offering}", new OfferingFilter())
        {
        }
    }

    internal class PopularGemsVisitor : SimpleVisitor
    {
        public PopularGemsVisitor() : base("{PopularGems}", new PopularGemsFilter())
        {
        }
    }

    internal class ScarabVisitor : SimpleVisitor
    {
        public ScarabVisitor() : base("{Scarab}", new ScarabFilter())
        {
        }
    }

    internal class SixSocketVisitor : SimpleVisitor
    {
        public SixSocketVisitor() : base("{6Socket}", new SixSocketFilter())
        {
        }
    }

    internal class SynthesisedItemVisitor : SimpleVisitor
    {
        public SynthesisedItemVisitor() : base("{SynthesisedItems}", new SynthesisedItemFilter())
        {
        }
    }

    internal class TripResVisitor : SimpleVisitor
    {
        public TripResVisitor() : base("{TripRes}", new TripleResistance())
        {
        }
    }

    internal class UniquesVisitor : SimpleVisitor
    {
        public UniquesVisitor() : base("{Uniques}", new UniqueRarity())
        {
        }
    }

    internal class UnknownItemVisitor : SimpleVisitor
    {
        public UnknownItemVisitor() : base("{UnknownItems}", new UnknownItemFilter())
        {
        }
    }

    internal class IncubatorVisitor : SimpleVisitor
    {
        public IncubatorVisitor() : base("{Incubator}", new IncubatorFilter())
        {
            
        }
    }
}