using POEApi.Infrastructure;
using POEApi.Model;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Linq;

namespace Procurement.Utility
{
    class PoeTradeOnlineHelper
    {

        private static PoeTradeOnlineHelper instance;
        private Timer refreshTimer;
        private Uri refreshUri;

        private const double TWO_MINUTES = 120000;

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        private PoeTradeOnlineHelper()
        {
            refreshTimer = new Timer();
            refreshTimer.Elapsed += (s, e) => { RefreshOnlineStatus(); };
            refreshTimer.Interval = TWO_MINUTES;
        }

        public static PoeTradeOnlineHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new PoeTradeOnlineHelper();

                return instance;
            }
        }

        private bool currentlyOnline;
        private void RefreshOnlineStatus()
        {
            try
            {
                Func<Process, bool> IsPoE = c => c.MainWindowTitle.Contains("Path of Exile") || c.ProcessName.Contains("PathOfExile") || c.ProcessName.Contains("PathOfExile_x64");
                var idleTime = GetIdleTime();

                if (idleTime >= TimeSpan.FromMinutes(10) || !Process.GetProcesses().Any(IsPoE))
                {
                    // Prevent from spamming poe.trade
                    if (currentlyOnline)
                        SetOffline();

                    // User is AFK or PoE is not running.
                    currentlyOnline = false;
                    return;
                }

                SetOnline();
            }
            catch (Exception ex)
            {
                Logger.Log("Error refreshing online/offline status in PoeTradeOnlineHelper: " + ex);
            }
        }

        private void SetOnline()
        {
            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                client.UploadValuesAsync(refreshUri, "POST", data);
                currentlyOnline = true;
            }
        }

        private void SetOffline()
        {
            using (var client = new WebClient())
            {
                var offlineUri = new Uri(refreshUri.OriginalString + "/offline");
                var data = new NameValueCollection();
                client.UploadValuesAsync(offlineUri, "POST", data);
            }
        }

        private TimeSpan GetIdleTime()
        {
            var inputInfo = new LASTINPUTINFO()
            {
                cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO)),
                dwTime = 0
            };
            GetLastInputInfo(ref inputInfo);
            // Allow for TickCount wrap-around.
            return TimeSpan.FromMilliseconds(unchecked(Environment.TickCount - (int)inputInfo.dwTime));
        }

        internal void Start()
        {
            try
            {
                var enabled = Convert.ToBoolean(Settings.UserSettings["PoeTradeRefreshEnabled"]);

                if (!enabled || refreshTimer.Enabled)
                    return;

                var poeTradeUrl = Settings.UserSettings["PoeTradeRefreshUrl"];
                refreshUri = new Uri(poeTradeUrl);

                refreshTimer.Start();
                RefreshOnlineStatus();
            }
            catch (Exception ex)
            {
                Logger.Log("Error starting PoeTradeOnlineHelper: " + ex);
                MessageBox.Show("Error refreshing poe.trade online status, ensure you have a valid url entered.", "Error Starting PoeTradeOnlineHelper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal void Stop()
        {
            refreshTimer.Stop();
        }
    }
}
