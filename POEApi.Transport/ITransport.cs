using System.IO;
using System.Security;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
    public interface ITransport
    {
        string Authenticate(string email, SecureString password, bool useSessionID, string current_accname, string server_type);
        Stream GetStash(int index, string league, string accname, string server_type);
        Stream GetStash(int index, string league, bool refresh, string accname, string server_type);
        Stream GetImage(string url);
        Stream GetCharacters(string server_type);
        Stream GetInventory(string characterName, bool forceRefresh, string accname, string server_type);
        event ThottledEventHandler Throttled;
        bool UpdateThread(string threadID, string threadTitle, string threadText, string server_type);
        bool BumpThread(string threadID, string threadTitle, string server_type);
    }
}
