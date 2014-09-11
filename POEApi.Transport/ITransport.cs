using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        bool Authenticate(string email, SecureString password, bool useSessionID);
        Stream GetStash(int index, string league);
        Stream GetStash(int index, string league, bool refresh);
        Stream GetImage(string url);
        Stream GetCharacters();
        Stream GetInventory(string characterName, bool forceRefresh);
        event ThottledEventHandler Throttled;
        bool UpdateThread(string threadID, string threadTitle, string threadText);
        bool BumpThread(string threadID, string threadTitle);
    }
}
