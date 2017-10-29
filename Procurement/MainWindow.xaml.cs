using POEApi.Model;
using Procurement.Controls;
using Procurement.ViewModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System;

namespace Procurement
{
    public partial class MainWindow : Window
    {
        private static readonly int WM_SYSKEYDOWN = 0x0104;
        private static readonly int VK_MENU = 0x12;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = ApplicationState.Version;
            ScreenController.Create(this);
            this.DataContext = ScreenController.Instance;
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseLeftButtonDown);

            Loaded += (o, e) =>
            {
                // Hook the window procedure
                var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
                source.AddHook(new HwndSourceHook(WndProc));
            };

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

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Disables the ALT key
            if ((msg == WM_SYSKEYDOWN) && (wParam.ToInt32() == VK_MENU))
            {
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
