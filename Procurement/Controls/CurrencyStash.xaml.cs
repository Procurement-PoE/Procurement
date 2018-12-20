using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for CurrencyStash.xaml
    /// </summary>
    public partial class CurrencyStash : AbstractStashControl
    {
        public CurrencyStashViewModel viewModel;

        public CurrencyStash(int tabNumber) : base(tabNumber)
        {
            Refresh();

            viewModel = new CurrencyStashViewModel(StashByLocation);

            DataContext = viewModel;

            InitializeComponent();

            Ready = true;

            SetPremiumTabBorderColour();
        }

        public CurrencyStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filter = list;
        }

        public override Border Border => this.LocalBorder;
    }
}