using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POEApi.Model
{
    public class Net : Currency
    {
        public Net(JSONProxy.Item item) : base(item)
        {
            this.NetTier = ProxyMapper.GetNetTier(item.Properties);
        }

        public int NetTier { get; }
    }
}