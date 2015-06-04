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
            if (ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                lblCharSelection.Content = "Выбор персонажа:";
                tabGear.Header = "Экипировка";
                tabInventory.Header = "Инвертарь";
            }
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }
    }
}
