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
        private ITransport _innerTranport;
        private const string _stashKey = "stash";
        private CacheService _userCacheService;
        private CacheService _commonCacheService;

        public event ThottledEventHandler Throttled;

        public CachedTransport(string email, ITransport innerTranport, bool offline)
        {
            Offline = offline;
            _userCacheService = new CacheService(email);
            _commonCacheService = new CacheService();
            _innerTranport = innerTranport;
            _innerTranport.Throttled += instance_Throttled;
        }

        private void instance_Throttled(object sender, ThottledEventArgs e)
        {
            if (Throttled != null)
                Throttled(sender, e);
        }

        public bool Authenticate(string email, SecureString password)
        {
            return _innerTranport.Authenticate(email, password);
        }

        public Stream GetAccountName(string realm)
        {
            return _innerTranport.GetAccountName(realm);
        }

        public Stream GetStash(int index, string league, string accountName, bool refresh, string realm)
        {
            string key = string.Format("{0}-{1}-{2}", league, _stashKey, index);

            if (refresh && _userCacheService.Exists(key))
                _userCacheService.Remove(key);

            if (!Offline && !_userCacheService.Exists(key))
                _userCacheService.Set(key, _innerTranport.GetStash(index, league, accountName, realm));

            if(_userCacheService.Exists(key))
                return _userCacheService.Get(key);

            return Stream.Null;
        }

        public Stream GetStash(int index, string league, string accountName, string realm)
        {
            return GetStash(index, league, accountName, false, realm);
        }

        public Stream GetImage(string url)
        {
            Stream ms;
            try
            {
                string key = string.Concat(url.GetHash(), ".png");
                if (!_commonCacheService.Exists(key))
                    _commonCacheService.Set(key, _innerTranport.GetImage(url));

                ms = _commonCacheService.Get(key);
            }
            catch (WebException ex)
            {
                Logger.Log(string.Format("Failed to get image '{0}': {1}", url, ex.ToString()));

                ms = new MemoryStream();
                SystemIcons.Error.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
            }

            return ms;
        }

        public Stream GetCharacters(string realm)
        {
            string key = "characterdata";

            if (!Offline && !_userCacheService.Exists(key))
                _userCacheService.Set(key, _innerTranport.GetCharacters(realm));

            if(_userCacheService.Exists(key))
                return _userCacheService.Get(key);

            return Stream.Null;
        }

        public Stream GetInventory(string characterName, bool forceRefresh, string accountName, string realm)
        {
            if (forceRefresh && _userCacheService.Exists(characterName))
                _userCacheService.Remove(characterName);

            if (!Offline && !_userCacheService.Exists(characterName))
                _userCacheService.Set(characterName, _innerTranport.GetInventory(characterName, forceRefresh, accountName, realm));

            if(_userCacheService.Exists(characterName))
                return _userCacheService.Get(characterName);

            return Stream.Null;
        }

        public bool UpdateThread(string threadID, string threadTitle, string threadText)
        {
            return _innerTranport.UpdateThread(threadID, threadTitle, threadText);
        }

        public bool BumpThread(string threadID, string threadTitle)
        {
            return _innerTranport.BumpThread(threadID, threadTitle);
        }
    }
}