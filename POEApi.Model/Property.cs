using System;
using System.Collections.Generic;

namespace POEApi.Model
{
    public class Property
    {
        public string Name { get; set; }
        public int DisplayMode { get; set; }
        public List<Tuple<string, int>> Values { get; set; }

        internal Property(JSONProxy.Property property)
        {
            this.Name = property.Name;
            Values = new List<Tuple<string, int>>();

            foreach (object value in property.Values)
            {
                object[] pair = (object[])value;
                Values.Add(new Tuple<string, int>(pair[0].ToString(), int.Parse(pair[1].ToString())));
            }
            
            this.DisplayMode = property.DisplayMode;
        }
    }
}
