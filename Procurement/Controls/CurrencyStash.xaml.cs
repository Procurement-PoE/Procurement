using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Procurement.Interfaces;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for CurrencyStash.xaml
    /// </summary>
    public partial class CurrencyStash : UserControl, IStashControl
    {
        private List<IFilter> list;
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
            this.list = list;
        }

        public void RefreshTab(string accountName)
        {
            //throw new NotImplementedException();
        }

        public int TabNumber { get; set; }
        public int FilterResults { get; set; }
        public List<IFilter> Filter { get; set; }

        public void SetPremiumTabBorderColour()
        {
            var color = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[TabNumber].Colour.WpfColor;
            
            Border.BorderBrush = new SolidColorBrush(color);
        }

        public void ForceUpdate()
        {
            //throw new NotImplementedException();
        }
    }
}