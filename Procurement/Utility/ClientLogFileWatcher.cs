using POEApi.Infrastructure;
using POEApi.Model;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Procurement.Utility
{
    class ClientLogFileWatcher
    {
        private const string _clientLogFileLocationConfigName = "ClientLogFileLocation";
        private const string _enableClientLogFileMonitoringConfigName = "EnableClientLogFileMonitoring";
        private const int _clientLogFilePollingIntervalMilliseconds = 30000;  // 30 seconds

        private static ClientLogFileWatcher _instance;
        public static ClientLogFileWatcher Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ClientLogFileWatcher();

                return _instance;
            }
        }

        protected static FileSystemWatcher FileWatcher
        {
            get;
            private set;
        }

        public delegate void ClientLogFileEventHandler(ClientLogFileWatcher sender, ClientLogFileEventArgs e);
        public static event ClientLogFileEventHandler ClientLogFileChanged;

        public DateTime LastDateTimeSeen
        {
            get;
            protected set;
        }

        public long LastTimestampSeen
        {
            get;
            protected set;
        }

        public long LastFileSizeSeen
        {
            get;
            protected set;
        }

        protected System.Timers.Timer PollingTimer
        {
            get;
            set;
        }

        protected Regex LocationChangedRegex
        {
            get;
            set;
        }

        protected void Initialize()
        {
            if (!Settings.UserSettings.Keys.Contains(_clientLogFileLocationConfigName))
                return;
            string fullFilePath = Settings.UserSettings[_clientLogFileLocationConfigName];
            if (string.IsNullOrWhiteSpace(fullFilePath))
                return;

            if (FileWatcher == null)
            {
                FileWatcher = new FileSystemWatcher();
                FileWatcher.Path = Path.GetDirectoryName(fullFilePath);
                FileWatcher.Filter = Path.GetFileName(fullFilePath);
                FileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.LastAccess;

                FileWatcher.Changed += OnFileChanged;
            }

            if (PollingTimer == null)
            {
                PollingTimer = new System.Timers.Timer();
                PollingTimer.Elapsed += (s, e) => { ReadClientLogFile(); };
                PollingTimer.Interval = _clientLogFilePollingIntervalMilliseconds;
            }

            // Regex to catch lines formatted like:
            //   2019/06/21 19:16:37 245842781 aa1 [INFO Client 19844] : You have entered Sunspire Hideout.
            LocationChangedRegex = new Regex(
                @"(\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}) (\d+) [^ .]* \[.*\] : You have entered (.*).$",
                RegexOptions.Compiled);
        }

        internal void Start()
        {
            if (!Settings.UserSettings.Keys.Contains(_enableClientLogFileMonitoringConfigName))
                return;
            var enabled = Convert.ToBoolean(Settings.UserSettings[_enableClientLogFileMonitoringConfigName]);
            if (!enabled)
                return;

            if (FileWatcher == null)
                Initialize();

            FileWatcher.EnableRaisingEvents = true;

            if (!PollingTimer.Enabled)
                PollingTimer.Start();
        }

        internal void Stop()
        {
            if (FileWatcher != null)
                FileWatcher.EnableRaisingEvents = false;

            PollingTimer.Stop();
        }

        protected void ReadClientLogFile()
        {
            lock (Instance)
            {
                try
                {
                    using (Stream stream = new FileStream(Settings.UserSettings[_clientLogFileLocationConfigName],
                        FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(stream))
                    {
                        // Quit early if the log file is no longer than the last time we read it.
                        if (reader.BaseStream.Length <= Instance.LastFileSizeSeen)
                        {
                            Instance.LastFileSizeSeen = reader.BaseStream.Length;
                            return;
                        }

                        reader.BaseStream.Seek(Instance.LastFileSizeSeen, System.IO.SeekOrigin.Begin);

                        DateTime eventTime = Instance.LastDateTimeSeen;
                        long eventTimestamp = Instance.LastTimestampSeen;
                        string line, location;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Match match = LocationChangedRegex.Match(line);
                            if (!match.Success)
                                continue;

                            eventTime = DateTime.ParseExact(match.Groups[1].Value, "yyyy/MM/dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture);
                            if (!long.TryParse(match.Groups[2].Value, out eventTimestamp))
                            {
                                Logger.Log(string.Format("Failed to parse event timestamp from string '{0}'.",
                                    match.Groups[2].Value));
                            }
                            location = match.Groups[3].Value;

                            if ((DateTime.Now - eventTime).TotalSeconds > 600 ||
                                eventTime < Instance.LastDateTimeSeen)
                            {
                                continue;
                            }

                            ClientLogFileChanged?.Invoke(Instance,
                                new ClientLogFileEventArgs(eventTime, eventTimestamp, location));
                        }

                        Instance.LastDateTimeSeen = eventTime;
                        Instance.LastTimestampSeen = eventTimestamp;
                        Instance.LastFileSizeSeen = reader.BaseStream.Length;
                    }
                }
                catch (IOException ex)
                {
                    Logger.Log(string.Format("Failed to open config log file: {0}", ex.ToString()));
                }
            }
        }

        protected static void OnFileChanged(object source, FileSystemEventArgs e)
        {
            Instance.ReadClientLogFile();
        }
    }
}
