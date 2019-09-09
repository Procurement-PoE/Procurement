using System.Collections.Generic;

namespace POEApi.Model
{
    public abstract class SocketableItem : Item
    {

        public int Socket { get; set; }
        public string Color { get; set; }

        protected SocketableItem(JSONProxy.Item item) : base(item)
        {
            Socket = item.Socket;
            Color = item.Colour;
        }
    }
}