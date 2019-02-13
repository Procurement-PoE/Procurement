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

        /// <summary>
        /// Convert the Socket attribute to something consumable by path of building
        /// </summary>
        /// <returns></returns>
        public string ToPobFormat()
        {
            switch (Attribute)
            {
                case "S":
                    return RED;
                case "I":
                    return BLUE;
                case "D":
                    return GREEN;
                case "G":
                    return WHITE;
                default:
                    return ABYSSAL; 
            }
        }

        private const string RED = "R";
        private const string BLUE = "B";
        private const string GREEN = "G";
        private const string WHITE = "W";
        private const string ABYSSAL = "A";
    }
}
