using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using POEApi.Infrastructure;
using POEApi.Model.Events;
using POEApi.Transport;
using System.Security;
using System;
using POEApi.Infrastructure.Events;
using System.Runtime.Serialization;

namespace POEApi.Model
{
    public class POEModel
    {
        private ITransport transport;
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

        public bool Authenticate(string email, SecureString password, bool offline, bool useSessionID)
        {
            if (transport != null)
                transport.Throttled -= new ThottledEventHandler(instance_Throttled);

            transport = getTransport(email);
            cacheService = new CacheService(email);
            Offline = offline;

            if (offline)
                return true;

            transport.Throttled += new ThottledEventHandler(instance_Throttled);
            onAuthenticate(POEEventState.BeforeEvent, email);

            transport.Authenticate(email, password, useSessionID);

            onAuthenticate(POEEventState.AfterEvent, email);

            return true;
        }

        void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(sender, e);
        }

        private ITransport getTransport(string email)
        {
            if (Settings.ProxySettings["Enabled"] != bool.TrueString)
                return new CachedTransport(email, new HttpTransport(email));

            return new CachedTransport(email, new HttpTransport(email, Settings.ProxySettings["Username"], Settings.ProxySettings["Password"], Settings.ProxySettings["Domain"]));
        }

        public void ForceRefresh()
        {
            cacheService.Clear();
        }

        public Stash GetStash(int index, string league, bool forceRefresh)
        {
            DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(JSONProxy.Stash));
            JSONProxy.Stash proxy = null;

            onStashLoaded(POEEventState.BeforeEvent, index, -1);

            using (Stream stream = transport.GetStash(index, league, forceRefresh))
            {
                try
                {
                    proxy = (JSONProxy.Stash)serialiser.ReadObject(stream);
                    if (proxy == null)
                        logNullStash(stream, "Proxy was null");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    logNullStash(stream, "JSON Serialization Failed");
                }
            }

            onStashLoaded(POEEventState.AfterEvent, index, proxy.NumTabs);

            return new Stash(proxy);
        }

        private void logNullStash(Stream stream, string errorPrefix)
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

            throw new Exception(@"Downloading stash, details logged to DebugInfo.log, please open a ticket at https://github.com/Stickymaddness/Procurement/issues");
        }

        public Stash GetStash(string league)
        {
            try
            {
                var myTabs = Settings.Lists["MyTabs"];
                bool onlyMyTabs = myTabs.Count != 0;

                Stash stash = GetStash(0, league, false);

                if (stash.Tabs[0].Hidden)
                    stash.ClearItems();

                List<Tab> skippedTabs = new List<Tab>();

                if (!onlyMyTabs)
                    return getAllTabs(league, stash);

                int tabCount = 0;

                for (int i = 1; i < stash.NumberOfTabs; i++)
                {
                    if (myTabs.Contains(stash.Tabs[i].Name))
                    {
                        stash.Add(GetStash(i, league, false));
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
                throw new Exception(@"Downloading stash for " + league + ", details logged to DebugInfo.log, please open a ticket at https://github.com/Stickymaddness/Procurement/issues");
            }
        }

        private Stash getAllTabs(string league, Stash stash)
        {
            List<Tab> hiddenTabs = new List<Tab>();

            for (int i = 1; i < stash.NumberOfTabs; i++)
                if (!stash.Tabs[i].Hidden)
                    stash.Add(GetStash(i, league, false));
                else
                    hiddenTabs.Add(stash.Tabs[i]);

            if (stash.Tabs[0].Hidden)
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

        public List<Character> GetCharacters()
        {
            DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(List<JSONProxy.Character>));
            List<JSONProxy.Character> characters;

            using (Stream stream = transport.GetCharacters())
                characters = (List<JSONProxy.Character>)serialiser.ReadObject(stream);

            return characters.Select(c => new Character(c)).ToList();
        }

        public List<Item> GetInventory(string characterName, bool forceRefresh)
        {
            try
            {
                if (downOnlyMyCharacters && !Settings.Lists["MyCharacters"].Contains(characterName))
                    return new List<Item>();

                DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(JSONProxy.Inventory));
                JSONProxy.Inventory item;

                using (Stream stream = transport.GetInventory(characterName, forceRefresh))
                    item = (JSONProxy.Inventory)serialiser.ReadObject(stream);

                if (item.Items == null)
                    return new List<Item>();

                return item.Items.Select(i => ItemFactory.Get(i)).ToList();
            }
            catch (SerializationException sex)
            {
                Logger.Log(string.Format("Error reading character data for character '{0}', Exception info: ", characterName, sex.ToString()));
                throw new Exception(string.Format("Error reading character data for {0}, if you are in offline mode you will need to login and update. If you received this error while logging in, the authenticated session may have expired or bad data has been returned by GGG or a network issue may have occurred - Please try again.", characterName));
            }
        }

        public void GetImages(IEnumerable<Item> items)
        {
            foreach (var item in items.Distinct(new ImageComparer()))
                getImageWithEvents(item);
        }

        private void getImageWithEvents(Item item)
        {
            try
            {
                getImageWithEvents(GetItemName(item), item.IconURL);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Error downloading image for : {0}, exception details: {1}", item.IconURL, ex.ToString()));
                throw;
            }
        }

        private void getImageWithEvents(string name, string url)
        {
            onImageLoaded(POEEventState.BeforeEvent, name);
            transport.GetImage(url);
            onImageLoaded(POEEventState.AfterEvent, name);
        }

        public Stream GetImage(string url)
        {
            return transport.GetImage(url);
        }

        public IEnumerable<Stream> GetImage(Tab tab)
        {
            onImageLoaded(POEEventState.BeforeEvent, tab.Name);
            yield return transport.GetImage(tab.srcL);
            yield return transport.GetImage(tab.srcC);
            yield return transport.GetImage(tab.srcR);
            onImageLoaded(POEEventState.AfterEvent, tab.Name);
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText)
        {
            return transport.UpdateThread(threadID, threadTitle, threadText);
        }

        public bool BumpThread(string threadId, string threadTitle)
        {
            return transport.BumpThread(threadId, threadTitle);
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

        private void onAuthenticate(POEEventState state, string email)
        {
            if (Authenticating != null)
                Authenticating(this, new AuthenticateEventArgs(email, state));
        }
    }
}
