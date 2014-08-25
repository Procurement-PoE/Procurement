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
            MessageBox.Show("There was an unhandled error - Sorry! Please create a ticket on github https://github.com/Stickymaddness/Procurement/issues or google code http://code.google.com/p/procurement/issues. If the crash occured after Procurement was done downloading, zip and include your .tab files.");           
        }

        private string getEnvironementDetails()
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.AppendLine("CurrentCulture: " + System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                builder.AppendLine("CurrentUICulture: " + System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
                builder.AppendLine("Operating System: " + Environment.OSVersion.ToString());
            }
            catch (Exception ex)
            {
                builder.AppendLine(ex.ToString());
            }

            return builder.ToString();
        }
    }
}