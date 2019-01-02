using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for FragmentStash.xaml
    /// </summary>
    public partial class FragmentStashTab : AbstractStashTabControl
    {
        private readonly FragmentStashViewModel viewModel;

        public FragmentStashTab(int tabNumber) : base(tabNumber)
        {
            Refresh();

            viewModel = new FragmentStashViewModel(StashByLocation);

            DataContext = viewModel;

            InitializeComponent();

            SetPremiumTabBorderColour();

            Ready = true;
        }

        public FragmentStashTab(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filter = list;
        }

        public override Border Border => LocalBorder;
    }
}