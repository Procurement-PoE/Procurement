using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        bool Authenticate(string email, SecureString password);
        Stream GetAccountName(string realm);
        Stream GetStash(int index, string league, string accountName, string realm);
        Stream GetImage(string url);
        Stream GetCharacters(string realm);
        Stream GetInventory(string characterName, bool forceRefresh, string accountName, string realm);
        event ThottledEventHandler Throttled;
        bool UpdateThread(string threadID, string threadTitle, string threadText);
        bool BumpThread(string threadID, string threadTitle);
    }
}
