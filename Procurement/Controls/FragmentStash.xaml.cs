using System.Collections.Generic;
using System.Windows.Controls;
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
            //Todo: Implement!
        }

        public void ForceUpdate()
        {
        }
    }
}