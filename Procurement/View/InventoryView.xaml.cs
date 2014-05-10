using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.View
{
    public partial class InventoryView : IView
    {
        public InventoryView()
        {
            InitializeComponent();
            this.DataContext = new InventoryViewModel(this);
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }
    }
}
