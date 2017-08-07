using POEApi.Infrastructure;
using POEApi.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;

namespace Procurement.Utility
{
    internal class VersionChecker
    {
        private const string VERSION_URL = @"https://raw.githubusercontent.com/Stickymaddness/Procurement/master/latest-release.txt";
        public static void CheckForUpdates()
        {
#if DEBUG
#else
            try
            {
                if (bool.Parse(Settings.UserSettings["CheckForUpdates"]) == false)
                    return;

                using (WebClient client = new WebClient())
                {
                    client.DownloadStringAsync(new Uri(VERSION_URL));
                    client.DownloadStringCompleted += client_DownloadStringCompleted;
                }
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("Unable to check for updates as the CheckForUpdates setting is missing from your settings file.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
#endif
        }

        private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string[] updateInfo = e.Result.Split(',');

                updateInfo[0] = updateInfo[0].Replace("Procurement ", "");
                var appVersion = ApplicationState.Version.Replace("Procurement ", "");
                var currentVersion = new Version(appVersion);
                var latestVersion = new Version(updateInfo[0]);

                if (currentVersion >= latestVersion || MessageBox.Show("A new version of Procurement is available! Would you like to download now? (Opens in browser)", "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;

                Process.Start(updateInfo[1]);
            }
            catch (Exception ex)
            {
                handleException(ex);
            }
        }

        private static void handleException(Exception ex)
        {
            Logger.Log(ex.ToString());
            MessageBox.Show("Error checking for updates, details logged to DebugInfo.log", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}