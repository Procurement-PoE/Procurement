﻿using POEApi.Infrastructure;
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
        private const string VERSION_URL = @"https://raw.githubusercontent.com/medvedttn/Procurement/master/latest-release.txt";
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
            catch (KeyNotFoundException kex)
            {
                MessageBox.Show("Unable to check for updates as the CheckForUpdates setting is missing from your settings file.", Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
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

                //extract only version substring like "1.9.0"
                updateInfo[0] = updateInfo[0].Replace("Procurement ", "");
                Version curr_ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                Version new_ver = new Version(updateInfo[0]);

                if (curr_ver>=new_ver || MessageBox.Show(Procurement.MessagesRes.ANewVersionOfProcurementMedvedEditionIsAvailable, Procurement.MessagesRes.UpdateAvailable, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
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
            MessageBox.Show(Procurement.MessagesRes.ErrorCheckingForUpdatesDetailsLoggedToDebugInfoLog, Procurement.MessagesRes.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}