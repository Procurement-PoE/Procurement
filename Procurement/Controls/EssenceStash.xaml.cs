using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.Interfaces;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for EssenceStash.xaml
    /// </summary>
    public partial class EssenceStash : UserControl, IStashControl
    {
        private List<IFilter> list;
        public EssenceStashViewModel viewModel;

        public EssenceStash(int tabNumber)
        {
            TabNumber = tabNumber;

            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(tabNumber);

            viewModel = new EssenceStashViewModel(stash);

            DataContext = viewModel;

            InitializeComponent();
        }

        public EssenceStash(int tabNumber, List<IFilter> list) : this(tabNumber)
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

        public void ForceUpdate()
        {
            //throw new NotImplementedException();
        }
    }
}