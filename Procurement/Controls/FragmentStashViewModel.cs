using System;
using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using POEApi.Model.Interfaces;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public class FragmentStashViewModel
    {
        private readonly List<Item> _items;

        public FragmentStashViewModel(List<Item> items)
        {
            _items = items;
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

        public ItemDisplayViewModel ChayulaSplinter => new ItemDisplayViewModel(GetBreach<Splinter>(BreachType.Chayula));
        public ItemDisplayViewModel EshSplinter => new ItemDisplayViewModel(GetBreach<Splinter>(BreachType.Esh));
        public ItemDisplayViewModel TulSplinter => new ItemDisplayViewModel(GetBreach<Splinter>(BreachType.Tul));
        public ItemDisplayViewModel UulNetolSplinter => new ItemDisplayViewModel(GetBreach<Splinter>(BreachType.UulNetol));
        public ItemDisplayViewModel XophSplinter => new ItemDisplayViewModel(GetBreach<Splinter>(BreachType.Xoph));

        public ItemDisplayViewModel ChayulaBlessing => new ItemDisplayViewModel(GetBreach<BreachBlessing>(BreachType.Chayula));
        public ItemDisplayViewModel EshBlessing => new ItemDisplayViewModel(GetBreach<BreachBlessing>(BreachType.Esh));
        public ItemDisplayViewModel TulBlessing => new ItemDisplayViewModel(GetBreach<BreachBlessing>(BreachType.Tul));
        public ItemDisplayViewModel UulNetolBlessing => new ItemDisplayViewModel(GetBreach<BreachBlessing>(BreachType.UulNetol));

        public ItemDisplayViewModel XophBlessing => new ItemDisplayViewModel(GetBreach<BreachBlessing>(BreachType.Xoph));

        public ItemDisplayViewModel GildedAmbushScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Ambush));
        public ItemDisplayViewModel GildedBestiaryScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Bestiary));
        public ItemDisplayViewModel GildedBreachScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Breach));
        public ItemDisplayViewModel GildedCartographyScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Cartography));
        public ItemDisplayViewModel GildedDivinationScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Divination));
        public ItemDisplayViewModel GildedElderScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Elder));
        public ItemDisplayViewModel GildedHarbingerScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Harbinger));
        public ItemDisplayViewModel GildedPerandusScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Perandus));
        public ItemDisplayViewModel GildedReliquaryScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Reliquary));
        public ItemDisplayViewModel GildedShaperScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Shaper));
        public ItemDisplayViewModel GildedSulphiteScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Sulphite));
        public ItemDisplayViewModel GildedTormentScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Gilded, ScarabEffect.Torment));

        public ItemDisplayViewModel PolishedAmbushScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Ambush));
        public ItemDisplayViewModel PolishedBestiaryScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Bestiary));
        public ItemDisplayViewModel PolishedBreachScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Breach));
        public ItemDisplayViewModel PolishedCartographyScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Cartography));
        public ItemDisplayViewModel PolishedDivinationScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Divination));
        public ItemDisplayViewModel PolishedElderScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Elder));
        public ItemDisplayViewModel PolishedHarbingerScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Harbinger));
        public ItemDisplayViewModel PolishedPerandusScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Perandus));
        public ItemDisplayViewModel PolishedReliquaryScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Reliquary));
        public ItemDisplayViewModel PolishedShaperScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Shaper));
        public ItemDisplayViewModel PolishedSulphiteScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Sulphite));
        public ItemDisplayViewModel PolishedTormentScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Polished, ScarabEffect.Torment));

        public ItemDisplayViewModel RustedAmbushScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Ambush));
        public ItemDisplayViewModel RustedBestiaryScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Bestiary));
        public ItemDisplayViewModel RustedBreachScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Breach));
        public ItemDisplayViewModel RustedCartographyScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Cartography));
        public ItemDisplayViewModel RustedDivinationScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Divination));
        public ItemDisplayViewModel RustedElderScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Elder));
        public ItemDisplayViewModel RustedHarbingerScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Harbinger));
        public ItemDisplayViewModel RustedPerandusScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Perandus));
        public ItemDisplayViewModel RustedReliquaryScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Reliquary));
        public ItemDisplayViewModel RustedShaperScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Shaper));
        public ItemDisplayViewModel RustedSulphiteScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Sulphite));
        public ItemDisplayViewModel RustedTormentScarab => new ItemDisplayViewModel(GetScarab(ScarabRank.Rusted, ScarabEffect.Torment));

        public ItemDisplayViewModel Offering => new ItemDisplayViewModel(_items.OfType<Offering>().FirstOrDefault());

        //Todo: Update these so that they become part of the filtering.

        private T GetBreach<T>(BreachType breachType) where T : IBreachCurrency
        {
            return _items.OfType<T>().FirstOrDefault(x => x.Type == breachType);
        }

        private Scarab GetScarab(ScarabRank rank, ScarabEffect effect)
        {
            return _items.OfType<Scarab>().FirstOrDefault(x => x.ScarabRank == rank && x.ScarabEffect == effect);
        }

        private ItemDisplayViewModel GetItemCalled(string name)
        {
            var item = _items.FirstOrDefault(x => string.Equals(x.TypeLine, name, StringComparison.CurrentCultureIgnoreCase));

            return new ItemDisplayViewModel(item);
        }
    }
}