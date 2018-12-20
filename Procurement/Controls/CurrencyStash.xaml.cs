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

        public CurrencyStash(int tabNumber)
        {
            TabNumber = tabNumber;

            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(tabNumber);

            viewModel = new CurrencyStashViewModel(stash);

            DataContext = viewModel;

            InitializeComponent();
            
            SetPremiumTabBorderColour();
        }

        public CurrencyStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filter = list;
        }

        public override Border Border => this.LocalBorder;

        public override void ForceUpdate()
        {
            //throw new System.NotImplementedException();
        }
    }
}