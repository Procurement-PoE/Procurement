using System.Windows;
using System.Windows.Controls;
using Procurement.ViewModel;

namespace Procurement.Controls
{
    public partial class ForumExport : UserControl
    {
        bool toggleAll = false;
        
        public ForumExport()
        {
            InitializeComponent();
            this.DataContext = new ForumExportViewModel();
        }

        void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            ForumExportViewModel vm = this.DataContext as ForumExportViewModel;
            CheckBox cb = sender as CheckBox;
            vm.update(int.Parse(cb.Tag.ToString()), cb.IsChecked.Value, !toggleAll);
        }

        private void ToggleAll(object sender, RoutedEventArgs e)
        {
            toggleAll = true;
            ForumExportViewModel vm = this.DataContext as ForumExportViewModel;
            CheckBox cb = sender as CheckBox;
            vm.ToggleAll(cb.IsChecked.Value);
            toggleAll = false;
            vm.updateText();
        }
    }
}
