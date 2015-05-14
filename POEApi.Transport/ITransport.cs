using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        string Authenticate(string email, SecureString password, bool useSessionID, string current_accname);
        Stream GetStash(int index, string league, string accname);
        Stream GetStash(int index, string league, bool refresh, string accname);
        Stream GetImage(string url);
        Stream GetCharacters();
        Stream GetInventory(string characterName, bool forceRefresh, string accname);
        event ThottledEventHandler Throttled;
        bool UpdateThread(string threadID, string threadTitle, string threadText);
        bool BumpThread(string threadID, string threadTitle);
    }
}
