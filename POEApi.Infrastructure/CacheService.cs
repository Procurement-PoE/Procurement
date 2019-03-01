using System.IO;
using System.Linq;
using System.Reflection;

namespace POEApi.Infrastructure
{
    public class CacheService
    {
        private string _location;

        private string GetFullPath(string key)
        {
            // Percent-encode any characters in the key name that aren't valid in filenames
            key = Path.GetInvalidFileNameChars()
                .Aggregate(key, (current, c) => current.Replace(c.ToString(), "%" + ((int)c).ToString("X2")));

            if (Path.GetExtension(key) != string.Empty)
                return Path.Combine(_location, key);

            return Path.Combine(_location, key + ".bin");
        }

        public CacheService()
            : this("Common")
        { }

        public CacheService(string email)
        {
            _location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), email);
            if (!Directory.Exists(_location))
                Directory.CreateDirectory(_location);
        }

        public bool Exists(string key)
        {
            return File.Exists(GetFullPath(key));
        }

        public Stream Get(string key)
        {
            return new MemoryStream(File.ReadAllBytes(GetFullPath(key)));
        }

        public void Set(string key, Stream data)
        {
            StreamReader reader = new StreamReader(data);
            File.WriteAllBytes(GetFullPath(key), reader.ReadAllBytes());
        }

        public void Remove(string key)
        {
            File.Delete(GetFullPath(key));
        }

        public void Clear()
        {
            Directory.Delete(_location, true);
            Directory.CreateDirectory(_location);
        }
    }
}
