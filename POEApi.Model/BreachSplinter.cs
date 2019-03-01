using System;
using System.Collections.Generic;
using System.Linq;
using POEApi.Model.Interfaces;

namespace POEApi.Model
{
    public class BreachSplinter : Item, IBreachCurrency
    {
        public BreachType Type { get; set; }

        public BreachSplinter(JSONProxy.Item item) : base(item)
        {
            Type = ProxyMapper.GetBreachType(item);
        }
    }
}