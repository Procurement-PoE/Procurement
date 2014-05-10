using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.View
{
    public partial class TradingView : UserControl, IView
    {
        public TradingView()
        {
            InitializeComponent();
            this.DataContext = new TradingViewModel();
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }
    }
}
