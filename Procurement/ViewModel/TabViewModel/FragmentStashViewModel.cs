using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public class FragmentStashViewModel : CommonTabViewModel
    {
        public FragmentStashViewModel(Dictionary<Item, ItemDisplayViewModel> stashByLocation) : base(stashByLocation)
        {

        }

        public ItemDisplayViewModel Dawn => GetItemCalled("Sacrifice at Dawn");
        public ItemDisplayViewModel Dusk => GetItemCalled("Sacrifice at Dusk");
        public ItemDisplayViewModel Noon => GetItemCalled("Sacrifice at Noon");
        public ItemDisplayViewModel Midnight => GetItemCalled("Sacrifice at Midnight");

        public ItemDisplayViewModel Grief => GetItemCalled("Mortal Grief");
        public ItemDisplayViewModel Rage => GetItemCalled("Mortal Rage");
        public ItemDisplayViewModel Ignorance => GetItemCalled("Mortal Ignorance");
        public ItemDisplayViewModel Hope => GetItemCalled("Mortal Hope");

        public ItemDisplayViewModel Volkuur => GetItemCalled("Volkuur's Key");
        public ItemDisplayViewModel Eber => GetItemCalled("Eber's Key");
        public ItemDisplayViewModel Yriel => GetItemCalled("Yriel's Key");
        public ItemDisplayViewModel Inya => GetItemCalled("Inya's Key");

        public ItemDisplayViewModel Hydra => GetItemCalled("Fragment of the Hydra");
        public ItemDisplayViewModel Phoenix => GetItemCalled("Fragment of the Phoenix");
        public ItemDisplayViewModel Minotaur => GetItemCalled("Fragment of the Minotaur");
        public ItemDisplayViewModel Chimera => GetItemCalled("Fragment of the Chimera");

        public ItemDisplayViewModel DivineVessel => GetItemCalled("Divine Vessel");

        public ItemDisplayViewModel ChayulaSplinter => GetBreach<BreachSplinter>(BreachType.Chayula);
        public ItemDisplayViewModel EshSplinter => GetBreach<BreachSplinter>(BreachType.Esh);
        public ItemDisplayViewModel TulSplinter => GetBreach<BreachSplinter>(BreachType.Tul);
        public ItemDisplayViewModel UulNetolSplinter => GetBreach<BreachSplinter>(BreachType.UulNetol);
        public ItemDisplayViewModel XophSplinter => GetBreach<BreachSplinter>(BreachType.Xoph);

        public ItemDisplayViewModel ChayulaBreachstone => GetBreach<Breachstone>(BreachType.Chayula);
        public ItemDisplayViewModel EshBreachstone => GetBreach<Breachstone>(BreachType.Esh);
        public ItemDisplayViewModel TulBreachstone => GetBreach<Breachstone>(BreachType.Tul);
        public ItemDisplayViewModel UulNetolBreachstone => GetBreach<Breachstone>(BreachType.UulNetol);
        public ItemDisplayViewModel XophBreachstone => GetBreach<Breachstone>(BreachType.Xoph);

        public ItemDisplayViewModel GildedAmbushScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Ambush);
        public ItemDisplayViewModel GildedBestiaryScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Bestiary);
        public ItemDisplayViewModel GildedBreachScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Breach);
        public ItemDisplayViewModel GildedCartographyScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Cartography);
        public ItemDisplayViewModel GildedDivinationScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Divination);
        public ItemDisplayViewModel GildedElderScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Elder);
        public ItemDisplayViewModel GildedHarbingerScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Harbinger);
        public ItemDisplayViewModel GildedPerandusScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Perandus);
        public ItemDisplayViewModel GildedReliquaryScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Reliquary);
        public ItemDisplayViewModel GildedShaperScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Shaper);
        public ItemDisplayViewModel GildedSulphiteScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Sulphite);
        public ItemDisplayViewModel GildedTormentScarab => GetScarab(ScarabRank.Gilded, ScarabEffect.Torment);

        public ItemDisplayViewModel PolishedAmbushScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Ambush);
        public ItemDisplayViewModel PolishedBestiaryScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Bestiary);
        public ItemDisplayViewModel PolishedBreachScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Breach);
        public ItemDisplayViewModel PolishedCartographyScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Cartography);
        public ItemDisplayViewModel PolishedDivinationScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Divination);
        public ItemDisplayViewModel PolishedElderScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Elder);
        public ItemDisplayViewModel PolishedHarbingerScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Harbinger);
        public ItemDisplayViewModel PolishedPerandusScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Perandus);
        public ItemDisplayViewModel PolishedReliquaryScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Reliquary);
        public ItemDisplayViewModel PolishedShaperScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Shaper);
        public ItemDisplayViewModel PolishedSulphiteScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Sulphite);
        public ItemDisplayViewModel PolishedTormentScarab => GetScarab(ScarabRank.Polished, ScarabEffect.Torment);

        public ItemDisplayViewModel RustedAmbushScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Ambush);
        public ItemDisplayViewModel RustedBestiaryScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Bestiary);
        public ItemDisplayViewModel RustedBreachScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Breach);
        public ItemDisplayViewModel RustedCartographyScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Cartography);
        public ItemDisplayViewModel RustedDivinationScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Divination);
        public ItemDisplayViewModel RustedElderScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Elder);
        public ItemDisplayViewModel RustedHarbingerScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Harbinger);
        public ItemDisplayViewModel RustedPerandusScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Perandus);
        public ItemDisplayViewModel RustedReliquaryScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Reliquary);
        public ItemDisplayViewModel RustedShaperScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Shaper);
        public ItemDisplayViewModel RustedSulphiteScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Sulphite);
        public ItemDisplayViewModel RustedTormentScarab => GetScarab(ScarabRank.Rusted, ScarabEffect.Torment);

        //Todo: Get this inline with other common search methods
        public ItemDisplayViewModel Offering => GetOffering();
    }
}