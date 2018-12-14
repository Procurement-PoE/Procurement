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

        private void sessionidQ_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Since DragMove() called by MouseLeftButtonDown of MainWindow may cause System.InvalidOperationException, the notice of a mouse event is stopped here.
            // Moreover, it is also a problem that a mouse event is blocked by DragMove() and MouseLeftButtonUp is not called. 
            e.Handled = true;
        }

        private void sessionidQ_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;

            (this.DataContext as LoginWindowViewModel).NavigateHowToSessionIDwiki();
        }

        // TODO: Can we have each button's mouse down and up handlers routed to the same two functions, and those
        // functions properly route to the correct LoginWindowViewModel function?  Also, should we have the URI for
        // the buttons a property of the buttons themselves, so we do not need two different LoginWindowViewModel
        // functions?
        private void userAgentQ_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Since DragMove() called by MouseLeftButtonDown of MainWindow may cause System.InvalidOperationException, the notice of a mouse event is stopped here.
            // Moreover, it is also a problem that a mouse event is blocked by DragMove() and MouseLeftButtonUp is not called.
            e.Handled = true;
        }

        private void userAgentQ_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;

            (this.DataContext as LoginWindowViewModel).NavigateWhatIsMyUserAgent();
        }
    }
}
