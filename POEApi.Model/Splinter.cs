using System;
using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public class Splinter : StackableItem
    {
        public BreachType Type { get; set; }

        public Splinter(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}