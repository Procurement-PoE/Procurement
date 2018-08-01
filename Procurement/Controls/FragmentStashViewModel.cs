using System;
using System.Collections.Generic;
using System.Linq;
using POEApi.Model;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public class FragmentStashViewModel
    {
        private readonly List<Item> _stash;

        public FragmentStashViewModel(List<Item> stash)
        {
            _stash = stash;
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

        public ItemDisplayViewModel ChayulaSplinter => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Chayula));
        public ItemDisplayViewModel EshSplinter => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Esh));
        public ItemDisplayViewModel TulSplinter => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Tul));
        public ItemDisplayViewModel UulNetolSplinter => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.UulNetol));
        public ItemDisplayViewModel XophSplinter => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Xoph));

        public ItemDisplayViewModel ChayulaBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Chayula));
        public ItemDisplayViewModel EshBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Esh));
        public ItemDisplayViewModel TulBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Tul));
        public ItemDisplayViewModel UulNetolBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.UulNetol));
        public ItemDisplayViewModel XophBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Xoph));

        public ItemDisplayViewModel Offering => new ItemDisplayViewModel(_stash.OfType<Offering>().FirstOrDefault());


        private ItemDisplayViewModel GetItemCalled(string name)
        {
            var item = _stash.FirstOrDefault(x => x.TypeLine.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            return new ItemDisplayViewModel(item);
        }
    }
}