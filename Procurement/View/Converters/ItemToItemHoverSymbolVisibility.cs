using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using POEApi.Model;

namespace Procurement.View
{
    public class ItemToItemHoverSymbolVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as Item;
            if (item == null)
            {
                return Visibility.Collapsed;
            }

            if (item.Shaper || item.Elder || item.VeiledMods?.Count > 0 || item.Synthesised)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}