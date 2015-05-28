using System.IO;
using System.Linq;
using System.Reflection;

namespace POEApi.Infrastructure
{
    public class CacheService
    {
        private string location;

        private string getFullPath(string key)
        {
            // Percent-encode any characters in the key name that aren't valid in filenames
            key = Path.GetInvalidFileNameChars()
                .Aggregate(key, (current, c) => current.Replace(c.ToString(), "%" + ((int)c).ToString("X2")));

            if (Path.GetExtension(key) != string.Empty)
                return Path.Combine(location, key);

            return Path.Combine(location, key + ".bin");
        }

        public CacheService()
            : this("Common")
        { }

        public CacheService(string email)
        {
            if (email == "") email = "NoEmail";
            location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), email);
            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);
        }

        public bool Exists(string key)
        {
            return File.Exists(getFullPath(key));
        }

        public Stream Get(string key)
        {
            return new MemoryStream(File.ReadAllBytes(getFullPath(key)));
        }

        public void Set(string key, Stream data)
        {
            StreamReader reader = new StreamReader(data);
            File.WriteAllBytes(getFullPath(key), reader.ReadAllBytes());
        }

        public void Remove(string key)
        {
            File.Delete(getFullPath(key));
        }

        public void Clear()
        {
            Directory.Delete(location, true);
            Directory.CreateDirectory(location);
        }
    }
}
