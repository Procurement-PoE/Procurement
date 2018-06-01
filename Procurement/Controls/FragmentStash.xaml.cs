using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using POEApi.Model;
using Procurement.Interfaces;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for FragmentStash.xaml
    /// </summary>
    public partial class FragmentStash : UserControl, IStashControl
    {
        private readonly FragmentStashViewModel viewModel;
        private List<IFilter> list;

        public FragmentStash(int tabNumber)
        {
            TabNumber = tabNumber;

            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(tabNumber);

            viewModel = new FragmentStashViewModel(stash);

            DataContext = viewModel;

            InitializeComponent();
        }

        public FragmentStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            this.list = list;
        }

        public int TabNumber { get; set; }
        public int FilterResults { get; set; }
        public List<IFilter> Filter { get; set; }

        public void RefreshTab(string accountName)
        {
        }

        public void ForceUpdate()
        {
        }
    }

    internal class FragmentStashViewModel
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

        public ItemDisplayViewModel Divine => GetItemCalled("Divine Vessel");

        public ItemDisplayViewModel Chayula => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Chayula));
        public ItemDisplayViewModel Esh => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Esh));
        public ItemDisplayViewModel Tul => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Tul));
        public ItemDisplayViewModel UulNetol => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.UulNetol));
        public ItemDisplayViewModel Xoph => new ItemDisplayViewModel(_stash.OfType<Splinter>().FirstOrDefault(x => x.Type == BreachType.Xoph));

        public ItemDisplayViewModel ChayulaBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Chayula));
        public ItemDisplayViewModel EshBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Esh));
        public ItemDisplayViewModel TulBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Tul));
        public ItemDisplayViewModel UulNetolBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.UulNetol));
        public ItemDisplayViewModel XophBlessing => new ItemDisplayViewModel(_stash.OfType<BreachBlessing>().FirstOrDefault(x => x.Type == BreachType.Xoph));


        private ItemDisplayViewModel GetItemCalled(string name)
        {
            var item = _stash.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            return new ItemDisplayViewModel(item);
        }
    }
}