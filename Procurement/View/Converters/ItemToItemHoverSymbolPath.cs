using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using POEApi.Model;

namespace Procurement.View
{
    public class ItemToItemHoverSymbolPath : IValueConverter
    {
        private static readonly Color NormalItemColor = Colors.White;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as Item;
            if (item == null)
            {
                return "";
            }

            if (item.Shaper)
            {
                return "/Images/ItemHover/ShaperIconAdorner.png";
            }
            if (item.Elder)
            {
                return "/Images/ItemHover/ElderIconAdorner.png";
            }
            if (item.VeiledMods?.Count > 0)
            {
                return "/Images/ItemHover/VeiledIconAdorner.png";
            }
            if (item.Synthesised)
            {
                return "/Images/ItemHover/SynthesisedIconAdorner.png";
            }
            if (item.Fractured)
            {
                return "/Images/ItemHover/FracturedIconAdorner.png";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}