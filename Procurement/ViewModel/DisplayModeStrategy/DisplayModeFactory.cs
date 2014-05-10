using System;
using POEApi.Model;

namespace Procurement.ViewModel
{
    internal static class DisplayModeFactory
    {
        public static IDisplayModeStrategy Create(Property property)
        {
            switch (property.DisplayMode)
            {
                case 0:
                    return new NamePlusValueStrategy(property);
                case 1:
                    return new ValuesPlusNameStrategy(property);
                case 2:
                case 3:
                    return new StringFormatStrategy(property);
            }
            throw new NotImplementedException("DispalyMode Factory, I have no idea what I am doing");
        }
    }
}
