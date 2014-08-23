using Procurement.ViewModel;
using System.Windows.Controls;

namespace Procurement.Controls
{
    public partial class BuyoutSettings : UserControl
    {
        public BuyoutSettings()
        {
            InitializeComponent();
            this.DataContext = new TradeSettingsViewModel();
        }
    }
}
