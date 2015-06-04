using Procurement.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Procurement.Controls
{
    public partial class SetTabBuyoutView : Window
    {
        public event TabwideBuyoutHandler Update;
        public delegate void TabwideBuyoutHandler(PricingInfo buyoutInfo, string tabName);

        private string tabName;

        public SetTabBuyoutView(PricingInfo buyoutInfo, string tabName)
        {
            InitializeComponent();

            var vm = new SetTabBuyoutViewModel();

            if (buyoutInfo != null)
                vm.BuyoutInfo = buyoutInfo;

            this.DataContext = vm;
            this.tabName = tabName;

            if (ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                this.chkTabBuyout.Content = "Цена вкладки";
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        public void Save_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = (this.DataContext as SetTabBuyoutViewModel);
            Update(vm.BuyoutInfo, tabName);
            this.Close();
        }

        public void Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
