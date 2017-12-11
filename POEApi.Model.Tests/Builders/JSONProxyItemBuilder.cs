using System;
using POEApi.Model.JSONProxy;

namespace POEApi.Model.Tests.Builders
{
    internal class JSONProxyItemBuilder
    {
        public static implicit operator JSONProxy.Item(JSONProxyItemBuilder builder) => builder._item;

        private JSONProxy.Item _item = new JSONProxy.Item();

        internal JSONProxyItemBuilder WithoutSockets()
        {
            _item.Sockets = null;
            _item.SocketedItems = null;

            return this;
        }

        internal JSONProxyItemBuilder WithTypeLine(string typeLine)
        {
            _item.TypeLine = typeLine;

            return this;
        }
    }
}
