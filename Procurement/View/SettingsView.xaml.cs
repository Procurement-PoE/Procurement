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
            if (LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                lblUserSettings.Content = "Настройки пользователя";
                lblFavLeague.Content = "Избранная лига";
                lblFavChar.Content = "Избранный персонаж";
                lblCompactMode.Content = "Компактный режим";
                lblCurrencyRatios.Content = "Курсы обмена:";
                lblCharsAndLeague.Content = "Персонажи и лиги";
                lblExcludeTabsFromRecResults.Content = "Исключить вкладки для рецептов крафта";
                chkDownloadOnlySelectedChars.Content = "Загружать только выбранных персонажей";
                chkDownloadOnlySelectedLeagues.Content = "Загружать только выбранные лиги";
                CurrencyGrid.Columns[0].Header = "Имя";
                CurrencyGrid.Columns[1].Header = "Кол-во сфер";
                CurrencyGrid.Columns[2].Header = "Сфер хаоса";

                Procurement.View.LoginView.ChangeImageStyle(AboutButton.Content as Image, LoginWindowViewModel.ServerType);
            }
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
