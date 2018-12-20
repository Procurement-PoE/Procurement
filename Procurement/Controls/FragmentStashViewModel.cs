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

        public ItemDisplayViewModel Offering => new ItemDisplayViewModel(_items.OfType<Offering>().FirstOrDefault());

        //Todo: Update these so that they become part of the filtering.

        private T GetBreach<T>(BreachType breachType) where T : IBreachCurrency
        {
            return _items.OfType<T>().FirstOrDefault(x => x.Type == breachType);
        }

        private ItemDisplayViewModel GetItemCalled(string name)
        {
            var item = _items.FirstOrDefault(x => string.Equals( x.TypeLine, name, StringComparison.CurrentCultureIgnoreCase));

            return new ItemDisplayViewModel(item);
        }
    }
}