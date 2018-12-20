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
            Refresh();

            viewModel = new FragmentStashViewModel(Stash);

            DataContext = viewModel;

            InitializeComponent();

            SetPremiumTabBorderColour();

            Ready = true;
        }

        public FragmentStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filter = list;
        }

        public override Border Border => LocalBorder;
    }
}