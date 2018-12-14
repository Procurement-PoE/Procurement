using System.IO;
using POEApi.Infrastructure;
using System.Security;
using POEApi.Infrastructure.Events;
using System.Net;
using System.Drawing;

namespace POEApi.Transport
{
    public class CachedTransport : ITransport
    {
        public bool Offline { get; }
        private ITransport innerTranport;
        private const string stashKey = "stash";
        private CacheService userCacheService;
        private CacheService commonCacheService;

        public event ThottledEventHandler Throttled;

        private string _userAgent;
        public string UserAgent
        {
            get
            {
                return innerTranport.UserAgent;
            }
            set
            {
                innerTranport.UserAgent = value;
            }
        }

        public CachedTransport(string email, ITransport innerTranport, bool offline)
        {
            Offline = offline;
            userCacheService = new CacheService(email);
            commonCacheService = new CacheService();
            this.innerTranport = innerTranport;
            this.innerTranport.Throttled += instance_Throttled;
        }

        private void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(sender, e);
        }

        public bool Authenticate(string email, SecureString password, bool useSessionID, string cloudFlareId,
            string cloudFlareClearance)
        {
            return innerTranport.Authenticate(email, password, useSessionID, cloudFlareId, cloudFlareClearance);
        }

        public Stream GetAccountName()
        {
            return innerTranport.GetAccountName();
        }

        public Stream GetStash(int index, string league, string accountName, bool refresh)
        {
            string key = string.Format("{0}-{1}-{2}", league, stashKey, index);

            if (refresh && userCacheService.Exists(key))
                userCacheService.Remove(key);

            if (!Offline && !userCacheService.Exists(key))
                userCacheService.Set(key, innerTranport.GetStash(index, league, accountName));

            if(userCacheService.Exists(key))
                return userCacheService.Get(key);

            return Stream.Null;
        }

        public Stream GetStash(int index, string league, string accountName)
        {
            return GetStash(index, league, accountName, false);
        }

        public Stream GetImage(string url)
        {
            Stream ms;
            try
            {
                string key = string.Concat(url.GetHash(), ".png");
                if (!commonCacheService.Exists(key))
                    commonCacheService.Set(key, innerTranport.GetImage(url));

                ms = commonCacheService.Get(key);
            }
            catch (WebException)
            {
                ms = new MemoryStream();
                SystemIcons.Error.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }

            return ms;
        }

        public Stream GetCharacters()
        {
            string key = "characterdata";

            if (!Offline && !userCacheService.Exists(key))
                userCacheService.Set(key, innerTranport.GetCharacters());

            if(userCacheService.Exists(key))
                return userCacheService.Get(key);

            return Stream.Null;
        }

        public Stream GetInventory(string characterName, bool forceRefresh, string accountName)
        {
            if (forceRefresh && userCacheService.Exists(characterName))
                userCacheService.Remove(characterName);

            if (!Offline && !userCacheService.Exists(characterName))
                userCacheService.Set(characterName, innerTranport.GetInventory(characterName, forceRefresh, accountName));

            if(userCacheService.Exists(characterName))
                return userCacheService.Get(characterName);

            return Stream.Null;
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText)
        {
            return innerTranport.UpdateThread(threadID, threadTitle, threadText);
        }

        public bool BumpThread(string threadID, string threadTitle)
        {
            return innerTranport.BumpThread(threadID, threadTitle);
        }
    }
}