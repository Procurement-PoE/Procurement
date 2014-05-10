using System.Windows;
using System.Text;
using System;
using POEApi.Infrastructure;

namespace Procurement
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Log("Application Version: " + ApplicationState.Version);
            Logger.Log(getEnvironementDetails());
            Logger.Log(e.Exception.ToString());
            MessageBox.Show("There was an unhandled error - Sorry! Please send the debuginfo.log to one of the developers. For quickest response please log a ticket on our google code page at http://code.google.com/p/procurement/issues");
        }

        private string getEnvironementDetails()
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.AppendLine("CurrentCulture: " + System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                builder.AppendLine("CurrentUICulture: " + System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
            }
            catch (Exception ex)
            {
                builder.AppendLine(ex.ToString());
            }

            return builder.ToString();
        }
    }
}