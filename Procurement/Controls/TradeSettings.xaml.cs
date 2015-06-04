using Procurement.ViewModel;
using System.Windows.Controls;
using System.Diagnostics;

namespace Procurement.Controls
{
    public partial class BuyoutSettings : UserControl
    {
        public BuyoutSettings()
        {
            InitializeComponent();
            this.DataContext = new TradeSettingsViewModel();
            if (ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
            {
                lblBuyoutSettings.Content = "Настройки торговли";
                lblThreadID.Content = "Идентификатор темы";
                lblThreadTitle.Content = "Заголовок темы";
                chkBuyoutsTag.Content = "Продаваемые предметы отображаются только под тегом продажи";
                chkEmbedBuyouts.Content = "Встроенная продажа включена";
                chkOnlyDisplayWithBuyout.Content = "Выставлять на продажу только предметы с ценой";
                chkEnableAutoOnlineRefresh.Content = "Автоматическое обновление онлайн статуса";
                btnSavePoeTradeSettings.Content = "Сохранить настройки онлайн статуса";
                btnSaveThreadSettings.Content = "Сохранить настройки форума";
                chkEnableAutoOnlineRefresh.Visibility = System.Windows.Visibility.Hidden;
                lblEnterPersonalURL.Visibility = System.Windows.Visibility.Hidden;
                lblPoeTradeURL.Visibility = System.Windows.Visibility.Hidden;
                txtPoeTradeURL.Visibility = System.Windows.Visibility.Hidden;
                btnSavePoeTradeSettings.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("http://www.pathofexile.com/private-messages/compose/to/poexyzis");
        }
    }
}
