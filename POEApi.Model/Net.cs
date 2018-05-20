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

        // Note: Necromancy nets technically do not have a net tier, but uses the default value of 0 here.
        public int NetTier { get; }
    }
}