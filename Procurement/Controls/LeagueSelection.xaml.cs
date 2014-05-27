using Procurement.ViewModel;
using System.Linq;
using System.Windows.Controls;

namespace Procurement.Controls
{
    public partial class LeagueSelection : UserControl
    {
        public LeagueSelection()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;

            if (ApplicationState.CurrentLeague != null && !cb.IsLoaded)
                return;

            string league = cb.SelectedItem.ToString();
            ApplicationState.CurrentLeague = league;
            ScreenController.Instance.UpdateTrading();
        }
    }
}
