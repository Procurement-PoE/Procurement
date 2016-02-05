using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        bool Authenticate(string email, SecureString password, bool useSessionID);
        StreamReader GetAccountName();
        Stream GetStash(int index, string league, string accountName);
        Stream GetStash(int index, string league, string accountName, bool refresh);
        Stream GetImage(string url);
        Stream GetCharacters();
        Stream GetInventory(string characterName, bool forceRefresh, string accountName);
        event ThottledEventHandler Throttled;
        bool UpdateThread(string threadID, string threadTitle, string threadText);
        bool BumpThread(string threadID, string threadTitle);
    }
}
