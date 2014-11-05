using Procurement.ViewModel;
using System.Windows.Controls;
using System.Diagnostics;

namespace Procurement.Controls
{
    public partial class BuyoutSettings : UserControl
    {
        public BuyoutSettings()
        {
            InitializeComponent();
            this.DataContext = new TradeSettingsViewModel();
        }

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("http://www.pathofexile.com/private-messages/compose/to/poexyzis");
        }
    }
}
