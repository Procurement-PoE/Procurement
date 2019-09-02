using System.Collections.Generic;
using System.IO;
using System.Linq;
using POEApi.Infrastructure;
using POEApi.Model.Events;
using POEApi.Transport;
using System.Security;
using System;
using System.Diagnostics;
using POEApi.Infrastructure.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using POEApi.Model.JSONProxy;
using System.Threading.Tasks;

namespace POEApi.Model
{
    public class POEModel
    {
        public ITransport Transport;
        private CacheService cacheService;
        private bool downOnlyMyCharacters;


        public delegate void AuthenticateEventHandler(POEModel sender, AuthenticateEventArgs e);
        public event AuthenticateEventHandler Authenticating;

        public delegate void StashLoadEventHandler(POEModel sender, StashLoadedEventArgs e);
        public event StashLoadEventHandler StashLoading;

        public delegate void ImageLoadEventHandler(POEModel sender, ImageLoadedEventArgs e);
        public event ImageLoadEventHandler ImageLoading;

        public event ThottledEventHandler Throttled;

        public bool Offline { get; private set; }

        public POEModel()
        {
            downOnlyMyCharacters = bool.Parse(Settings.UserSettings["DownloadOnlyMyCharacters"]);
        }

        public bool Authenticate(string email, SecureString password, bool offline, string realm)
        {
            if (Transport != null)
                Transport.Throttled -= new ThottledEventHandler(instance_Throttled);

            Transport = GetTransport(email, offline);
            cacheService = new CacheService(email);
            Offline = offline;

            if (offline)
                return true;

            Transport.Throttled += new ThottledEventHandler(instance_Throttled);
            onAuthenticate(POEEventState.BeforeEvent, email,realm);

            Transport.Authenticate(email, password);

            onAuthenticate(POEEventState.AfterEvent, email, realm);

            return true;
        }

        public string GetAccountName(string realm)
        {
            try
            {
                if (Offline)
                {
                    return string.Empty;
                }

                var account = GetProperObjectFromTransport<Account>(Transport.GetAccountName(realm));

                if (string.IsNullOrEmpty(account?.AccountName))
                {
                    throw new Exception("Null account name received from API");
                }

                return account.AccountName;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Error downloading account name, exception details: {0}", ex.ToString()));

                throw new Exception(@"Error downloading account name, details logged to DebugInfo.log. Please open a ticket at https://github.com/Stickymaddness/Procurement/issues and include your DebugInfo.log");
            }
        }

        /// <summary>
        /// With a stream from the transport and a strong type, parse the json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="networkData"></param>
        /// <returns></returns>
        private T GetProperObjectFromTransport<T>(Stream networkData)
        {
            using (var stream = networkData)
            using (var textReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(textReader))
            {
                try
                {
                    var serializer = new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver()};

                    return (T) serializer.Deserialize(jsonTextReader, typeof (T));
                }
                catch
                {
                    MemoryStream ms = stream as MemoryStream;
                    ms.Seek(0, SeekOrigin.Begin);
                    var sr = new StreamReader(ms);
                    var text = sr.ReadToEnd();
                    Debug.Write(text);
                }
            }
            
            throw new ApplicationException("Unable to deserialize object");
        }

