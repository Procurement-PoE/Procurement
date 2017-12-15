using System;
using POEApi.Model.JSONProxy;
using System.Collections.Generic;

namespace POEApi.Model.Tests.Builders
{
    internal class JSONProxyItemBuilder
    {
        public static implicit operator JSONProxy.Item(JSONProxyItemBuilder builder) => builder._item;

        private JSONProxy.Item _item = new JSONProxy.Item
        {
            Name = Guid.NewGuid().ToString(),
            TypeLine = Guid.NewGuid().ToString(),
            Properties = new List<JSONProxy.Property>(),
            Requirements = new List<JSONProxy.Requirement>()
        };

        internal JSONProxyItemBuilder ThatIsAnAbyssJewel()
        {
            _item.AbyssJewel = true;

            return this;
        }

        internal JSONProxyItemBuilder WithoutSockets()
        {
            _item.Sockets = null;
            _item.SocketedItems = null;

            return this;
        }

        internal JSONProxyItemBuilder WithDescrText(string descrText)
        {
            _item.DescrText = descrText;

            return this;
        }

        internal JSONProxyItemBuilder WithSocketedItem(JSONProxy.Item item)
        {
            if (_item.SocketedItems == null)
                _item.SocketedItems = new List<JSONProxy.Item>();

            _item.SocketedItems.Add(item);

            return this;
        }

        internal JSONProxyItemBuilder WithTypeLine(string typeLine)
        {
            _item.TypeLine = typeLine;

            return this;
        }
    }
}