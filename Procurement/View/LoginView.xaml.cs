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
            (this.DataContext as LoginWindowViewModel).Login(false);
        }

        private void Offline_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as LoginWindowViewModel).Login(true);
        }
    }
}
