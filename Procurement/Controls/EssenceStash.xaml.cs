using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for EssenceStash.xaml
    /// </summary>
    public partial class EssenceStash : AbstractStashControl
    {
        public EssenceStashViewModel viewModel;

        public EssenceStash(int tabNumber) : base(tabNumber)
        {
            var stash = ApplicationState.Stash[ApplicationState.CurrentLeague].GetItemsByTab(TabNumber);

            viewModel = new EssenceStashViewModel(stash);

            DataContext = viewModel;

            InitializeComponent();

            SetPremiumTabBorderColour();
        }

        public EssenceStash(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filter = list;
        }

        public override Border Border => this.LocalBorder;

        public override void ForceUpdate()
        {
            //throw new NotImplementedException();
        }

    }
}