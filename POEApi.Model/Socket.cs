namespace POEApi.Model
{
    public class Socket
    {
        public string Attribute { get; set; }
        public int Group { get; set; }

        internal Socket(JSONProxy.Socket s)
        {
            this.Attribute = s.Attr;
            this.Group = s.Group;
        }
    }
}
