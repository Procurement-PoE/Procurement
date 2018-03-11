using System;
using POEApi.Model.JSONProxy;
using System.Collections.Generic;

namespace POEApi.TestHelpers.Builders
{
    public class JSONProxyItemBuilder
    {
        public static implicit operator Model.JSONProxy.Item(JSONProxyItemBuilder builder) => builder._item;

        private Model.JSONProxy.Item _item = new Model.JSONProxy.Item
        {
            Name = Guid.NewGuid().ToString(),
            TypeLine = Guid.NewGuid().ToString(),
            Properties = new List<Model.JSONProxy.Property>(),
            Requirements = new List<Model.JSONProxy.Requirement>()
        };

        public JSONProxyItemBuilder ThatIsAnAbyssJewel()
        {
            _item.AbyssJewel = true;

            return this;
        }

        public JSONProxyItemBuilder WithoutSockets()
        {
            _item.Sockets = null;
            _item.SocketedItems = null;

            return this;
        }

        public JSONProxyItemBuilder WithDescrText(string descrText)
        {
            _item.DescrText = descrText;

            return this;
        }

        public JSONProxyItemBuilder WithSocketedItem(Model.JSONProxy.Item item)
        {
            if (_item.SocketedItems == null)
                _item.SocketedItems = new List<Model.JSONProxy.Item>();

            _item.SocketedItems.Add(item);

            return this;
        }

        public JSONProxyItemBuilder WithTypeLine(string typeLine)
        {
            _item.TypeLine = typeLine;

            return this;
        }

        public JSONProxyItemBuilder WithName(string name)
        {
            _item.Name = name;

            return this;
        }
    }
}