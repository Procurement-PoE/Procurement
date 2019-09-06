using System.Windows.Controls;
using System.Windows.Input;
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

        private void AdvancedSearchFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var vm = this.DataContext as StashViewModel;

            if (vm == null)
                return;

            var cb = sender as TextBox;

            foreach (AdvancedSearchCategory category in AdvancedSearchItemControl.ItemsSource)
            {
                if (category.Key.ToLowerInvariant().Contains(cb.Text.ToLowerInvariant()))
                {
                    category.IsVisible = true;
                }
                else
                {
                    category.IsVisible = false;
                }
            }

            AdvancedSearchItemControl.Items.Refresh();
        }

        private void HeaderPanel_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scv = (ScrollViewer)sender;
            if (scv == null)
            {
                return;
            }

            if (e.Delta > 0)
            {
                scv.PageLeft();
            }
            else
            {
                scv.PageRight();
            }

            e.Handled = true;
        }
    }
}
