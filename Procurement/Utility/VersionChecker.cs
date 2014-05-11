using System;
using System.Diagnostics;
using System.Net;
using System.Windows;

namespace Procurement.Utility
{
    internal class VersionChecker
    {
        private const string VERSION_URL = @"https://raw.githubusercontent.com/Stickymaddness/Procurement/master/lastest-release.txt";
        public static void CheckForUpdates()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadStringAsync(new Uri(VERSION_URL));
                client.DownloadStringCompleted += client_DownloadStringCompleted;
            }
        }

        private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var updateInfo = e.Result.Split(',');

            if (updateInfo[0] == ApplicationState.Version || MessageBox.Show("A new version of Procurement is available! Would you like to download now? (Opens in browser)", "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            Process.Start(updateInfo[1]);
        }
    }
}
