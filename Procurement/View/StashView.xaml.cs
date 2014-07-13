using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.View
{
    public partial class StashView : UserControl, IView
    {
        public StashView()
        {
            InitializeComponent();
            this.DataContext = new StashViewModel(this);
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = this.DataContext as StashViewModel;

            if (vm == null)
                return;

            var cb = sender as CheckBox;
            if (cb.Content.ToString() == "None" && cb.IsChecked.Value)
            {
                foreach (var item in VisualTreeHelper.GetVisualChildren<CheckBox>(cb.Parent))
                    item.IsChecked = false;

                return;
            }

            vm.SetCategoryFilter(cb.Content.ToString(), cb.IsChecked);
        }

        private void test(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
