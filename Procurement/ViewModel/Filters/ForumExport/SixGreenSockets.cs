﻿using POEApi.Model;
using System;
using System.Linq;

namespace Procurement.ViewModel.Filters.ForumExport
{
    class SixGreenSockets : IFilter
    {
        public bool CanFormCategory
        {
            get { throw new NotImplementedException(); }
        }

        public string Keyword
        {
            get 
            {
                if (Procurement.ViewModel.LoginWindowViewModel.ServerType == "Garena (RU)")
                {
                    return "6 зелёных сокетов";
                }
                else
                {
                    return "6 Green Sockets";
                }
            }
        }

        public string Help
        {
            get { return "Gear with 6 green sockets"; }
        }

        public FilterGroup Group
        {
            get { return FilterGroup.Default; }
        }

        public bool Applicable(Item item)
        {
            var gear = item as Gear;

            if (gear == null)
                return false;

            return gear.Sockets.Where(s => s.Attribute.Equals("D", StringComparison.OrdinalIgnoreCase)).Count() == 6;
        }
    }
}
