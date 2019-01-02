using System.Collections.Generic;
using System.Windows.Controls;
using Procurement.ViewModel;
using Procurement.ViewModel.Filters;

namespace Procurement.Controls
{
    /// <summary>
    ///     Interaction logic for EssenceStash.xaml
    /// </summary>
    public partial class EssenceStashTab : AbstractStashTabControl
    {
        public EssenceStashViewModel viewModel;

        public EssenceStashTab(int tabNumber) : base(tabNumber)
        {
            Refresh();

            viewModel = new EssenceStashViewModel(StashByLocation);

            DataContext = viewModel;

            InitializeComponent();

            Ready = true;

            SetPremiumTabBorderColour();
        }

        public EssenceStashTab(int tabNumber, List<IFilter> list) : this(tabNumber)
        {
            Filters = list;
        }

        public override Border Border => this.LocalBorder;
    }
}