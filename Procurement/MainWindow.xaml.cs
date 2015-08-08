using POEApi.Model;
using Procurement.Controls;
using Procurement.ViewModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Procurement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = ApplicationState.Version;
            ScreenController.Create(this);
            this.DataContext = ScreenController.Instance;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseLeftButtonDown);
            initLayout();
        }

        private void initLayout()
        {
            if (Settings.UserSettings["MinimalMode"] == "false")
                return;

            this.Height = 720;
            this.ResizeMode = ResizeMode.CanMinimize;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.AllowsTransparency = false;
            this.MainGrid.Background = Brushes.Black;
            this.WindowControls.Visibility = Visibility.Hidden;
            this.ButtonSpacer.Width = new GridLength(128);
        }

        void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ItemDisplay.ClosePopups();
            DragMove();
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