        void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(sender, e);
        }

        private ITransport GetTransport(string email, bool offline)
        {
            if (Settings.ProxySettings["Enabled"] != bool.TrueString)
                return new CachedTransport(email, new HttpTransport(email), offline);

            return new CachedTransport(email, new HttpTransport(email, Settings.ProxySettings["Username"], Settings.ProxySettings["Password"], Settings.ProxySettings["Domain"]), offline);
        }

        public void ForceRefresh()
        {
            cacheService.Clear();
        }

        public Stash GetStash(int index, string league, string accountName, string realm, bool forceRefresh = false)
        {
            JSONProxy.Stash proxy = null;

            onStashLoaded(POEEventState.BeforeEvent, index, -1);

            using (var stream = Transport.GetStash(index, league, accountName, realm, forceRefresh))
            {
                try
                {
                    if (stream == Stream.Null)
                    {
                        return new Stash(null);
                    }

                    proxy = GetProperObjectFromTransport<JSONProxy.Stash>(stream);
                    if (proxy == null)
                        LogNullStash(stream, "Proxy was null");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    LogNullStash(stream, "JSON Serialization Failed");
                }
            }

            onStashLoaded(POEEventState.AfterEvent, index, proxy.NumTabs);

            return new Stash(proxy);
        }

        private void LogNullStash(Stream stream, string errorPrefix)
        {
            try
            {
                MemoryStream ms = stream as MemoryStream;
                ms.Seek(0, SeekOrigin.Begin);
                Logger.Log(errorPrefix + ": base64 bytes:");
                Logger.Log(Convert.ToBase64String(ms.ToArray()));
                Logger.Log("END");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

            throw new Exception(@"Downloading stash, details logged to DebugInfo.log, please open a ticket at https://github.com/Stickymaddness/Procurement/issues and include your DebugInfo.log");
        }

        public Stash GetStash(string league, string accountName, string realm)
        {
            try
            {
                var myTabs = Settings.Lists["MyTabs"];
                bool onlyMyTabs = myTabs.Count != 0;

                Stash stash = GetStash(0, league, accountName, realm);

                var firstTab = stash.Tabs.FirstOrDefault();
                if (firstTab != null && firstTab.Hidden)
                    stash.ClearItems();

                List<Tab> skippedTabs = new List<Tab>();

                if (!onlyMyTabs)
                    return GetAllTabs(league, accountName, stash, realm);

                int tabCount = 0;

                for (int i = 1; i < stash.NumberOfTabs; i++)
                {
                    if (myTabs.Contains(stash.Tabs[i].Name))
                    {
                        stash.Add(GetStash(i, league, accountName, realm));
                        ++tabCount;
                    }
                    else
                        skippedTabs.Add(stash.Tabs[i]);
                }

                foreach (var tab in skippedTabs)
                    stash.Tabs.Remove(tab);

                stash.NumberOfTabs = tabCount + 1;

                return stash;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Error downloading stash for {0}, exception details: {1}", league, ex.ToString()));
                throw new Exception(@"Downloading stash for " + league + ", details logged to DebugInfo.log, please open a ticket at https://github.com/Stickymaddness/Procurement/issues and include your DebugInfo.log");
            }
        }

        private Stash GetAllTabs(string league, string accountName, Stash stash, string realm)
        {
            List<Tab> hiddenTabs = new List<Tab>();

            for (int i = 1; i < stash.NumberOfTabs; i++)
            {
                if (!stash.Tabs[i].Hidden)
                {
                    stash.Add(GetStash(i, league, accountName, realm));
                }
                else
                {
                    hiddenTabs.Add(stash.Tabs[i]);
                }
            }

            var firstTab = stash.Tabs.FirstOrDefault();
            if (firstTab != null && firstTab.Hidden)
            {
                stash.Tabs.Remove(stash.Tabs[0]);
                --stash.NumberOfTabs;
            }

            foreach (var tab in hiddenTabs)
            {
                stash.Tabs.Remove(tab);
                --stash.NumberOfTabs;
            }

            return stash;
        }

        public List<Character> GetCharacters(string realm)
        {
            return GetProperObjectFromTransport<List<Character>>(Transport.GetCharacters(realm));
        }

        public List<Item> GetInventory(string characterName, int index, bool forceRefresh, string accountName, string realm)
        {
            try
            {
                onStashLoaded(POEEventState.BeforeEvent, index, -1);

                if (downOnlyMyCharacters && !Settings.Lists["MyCharacters"].Contains(characterName))
                    return new List<Item>();

                Inventory item = null;
                using (var stream = Transport.GetInventory(characterName, forceRefresh, accountName, realm))
                {
                    item = GetProperObjectFromTransport<Inventory>(stream);
                }

                onStashLoaded(POEEventState.AfterEvent, index, -1);

                if (item?.Items == null)
                    return new List<Item>();

                return item.Items.Select(i => ItemFactory.Get(i)).ToList();
            }
            catch (Exception sex)
            {
                Logger.Log(string.Format("Error reading character data for character '{0}', Exception info: ", characterName, sex.ToString()));
                throw new Exception(string.Format("Error reading character data for {0}, if you are in offline mode you will need to login and update. If you received this error while logging in, the authenticated session may have expired or bad data has been returned by GGG or a network issue may have occurred - Please try again.", characterName));
            }
        }

        public void GetImages(IEnumerable<Item> items)
        {
            foreach (var item in items.Distinct(new ImageComparer()))
                GetImageWithEvents(item);

            LoadShaperElderImages(items);
        }

        private void LoadShaperElderImages(IEnumerable<Item> items)
        {
            IEnumerable<Item> elderShaperItems = items.Where(i => i.Shaper || i.Elder);

            foreach (var item in elderShaperItems)
            {
                var prefix = item.Shaper ? "Shaper" : "Elder";

                onImageLoaded(POEEventState.BeforeEvent, $"{prefix} {item.Name}");
                Transport.GetImage(item.BackgroundUrl);
                onImageLoaded(POEEventState.AfterEvent, $"{prefix} {item.Name}");
            }
        }

        private void GetImageWithEvents(Item item)
        {
            try
            {
                GetImageWithEvents(GetItemName(item), item.IconURL);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Error downloading image for : {0}, exception details: {1}", item.IconURL, ex.ToString()));
                throw;
            }
        }

        private void GetImageWithEvents(string name, string url)
        {
            onImageLoaded(POEEventState.BeforeEvent, name);
            Transport.GetImage(url);
            onImageLoaded(POEEventState.AfterEvent, name);
        }

        public Stream GetImage(string url)
        {
            return Transport.GetImage(url);
        }

        public IEnumerable<Stream> GetImage(Tab tab)
        {
            onImageLoaded(POEEventState.BeforeEvent, tab.Name);
            yield return Transport.GetImage(tab.srcL);
            yield return Transport.GetImage(tab.srcC);
            yield return Transport.GetImage(tab.srcR);
            onImageLoaded(POEEventState.AfterEvent, tab.Name);
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText)
        {
            return Transport.UpdateThread(threadID, threadTitle, threadText);
        }

        public bool BumpThread(string threadId, string threadTitle)
        {
            return Transport.BumpThread(threadId, threadTitle);
        }

        private static string GetItemName(Item item)
        {
            if (item.Name != string.Empty)
                return item.Name;

            return item.TypeLine;
        }

        private void onStashLoaded(POEEventState state, int index, int numberOfTabs)
        {
            if (StashLoading != null)
                StashLoading(this, new StashLoadedEventArgs(index, numberOfTabs, state));
        }

        private void onImageLoaded(POEEventState state, string url)
        {
            if (ImageLoading != null)
                ImageLoading(this, new ImageLoadedEventArgs(url, state));
        }

        private void onAuthenticate(POEEventState state, string email, string realm)
        {
            if (Authenticating != null)
                Authenticating(this, new AuthenticateEventArgs(email, realm, state));
        }
    }
}
