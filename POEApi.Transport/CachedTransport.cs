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
        private ITransport innerTranport;
        private const string stashKey = "stash";
        private CacheService userCacheService;
        private CacheService commonCacheService;

        public event ThottledEventHandler Throttled;

        public CachedTransport(string email, ITransport innerTranport)
        {
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

        public bool Authenticate(string email, SecureString password, bool useSessionID)
        {
            return innerTranport.Authenticate(email, password, useSessionID);
        }

        public Stream GetStash(int index, string league, bool refresh)
        {
            string key = string.Format("{0}-{1}-{2}", league, stashKey, index);

            if (refresh && userCacheService.Exists(key))
                userCacheService.Remove(key);

            if (!userCacheService.Exists(key))
                userCacheService.Set(key, innerTranport.GetStash(index, league));

            return userCacheService.Get(key);
        }

        public Stream GetStash(int index, string league)
        {
            return GetStash(index, league, false);
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
            string key ="characterdata";

            if (!userCacheService.Exists(key))
                userCacheService.Set(key, innerTranport.GetCharacters());

            return userCacheService.Get(key);
        }

        public Stream GetInventory(string characterName)
        {
            if (!userCacheService.Exists(characterName))
                userCacheService.Set(characterName, innerTranport.GetInventory(characterName));

            return userCacheService.Get(characterName);
        }
    }
}