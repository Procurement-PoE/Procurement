using System.Collections.Generic;

namespace POEApi.Model
{
    public class Requirement
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool NameFirst { get; set; }

        internal Requirement(JSONProxy.Requirement proxy)
        {
            this.Name = proxy.Name;
            this.NameFirst = proxy.DisplayMode == 0;
            this.Value = ((object[])proxy.Value[0])[0].ToString();
        }
    }
}
