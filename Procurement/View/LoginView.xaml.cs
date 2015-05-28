using System.Windows;
using System.Windows.Controls;
using Procurement.ViewModel;
using System.Windows.Media;
using System;

namespace Procurement.View
{
    public partial class LoginView : UserControl, IView
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginWindowViewModel(this);
        }

        public new Grid Content
        {
            get { return this.ViewContent; }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as LoginWindowViewModel).Login(false, ((ComboBoxItem)cmbRealmType.SelectedItem).Content.ToString());
        }

        private void Offline_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as LoginWindowViewModel).Login(true,"");
        }

        private void cmbRealmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRealmType.SelectedItem != null)
            {
                LoginWindowViewModel.ServerType = ((ComboBoxItem)cmbRealmType.SelectedItem).Content.ToString();
                if (LoginWindowViewModel.ServerType != "International")
                {
                    //Garena servers
                    useSession.IsEnabled = false;
                    lblEmail.IsEnabled = false;
                    txtLogin.IsEnabled = false;
                    lblPassword.Content = "SessionID";
                }
                else
                {
                    useSession.IsEnabled = true;
                    lblEmail.IsEnabled = true;
                    txtLogin.IsEnabled = true;
                    lblPassword.Content = "Password";
                }
            }
            else
	        {
                cmbRealmType.SelectedIndex = 0;
	        }
        }
    }
}
