using System;
namespace POEApi.Model
{
    public class Socket
    {
        public string Attribute { get; set; }
        public int Group { get; set; }

        internal Socket(JSONProxy.Socket s)
        {
            this.Attribute = s.Attribute;
            this.Group = s.Group;
        }
        public static String encodeSocketChar(String socketlabel)
        {
            switch (socketlabel)
            {
                case "S":
                    return "R";
                case "D":
                    return "G";
                case "I":
                    return "B";
                default:
                    return "W";
            }
        }
    }
}
