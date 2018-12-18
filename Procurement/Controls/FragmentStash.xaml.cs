using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Procurement.Interfaces;
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

            var items = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(tabNumber);

            viewModel = new FragmentStashViewModel(items);

            DataContext = viewModel;

            SetPremiumTabBorderColour();

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
            //Todo: Implement!
        }

        public void ForceUpdate()
        {
        }

        public void SetPremiumTabBorderColour()
        {
            var color = ApplicationState.Stash[ApplicationState.CurrentLeague].Tabs[TabNumber].Colour.WpfColor;
            
            this.Border.BorderBrush = new SolidColorBrush(color);
        }
    }
}