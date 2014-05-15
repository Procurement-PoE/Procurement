using System.Windows.Controls;
using Procurement.ViewModel;
using POEApi.Model;
using System.Windows;

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

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenController.Instance.LoadView(new AboutView());
        }

        private void LeagueCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string leagueName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).AddDownloadLeague(leagueName);
        }

        private void LeagueCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string characterName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).RemoveDownloadLeague(characterName);
        }

        private void CharacterCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string characterName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).AddDownloadCharacter(characterName);
        }

        private void CharacterCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string characterName = ((sender as CheckBox).Content as TextBlock).Text;
            (DataContext as SettingsViewModel).RemoveDownloadCharacter(characterName);
        }

        private void recipeTab_Checked(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsViewModel).RecipeTabChecked((sender as CheckBox).Tag.ToString());            
        }

        private void recipeTab_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsViewModel).RecipeTabUnchecked((sender as CheckBox).Tag.ToString());
        }
    }
}
