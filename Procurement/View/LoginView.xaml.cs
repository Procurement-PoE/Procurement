﻿using System.Windows;
using System.Windows.Controls;
using Procurement.ViewModel;
using System.Windows.Media;
using System;
using POEApi.Model;
using System.Linq;

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
            (this.DataContext as LoginWindowViewModel).Login(true, ((ComboBoxItem)cmbRealmType.SelectedItem).Content.ToString());
        }

        private void cmbRealmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRealmType.SelectedItem != null)
            {
                LoginWindowViewModel.ServerType = ((ComboBoxItem)cmbRealmType.SelectedItem).Content.ToString();
                if (LoginWindowViewModel.ServerType != "International")
                {
                    //Garena servers
                    useSession.IsChecked = false;
                    useSession.IsEnabled = false;
                    lblEmail.IsEnabled = false;
                    txtLogin.Text = "NoEmail";
                    txtLogin.IsEnabled = false;
                    lblPassword.Content = "SessionID";

                    //RU GUI
                    lblEmail.Content = "Эл.почта";
                    lblPassword.Content = "Идентификатор сессии";
                    //TODO: img RU buttons LoginButton.Content = "Войти";
                    //OfflineButton.Content = "Оффлайн";
                }
                else
                {
                    useSession.IsEnabled = true;
                    lblEmail.IsEnabled = true;
                    txtLogin.IsEnabled = true;
                    txtLogin.Text = "";
                    lblPassword.Content = "Password";
                }
            }
            else
	        {
                cmbRealmType.SelectedIndex = 0;
	        }
        }

        private void cmbRealmType_Loaded(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Settings.UserSettings["ServerType"]) && Settings.UserSettings["ServerType"] != "")
            {
                cmbRealmType.SelectedItem = cmbRealmType.Items.Cast<ComboBoxItem>().FirstOrDefault(cmbi => cmbi.Content.ToString() == Settings.UserSettings["ServerType"]);
            }
        }
    }
}
