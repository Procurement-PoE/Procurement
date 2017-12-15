using System.Collections.Generic;

namespace POEApi.Model
{
    public abstract class SocketableItem : Item
    {
        public List<Requirement> Requirements { get; set; }

        public int Socket { get; set; }
        public string Color { get; set; }

        protected SocketableItem(JSONProxy.Item item) : base(item)
        {
            this.Socket = item.Socket;
            this.Color = item.Colour;
            this.Requirements = ProxyMapper.GetRequirements(item.Requirements);
        }
    }
}