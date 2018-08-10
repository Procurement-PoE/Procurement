using System;
using System.Collections.Generic;
using System.Linq;
using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class Splinter : StackableItem, IBreachCurrency
    {
        public BreachType Type { get; set; }

        public Splinter(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}