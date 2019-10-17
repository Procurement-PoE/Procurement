using POEApi.Model;
using System;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class SocketColourFilter : IFilter
    {
        private string colour;
        private int count;
        private string keyword;
        private string help;

        public SocketColourFilter(string colour, int count, string keyword, string help)
        {
            this.colour = colour;
            this.count = count;
            this.keyword = keyword;
            this.help = help;
        }

        public bool CanFormCategory
        {
            get { return true; }
        }

        public string Keyword
        {
            get { return keyword; }
        }

        public string Help
        {
            get { return help; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.SocketColour; }
        }

        public bool Applicable(Item item)
        {
            var gear = item as Gear;

            if (gear == null)
                return false;

            return gear.Sockets.Where(s => s.Attribute.Equals(colour, StringComparison.OrdinalIgnoreCase)).Count() >= count;
        }
    }

    class OneAbyssalSocket : SocketColourFilter
    {
        public OneAbyssalSocket()
            : base("A", 1, "At Least 1 Abyssal Socket", "Gear with 1 or more abyssal sockets")
        { }
    }

    class OneWhiteSocket : SocketColourFilter
    {
        public OneWhiteSocket()
            : base("G", 1, "At Least 1 White Socket", "Gear with 1 or more white sockets")
        { }
    }

    class OneRedSocket : SocketColourFilter
    {
        public OneRedSocket()
            : base("S", 1, "At Least 1 Red Socket", "Gear with 1 or more red sockets")
        { }
    }

    class OneGreenSocket : SocketColourFilter
    {
        public OneGreenSocket()
            : base("D", 1, "At Least 1 Green Socket", "Gear with 1 or more green sockets")
        { }
    }

    class OneBlueSocket : SocketColourFilter
    {
        public OneBlueSocket()
            : base("I", 1, "At Least 1 Blue Socket", "Gear with 1 or more blue sockets")
        { }
    }

    class TwoAbyssalSockets : SocketColourFilter
    {
        public TwoAbyssalSockets()
            : base("A", 2, "At Least 2 Abyssal Sockets", "Gear with 2 or more abyssal sockets")
        { }
    }

    class TwoWhiteSockets : SocketColourFilter
    {
        public TwoWhiteSockets()
            : base("G", 2, "At Least 2 White Sockets", "Gear with 2 or more white sockets")
        { }
    }

    class TwoRedSockets : SocketColourFilter
    {
        public TwoRedSockets()
            : base("S", 2, "At Least 2 Red Sockets", "Gear with 2 or more red sockets")
        { }
    }

    class TwoGreenSockets : SocketColourFilter
    {
        public TwoGreenSockets()
            : base("D", 2, "At Least 2 Green Sockets", "Gear with 2 or more green sockets")
        { }
    }

    class TwoBlueSockets : SocketColourFilter
    {
        public TwoBlueSockets()
            : base("I", 2, "At Least 2 Blue Sockets", "Gear with 2 or more blue sockets")
        { }
    }

    class ThreeWhiteSockets : SocketColourFilter
    {
        public ThreeWhiteSockets()
            : base("G", 3, "At Least 3 White Sockets", "Gear with 3 or more white sockets")
        { }
    }

    class ThreeRedSockets : SocketColourFilter
    {
        public ThreeRedSockets()
            : base("S", 3, "At Least 3 Red Sockets", "Gear with 3 or more red sockets")
        { }
    }

    class ThreeGreenSockets : SocketColourFilter
    {
        public ThreeGreenSockets()
            : base("D", 3, "At Least 3 Green Sockets", "Gear with 3 or more green sockets")
        { }
    }

    class ThreeBlueSockets : SocketColourFilter
    {
        public ThreeBlueSockets()
            : base("I", 3, "At Least 3 Blue Sockets", "Gear with 3 or more blue sockets")
        { }
    }

    class FourWhiteSockets : SocketColourFilter
    {
        public FourWhiteSockets()
            : base("G", 4, "At Least 4 White Sockets", "Gear with 4 or more white sockets")
        { }
    }

    class FourRedSockets : SocketColourFilter
    {
        public FourRedSockets()
            : base("S", 4, "At Least 4 Red Sockets", "Gear with 4 or more red sockets")
        { }
    }

    class FourGreenSockets : SocketColourFilter
    {
        public FourGreenSockets()
            : base("D", 4, "At Least 4 Green Sockets", "Gear with 4 or more green sockets")
        { }
    }

    class FourBlueSockets : SocketColourFilter
    {
        public FourBlueSockets()
            : base("I", 4, "At Least 4 Blue Sockets", "Gear with 4 or more blue sockets")
        { }
    }

    class FiveWhiteSockets : SocketColourFilter
    {
        public FiveWhiteSockets()
            : base("G", 5, "At Least 5 White Sockets", "Gear with 5 or more white sockets")
        { }
    }

    class FiveRedSockets : SocketColourFilter
    {
        public FiveRedSockets()
            : base("S", 5, "At Least 5 Red Sockets", "Gear with 5 or more red sockets")
        { }
    }

    class FiveGreenSockets : SocketColourFilter
    {
        public FiveGreenSockets()
            : base("D", 5, "At Least 5 Green Sockets", "Gear with 5 or more green sockets")
        { }
    }

    class FiveBlueSockets : SocketColourFilter
    {
        public FiveBlueSockets()
            : base("I", 5, "At Least 5 Blue Sockets", "Gear with 5 or more blue sockets")
        { }
    }
}
