using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Procurement.View;

namespace Procurement.ViewModel
{
    public class ScreenController
    {
        private static MainWindow mainView;
        private static Dictionary<string, IView> screens = new Dictionary<string, IView>();

        public DelegateCommand MenuButtonCommand { get; set; }

        private const string STASH_VIEW = "StashView";

        public ScreenController(MainWindow layout)
        {
            MenuButtonCommand = new DelegateCommand(execute);
            mainView = layout;
            initLogin();
        }

        private void execute(object obj)
        {
            string key = obj.ToString();
            
            if (key == "Exit")
                Application.Current.Shutdown();
            else
                loadView(screens[key]);
        }

        private void initScreens()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
            new Action(() =>
            {
                screens.Add(STASH_VIEW, new StashView());
                screens.Add("Inventory", new InventoryView());
                screens.Add("Trading", new TradingView());
                screens.Add("Settings", new SettingsView());
            }));
        }

        private void initLogin()
        {
            var loginView = new LoginView();
            var loginVM = (loginView.DataContext as LoginWindowViewModel);
            loginVM.OnLoginCompleted += new LoginWindowViewModel.LoginCompleted(loginCompleted);
            loadView(loginView);
        }

        void loginCompleted()
        {
            initScreens();
            loadView(screens.First().Value);
            showMenuButtons();
        }

        private static void showMenuButtons()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            new Action(() =>
            {
                mainView.Buttons.Visibility = Visibility.Visible;
            }));
        }

        private void loadView(IView view)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, 
                new Action(() => 
                {
                     mainView.MainRegion.Children.Clear();
                     mainView.MainRegion.Children.Add(view as UserControl);
                }));
        }
    }
}