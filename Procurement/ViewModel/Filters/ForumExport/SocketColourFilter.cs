using POEApi.Model;
using System;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    public class SocketColourFilter : IFilter
    {
        private string colour;
        private int count;
        internal string keyword;
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

    class OneRedSocket : SocketColourFilter
    {
        public OneRedSocket()
            : base("S", 1, "At Least 1 Red Socket", "Gear with 1 or more red sockets")
        {
            this.keyword = "Как минимум 1 красный сокет";
        }
    }

    class OneGreenSocket : SocketColourFilter
    {
        public OneGreenSocket()
            : base("D", 1, "At Least 1 Green Socket", "Gear with 1 or more green sockets")
        {
            this.keyword = "Как минимум 1 зеленый сокет";
        }
    }

    class OneBlueSocket : SocketColourFilter
    {
        public OneBlueSocket()
            : base("I", 1, "At Least 1 Blue Socket", "Gear with 1 or more blue sockets")
        {
            this.keyword = "Как минимум 1 синий сокет";
        }
    }

    class TwoRedSockets : SocketColourFilter
    {
        public TwoRedSockets()
            : base("S", 2, "At Least 2 Red Sockets", "Gear with 2 or more red sockets")
        {
            this.keyword = "Как минимум 2 красных сокета";
        }
    }

    class TwoGreenSockets : SocketColourFilter
    {
        public TwoGreenSockets()
            : base("D", 2, "At Least 2 Green Sockets", "Gear with 2 or more green sockets")
        {
            this.keyword = "Как минимум 2 зеленых сокета";
        }
    }

    class TwoBlueSockets : SocketColourFilter
    {
        public TwoBlueSockets()
            : base("I", 2, "At Least 2 Blue Sockets", "Gear with 2 or more blue sockets")
        {
            this.keyword = "Как минимум 2 синих сокета";
        }
    }

    class ThreeRedSockets : SocketColourFilter
    {
        public ThreeRedSockets()
            : base("S", 3, "At Least 3 Red Sockets", "Gear with 3 or more red sockets")
        {
            this.keyword = "Как минимум 3 красных сокета";
        }
    }

    class ThreeGreenSockets : SocketColourFilter
    {
        public ThreeGreenSockets()
            : base("D", 3, "At Least 3 Green Sockets", "Gear with 3 or more green sockets")
        {
            this.keyword = "Как минимум 3 зеленых сокета";
        }
    }

    class ThreeBlueSockets : SocketColourFilter
    {
        public ThreeBlueSockets()
            : base("I", 3, "At Least 3 Blue Sockets", "Gear with 3 or more blue sockets")
        {
            this.keyword = "Как минимум 3 синих сокета";
        }
    }

    class FourRedSockets : SocketColourFilter
    {
        public FourRedSockets()
            : base("S", 4, "At Least 4 Red Sockets", "Gear with 4 or more red sockets")
        {
            this.keyword = "Как минимум 4 красных сокета";
        }
    }

    class FourGreenSockets : SocketColourFilter
    {
        public FourGreenSockets()
            : base("D", 4, "At Least 4 Green Sockets", "Gear with 4 or more green sockets")
        {
            this.keyword = "Как минимум 4 зеленых сокета";
        }
    }

    class FourBlueSockets : SocketColourFilter
    {
        public FourBlueSockets()
            : base("I", 4, "At Least 4 Blue Sockets", "Gear with 4 or more blue sockets")
        {
            this.keyword = "Как минимум 4 синих сокета";
        }
    }

    class FiveRedSockets : SocketColourFilter
    {
        public FiveRedSockets()
            : base("S", 5, "At Least 5 Red Sockets", "Gear with 5 or more red sockets")
        {
            this.keyword = "Как минимум 5 красных сокетов";
        }
    }

    class FiveGreenSockets : SocketColourFilter
    {
        public FiveGreenSockets()
            : base("D", 5, "At Least 5 Green Sockets", "Gear with 5 or more green sockets")
        {
            this.keyword = "Как минимум 5 зеленых сокетов";
        }
    }

    class FiveBlueSockets : SocketColourFilter
    {
        public FiveBlueSockets()
            : base("I", 5, "At Least 5 Blue Sockets", "Gear with 5 or more blue sockets")
        {
            this.keyword = "Как минимум 5 синих сокетов";
        }
    }
}
