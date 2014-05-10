using System.Windows.Controls;
using Procurement.ViewModel;
using POEApi.Model;

namespace Procurement.View
{
    public partial class SettingsView : UserControl, IView
    {
        public SettingsView()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel(this);
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }

        private void CurrencyGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Settings.Save();
        }

        private void AboutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ScreenController.Instance.LoadView(new AboutView());
        }
    }
}
