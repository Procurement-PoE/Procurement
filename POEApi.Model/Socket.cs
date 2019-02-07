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
                    return "R";
                case "I":
                    return "B";
                case "D":
                    return "G";
                case "G":
                    return "W";
                default:
                    //Abyssal socket.
                    return "A"; 
            }
        }
    }
}
