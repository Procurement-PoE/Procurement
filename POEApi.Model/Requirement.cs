using Newtonsoft.Json.Linq;

namespace POEApi.Model
{
    public class Requirement
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool NameFirst { get; set; }

        internal Requirement(JSONProxy.Requirement proxy)
        {
            Name = proxy.Name;
            NameFirst = proxy.DisplayMode == 0;
            Value = ((JArray)proxy.Values[0])[0].ToString();
        }
    }
}
