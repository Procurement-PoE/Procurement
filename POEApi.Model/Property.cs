using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace POEApi.Model
{
    public class Property
    {
        public string Name { get; set; }
        public int DisplayMode { get; set; }
        public List<Tuple<string, int>> Values { get; set; }

        internal Property(JSONProxy.Property property)
        {
            Name = property.Name;
            Values = new List<Tuple<string, int>>();

            foreach (object value in property.Values)
            {
                var pair = (JArray)value;
                Values.Add(new Tuple<string, int>(Sanitize(pair[0]), int.Parse(pair[1].ToString())));
            }

            DisplayMode = property.DisplayMode;
        }

        private string Sanitize(JToken jToken)
        {
            const string infoText = "<<set:MS>><<set:M>><<set:S>>";

            return jToken.ToString().Replace(infoText, string.Empty);
        }
    }
}
