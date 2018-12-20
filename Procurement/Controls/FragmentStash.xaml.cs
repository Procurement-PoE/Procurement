using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for FragmentStash.xaml
    /// </summary>
    public partial class FragmentStash : AbstractStashControl
    {
        private readonly FragmentStashViewModel viewModel;

        public FragmentStash(int tabNumber) : base(tabNumber)
        {
            var items = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(TabNumber);

            viewModel = new FragmentStashViewModel(items);

            DataContext = viewModel;

            SetPremiumTabBorderColour();

            InitializeComponent();
        }

        public FragmentStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filter = list;
        }

        public override Border Border => LocalBorder;

        public override void ForceUpdate()
        {
        }
    }
}