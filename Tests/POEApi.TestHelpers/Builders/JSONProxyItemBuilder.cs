using Newtonsoft.Json.Linq;
using POEApi.Model.JSONProxy;
using System;
using System.Collections.Generic;

namespace POEApi.TestHelpers.Builders
{
    public class JSONProxyItemBuilder
    {
        public static implicit operator Model.JSONProxy.Item(JSONProxyItemBuilder builder) => builder._item;

        private Model.JSONProxy.Item _item = new Model.JSONProxy.Item
        {
            // TODO: Id, not Name, should be set to a random GUID.  But we should not have any avoidable randomness in
            // tests, anyway.
            Name = Guid.NewGuid().ToString(),
            TypeLine = Guid.NewGuid().ToString(),
            Properties = new List<Model.JSONProxy.Property>(),
            Requirements = new List<Model.JSONProxy.Requirement>()
        };

        public JSONProxyItemBuilder ThatIsAnAbyssJewel(bool isAbyssJewel)
        {
            _item.AbyssJewel = isAbyssJewel;

            return this;
        }

        public JSONProxyItemBuilder ThatIsARelic(bool isRelic)
        {
            _item.IsRelic = isRelic;

            return this;
        }

        public JSONProxyItemBuilder ThatIsIdentified(bool isIdentified)
        {
            _item.Identified = isIdentified;

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

        public JSONProxyItemBuilder WithId(string id)
        {
            _item.Id = id;

            return this;
        }

        public JSONProxyItemBuilder WithItemLevel(int itemLevel)
        {
            _item.Ilvl = itemLevel;

            return this;
        }

        public JSONProxyItemBuilder WithFrameType(int frameType)
        {
            _item.FrameType = frameType;

            return this;
        }

        public JSONProxyItemBuilder WithProperty(string name, string firstValuePair, int secondValuePair)
        {
            _item.Properties.Add(new Property
            {
                Name = name,
                Values = new List<object> { new JArray(firstValuePair, secondValuePair) },
            });

            return this;
        }

        public JSONProxyItemBuilder WithQuality(int qualityAmount)
        {
            return this.WithProperty("Quality", "+" + qualityAmount, 1);
        }
    }
}