namespace POEApi.Model
{
    public class Socket
    {
        public string Attribute { get; set; }
        public int Group { get; set; }

        internal Socket(JSONProxy.Socket s)
        {
            Attribute = s.Attr;
            Group = s.Group;
        }

        /// <summary>
        /// Convert the Socket attribute to something consumable by path of building
        /// </summary>
        /// <returns></returns>
        public string ToPobFormat()
        {
            switch (Attribute)
            {
                case "S":
                    return Red;
                case "I":
                    return Blue;
                case "D":
                    return Green;
                case "G":
                    return White;
                default:
                    return Abyssal; 
            }
        }

        private const string Red     = "R";
        private const string Green   = "G";
        private const string Blue    = "B";
        private const string White   = "W";
        private const string Abyssal = "A";
    }
}
